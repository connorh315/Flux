using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class HalfValueViewModel : FloatValueViewModel
    {
        public override FieldType Type => FieldType.Half;

        public HalfValueViewModel(PrimitiveField model) : base(model)
        {
        }

        public override object GetValue()
        {
            return (Half)Convert.ToSingle(base.Value);
        }
    }
}
