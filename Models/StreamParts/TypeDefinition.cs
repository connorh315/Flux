using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models.StreamParts
{
    public class TypeDefinition
    {
        public string Name;
        public FieldType Type;
        public uint Size;

        public TypeDefinition(string name, FieldType type, uint size)
        {
            Name = name;
            Type = type;
            Size = size;
        }
    }
}
