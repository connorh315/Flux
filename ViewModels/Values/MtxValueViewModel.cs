using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System;
using System.Linq;
using System.Reflection;

namespace Flux.ViewModels.Values
{
    public class MtxValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Mtx;

        public float A1 { get => _a1; set => SetField(ref _a1, value); }
        public float B1 { get => _b1; set => SetField(ref _b1, value); }
        public float C1 { get => _c1; set => SetField(ref _c1, value); }
        public float D1 { get => _d1; set => SetField(ref _d1, value); }

        public float A2 { get => _a2; set => SetField(ref _a2, value); }
        public float B2 { get => _b2; set => SetField(ref _b2, value); }
        public float C2 { get => _c2; set => SetField(ref _c2, value); }
        public float D2 { get => _d2; set => SetField(ref _d2, value); }

        public float A3 { get => _a3; set => SetField(ref _a3, value); }
        public float B3 { get => _b3; set => SetField(ref _b3, value); }
        public float C3 { get => _c3; set => SetField(ref _c3, value); }
        public float D3 { get => _d3; set => SetField(ref _d3, value); }

        public float A4 { get => _a4; set => SetField(ref _a4, value); }
        public float B4 { get => _b4; set => SetField(ref _b4, value); }
        public float C4 { get => _c4; set => SetField(ref _c4, value); }
        public float D4 { get => _d4; set => SetField(ref _d4, value); }

        private float _a1, _b1, _c1, _d1;
        private float _a2, _b2, _c2, _d2;
        private float _a3, _b3, _c3, _d3;
        private float _a4, _b4, _c4, _d4;
        private static readonly string[] sourceArray = [
                                "A1","B1","C1","D1",
                                "A2","B2","C2","D2",
                                "A3","B3","C3","D3",
                                "A4","B4","C4","D4"
                            ];

        public MtxValueViewModel(PrimitiveField model) : base(model)
        {
            var tuple4x4 = ((ValueTuple<float, float, float, float>,
                             ValueTuple<float, float, float, float>,
                             ValueTuple<float, float, float, float>,
                             ValueTuple<float, float, float, float>))model.Value;

            float[] values =
            [
                tuple4x4.Item1.Item1, tuple4x4.Item1.Item2, tuple4x4.Item1.Item3, tuple4x4.Item1.Item4,
                tuple4x4.Item2.Item1, tuple4x4.Item2.Item2, tuple4x4.Item2.Item3, tuple4x4.Item2.Item4,
                tuple4x4.Item3.Item1, tuple4x4.Item3.Item2, tuple4x4.Item3.Item3, tuple4x4.Item3.Item4,
                tuple4x4.Item4.Item1, tuple4x4.Item4.Item2, tuple4x4.Item4.Item3, tuple4x4.Item4.Item4,
            ];

            var props = typeof(MtxValueViewModel)
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanWrite && sourceArray.Contains(p.Name))
                            .OrderBy(p => p.Name)
                            .ToArray();

            for (int i = 0; i < values.Length; i++)
                props[i].SetValue(this, values[i]);
        }
    }
}