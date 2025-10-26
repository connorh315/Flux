using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models
{
    public class BlankClassObject : FieldValue
    {
        public BlankClassObject() : base("null")
        {
        }
    }
}
