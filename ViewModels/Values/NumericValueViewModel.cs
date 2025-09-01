using Flux.Models;

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
