using Flux.Models.StreamContainers;
using Flux.Models.StreamParts;
using Flux.ViewModels.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private StreamViewModel currentStreamFile;
        public StreamViewModel CurrentStreamFile { get => currentStreamFile; set => SetField(ref currentStreamFile, value); }

        private ClassViewModel selectedClass;
        public ClassViewModel SelectedClass { get => selectedClass; set => SetField(ref selectedClass, value); }

        public void OpenFile(string filePath)
        {
            CurrentStreamFile = StreamViewModel.Parse(filePath);
        }
    }
}
