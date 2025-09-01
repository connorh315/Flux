using Flux.Models;
using Flux.Models.StreamContainers.StreamInfo.Definitions;
using System.Collections.ObjectModel;

namespace Flux.ViewModels.Values
{
    public class ClassViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.ClassObject;

        public ObservableCollection<ValueViewModel> Properties { get; set; } = [];

        public ObservableCollection<ClassListViewModel> Components = [];

        public ObservableCollection<object> Children { get; } = new();

        public ClassViewModel(ContainerInstance model) : base(model)
        {
            Properties.CollectionChanged += (s, e) => Sync();
            Components.CollectionChanged += (s, e) => Sync();
        }

        private void Sync()
        {
            Children.Clear();

            foreach (var p in Properties) 
            {
                if (p is ClassViewModel) 
                { 
                    Children.Add(p); 
                }
            }

            foreach (var c in Components)
            {
                Children.Add(c);
            }
        }
    }
}
