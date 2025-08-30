using Flux.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public class ClassViewModel : ValueViewModel
    {
        public override FieldType Type => FieldType.ClassObject;

        public ObservableCollection<ValueViewModel> Properties = new();

        public ObservableCollection<ClassViewModel> Components = new();

        public ClassViewModel(ContainerInstance model) : base(model)
        {
        }
    }
}
