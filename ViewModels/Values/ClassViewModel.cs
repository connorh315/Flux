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

        public ObservableCollection<ValueViewModel> Properties { get; set; } = new();

        public ObservableCollection<ClassListViewModel> Components = new();

        public ObservableCollection<object> Children { get; } = new();

        public ClassViewModel Params { get; set; }

        public ClassListViewModel Parent { get; set; }

        public bool CanRemove => Parent != null;

        public ClassViewModel(MemberObject model) : base(model)
        {
            Properties.CollectionChanged += (s, e) => Sync();
            Components.CollectionChanged += (s, e) => Sync();
        }

        private void Sync()
        {
            Children.Clear();

            if (Params != null)
                Children.Add(Params);

            foreach (var p in Properties) 
            {
                if (p is ClassViewModel) 
                { 
                    Children.Add(p); 
                }
            }

            foreach (var c in Components) Children.Add(c);
        }

        public override object GetValue()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            Parent.Instances.Remove(this);
        }
    }
}
