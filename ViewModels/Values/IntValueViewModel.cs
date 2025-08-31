using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
