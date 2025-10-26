using Flux.Models;
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


        private string _windowTitle = "Flux";
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                if (_windowTitle != value)
                {
                    _windowTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        public void AddMember(ClassListViewModel list)
        {
            SelectedClass = CurrentStreamFile.AddMemberToList(list);
        }

        public void OpenFile(string filePath)
        {
            SelectedClass = null;
            CurrentStreamFile = null;
            CurrentStreamFile = StreamViewModel.Parse(filePath);

            if (CurrentStreamFile != null)
            {
                WindowTitle = $"Flux - Editing file: {filePath}";
            }
        }

        public void WriteFile(string filePath)
        {
            if (CurrentStreamFile == null) return;

            StreamViewModel.Save(filePath, CurrentStreamFile);
        }
    }
}
