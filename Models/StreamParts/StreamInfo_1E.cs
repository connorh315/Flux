using Avalonia.Controls;
using Flux.Models;
using Flux.NuTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class StreamInfo_1E : StreamInfo
    {
        public override uint Version => 0x1E;

        public override void ReadTypes(RawFile file)
        {
            uint typeListCount = file.ReadUInt();
            Types = new TypeDefinition[typeListCount];

            for (int i = 0; i < typeListCount; i++)
            {
                uint typeDefBlockSize = file.ReadUInt();
                string typeStr = file.ReadIntPascalString(false);
                if (typeStr != "Type") throw new DataMisalignedException($"Expected 'Type' got '{typeStr}'");

                string typeName = file.ReadIntPascalString(false);

                uint typeSize = file.ReadUInt(); // number of bytes this type uses

                if (!TypeStringMap.TryGetValue(typeName, out FieldType fieldType))
                {
                    continue;
                    throw new NotSupportedException($"Type '{typeName}' not supported!");
                }

                Types[i] = new TypeDefinition(typeName, fieldType, typeSize);
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

                    int countVal = file.ReadInt(); // usually -1, but appears to be 0 if it's a list ("Bone Names" in .AS files), and then two bytes need to be read before reading the type that is the number of items (I'd assume that if this is > 0, then it's a fixed size array)

                    var def = new ClassTypeDefinition
                    {
                        Name = classTypeName,
                        CountVal = countVal,
                        Index = typeIndex,
                        UseClassRef = ((shouldUseAlternative & 0xc0000000) != 0),
                        ReadAsBlock = ((shouldUseAlternative & 0x00900000) == 0x00900000),

                        PackedBytes1 = flag1,
                        PackedBytes2 = flag2,
                        PackedBytes3 = shouldUseAlternative,
                        PackedBytes4 = flag4,
                    };

                    if (def.UseClassRef)
                    {
                        Console.WriteLine($"\tProperty: {classTypeName} - Class: {Classes[typeIndex].Name}");
                    }
                    else
                    {
                        Console.WriteLine($"\tProperty: {classTypeName} - Type: {Types[typeIndex].Type}");
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

        public override ObjectList ReadOLST(RawFile file)
        {
            uint rootBlockSize = file.ReadUInt();

            string olstStr = file.ReadIntPascalString(false);

            ushort classIndex = file.ReadUShort();
            int mobjCount = file.ReadInt();
            ObjectList containers = new ObjectList(mobjCount);
            containers.Definition = Classes[classIndex]; // Important for serializing, and also for instantiating new MOBJs

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
                FieldType.Colour3 => new Colour3(file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                FieldType.Vec3 => new Vec3(file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                FieldType.Vec4 => new Vec4(file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                FieldType.Mtx => new Mtx(
                    new Vec4(file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                    new Vec4(file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                    new Vec4(file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat()),
                    new Vec4(file.ReadFloat(), file.ReadFloat(), file.ReadFloat(), file.ReadFloat())
                ),
                FieldType.Half => file.ReadHalf(),
                FieldType.HalfVec3 => new HalfVec3(file.ReadHalf(), file.ReadHalf(), file.ReadHalf()),   // TODO: OBVIOUSLY WRONG, FIX!!!

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
                int blockSize = file.ReadInt();
                byte[] block = new byte[blockSize - 4];
                file.ReadInto(block, blockSize - 4);
                return new PrimitiveField(typeDef.Name, FieldType.Data, block);
            }

            FieldType type = Types[typeDef.Index].Type;
            if (type == FieldType.ClassObject) // Let's just hope that there can't be multiple of these
            {
                byte classObjectExists = file.ReadByte();
                if (classObjectExists == 0) 
                {
                    return new BlankClassObject(); 
                }
                var def = GetClass(file.ReadIntPascalString(false)); 
                Console.WriteLine($"Instantiating {def.Name}");
                var mobj = ReadMOBJ(def, file);
                mobj.ClassObject = true; // Used when serializing
                ClassObjectLinks[typeDef] = def; // Used when instantiating (Adding)
                return mobj;
            }

            if (count == 1)
            {
                return new PrimitiveField(typeDef.Name, type, ReadSingleValue(file, type));
            }
            else
            {
                var array = new FieldArray(typeDef.Name, type, count);

                for (int i = 0; i < count; i++)
                    array.Values.Add(ReadSingleValue(file, type));

                return array;
            }
        }

        public MemberObject ReadOBJ(ClassDefinition thisClass, RawFile file)
        {
            Console.WriteLine($"Reading class {thisClass.Name} with {thisClass.Types?.Length} types:");

            MemberObject instance = new MemberObject(thisClass.Name, thisClass.Types!.Length, thisClass.Components?.Length ?? 0);

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
                        var array = new FieldArray(typeDef.Name, count);

                        for (int i = 0; i < count; i++)
                            array.Values.Add(ReadOBJ(Classes[typeDef.Index], file));

                        value = array;
                    }
                    else
                        throw new Exception("Unsure what to do here!");
                }
                else
                {
                    value = ReadField(file, typeDef);
                    Console.Write($"\tProperty: {typeDef.Name} ({Types[typeDef.Index].Type}): ");
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
                    var olst = ReadOLST(file);
                    if (olst == null || olst.Containers.Length == 0)
                    {
                        olst = new ObjectList(0)
                        {
                            Definition = Classes[component.ClassIndex] // Allows the user to add components when there aren't any in the file.
                        };
                    }
                    instance.AddComponent(olst);
                }
            }

            return instance;
        }

        public override MemberObject ReadMOBJ(ClassDefinition thisClass, RawFile file)
        {
            uint rootMOBJBlockSize = file.ReadUInt();
            string mobjStr = file.ReadIntPascalString(false);

            uint blank = file.ReadUInt(true);

            MemberObject parameters = null;

            if (thisClass.Params != null)
            {
                uint paramsBlockSize = file.ReadUInt();
                string prmStr = file.ReadIntPascalString(false);
                if (prmStr != "PRM") throw new DataMisalignedException($"Expected 'PRM' got '{prmStr}'");
                parameters = ReadOBJ(thisClass.Params, file);
            }

            if (thisClass.Types == null) return null; // TODO: Should this actually be here?

            var instance = ReadOBJ(thisClass, file);

            instance.Params = parameters;

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
            Objects = new ObjectList[rootCount];
            for (int i = 0; i < rootCount; i++)
            {
                Objects[i] = ReadOLST(file);
            }

        }

        public virtual void WriteStreamInfoHeader(RawFile file)
        {
            file.WriteInt(0x17);
            file.WriteIntPascalString("StreamInfo", false, 1);
            file.WriteUInt(WriteVersion);
        }

        public virtual void WriteTypeList(RawFile file)
        {
            using (var typeListSection = new RawFileSection(file))
            {
                file.WriteIntPascalString("TypeList", false, 1);
                file.WriteInt(Types.Length);
                for (int i = 0; i < Types.Length; i++)
                {
                    var type = Types[i];
                    using (var typeDefSection = new RawFileSection(file))
                    {
                        file.WriteIntPascalString("Type", false, 1);
                        file.WriteIntPascalString(type.Name, false, 1);
                        file.WriteUInt(type.Size);
                    }
                }
            }
        }

        public virtual void WriteClassList(RawFile file)
        {
            using (var classListSection = new RawFileSection(file))
            {
                file.WriteIntPascalString("ClassList", false, 1);
                file.WriteInt(Classes.Length);
                for (int i = 0; i < Classes.Length; i++)
                {
                    using (var classSection = new RawFileSection(file))
                    {
                        var thisClass = Classes[i];

                        file.WriteIntPascalString("Class", false, 1);
                        file.WriteIntPascalString(thisClass.Name, false, 1);

                        using (var versionSection = new RawFileSection(file))
                        {
                            file.WriteIntPascalString("Version", false, 1);
                            file.WriteFloat(thisClass.Version);
                        }

                        using (var typesSection = new RawFileSection(file))
                        {
                            file.WriteIntPascalString("Types", false, 1);
                            file.WriteInt(thisClass.Types.Length);

                            for (int j = 0; j < thisClass.Types.Length; j++)
                            {
                                var thisType = thisClass.Types[j];
                                file.WriteUInt(thisType.Index); // Reference index
                                file.WriteIntPascalString(thisType.Name, false, 1);

                                file.WriteUInt(thisType.PackedBytes1);
                                file.WriteUInt(thisType.PackedBytes2);
                                file.WriteUInt(thisType.PackedBytes3);
                                file.WriteUInt(thisType.PackedBytes4);
                                file.WriteInt(thisType.CountVal);
                            }
                        }

                        if (thisClass.Params != null)
                        {
                            using (var paramsSection = new RawFileSection(file))
                            {
                                file.WriteIntPascalString("Params", false, 1);
                                file.WriteInt(Array.IndexOf(Classes, thisClass.Params));
                            }
                        }

                        if (thisClass.Components?.Length > 0)
                        {
                            using (var componentsSection = new RawFileSection(file))
                            {
                                file.WriteIntPascalString("Components", false, 1);
                                file.WriteInt(thisClass.Components.Length);

                                for (int j = 0; j < thisClass.Components.Length; j++)
                                {
                                    using (var componentSection = new RawFileSection(file))
                                    {
                                        var thisComponent = thisClass.Components[j];
                                        file.WriteIntPascalString("Component", false, 1);
                                        file.WriteIntPascalString(thisComponent.Name, false, 1);
                                        file.WriteUInt(thisComponent.ClassIndex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual void WriteOLST(RawFile file, ObjectList obj)
        {
            using (var olstSection = new RawFileSection(file))
            {
                file.WriteIntPascalString("OLST", false, 1);
                file.WriteUShort((ushort)GetClassIndex(obj.Definition));
                file.WriteInt(obj.Containers.Length);

                foreach (var instance in obj.Containers)
                {
                    WriteMOBJ(file, instance);
                }
            }
        }

        public virtual void WriteMOBJ(RawFile file, MemberObject obj)
        {
            using (var mobjSection = new RawFileSection(file))
            {
                file.WriteIntPascalString("MOBJ", false, 1);
                file.WriteUInt(0); // blank

                WriteOBJ(file, obj);
            }
        }

        public virtual void WriteOBJ(RawFile file, MemberObject obj)
        {
            if (obj.Params != null)
            {
                using (var paramsSection = new RawFileSection(file))
                {
                    file.WriteIntPascalString("PRM", false, 1);
                    WriteOBJ(file, obj.Params);
                }
            }

            for (int i = 0; i < obj.Fields.Length; i++)
            {
                var field = obj.Fields[i];
                if (field is PrimitiveField pf)
                {
                    switch (pf.FieldType)
                    {
                        case FieldType.Char:
                            file.WriteByte((byte)pf.Value);
                            break;
                        case FieldType.UChar:
                            file.WriteByte((byte)pf.Value);
                            break;
                        case FieldType.Short:
                            file.WriteShort((short)pf.Value);
                            break;
                        case FieldType.Int:
                            file.WriteInt((int)pf.Value);
                            break;
                        case FieldType.Int64:
                            file.WriteLong((long)pf.Value);
                            break;
                        case FieldType.Half:
                            file.WriteHalf((Half)pf.Value);
                            break;
                        case FieldType.Float:
                            file.WriteFloat((float)pf.Value);
                            break;
                        case FieldType.String:
                            file.WriteIntPascalString((string)pf.Value, false, 1);
                            break;
                        case FieldType.Colour3:
                            Colour3 col = (Colour3)pf.Value;
                            col.Write(file);
                            break;
                        case FieldType.Vec3:
                            Vec3 vec3 = (Vec3)pf.Value;
                            vec3.Write(file);
                            break;
                        case FieldType.HalfVec3:
                            HalfVec3 hvec3 = (HalfVec3)pf.Value;
                            hvec3.Write(file);
                            break;
                        case FieldType.Vec4:
                            Vec4 vec4 = (Vec4)pf.Value;
                            vec4.Write(file);
                            break;
                        case FieldType.Mtx:
                            Mtx mtx = (Mtx)pf.Value;
                            mtx.Write(file);
                            break;
                        case FieldType.Data:
                            byte[] block = (byte[])pf.Value;
                            file.WriteInt(block.Length + 4);
                            file.fileStream.Write(block);
                            break;
                        default:
                            Console.WriteLine($"Writer not implemented: {pf.FieldType}");
                            break;
                    }
                }
                else if (field is MemberObject objField)
                {
                    if (objField.ClassObject)
                    {
                        file.WriteByte(1);
                        file.WriteIntPascalString(objField.Name, false, 1);
                        WriteMOBJ(file, objField);
                    }
                    else
                    {
                        WriteOBJ(file, objField);
                    }
                }
                else if (field is BlankClassObject _)
                {
                    file.WriteByte(0);
                }
            }

            if (obj.Components != null)
            {
                foreach (var olst in obj.Components)
                {
                    WriteOLST(file, olst);
                }
            }
        }

        public virtual void WriteObjects(RawFile file)
        {
            file.WriteInt(Objects.Length);
            for (int i = 0; i < Objects.Length; i++)
            {
                WriteOLST(file, Objects[i]);
            }
        }

        public override void Write(RawFile file)
        {
            WriteStreamInfoHeader(file);
            if (LegoEditorFile)
            {
                file.WriteByte(1);
            }
            WriteTypeList(file);
            WriteClassList(file);

            WriteObjects(file);
        }
    }
}
