using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class StreamInfo_1B : StreamInfo_1E
    {
        public override MemberObject ReadMOBJ(ClassDefinition thisClass, RawFile file)
        {
            uint rootMOBJBlockSize = file.ReadUInt();
            string mobjStr = file.ReadIntPascalString(false);

            ushort blank = file.ReadUShort();
            byte blank2 = file.ReadByte();

            if (thisClass.Params != null)
            {
                uint paramsBlockSize = file.ReadUInt();
                string prmStr = file.ReadIntPascalString(false);
                if (prmStr != "PRM") throw new DataMisalignedException($"Expected 'PRM' got '{prmStr}'");
                ReadOBJ(thisClass.Params, file);
            }

            if (thisClass.Types == null) return null;

            var instance = ReadOBJ(thisClass, file);

            return instance;
        }
    }
}
