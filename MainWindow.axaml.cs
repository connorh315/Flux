using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Flux.ViewModels;
using Flux.ViewModels.Values;
using System;
using System.IO;

namespace Flux
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainWindowViewModel();

            DataContext = vm;

            KeyDown += MainWindow_KeyDown;

            if (App.Args != null && App.Args.Length > 0)
            {
                vm.OpenFile(App.Args[0]);
            }

        }

        private async void OpenFileMenu()
        {
            if (StorageProvider == null)
                throw new Exception("Unable to access filesystem");

            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open StreamInfo File",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("StreamInfo files") { Patterns = new[] { "*.CD", "*.LED", "*.AS", "*.CPD", "*.CPJ" } }
                }
            });

            if (files.Count > 0)
            {
                string filePath = files[0].Path.LocalPath;

                if (DataContext is not MainWindowViewModel vm) return;
                vm.OpenFile(filePath);
            }
        }

        private void WriteFile(string location = "")
        {
            if (DataContext is not MainWindowViewModel vm) return;
            if (location == string.Empty)
            {
                location = vm.CurrentStreamFile.FilePath;
            }

            vm.WriteFile(location);
        }

        private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && e.KeyModifiers == KeyModifiers.Control)
            {
                OpenFileMenu();
                e.Handled = true;
            }

            if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
            {
                WriteFile();
                e.Handled = true;
            }
        }

        private async void MenuItem_OpenFile_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OpenFileMenu();
        }

        private async void MenuItem_SaveFile_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            WriteFile();
        }

        private async void MenuItem_SaveAs_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (StorageProvider == null)
                throw new Exception("Unable to access filesystem");

            if (DataContext is not MainWindowViewModel vm) return;

            string ext = Path.GetExtension(vm.CurrentStreamFile.FilePath);

            var customFileType = new FilePickerFileType(ext + " File")
            {
                Patterns = new[] { "*" + ext }
            };

            var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save As",
                DefaultExtension = ext,
                FileTypeChoices = new[] { customFileType }
            });

            if (file != null)
            {
                string filePath = file.Path.LocalPath;
                WriteFile(filePath);
            }
        }

        private async void AddMember_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ClassListViewModel list)
            {
                if (DataContext is not MainWindowViewModel vm) return;

                vm.AddMember(list);
            }

            e.Handled = true;
        }

        private void Button_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            e.Handled = true;
        }

        private void DeleteClass_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ClassViewModel classViewModel)
            {
                classViewModel.Remove();
            }
        }
    }
}