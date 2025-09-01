using Flux.Models.StreamContainers.StreamInfo;
using System;
using System.IO;

namespace Flux.Models.StreamContainers
{
    public class StreamInfoContainer
    {
        public static FileFormat Format { get; private set; }

        public static MemoryStream Stream { get; private set; }

        public StreamInfoBase StreamInfoBase { get; set; }

        protected virtual void Read()
        {
            StreamInfoBase = StreamInfoBase.Deserialize(Stream, Format);
        }

        public static StreamInfoContainer Deserialize(string filePath)
        {
            StreamInfoContainer container;

            Stream = new(File.ReadAllBytes(filePath));

            using BinaryReader reader = new(Stream);

            string fileExtension = Path.GetExtension(filePath).ToLower();
            switch (fileExtension)
            {
                case ".cd":
                    Format = FileFormat.CharacterDefinition;
                    container = new ResourceStreamInfoContainer();
                    break;

                case ".as":
                    Format = FileFormat.AnimationSet;
                    container = new ResourceStreamInfoContainer();
                    break;

                case ".cpj":
                    Format = FileFormat.CharacterProject;
                    container = new StreamInfoContainer();
                    break;

                // NOTE: Stupid file can be either...
                case ".led":
                    Format = FileFormat.LevelEditor;

                    Stream.Seek(4, SeekOrigin.Begin);

                    if (reader.ReadSizedString(12) == ".CC4HSERHSER")
                    {
                        container = new ResourceStreamInfoContainer();
                    }
                    else
                    {
                        container = new StreamInfoContainer();
                    }

                    Stream.Seek(0, SeekOrigin.Begin);
                    break;

                default:
                    Format = FileFormat.Unknown;

                    Program.Logger.Debug($"Unknown file extension {fileExtension}. Defaulting to StreamContainer.");

                    container = new StreamInfoContainer();
                    break;
            }

            container.Read();

            return container;
        }

        public static void Dispose()
        {
            StreamInfoBase.Dispose();
            Stream.Dispose();
        }
    }
}
