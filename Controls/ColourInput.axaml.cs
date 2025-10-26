using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Flux.NuTypes;
using System.Globalization;
using System.Security.Cryptography;

namespace Flux;

public class ColourInput : TemplatedControl
{
    /// <summary>
    /// InputLabel StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<string> InputLabelProperty =
        AvaloniaProperty.Register<ColourInput, string>(nameof(InputLabel), "Input:");

    /// <summary>
    /// Gets or sets the InputLabel property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public string InputLabel
    {
        get => this.GetValue(InputLabelProperty);
        set => SetValue(InputLabelProperty, value);
    }


    /// <summary>
    /// UseHex DirectProperty definition
    /// </summary>
    public static readonly DirectProperty<ColourInput, bool> UseHexProperty =
        AvaloniaProperty.RegisterDirect<ColourInput, bool>(nameof(UseHex),
            o => o.UseHex);


    private bool _UseHex = true;
    /// <summary>
    /// Gets or sets the UseHex property. This DirectProperty 
    /// indicates ....
    /// </summary>
    public bool UseHex
    {
        get => _UseHex;
        private set
        {
            SetAndRaise(UseHexProperty, ref _UseHex, value);
        }
    }


    public static readonly DirectProperty<ColourInput, Colour3> ValueProperty =
        AvaloniaProperty.RegisterDirect<ColourInput, Colour3>(
            nameof(Value),
            o => o.Value,
            (o, v) => o.Value = v,
            defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    private Colour3 _value = new Colour3();

    public Colour3 Value
    {
        get => _value;
        set
        {
            if (value == null) return;
            _value = value;

            // update component props
            SetAndRaise(RProperty, ref _r, value.R);
            SetAndRaise(GProperty, ref _g, value.G);
            SetAndRaise(BProperty, ref _b, value.B);

            // update hex
            _hex = ToHex(value);
            RaisePropertyChanged(HexProperty, null, _hex);

            RaisePropertyChanged(ValueProperty, null, value);
        }
    }

    private float _r, _g, _b;
    public static readonly DirectProperty<ColourInput, float> RProperty =
        AvaloniaProperty.RegisterDirect<ColourInput, float>(
            nameof(R), o => o.R, (o, v) => o.R = v);

    public float R
    {
        get => _r;
        set
        {
            if (SetAndRaise(RProperty, ref _r, value))
            {
                Value.R = _r;
                _hex = ToHex(Value);
                RaisePropertyChanged(HexProperty, null, _hex);
            }
        }
    }

    public static readonly DirectProperty<ColourInput, float> GProperty =
    AvaloniaProperty.RegisterDirect<ColourInput, float>(
        nameof(G), o => o.G, (o, v) => o.G = v);
    public float G
    {
        get => _g;
        set
        {
            if (SetAndRaise(GProperty, ref _g, value))
            {
                Value.G = _g;
                _hex = ToHex(Value);
                RaisePropertyChanged(HexProperty, null, _hex);
            }
        }
    }

    public static readonly DirectProperty<ColourInput, float> BProperty =
        AvaloniaProperty.RegisterDirect<ColourInput, float>(
            nameof(B), o => o.B, (o, v) => o.B = v);
    public float B
    {
        get => _b;
        set
        {
            if (SetAndRaise(BProperty, ref _b, value))
            {
                Value.B = _b;
                _hex = ToHex(Value);
                RaisePropertyChanged(HexProperty, null, _hex);
            }
        }
    }

    public static readonly DirectProperty<ColourInput, string> HexProperty =
    AvaloniaProperty.RegisterDirect<ColourInput, string>(
        nameof(Hex),
        o => o.Hex,
        (o, v) => o.Hex = v,
        defaultBindingMode: BindingMode.TwoWay);

    private string _hex = "#000000";

    public string Hex
    {
        get => _hex;
        set
        {
            var old = _hex;
            _hex = value; // always set
            RaisePropertyChanged(HexProperty, old, value); // Ensures data validation clears

            // Try parse
            if (TryParseHex(value, out var c))
            {
                SetAndRaise(ValueProperty, ref _value, c);
                SetAndRaise(HexProperty, ref _hex, value);
            }
            else
            {
                throw new DataValidationException("Hex format required (e.g. #FFFFFF)");
            }
        }
    }

    private static string ToHex(Colour3 c)
    {
        int r = (int)(c.R * 255);
        int g = (int)(c.G * 255);
        int b = (int)(c.B * 255);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private static bool TryParseHex(string hex, out Colour3 c)
    {
        c = new Colour3();
        if (string.IsNullOrWhiteSpace(hex)) return false;
        if (hex.StartsWith("#")) hex = hex[1..];

        if (hex.Length == 6 &&
            int.TryParse(hex[..2], NumberStyles.HexNumber, null, out int r) &&
            int.TryParse(hex[2..4], NumberStyles.HexNumber, null, out int g) &&
            int.TryParse(hex[4..6], NumberStyles.HexNumber, null, out int b))
        {
            c = new Colour3(r / 255f, g / 255f, b / 255f);
            return true;
        }
        return false;
    }

    /// <summary>
    /// InputType StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<InputControlType> InputTypeProperty =
        AvaloniaProperty.Register<ColourInput, InputControlType>(nameof(InputType), InputControlType.String);

    /// <summary>
    /// Gets or sets the InputType property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public InputControlType InputType
    {
        get => this.GetValue(InputTypeProperty);
        set => SetValue(InputTypeProperty, value);
    }

    /// <summary>
    /// This is so so so stupid that I have to do this. Why is there nothing better than this in Avalonia anyway????
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not TextBox textbox) return;
    }
    private async void Switch_View(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        UseHex = !UseHex;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var tb = e.NameScope.Find<TextBox>("PART_Textbox");
        tb.LostFocus += TextBox_LostFocus;

        var b = e.NameScope.Find<Button>("PART_SwitchView");
        b.Click += Switch_View;
    }
}