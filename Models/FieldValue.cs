using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models
{
    public abstract class FieldValue
    {
        public string Name { get; set; }
        protected FieldValue(string name) => Name = name;
    }
}
