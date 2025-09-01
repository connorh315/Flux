using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System;

namespace Flux.ViewModels.Values
{
    public class Colour4ValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Colour4;

        private float _r;
        public float R { get => _r; set => SetField(ref _r, value); }

        private float _g;
        public float G { get => _g; set => SetField(ref _g, value); }

        private float _b;
        public float B { get => _b; set => SetField(ref _b, value); }

        private float _a;
        public float A { get => _a; set => SetField(ref _a, value); }

        public Colour4ValueViewModel(PrimitiveField model) : base(model)
        {
            (float, float, float, float) val = (ValueTuple<float, float, float, float>)model.Value;

            R = val.Item1;
            G = val.Item2;
            B = val.Item3;
            A = val.Item4;
        }
    }
}
