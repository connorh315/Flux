using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Value = model.Value != null ? (byte)model.Value : (byte)0;
        }

        public override object GetValue() => Value;
    }
}
