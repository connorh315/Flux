using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System.IO;

namespace Flux.Models.StreamContainers.StreamInfo
{
    public class StreamInfov27 : StreamInfov30
    {
        public override ContainerInstance ReadMOBJ(ClassDefinition thisClass)
        {
            uint   rootMOBJBlockSize = Reader.ReadUInt32();
            string mobjStr           = Reader.ReadSized32NullTerminatedString();
            ushort blank             = Reader.ReadUInt16();
            byte   blank2            = Reader.ReadByte();

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

            return ReadOBJ(thisClass);
        }
    }
}
