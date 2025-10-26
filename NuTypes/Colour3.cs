using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.NuTypes
{
    public class Colour3
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }

        public Colour3() { }

        public Colour3(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public void Write(RawFile file)
        {
            file.WriteFloat(R);
            file.WriteFloat(G);
            file.WriteFloat(B);
        }
    }
}
