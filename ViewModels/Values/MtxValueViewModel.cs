using Flux.Models;
using Flux.NuTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class MtxValueViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.Mtx;

        private Mtx _value;
        public Mtx Value { get => _value; set => SetField(ref _value, value); }

        public MtxValueViewModel(PrimitiveField model) : base(model)
        {
            Value = model.Value != null ? (Mtx)model.Value : new Mtx();
        }

        public override object GetValue() => Value;
    }
}
