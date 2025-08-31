using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public abstract class NumericValueViewModel : ValueViewModel
    {
        public abstract long MaxValue { get; }

        public NumericValueViewModel(PrimitiveField model) : base(model)
        {
        }
    }
}
