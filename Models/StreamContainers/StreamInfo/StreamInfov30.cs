using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System;
using System.IO;

namespace Flux.Models.StreamContainers.StreamInfo
{
    public class StreamInfov30 : StreamInfoBase
    {
        public override void ReadTypes()
        {
            uint typeListCount = Reader.ReadUInt32();

            Types = new FieldType[typeListCount];

            for (int i = 0; i < typeListCount; i++)
            {
                uint   typeDefinitionSize   = Reader.ReadUInt32();
                string typeDefinitionString = Reader.ReadSized32NullTerminatedString();
                if (typeDefinitionString != "Type")
                {
                    throw new InvalidDataException($"Expected 'Type' got '{typeDefinitionString}'");
                }

                string typeName = Reader.ReadSized32NullTerminatedString();
                uint   typeSize = Reader.ReadUInt32(); // NOTE: Number of bytes this type uses.

                if (!TypeStringMap.TryGetValue(typeName, out FieldType fieldType))
                {
                    throw new NotSupportedException($"Type '{typeName}' not supported!");
                }

                Types[i] = fieldType;
            }
        }

        public override void ReadClasses()
        {
            uint classCount = Reader.ReadUInt32();

            Classes = new ClassDefinition[classCount];

            for (int i = 0; i < classCount; i++)
            {
                Classes[i] = new ClassDefinition();
            }

            for (int i = 0; i < classCount; i++)
            {
                long startOfClass = Stream.Position;

                uint classDefinitionBlockSize = Reader.ReadUInt32();

                string classString = Reader.ReadSized32NullTerminatedString();
                if (classString != "Class")
                {
                    throw new InvalidDataException($"Expected 'Class' got '{classString}'");
                }

                string className = Reader.ReadSized32NullTerminatedString();

                Program.Logger.Debug($"Class {className} ({i}):");

                uint versionBlockSize = Reader.ReadUInt32();

                string versionString = Reader.ReadSized32NullTerminatedString();
                if (versionString != "Version")
                {
                    throw new InvalidDataException($"Expected 'Version' got '{versionString}'");
                }

                float version = Reader.ReadSingle();

                uint classTypesBlockSize = Reader.ReadUInt32();

                string classTypesString = Reader.ReadSized32NullTerminatedString();
                if (classTypesString != "Types")
                {
                    throw new InvalidDataException($"Expected 'Types' got '{classTypesString}'");
                }

                uint classTypeCount = Reader.ReadUInt32();

                ClassTypeDefinition[] classTypeDefinitions = new ClassTypeDefinition[classTypeCount];

                for (int j = 0; j < classTypeCount; j++)
                {
                    uint typeIndex = Reader.ReadUInt32();

                    string classTypeName = Reader.ReadSized32NullTerminatedString();

                    uint flag1                = Reader.ReadUInt32(); // NOTE: I think this is actually an array of byte flags regardless though I don't really use any of them...
                    uint flag2                = Reader.ReadUInt32();
                    uint shouldUseAlternative = Reader.ReadUInt32();
                    uint flag4                = Reader.ReadUInt32();

                    // NOTE: Usually -1, but appears to be 0 if it's an list ("Bone Names" in .AS files), and then two bytes need to be read before reading the type that is the number of items.
                    // (I'd assume that if this is > 0, then it's a fixed size array)
                    int countVal = Reader.ReadInt32();
                    if (countVal > 0)
                    {
                        Console.WriteLine();
                    }

                    ClassTypeDefinition definition = new()
                    {
                        Name        = classTypeName,
                        CountVal    = countVal,
                        Index       = typeIndex,
                        UseClassRef = (shouldUseAlternative & 0xc0000000) != 0,
                        ReadAsBlock = (shouldUseAlternative & 0x00900000) == 0x00900000,
                    };

                    if (definition.UseClassRef)
                    {
                        Program.Logger.Debug($"\tProperty: {classTypeName} - Class: {Classes[typeIndex].Name}");
                    }
                    else
                    {
                        Program.Logger.Debug($"\tProperty: {classTypeName} - Type: {Types[typeIndex]}");
                    }

                    /*
                    if ((shouldUseAlternative & 0xc0000000) != 0) // NOTE: Should use class list, not type list.
                    {
                        classTypeDefinitions[j].Reference = Classes[typeIndex];
                        Program.Logger.Debug($"\tClass: {classTypeName} - Class: {Classes[typeIndex].Name}");
                    }
                    else
                    {
                        classTypeDefinitions[j].Type = Types[typeIndex];
                        Program.Logger.Debug($"\tType: {classTypeName} - Type: {Types[typeIndex]}");
                    }
                    */

                    Program.Logger.Debug($"\t\tFlags: {flag1:X8} {flag2:X8} {shouldUseAlternative:X8} {flag4:X8} {countVal:X8}");

                    classTypeDefinitions[j] = definition;
                }

                Classes[i].Name    = className;
                Classes[i].Version = version;
                Classes[i].Types   = classTypeDefinitions;

                while (Stream.Position - startOfClass < classDefinitionBlockSize)
                {
                    uint extraPartBlockSize = Reader.ReadUInt32();

                    string extraPartTitle = Reader.ReadSized32NullTerminatedString();
                    if (extraPartTitle == "Components")
                    {
                        uint componentsCount = Reader.ReadUInt32(); // NOTE: Number of components in this class.

                        ComponentDefinition[] componentDefinitions = new ComponentDefinition[componentsCount];

                        for (int j = 0; j < componentsCount; j++)
                        {
                            uint   componentDefinitionSize = Reader.ReadUInt32(); // NOTE: Size of the component definition block.
                            string componentLabel          = Reader.ReadSized32NullTerminatedString(); // "Component"
                            string componentName           = Reader.ReadSized32NullTerminatedString(); // e.g. "Animation sets", "Texture"
                            uint   componentClassIndex     = Reader.ReadUInt32();

                            componentDefinitions[j] = new ComponentDefinition
                            {
                                Name       = componentName,
                                ClassIndex = componentClassIndex
                            };

                            Program.Logger.Debug($"\tComponent: {componentName} ({componentClassIndex})");
                        }

                        Classes[i].Components = componentDefinitions;
                    }
                    else if (extraPartTitle == "Params")
                    {
                        uint paramsOffset = Reader.ReadUInt32();

                        Classes[i].Params = Classes[paramsOffset];

                        Program.Logger.Debug($"\tParams: {Classes[paramsOffset].Name}");
                    }
                }

                if (Stream.Position - startOfClass != classDefinitionBlockSize)
                {
                    throw new DataMisalignedException("Failed at reading class definition. Refusing to go further.");
                }
            }
        }

