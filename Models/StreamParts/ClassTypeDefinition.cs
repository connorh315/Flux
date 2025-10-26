using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class ClassTypeDefinition
    {
        public string Name;

        public uint Index;

        public bool UseClassRef;

        public bool ReadAsBlock;

        public uint PackedBytes1; // Since I don't know what all these values do yet, this is used when re-serializing the data
        public uint PackedBytes2;
        public uint PackedBytes3;
        public uint PackedBytes4;

        public int CountVal;
    }
}
