using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamContainers
{
    public class ResourceStreamContainer : StreamContainer
    {
        byte[] resourceHeader;

        protected override void Read(RawFile file)
        {
            uint reshSize = file.ReadUInt(true);
            resourceHeader = new byte[reshSize];
            file.ReadInto(resourceHeader, (int)reshSize);
            base.Read(file);
        }

        public override void Write(RawFile file)
        {
            file.WriteInt(resourceHeader.Length, true);
            file.fileStream.Write(resourceHeader, 0, resourceHeader.Length);

            base.Write(file);
        }
    }
}
