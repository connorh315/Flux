using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public class UCharValueViewModel : NumericValueViewModel
    {
        public override FieldType Type => FieldType.UChar;

        public override long MaxValue => byte.MaxValue;

        private byte byteValue;
        public byte Value 
        { 
            get => byteValue; 
            set => SetField(ref byteValue, value); 
        }

        public UCharValueViewModel(PrimitiveField model) : base(model)
        {
            Value = (byte)model.Value;
        }
    }
}
