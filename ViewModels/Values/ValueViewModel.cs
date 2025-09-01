using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.ViewModels.Values
{
    public abstract class ValueViewModel : ViewModel
    {
        public abstract FieldType Type { get; }

        public string Name { get; }

        public FieldValue Model { get; }

        protected ValueViewModel(FieldValue model)
        {
            Model = model;
            Name = $"{model.Name} ({Type})";
        }
    }
}