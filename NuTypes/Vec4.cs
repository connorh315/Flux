using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.NuTypes
{
    public class Vec4 : Vec3
    {
        public float W { get; set; }

        public Vec4() { }

        public Vec4(float x, float y, float z, float w) : base(x, y, z)
        {
            W = w;
        }

        public override void Write(RawFile file)
        {
            base.Write(file);
            file.WriteFloat(W);
        }
    }
}
