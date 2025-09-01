using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System.IO;

namespace Flux.Models.StreamContainers.StreamInfo
{
    public class StreamInfov33 : StreamInfov30
    {
        public override ContainerList ReadOLST()
        {
            uint rootBlockSize = Reader.ReadUInt32();

            string olstStr = Reader.ReadSized32NullTerminatedString();
            if (olstStr != "OLST")
            {
                throw new InvalidDataException($"Expected 'OLST' got {olstStr}");
            }

            int mobjCount = Reader.ReadInt32();

            ContainerList containers = new(mobjCount);

            for (int i = 0; i < mobjCount; i++)
            {
                ushort classIndex = Reader.ReadUInt16();

                /*
                uint rootMOBJBlockSize = Reader.ReadUInt32();

                string mobjStr = Reader.ReadSized32NullTerminatedString();
                if (mobjStr != "MOBJ")
                {
                    throw new InvalidDataException($"Expected 'MOBJ' got '{mobjStr}'");
                }

                uint blank = Reader.ReadUInt32();
                */

                ClassDefinition thisClass = Classes[classIndex];

                containers.AddContainer(ReadMOBJ(thisClass));
            }

            return containers;
        }
    }
}
