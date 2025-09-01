using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flux.Models.StreamContainers.StreamInfo
{
    public abstract class StreamInfoBase
    {
        public static MemoryStream Stream { get; private set; }

        public static BinaryReader Reader { get; private set; }

        protected readonly Dictionary<string, FieldType> TypeStringMap = new()
        {
            ["Char"]           = FieldType.Char,
            ["Short"]          = FieldType.Short,
            ["Int"]            = FieldType.Int,
            ["Int64"]          = FieldType.Int64,
            ["Float"]          = FieldType.Float,
            ["String"]         = FieldType.String,
            ["Colour3"]        = FieldType.Colour3,
            ["Colour4"]        = FieldType.Colour4,
            ["NuHSpecial"]     = FieldType.NuHSpecial,
            ["Vec3"]           = FieldType.Vec3,
            ["Vec4"]           = FieldType.Vec4,
            ["Mtx"]            = FieldType.Mtx,
            ["Ptr"]            = FieldType.Ptr,
            ["ClassObject"]    = FieldType.ClassObject,
            ["ClassObjectRef"] = FieldType.ClassObjectRef,
            ["Half"]           = FieldType.Half,
            ["Data"]           = FieldType.Data,
            ["HalfVec3"]       = FieldType.HalfVec3,
            ["UChar"]          = FieldType.UChar,
            ["bool"]           = FieldType.Char,
            ["NuVec"]          = FieldType.Vec3,
            ["VuVec"]          = FieldType.Vec4,
            ["NuMtx"]          = FieldType.Mtx,
            ["VuMtx"]          = FieldType.Mtx,
            [""]               = FieldType.Unk,
        };

        public FieldType[] Types;

        public abstract void ReadTypes();

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

        public abstract void ReadClasses();

        public abstract ContainerList ReadOLST();

        public abstract ContainerInstance ReadMOBJ(ClassDefinition definition);

        public abstract void Read();

        public ContainerList[] Objects;

        public static StreamInfoBase Deserialize(MemoryStream stream, FileFormat fileFormat)
        {
            Stream = stream;
            Reader = new(stream);

            uint xVersion = Reader.ReadUInt32();
            if (xVersion != 23)
            {
                throw new NotSupportedException($"Expected section to begin 23, got {xVersion}!");
            }

            string streamInfoString = Reader.ReadSized32NullTerminatedString();
            if (streamInfoString != "StreamInfo")
            {
                throw new NotSupportedException($"Expected section to be named as \"StreamInfo\", got \"{streamInfoString}\"!");
            }

            uint streamInfoVersion = Reader.ReadUInt32();
            StreamInfoBase info = streamInfoVersion switch
            {
                // This one might have the extra "Editors" field, however I just need to figure out if that's always, or just sometimes...
                22 or 25             => new StreamInfov25(),
                27                   => new StreamInfov27(),
                28 or 29 or 30 or 31 => new StreamInfov30(),
                33                   => new StreamInfov33(),

                _ => throw new NotSupportedException($"StreamInfo version {streamInfoVersion} not supported!"),
            };

            if (fileFormat == FileFormat.LevelEditor && streamInfoVersion >= 28)
            {
                Reader.ReadByte(); // NOTE: Always 1, idk why
            }

            info.Read();

            return info;
        }

        public static void Dispose()
        {
            Stream.Dispose();
            Reader.Dispose();
        }
    }
}
