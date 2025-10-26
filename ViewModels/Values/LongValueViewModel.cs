using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Value = model.Value != null ? (long)model.Value : 0L;
        }

        public override object GetValue() => Value;
    }
}
