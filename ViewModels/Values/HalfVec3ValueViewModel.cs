using Flux.Models;
using Flux.NuTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class HalfVec3ValueViewModel : Vec3ValueViewModel
    {
        public override FieldType Type => FieldType.HalfVec3;

        public HalfVec3ValueViewModel(PrimitiveField model) : base(model)
        {
            if (Value is HalfVec3 _) return;

            Value = new HalfVec3();
        }

        public override object GetValue()
        {
            return (HalfVec3)Value;
        }
    }
}
