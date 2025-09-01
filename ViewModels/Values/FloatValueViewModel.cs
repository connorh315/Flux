using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public class FloatValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Float;

        private float floatValue;
        public float Value { get => floatValue; set => SetField(ref floatValue, value); }

        public FloatValueViewModel(PrimitiveField model) : base(model)
        {
            Value = (float)model.Value;
        }
    }
}
