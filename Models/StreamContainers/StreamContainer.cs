using Flux.Models.StreamParts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamContainers
{
    public class StreamContainer
    {
        public virtual string Name { get; }

        public StreamInfo Stream { get; set; }

        protected virtual void Read(RawFile file)
        {
            Stream = StreamInfo.Parse(file);
        }

        public static StreamContainer Parse(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();

            using (RawFile file = new RawFile(filePath))
            {
                StreamContainer container;
                file.Seek(4, SeekOrigin.Begin);
                string determinant = file.ReadString(12);
                if (determinant == ".CC4HSERHSER")
                {
                    container = new ResourceStreamContainer();
                }
                else
                {
                    container = new StreamContainer();
                }

                container.Read(file);

                return container;
            }
        }
    }
}