        public override ContainerList ReadOLST()
        {
            uint   rootBlockSize = Reader.ReadUInt32();
            string olstStr       = Reader.ReadSized32NullTerminatedString();
            ushort classIndex    = Reader.ReadUInt16();
            int    mobjCount     = Reader.ReadInt32();

            ContainerList containers = new(mobjCount);

            for (int i = 0; i < mobjCount; i++)
            {
                ClassDefinition thisClass = Classes[classIndex];

                containers.AddContainer(ReadMOBJ(thisClass));
            }

            return containers;
        }

        private static object ReadSingleValue(FieldType type)
        {
            return type switch
            {
                FieldType.String   => Reader.ReadSized32NullTerminatedString(),
                FieldType.Char     => Reader.ReadSByte(),
                FieldType.UChar    => Reader.ReadByte(),
                FieldType.Short    => Reader.ReadInt16(),
                FieldType.Int      => Reader.ReadInt32(),
                FieldType.Int64    => Reader.ReadInt64(),
                FieldType.Float    => Reader.ReadSingle(),
                FieldType.Colour3  => (Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle()),
                FieldType.Vec3     => (Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle()),
                FieldType.Vec4     => (Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle()),
                FieldType.Mtx      => (
                    (Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle()),
                    (Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle()),
                    (Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle()),
                    (Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle())
                ),
                FieldType.Half     => (float)(Half)Reader.ReadUInt16(),
                FieldType.HalfVec3 => ((float)(Half)Reader.ReadUInt16(), (float)(Half)Reader.ReadUInt16(), (float)(Half)Reader.ReadUInt16()),
            
                _ => throw new NotSupportedException($"Unimplemented type {type}"),
            };
        }

