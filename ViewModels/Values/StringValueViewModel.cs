using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public class StringValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.String;

        public string Value { get; set; }

        public StringValueViewModel(PrimitiveField model) : base(model)
        {
            Value = model.Value as string;
        }
    }
}
