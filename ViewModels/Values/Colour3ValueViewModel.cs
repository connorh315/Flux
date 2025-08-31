using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class Colour3ValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Float;

        private float floatValue1;
        public float Value1 { get => floatValue1; set => SetField(ref floatValue1, value); }

        private float floatValue2;
        public float Value2 { get => floatValue2; set => SetField(ref floatValue2, value); }

        private float floatValue3;
        public float Value3 { get => floatValue3; set => SetField(ref floatValue3, value); }

        public Colour3ValueViewModel(PrimitiveField model) : base(model)
        {
            (float, float, float) val = ((ValueTuple<float, float, float>)model.Value);

            Value1 = val.Item1;
            Value2 = val.Item2;
            Value3 = val.Item3;
        }
    }
}
