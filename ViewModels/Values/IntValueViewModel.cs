using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public class IntValueViewModel : NumericValueViewModel
    {
        public override FieldType Type => FieldType.Int;

        public override long MaxValue => int.MaxValue;

        public int intValue;
        public int Value { get => intValue; set => SetField(ref intValue, value); }

        public IntValueViewModel(PrimitiveField model) : base(model)
        {
            Value = (int)model.Value;
        }
    }
}
