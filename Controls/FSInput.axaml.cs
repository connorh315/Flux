using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;

namespace Flux;

public class FSInput : TemplatedControl
{

    /// <summary>
    /// InputLabel StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<string> InputLabelProperty =
        AvaloniaProperty.Register<FSInput, string>(nameof(InputLabel), "Input:");

    /// <summary>
    /// Gets or sets the InputLabel property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public string InputLabel
    {
        get => this.GetValue(InputLabelProperty);
        set => SetValue(InputLabelProperty, value);
    }

    private string _value = "";

    public static readonly DirectProperty<FSInput, string> ValueProperty =
        AvaloniaProperty.RegisterDirect<FSInput, string>(
            nameof(Value),
            o => o.Value,
            (o, v) => o.Value = v,
            defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);


    /// <summary>
    /// IsFileInput StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<bool> IsFileInputProperty =
        AvaloniaProperty.Register<FSInput, bool>(nameof(IsFileInput), true);

    /// <summary>
    /// Gets or sets the IsFileInput property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public bool IsFileInput
    {
        get => this.GetValue(IsFileInputProperty);
        set => SetValue(IsFileInputProperty, value);
    }

    public string Value
    {
        get => _value;
        set
        {
            if (value == _value) return;

            SetAndRaise(ValueProperty, ref _value, value);
        }
    }

    /// <summary>
    /// This is so so so stupid that I have to do this. Why is there nothing better than this in Avalonia anyway????
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not TextBox textbox) return;

        // Basically, this forces the input to be valid, either by using the last appropriate value, or best case scenario it just updates back to itself again.
        string original = Value;
        Value = "00000";
        Value = original;
    }

    private string CleanupPath(string path)
    {
        return path.Replace("/", "\\").Replace("%20", " ");
    }

    private async void Open_Filesystem_Picker(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = this.GetVisualRoot() as Window;
        if (window?.StorageProvider is not { CanOpen: true } storage)
            return;

        if (IsFileInput)
        {
            var file = await window.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Set Output DAT File",
                DefaultExtension = "DAT",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("DAT Files") { Patterns = new[] { "*.DAT" } },
                },
            });

            if (file?.Path?.AbsolutePath != null)
            {
                Value = CleanupPath(file?.Path?.AbsolutePath);
            }
        }
        else
        {
            var folder = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Set Input Folder",
                AllowMultiple = false,
            });

            if (folder.Count > 0)
            {
                Value = CleanupPath(folder[0].Path.IsAbsoluteUri ? folder[0].Path.AbsolutePath : folder[0].Path.OriginalString); // If the user selects the root of a filesystem (i.e. "C:\") then AbsolutePath throws an exception.
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var tb = e.NameScope.Find<TextBox>("PART_Textbox");
        tb.LostFocus += TextBox_LostFocus;

        var button = e.NameScope.Find<Button>("PART_Button");
        button.Click += Open_Filesystem_Picker;
    }
}