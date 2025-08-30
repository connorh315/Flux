using Flux.Models;
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

        public FieldType[] Types;
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
        public abstract void ReadClasses(RawFile file);

        public abstract ContainerList ReadOLST(RawFile file);

        public abstract ContainerInstance ReadMOBJ(ClassDefinition definition, RawFile file);

        public abstract void Read(RawFile file);

        public ContainerList[] Objects;

        public static StreamInfo Parse(RawFile file)
        {
            uint xVersion = file.ReadUInt();
            if (xVersion != 0x17) throw new NotSupportedException($"Expected section to begin 0x17! got {xVersion}");

            string streamInfoStr = file.ReadIntPascalString(false);

            uint streamInfoVersion = file.ReadUInt();

            StreamInfo info;
            switch (streamInfoVersion)
            {
                case 0x16: // This one might have the extra "Editors" field, however I just need to figure out if that's always, or just sometimes...
                case 0x19:
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
                    info = new StreamInfo_21();
                    break;
                default:
                    throw new NotSupportedException($"StreamInfo version 0x{streamInfoVersion:X} not supported!");
            }

            if (Path.GetExtension(file.FileLocation).ToLower() == ".led" && streamInfoVersion >= 0x1c)
            {
                file.ReadByte(); // a 1, idk why
            }

            info.Read(file);

            return info;
        }
    }
}
