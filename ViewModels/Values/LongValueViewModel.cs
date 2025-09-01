using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public class LongValueViewModel : NumericValueViewModel
    {
        public override FieldType Type => FieldType.Int64;

        public override long MaxValue => long.MaxValue;

        private long longValue;
        public long Value { get => longValue; set => SetField(ref longValue, value); }
        public LongValueViewModel(PrimitiveField model) : base(model)
        {
            Value = (long)model.Value;
        }
    }
}
