using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models
{
    public class FieldArray : FieldValue
    {
        public FieldType Type { get; set; }

        public List<object> Values { get; set; } = new();

        public FieldArray(string name, FieldType type, int count) : base(name)
        {
            Type = type;
        }

        public FieldArray(string name, int count) : base(name)
        {
        }
    }
}
