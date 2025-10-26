using Flux.Models;
using Flux.NuTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class Vec3ValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Vec3;

        private Vec3 _value;
        public Vec3 Value { get => _value; set => SetField(ref _value, value); }

        public Vec3ValueViewModel(PrimitiveField model) : base(model)
        {
            Value = model.Value != null ? (Vec3)model.Value : new Vec3();
        }

        public override object GetValue() => Value;
    }
}
