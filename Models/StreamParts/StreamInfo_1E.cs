using Flux.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class StreamInfo_1E : StreamInfo
    {
        public override void ReadTypes(RawFile file)
        {
            uint typeListCount = file.ReadUInt();
            Types = new FieldType[typeListCount];

            for (int i = 0; i < typeListCount; i++)
            {
                uint typeDefBlockSize = file.ReadUInt();
                string typeStr = file.ReadIntPascalString(false);
                if (typeStr != "Type") throw new DataMisalignedException($"Expected 'Type' got '{typeStr}'");

                string typeName = file.ReadIntPascalString(false);

                uint typeSize = file.ReadUInt(); // number of bytes this type uses

                if (!TypeStringMap.TryGetValue(typeName, out FieldType fieldType))
                {
                    throw new NotSupportedException($"Type '{typeName}' not supported!");
                }

                Types[i] = fieldType;
            }
        }

        public override void ReadClasses(RawFile file)
        {
            uint classCount = file.ReadUInt();

            Classes = new ClassDefinition[classCount];
            for (int i = 0; i < classCount; i++)
                Classes[i] = new ClassDefinition();

            for (int i = 0; i < classCount; i++)
            {
                long startOfClass = file.Position;

                uint classDefBlockSize = file.ReadUInt();

                string classStr = file.ReadIntPascalString(false);
                if (classStr != "Class") throw new DataMisalignedException($"Expected 'Class' got '{classStr}'");

                string className = file.ReadIntPascalString(false);

                Console.WriteLine($"Class {className} ({i}):");

                uint versionBlockSize = file.ReadUInt();
                string versionStr = file.ReadIntPascalString(false);
                if (versionStr != "Version") throw new DataMisalignedException($"Expected 'Version' got '{versionStr}'");

                float version = file.ReadFloat();

                uint classTypesBlockSize = file.ReadUInt();
                string classTypesStr = file.ReadIntPascalString(false);
                if (classTypesStr != "Types") throw new DataMisalignedException($"Expected 'Types' got '{classTypesStr}'");

                uint classTypeCount = file.ReadUInt();
                ClassTypeDefinition[] classTypeDefinitions = new ClassTypeDefinition[classTypeCount];

                for (int j = 0; j < classTypeCount; j++)
                {
                    uint typeIndex = file.ReadUInt();

                    string classTypeName = file.ReadIntPascalString(false);

                    uint flag1 = file.ReadUInt(); // I think this is actually an array of byte flags regardless though I don't really use any of them...
                    uint flag2 = file.ReadUInt();
                    uint shouldUseAlternative = file.ReadUInt();
                    uint flag4 = file.ReadUInt();

                    int countVal = file.ReadInt(); // usually -1, but appears to be 0 if it's an list ("Bone Names" in .AS files), and then two bytes need to be read before reading the type that is the number of items (I'd assume that if this is > 0, then it's a fixed size array)

                    if (countVal > 0)
                    {
                        Console.WriteLine();
                    }

                    var def = new ClassTypeDefinition
                    {
                        Name = classTypeName,
                        CountVal = countVal,
                        Index = typeIndex,
                        UseClassRef = ((shouldUseAlternative & 0xc0000000) != 0),
                        ReadAsBlock = ((shouldUseAlternative & 0x00900000) == 0x00900000),
                    };

                    if (def.UseClassRef)
                    {
                        Console.WriteLine($"\tProperty: {classTypeName} - Class: {Classes[typeIndex].Name}");
                    }
                    else
                    {
                        Console.WriteLine($"\tProperty: {classTypeName} - Type: {Types[typeIndex]}");
                    }

                    //if ((shouldUseAlternative & 0xc0000000) != 0) // Should use class list, not type list
                    //{
                    //    classTypeDefinitions[j].Reference = Classes[typeIndex];
                    //    Console.WriteLine($"\tClass: {classTypeName} - Class: {Classes[typeIndex].Name}");
                    //}
                    //else
                    //{
                    //    classTypeDefinitions[j].Type = Types[typeIndex];
                    //    Console.WriteLine($"\tType: {classTypeName} - Type: {Types[typeIndex]}");
                    //}

                    Console.WriteLine($"\t\tFlags: {flag1:X8} {flag2:X8} {shouldUseAlternative:X8} {flag4:X8} {countVal:X8}");

                    classTypeDefinitions[j] = def;
                }

                Classes[i].Name = className;
                Classes[i].Version = version;
                Classes[i].Types = classTypeDefinitions;

                while (file.Position - startOfClass < classDefBlockSize)
                {
                    uint extraPartBlockSize = file.ReadUInt();
                    string extraPartTitle = file.ReadIntPascalString(false);
                    if (extraPartTitle == "Components")
                    {
                        uint componentsCount = file.ReadUInt(); // number of components in this class

                        ComponentDefinition[] componentDefinitions = new ComponentDefinition[componentsCount];

                        for (int j = 0; j < componentsCount; j++)
                        {
                            uint componentDefinitionSize = file.ReadUInt(); // Size of the component definition block

                            string componentLabel = file.ReadIntPascalString(false); // "Component"

                            string componentName = file.ReadIntPascalString(false); // e.g. "Animation sets", "Texture"

                            uint componentClassIndex = file.ReadUInt();

                            componentDefinitions[j] = new ComponentDefinition
                            {
                                Name = componentName,
                                ClassIndex = componentClassIndex
                            };

                            Console.WriteLine($"\tComponent: {componentName} ({componentClassIndex})");
                        }

                        Classes[i].Components = componentDefinitions;
                    }
                    else if (extraPartTitle == "Params")
                    {
                        uint paramsOffset = file.ReadUInt();
                        Classes[i].Params = Classes[paramsOffset];
                        Console.WriteLine($"\tParams: {Classes[paramsOffset].Name}");
                    }
                }

                if (file.Position - startOfClass != classDefBlockSize)
                {
                    throw new DataMisalignedException("Failed at reading class definition. Refusing to go further.");
                }
            }
        }

        public override ContainerList ReadOLST(RawFile file)
        {
            uint rootBlockSize = file.ReadUInt();

            string olstStr = file.ReadIntPascalString(false);

            ushort classIndex = file.ReadUShort();
            int mobjCount = file.ReadInt();
            ContainerList containers = new ContainerList(mobjCount);

            for (int i = 0; i < mobjCount; i++)
            {
                var thisClass = Classes[classIndex];

                containers.AddContainer(ReadMOBJ(thisClass, file));
            }

            return containers;
        }

        private object ReadSingleValue(RawFile file, FieldType type)
        {
            return type switch
            {
                FieldType.String => file.ReadIntPascalString(false, 0x0fff),
                FieldType.Char => file.ReadByte(),
                FieldType.UChar => file.ReadByte(),
                FieldType.Short => file.ReadShort(),
                FieldType.Int => file.ReadInt(),
                FieldType.Int64 => file.ReadLong(),
                FieldType.Float => file.ReadFloat(),
                FieldType.Colour3 => (file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                FieldType.Vec3 => (file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                FieldType.Vec4 => (file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                FieldType.Mtx => (
                    (file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                    (file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                    (file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                    (file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat())
                ),
                FieldType.Half => file.ReadUShort(),   // TODO: OBVIOUSLY WRONG, FIX!!!
                FieldType.HalfVec3 => (file.ReadFloat(), file.ReadShort()),   // TODO: OBVIOUSLY WRONG, FIX!!!

                _ => throw new NotSupportedException($"Unimplemented type {type}"),
            };
        }

        public FieldValue ReadField(RawFile file, ClassTypeDefinition typeDef)
        {
            int count;
            if (typeDef.CountVal == -1)
            {
                count = 1;
            }
            else if (typeDef.CountVal == 0)
            {
                count = file.ReadUShort(); // two-byte count
            }
            else
            {
                count = file.ReadUShort(); // not sure what the value in the class type definition is for since it defines it anyway... (lp5_ghostbusters_b_tech_flow.led)
                //count = typeDef.CountVal; // fixed count
            }

            if (typeDef.ReadAsBlock)
            {
                uint blockSize = file.ReadUInt();
                file.Seek(blockSize - 4, SeekOrigin.Current); // skip for now
                return new PrimitiveField(typeDef.Name, FieldType.Data, $"<Data Block of {blockSize} bytes>");
            }

            FieldType type = Types[typeDef.Index];
            if (type == FieldType.ClassObject) // Let's just hope that there can't be multiple of these
            {
                byte classObjectExists = file.ReadByte();
                if (classObjectExists == 0) 
                { 
                    return new ContainerInstance("null", 0, 0); 
                }
                var def = GetClass(file.ReadIntPascalString(false)); 
                Console.WriteLine($"Instantiating {def.Name}"); 
                return ReadMOBJ(def, file);
            }

            if (count == 1)
            {
                return new PrimitiveField(typeDef.Name, type, ReadSingleValue(file, type));
            }
            else
            {
                var values = new object[count];
                for (int i = 0; i < count; i++)
                    values[i] = ReadSingleValue(file, type);

                return new PrimitiveField(typeDef.Name, type, values);
            }
        }

        public ContainerInstance ReadOBJ(ClassDefinition thisClass, RawFile file)
        {
            Console.WriteLine($"Reading class {thisClass.Name} with {thisClass.Types?.Length} types:");

            ContainerInstance instance = new ContainerInstance(thisClass.Name, thisClass.Types!.Length, thisClass.Components?.Length ?? 0);

            foreach (var typeDef in thisClass.Types)
            {
                FieldValue value;

                if (typeDef.UseClassRef)
                {
                    if (typeDef.CountVal == -1)
                    {
                        value = ReadOBJ(Classes[typeDef.Index], file);
                    }
                    else if (typeDef.CountVal == 0)
                    {
                        ushort count = file.ReadUShort();
                        value = new ContainerList(count);
                        for (int i = 0; i < count; i++)
                            ((ContainerList)value).AddContainer(ReadOBJ(Classes[typeDef.Index], file));
                    }
                    else
                        throw new Exception("Unsure what to do here!");
                }
                else
                {
                    value = ReadField(file, typeDef);
                    Console.Write($"\tProperty: {typeDef.Name} ({Types[typeDef.Index]}): ");
                    if (value is PrimitiveField pf)
                    {
                        Console.WriteLine(pf.Value);
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }


                instance.AddField(value);
            }

            if (thisClass.Components != null)
            {
                foreach (var component in thisClass.Components)
                {
                    Console.WriteLine($"\tComponent: {component.Name}");
                    instance.AddComponent(ReadOLST(file));
                }
            }

            return instance;
        }

        public override ContainerInstance ReadMOBJ(ClassDefinition thisClass, RawFile file)
        {
            uint rootMOBJBlockSize = file.ReadUInt();
            string mobjStr = file.ReadIntPascalString(false);

            uint blank = file.ReadUInt(true);

            if (thisClass.Params != null)
            {
                uint paramsBlockSize = file.ReadUInt();
                string prmStr = file.ReadIntPascalString(false);
                if (prmStr != "PRM") throw new DataMisalignedException($"Expected 'PRM' got '{prmStr}'");
                ReadOBJ(thisClass.Params, file);
            }

            if (thisClass.Types == null) return null;

            var instance = ReadOBJ(thisClass, file);

            return instance;
        }

        public override void Read(RawFile file)
        {
            uint typeListSize = file.ReadUInt();
            string typeListStr = file.ReadIntPascalString(false);
            if (typeListStr != "TypeList") throw new DataMisalignedException($"Expected 'TypeList' got '{typeListStr}'");
            
            ReadTypes(file);

            uint classListBlockSize = file.ReadUInt();
            string classListStr = file.ReadIntPascalString(false);
            if (classListStr != "ClassList") throw new DataMisalignedException($"Expected 'ClassList' got '{classListStr}'");

            ReadClasses(file);

            int rootCount = file.ReadInt();
            Objects = new ContainerList[rootCount];
            for (int i = 0; i < rootCount; i++)
            {
                Objects[i] = ReadOLST(file);
            }

        }
    }
}
