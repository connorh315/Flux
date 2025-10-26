using Flux.Models;
using Flux.Models.StreamParts;
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

        public string Name { get => $"{Model.Definition?.Name ?? "Empty"}"; }

        public ObjectList Model { get; set; }

        public ClassListViewModel(ObjectList model)
        {
            Model = model;
        }
    }
}
