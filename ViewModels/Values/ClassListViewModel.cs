using Flux.Models;
using System.Collections.ObjectModel;

namespace Flux.ViewModels.Values
{
    public class ClassListViewModel(ContainerList model)
    {
        public ObservableCollection<ClassViewModel> Instances { get; set; } = [];

        public string Name { get => $"{(Instances.Count > 0 ? Instances[0].Name : "Empty")}"; }

        public ContainerList Model { get; set; } = model;
    }
}