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
        }

        public override object GetValue()
        {
            return (HalfVec3)Value;
        }
    }
}
