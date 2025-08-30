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
        protected override void Read(RawFile file)
        {
            uint reshSize = file.ReadUInt(true);
            file.Seek(reshSize + 4, SeekOrigin.Begin);
            base.Read(file);
        }
    }
}
