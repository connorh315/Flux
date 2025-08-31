using Flux.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class ClassListViewModel
    {
        public ObservableCollection<ClassViewModel> Instances { get; set; } = new ObservableCollection<ClassViewModel>();

        public string Name { get => $"{(Instances.Count > 0 ? Instances[0].Name : "Empty")}"; }

        public ContainerList Model { get; set; }

        public ClassListViewModel(ContainerList model)
        {
            Model = model;
        }
    }
}
