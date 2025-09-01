using System.IO;

namespace Flux.Models.StreamContainers
{
    public class ResourceStreamInfoContainer : StreamInfoContainer
    {
        protected override void Read()
        {
            using BinaryReader reader = new(Stream);

            // NOTE: Read and Swap the Resource Header.
            // TODO: Deserialize it.
            uint resourceHeaderSize = reader.ReadUInt32BigEndian();

            Stream.Seek(resourceHeaderSize + 4, SeekOrigin.Begin);

            base.Read();
        }
    }
}