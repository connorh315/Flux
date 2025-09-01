using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Flux.ViewModels;
using System;

namespace Flux
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel vm = new();

            DataContext = vm;

            vm.OpenFile(@"D:\Lego\LegoWorlds\levels\builder\builderfunctional\builderdoors\builderdoors.led");
        }

        private async void MenuItem_OpenFile_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (StorageProvider == null)
            {
                throw new Exception("Unable to access filesystem");
            }

            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open StreamInfo File",
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("StreamInfo files") { Patterns = ["*.CD", "*.LED", "*.AS", "*.CPJ"] }
                ]
            });

            if (files.Count > 0)
            {
                string filePath = files[0].Path.LocalPath;

                if (DataContext is not MainWindowViewModel vm)
                {
                    return;
                }

                vm.OpenFile(filePath);
            }
        }
    }
}