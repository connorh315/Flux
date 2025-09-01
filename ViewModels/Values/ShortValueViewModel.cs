using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public class ShortValueViewModel : NumericValueViewModel
    {
        public override FieldType Type => FieldType.Short;

        public override long MaxValue => short.MaxValue;

        public short shortValue;
        public short Value { get => shortValue; set => SetField(ref shortValue, value); }

        public ShortValueViewModel(PrimitiveField model) : base(model)
        {
            Value = (short)model.Value;
        }
    }
}
