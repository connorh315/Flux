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

            var vm = new MainWindowViewModel();

            DataContext = vm;

            vm.OpenFile(@"F:\mods\lordvortechmod\CHARCACHE\LORDVORTECH\CHARS\MINIFIG\LORDVORTECH\lordvortech.cd");
        }

        private async void MenuItem_OpenFile_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (StorageProvider == null)
                throw new Exception("Unable to access filesystem");

            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open StreamInfo File",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("StreamInfo files") { Patterns = new[] { "*.CD", "*.LED", "*.AS", "*.CPJ" } }
                }
            });

            if (files.Count > 0)
            {
                string filePath = files[0].Path.LocalPath;

                if (DataContext is not MainWindowViewModel vm) return;
                vm.OpenFile(filePath);
            }
        }
    }
}