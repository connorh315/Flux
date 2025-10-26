using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Flux.NuTypes;

namespace Flux;

public class Vec3Input : LabelledInput
{
    /// <summary>
    /// Value StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<Vec3> ValueProperty =
        AvaloniaProperty.Register<Vec3Input, Vec3>(nameof(Value));

    /// <summary>
    /// Gets or sets the Value property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public Vec3 Value
    {
        get => this.GetValue(ValueProperty);
        set
        {
            SetValue(ValueProperty, value);
            IsHalf = value is HalfVec3;
        }
    }

    private bool IsHalf = false;

    private bool IsValid(float val)
    {
        return !IsHalf || (val >= -65504f && val <= 65504f);
    }

    public static readonly DirectProperty<Vec3Input, float> XProperty =
    AvaloniaProperty.RegisterDirect<Vec3Input, float>(
        nameof(X),
        o => o.X,
        (o, v) => o.X = v,
        defaultBindingMode: BindingMode.TwoWay);

    private float x;
    public float X
    {
        get => Value.X;
        set
        {
            if (!IsValid(value))
            {
                throw new DataValidationException("Half value only!");
            }

            if (IsValid(value) && SetAndRaise(XProperty, ref x, value))
            {
                Value.X = x;
            }
        }
    }

    public static readonly DirectProperty<Vec3Input, float> YProperty =
    AvaloniaProperty.RegisterDirect<Vec3Input, float>(
        nameof(Y),
        o => o.Y,
        (o, v) => o.Y = v,
        defaultBindingMode: BindingMode.TwoWay);

    private float y;
    public float Y
    {
        get => Value.Y;
        set
        {
            if (IsValid(value) && SetAndRaise(YProperty, ref y, value))
            {
                Value.Y = y;
            }
        }
    }

    public static readonly DirectProperty<Vec3Input, float> ZProperty =
        AvaloniaProperty.RegisterDirect<Vec3Input, float>(
            nameof(Z),
            o => o.Z,
            (o, v) => o.Z = v,
            defaultBindingMode: BindingMode.TwoWay);

    private float z;
    public float Z
    {
        get => Value.Z;
        set
        {
            if (IsValid(value) && SetAndRaise(ZProperty, ref z, value))
            {
                Value.Z = z;
            }
        }
    }


    private void TextBox1_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not TextBox textbox) return;
    }

    private void TextBox2_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not TextBox textbox) return;
    }

    private void TextBox3_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not TextBox textbox) return;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var tb1 = e.NameScope.Find<TextBox>("PART_Textbox1");
        tb1.LostFocus += TextBox1_LostFocus;

        var tb2 = e.NameScope.Find<TextBox>("PART_Textbox2");
        tb2.LostFocus += TextBox2_LostFocus;

        var tb3 = e.NameScope.Find<TextBox>("PART_Textbox3");
        tb3.LostFocus += TextBox3_LostFocus;
    }
}