        public FieldValue ReadField(ClassTypeDefinition typeDef)
        {
            int count;

            if (typeDef.CountVal == -1)
            {
                count = 1;
            }
            else if (typeDef.CountVal == 0)
            {
                count = Reader.ReadUInt16(); // Two-byte count.
            }
            else
            {
                count = Reader.ReadUInt16(); // TODO: Not sure what the value in the class type definition is for since it defines it anyway... (lp5_ghostbusters_b_tech_flow.led)
                //count = typeDef.CountVal; // Fixed count.
            }

            if (typeDef.ReadAsBlock)
            {
                uint blockSize = Reader.ReadUInt32();

                Stream.Seek(blockSize - 4, SeekOrigin.Current); // TODO: Skip for now.

                return new PrimitiveField(typeDef.Name, FieldType.Data, $"<Data Block of {blockSize} bytes>");
            }

            FieldType type = Types[typeDef.Index];
            if (type == FieldType.ClassObject) // NOTE: Let's just hope that there can't be multiple of these.
            {
                byte classObjectExists = Reader.ReadByte();
                if (classObjectExists == 0) 
                { 
                    return new ContainerInstance("null", 0, 0); 
                }

                ClassDefinition def = GetClass(Reader.ReadSized32NullTerminatedString());

                Program.Logger.Debug($"Instantiating {def.Name}");

                return ReadMOBJ(def);
            }

            if (count == 1)
            {
                return new PrimitiveField(typeDef.Name, type, ReadSingleValue(type));
            }
            else
            {
                object[] values = new object[count];

                for (int i = 0; i < count; i++)
                {
                    values[i] = ReadSingleValue(type);
                }

                return new PrimitiveField(typeDef.Name, type, values);
            }
        }

        public ContainerInstance ReadOBJ(ClassDefinition thisClass)
        {
            Program.Logger.Debug($"Reading class {thisClass.Name} with {thisClass.Types?.Length} types:");

            ContainerInstance instance = new(thisClass.Name, thisClass.Types!.Length, thisClass.Components?.Length ?? 0);

            foreach (var typeDef in thisClass.Types)
            {
                FieldValue value;

                if (typeDef.UseClassRef)
                {
                    if (typeDef.CountVal == -1)
                    {
                        value = ReadOBJ(Classes[typeDef.Index]);
                    }
                    else if (typeDef.CountVal == 0)
                    {
                        ushort count = Reader.ReadUInt16();

                        value = new ContainerList(count);

                        for (int i = 0; i < count; i++)
                        {
                            ((ContainerList)value).AddContainer(ReadOBJ(Classes[typeDef.Index]));
                        }
                    }
                    else
                    {
                        throw new Exception("Unsure what to do here!");
                    }
                }
                else
                {
                    value = ReadField(typeDef);
                    if (value is PrimitiveField pf)
                    {
                        Program.Logger.Debug($"\tOffset: 0x{Stream.Position:X} Property: {typeDef.Name} ({Types[typeDef.Index]}): '{pf.Value}'");
                    }
                    else
                    {
                        Program.Logger.Debug($"\tOffset: 0x{Stream.Position:X} Property: {typeDef.Name} ({Types[typeDef.Index]})");
                    }
                }

                instance.AddField(value);
            }

            if (thisClass.Components != null)
            {
                foreach (var component in thisClass.Components)
                {
                    Program.Logger.Debug($"\tComponent: {component.Name}");

                    instance.AddComponent(ReadOLST());
                }
            }

            return instance;
        }

        public override ContainerInstance ReadMOBJ(ClassDefinition thisClass)
        {
            uint   rootMOBJBlockSize = Reader.ReadUInt32();
            string mobjStr           = Reader.ReadSized32NullTerminatedString();
            uint   blank             = Reader.ReadUInt32();

            if (thisClass.Params != null)
            {
                uint paramsBlockSize = Reader.ReadUInt32();

                string prmStr = Reader.ReadSized32NullTerminatedString();
                if (prmStr != "PRM")
                {
                    throw new InvalidDataException($"Expected 'PRM' got '{prmStr}'");
                }

                ReadOBJ(thisClass.Params);
            }

            if (thisClass.Types == null)
            {
                return null;
            }

            ContainerInstance instance = ReadOBJ(thisClass);

            return instance;
        }

        public override void Read()
        {
            uint   typeListSize   = Reader.ReadUInt32();
            string typeListString = Reader.ReadSized32NullTerminatedString();
            if (typeListString != "TypeList")
            {
                throw new InvalidDataException($"Expected 'TypeList' got '{typeListString}'");
            }
            
            ReadTypes();

            uint   classListSize   = Reader.ReadUInt32();
            string classListString = Reader.ReadSized32NullTerminatedString();
            if (classListString != "ClassList")
            {
                throw new InvalidDataException($"Expected 'ClassList' got '{classListString}'");
            }

            ReadClasses();

            int rootCount = Reader.ReadInt32();

            Objects = new ContainerList[rootCount];

            for (int i = 0; i < rootCount; i++)
            {
                Objects[i] = ReadOLST();
            }
        }
    }
}