using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class CharValueViewModel : NumericValueViewModel
    {
        public override FieldType Type => FieldType.Char;

        public override long MaxValue => byte.MaxValue;

        private byte byteValue;
        public byte Value 
        { 
            get => byteValue; 
            set => SetField(ref byteValue, value); 
        }

        public CharValueViewModel(PrimitiveField model) : base(model)
        {
            Value = (byte)model.Value;
        }
    }
}
