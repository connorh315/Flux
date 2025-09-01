using Flux.ViewModels.Values;

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
            CurrentStreamFile = StreamViewModel.Deserialize(filePath);
        }
    }
}
