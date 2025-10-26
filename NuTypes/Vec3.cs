using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.NuTypes
{
    public class Vec3
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Vec3() { }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public virtual void Write(RawFile file)
        {
            file.WriteFloat(X);
            file.WriteFloat(Y);
            file.WriteFloat(Z);
        }
    }
}
