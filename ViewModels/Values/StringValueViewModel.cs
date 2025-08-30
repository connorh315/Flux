using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
