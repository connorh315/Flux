using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.NuTypes
{
    public class HalfVec3 : Vec3
    {
        public HalfVec3(Half x, Half y, Half z)
            : base((float)x, (float)y, (float)z)
        {

        }

        public HalfVec3() : base() { }

        public override void Write(RawFile file)
        {
            file.WriteHalf((Half)X);
            file.WriteHalf((Half)Y);
            file.WriteHalf((Half)Z);
        }
    }
}
