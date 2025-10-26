using Flux.Models;
using Flux.NuTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class Vec4ValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Vec4;

        private Vec4 _value;
        public Vec4 Value { get => _value; set => SetField(ref _value, value); }

        public Vec4ValueViewModel(PrimitiveField model) : base(model)
        {
            Value = model.Value != null ? (Vec4)model.Value : new Vec4();
        }

        public override object GetValue() => Value;
    }
}
