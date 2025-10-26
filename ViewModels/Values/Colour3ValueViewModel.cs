using Flux.Models;
using Flux.NuTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class Colour3ValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Colour3;

        private Colour3 _value = new Colour3();
        public Colour3 Value { get => _value; set => SetField(ref _value, value); }

        public Colour3ValueViewModel(PrimitiveField model) : base(model)
        {
            Value = model.Value != null ? (Colour3)model.Value : new Colour3();
        }

        public override object GetValue() => Value;
    }
}
