using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class StreamInfo_21 : StreamInfo_1E
    {
        public override ContainerList ReadOLST(RawFile file)
        {
            uint rootBlockSize = file.ReadUInt();

            string olstStr = file.ReadIntPascalString(false);
            if (olstStr != "OLST") throw new DataMisalignedException($"Expected 'OLST' got {olstStr}");

            int mobjCount = file.ReadInt();
            ContainerList containers = new ContainerList(mobjCount);

            for (int i = 0; i < mobjCount; i++)
            {
                ushort classIndex = file.ReadUShort();
                
                //uint rootMOBJBlockSize = file.ReadUInt();

                //string mobjStr = file.ReadIntPascalString(false);
                //if (mobjStr != "MOBJ") throw new DataMisalignedException($"Expected 'MOBJ' got '{mobjStr}'");

                //uint blank = file.ReadUInt(true);

                var thisClass = Classes[classIndex];

                containers.AddContainer(ReadMOBJ(thisClass, file));
            }

            return containers;
        }
    }
}
