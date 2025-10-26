using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class ShortValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Int;

        public short shortValue;
        public short Value { get => shortValue; set => SetField(ref shortValue, value); }

        public ShortValueViewModel(PrimitiveField model) : base(model)
        {
            Value = model.Value != null ? (short)model.Value : (short)0;
        }

        public override object GetValue() => Value;
    }
}
