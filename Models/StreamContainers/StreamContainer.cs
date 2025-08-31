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
                switch (ext)
                {
                    case ".cd":
                    case ".as":
                        container = new ResourceStreamContainer();
                        break;
                    case ".cpj":
                        container = new StreamContainer();
                        break;
                    case ".led": // Stupid file can be either...
                        file.Seek(4, SeekOrigin.Begin);
                        string resourceHeaderStr = file.ReadString(12);
                        if (resourceHeaderStr == ".CC4HSERHSER")
                        {
                            container = new ResourceStreamContainer();
                        }
                        else
                        {
                            container = new StreamContainer();
                        }
                        file.Seek(0, SeekOrigin.Begin);
                        break;
                    default:
                        Console.WriteLine($"Unknown file extension {ext}. Defaulting to StreamContainer.");
                        container = new StreamContainer();
                        break;
                }

                container.Read(file);

                return container;
            }
        }
    }
}
