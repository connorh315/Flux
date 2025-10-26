using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Flux.NuTypes;

namespace Flux;

public class Vec4Input : LabelledInput
{
    /// <summary>
    /// Value StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<Vec4> ValueProperty =
        AvaloniaProperty.Register<Vec4Input, Vec4>(nameof(Value));

    /// <summary>
    /// Gets or sets the Value property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public Vec4 Value
    {
        get => this.GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DirectProperty<Vec4Input, float> XProperty =
    AvaloniaProperty.RegisterDirect<Vec4Input, float>(
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
            if (SetAndRaise(XProperty, ref x, value))
            {
                Value.X = x;
            }
        }
    }

    public static readonly DirectProperty<Vec4Input, float> YProperty =
    AvaloniaProperty.RegisterDirect<Vec4Input, float>(
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
            if (SetAndRaise(YProperty, ref y, value))
            {
                Value.Y = y;
            }
        }
    }

    public static readonly DirectProperty<Vec4Input, float> ZProperty =
        AvaloniaProperty.RegisterDirect<Vec4Input, float>(
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
            if (SetAndRaise(ZProperty, ref z, value))
            {
                Value.Z = z;
            }
        }
    }

    public static readonly DirectProperty<Vec4Input, float> WProperty =
        AvaloniaProperty.RegisterDirect<Vec4Input, float>(
            nameof(W),
            o => o.W,
            (o, v) => o.W = v,
            defaultBindingMode: BindingMode.TwoWay);

    private float w;
    public float W
    {
        get => Value.W;
        set
        {
            if (SetAndRaise(WProperty, ref w, value))
            {
                Value.W = w;
            }
        }
    }
}