using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels.Values
{
    public abstract class ValueViewModel : ViewModel
    {
        public abstract FieldType Type { get; }

        public string Name { get; set; }

        public FieldValue Model { get; set; }

        public ValueViewModel(FieldValue model)
        {
            Name = model.Name;
            Model = model;
        }
    }
}
