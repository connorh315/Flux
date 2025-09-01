using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public class CharValueViewModel : NumericValueViewModel
    {
        public override FieldType Type => FieldType.Char;

        public override long MaxValue => sbyte.MaxValue;

        private sbyte byteValue;
        public sbyte Value 
        { 
            get => byteValue; 
            set => SetField(ref byteValue, value); 
        }

        public CharValueViewModel(PrimitiveField model) : base(model)
        {
            Value = (sbyte)model.Value;
        }
    }
}
