using Avalonia.Controls;
using Flux.Models;
using Flux.Models.StreamContainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public abstract class StreamInfo
    {
        public abstract uint Version { get; }

        public uint WriteVersion;

        /// <summary>
        /// ClassObjects are tricky as when a new class is instantiated, if a property is a classobject, the specific classobject to instantiate is not actually told. Instead it has to be inferred when parsing.
        /// So to ensure that the ClassObject is correctly initialized when the parent memberobject is instantiated, we store a link to which class should be instantiated for the classobject.
        /// </summary>
        public Dictionary<ClassTypeDefinition, ClassDefinition> ClassObjectLinks = new();

        protected readonly Dictionary<string, FieldType> TypeStringMap = new()
        {
            ["Char"] = FieldType.Char,
            ["Short"] = FieldType.Short,
            ["Int"] = FieldType.Int,
            ["Int64"] = FieldType.Int64,
            ["Float"] = FieldType.Float,
            ["String"] = FieldType.String,
            ["Colour3"] = FieldType.Colour3,
            ["NuHSpecial"] = FieldType.NuHSpecial,
            ["Vec3"] = FieldType.Vec3,
            ["Vec4"] = FieldType.Vec4,
            ["Mtx"] = FieldType.Mtx,
            ["Ptr"] = FieldType.Ptr,
            ["ClassObject"] = FieldType.ClassObject,
            ["ClassObjectRef"] = FieldType.ClassObjectRef,
            ["Half"] = FieldType.Half,
            ["Data"] = FieldType.Data,
            [""] = FieldType.Unk,
            ["HalfVec3"] = FieldType.HalfVec3,
            ["UChar"] = FieldType.UChar,
            ["Colour4"] = FieldType.Colour4
        };

        public TypeDefinition[] Types;
        public abstract void ReadTypes(RawFile file);

        public ClassDefinition[] Classes;
        public virtual ClassDefinition GetClass(string className)
        {
            foreach (var thisClass in Classes)
            {
                if (className == thisClass.Name)
                {
                    return thisClass;
                }
            }

            return null;
        }
        public int GetClassIndex(ClassDefinition definition)
        {
            for (int i = 0; i < Classes.Length; i++)
            {
                if (Classes[i] == definition) return i;
            }
            return -1;
        }

        public MemberObject Instantiate(ClassDefinition definition)
        {
            MemberObject instance = new MemberObject(definition.Name, definition.Types!.Length, definition.Components?.Length ?? 0);
            foreach (var type in definition.Types)
            {
                if (type.UseClassRef)
                {
                    instance.AddField(Instantiate(Classes[type.Index]));
                }
                else if (Types[type.Index].Type == FieldType.ClassObject)
                {
                    if (ClassObjectLinks.TryGetValue(type, out ClassDefinition? value))
                    {
                        MemberObject classObject = Instantiate(value);
                        classObject.ClassObject = true;
                        instance.AddField(classObject);
                    }
                    else
                    {
                        BlankClassObject classObject = new BlankClassObject(); // Unknown class object.
                        instance.AddField(classObject); // TODO: Add an option for the user to select which class this is likely.
                    }
                }
                else
                {
                    instance.AddField(new PrimitiveField(type.Name, Types[type.Index].Type, null));
                }
            }
            if (instance.Components?.Length > 0)
            {
                for (int i = 0; i < definition.Components!.Length; i++)
                {
                    instance.Components[i] = new ObjectList(0);
                    instance.Components[i].Definition = Classes[definition.Components[i].ClassIndex];
                }
            }

            if (definition.Params != null)
            {
                instance.Params = Instantiate(definition.Params);
            }

            return instance;
        }

        public abstract void ReadClasses(RawFile file);

        public abstract ObjectList ReadOLST(RawFile file);

        public abstract MemberObject ReadMOBJ(ClassDefinition definition, RawFile file);

        public abstract void Read(RawFile file);

        public abstract void Write(RawFile file);

        public ObjectList[] Objects;

        protected bool LegoEditorFile = false;

        public static StreamInfo Parse(RawFile file)
        {
            uint xVersion = file.ReadUInt();
            if (xVersion != 0x17) throw new NotSupportedException($"Expected section to begin 0x17! got {xVersion}");

            string streamInfoStr = file.ReadIntPascalString(false);

            uint streamInfoVersion = file.ReadUInt();

            Console.WriteLine($"StreamInfo Version: {streamInfoVersion}");

            StreamInfo info;
            switch (streamInfoVersion)
            {
                case 0x16: // This one might have the extra "Editors" field, however I just need to figure out if that's always, or just sometimes...
                case 0x19:
                case 0x1a:
                    info = new StreamInfo_19();
                    break;
                case 0x1b:
                    info = new StreamInfo_1B();
                    break;
                case 0x1c:
                case 0x1d:
                case 0x1e:
                case 0x1f:
                    info = new StreamInfo_1E();
                    break;
                case 0x21:
                case 0x26:
                    info = new StreamInfo_21();
                    break;
                default:
                    throw new NotSupportedException($"StreamInfo version 0x{streamInfoVersion:X} not supported!");
            }

            info.WriteVersion = streamInfoVersion;

            if (Path.GetExtension(file.FileLocation).ToLower() == ".led" && streamInfoVersion >= 0x1c)
            {
                file.ReadByte(); // a 1, idk why
                info.LegoEditorFile = true;
            }

            info.Read(file);

            return info;
        }
    }
}
