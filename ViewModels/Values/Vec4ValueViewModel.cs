using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System;

namespace Flux.ViewModels.Values
{
    public class Vec4ValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Vec4;

        private float _x;
        public float X { get => _x; set => SetField(ref _x, value); }

        private float _y;
        public float Y { get => _y; set => SetField(ref _y, value); }

        private float _z;
        public float Z { get => _z; set => SetField(ref _z, value); }

        private float _w;
        public float W { get => _w; set => SetField(ref _w, value); }

        public Vec4ValueViewModel(PrimitiveField model) : base(model)
        {
            (float, float, float, float) val = (ValueTuple<float, float, float, float>)model.Value;

            X = val.Item1;
            Y = val.Item2;
            Z = val.Item3;
            W = val.Item4;
        }
    }
}
