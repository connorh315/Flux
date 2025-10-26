using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class BlankClassObjectViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.ClassObject;

        public BlankClassObject Value;

        public BlankClassObjectViewModel(BlankClassObject model) : base(model)
        {
            Value = model;
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}
