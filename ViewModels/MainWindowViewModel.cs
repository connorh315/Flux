using Flux.Models.StreamContainers;
using Flux.Models.StreamParts;
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

        public void OpenFile(string filePath)
        {
            CurrentStreamFile = StreamViewModel.Parse(filePath);
        }
    }
}
