using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System;
using System.IO;

namespace Flux.Models.StreamContainers.StreamInfo
{
    public class StreamInfov25 : StreamInfov27
    {
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

                uint classDefBlockSize = Reader.ReadUInt32();

                string classStr = Reader.ReadSized32NullTerminatedString();
                if (classStr != "Class")
                {
                    throw new InvalidDataException($"Expected 'Class' got '{classStr}'");
                }

                string className = Reader.ReadSized32NullTerminatedString();

                Program.Logger.Debug($"Class {className} ({i}):");

                uint versionBlockSize = Reader.ReadUInt32();

                string versionStr = Reader.ReadSized32NullTerminatedString();
                if (versionStr != "Version")
                {
                    throw new InvalidDataException($"Expected 'Version' got '{versionStr}'");
                }

                float version = Reader.ReadSingle();

                uint classTypesBlockSize = Reader.ReadUInt32();

                string classTypesStr = Reader.ReadSized32NullTerminatedString();
                if (classTypesStr != "Types")
                {
                    throw new InvalidDataException($"Expected 'Types' got '{classTypesStr}'");
                }

                uint classTypeCount = Reader.ReadUInt32();

                ClassTypeDefinition[] classTypeDefinitions = new ClassTypeDefinition[classTypeCount];

                for (int j = 0; j < classTypeCount; j++)
                {
                    uint   typeIndex            = Reader.ReadUInt32();
                    string classTypeName        = Reader.ReadSized32NullTerminatedString();
                    uint   flag1                = Reader.ReadUInt32();
                    uint   flag2                = Reader.ReadUInt32();
                    uint   shouldUseAlternative = Reader.ReadUInt32();

                    // NOTE: Usually -1, but appears to be 0 if it's an list ("Bone Names" in .AS files), and then two bytes need to be read before reading the type that is the number of items.
                    // (I'd assume that if this is > 0, then it's a fixed size array)
                    int countVal = Reader.ReadInt32();

                    ClassTypeDefinition classTypeDefinition = new()
                    {
                        Name        = classTypeName,
                        CountVal    = countVal,
                        Index       = typeIndex,
                        UseClassRef = (shouldUseAlternative & 0xc0000000) != 0,
                        ReadAsBlock = (shouldUseAlternative & 0x00900000) == 0x00900000,
                    };

                    if (classTypeDefinition.UseClassRef)
                    {
                        Program.Logger.Debug($"\tProperty: {classTypeName} - Class: {Classes[typeIndex].Name}");
                    }
                    else
                    {
                        Program.Logger.Debug($"\tProperty: {classTypeName} - Type: {Types[typeIndex]}");
                    }

                    Program.Logger.Debug($"\t\tFlags: {flag1:X8} {flag2:X8} {shouldUseAlternative:X8} {countVal:X8}");

                    classTypeDefinitions[j] = classTypeDefinition;
                }

                Classes[i].Name    = className;
                Classes[i].Version = version;
                Classes[i].Types   = classTypeDefinitions;

                while (Stream.Position - startOfClass < classDefBlockSize)
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

                if (Stream.Position - startOfClass != classDefBlockSize)
                {
                    throw new DataMisalignedException("Failed at reading class definition. Refusing to go further.");
                }
            }
        }
    }
}