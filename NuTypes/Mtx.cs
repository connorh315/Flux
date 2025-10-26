using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.NuTypes
{
    public class Mtx
    {
        public Vec4 Row0 { get; set; }
        public Vec4 Row1 { get; set; }
        public Vec4 Row2 { get; set; }
        public Vec4 Row3 { get; set; }

        public Mtx()
        {
            Row0 = new Vec4();
            Row1 = new Vec4();
            Row2 = new Vec4();
            Row3 = new Vec4();
        }

        public Mtx(Vec4 row0, Vec4 row1, Vec4 row2, Vec4 row3)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }

        public void Write(RawFile file)
        {
            Row0.Write(file);
            Row1.Write(file);
            Row2.Write(file);
            Row3.Write(file);
        }
    }
}
