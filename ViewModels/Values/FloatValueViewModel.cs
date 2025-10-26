using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class FloatValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Float;

        private float floatValue;
        public float Value { get => floatValue; set => SetField(ref floatValue, value); }

        public FloatValueViewModel(PrimitiveField model) : base(model)
        {
            if (model.Value is Half half)
            {
                Value = (float)half;
            }
            else
            {
                Value = model.Value != null ? (float)model.Value : 0f;
            }
        }

        public override object GetValue() => Value;
    }
}
