using Avalonia;
using Avalonia.Controls.Primitives;
using Flux.NuTypes;

namespace Flux;

public class MtxInput : LabelledInput
{

    /// <summary>
    /// Value StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<Mtx> ValueProperty =
        AvaloniaProperty.Register<MtxInput, Mtx>(nameof(Value));

    /// <summary>
    /// Gets or sets the Value property. This StyledProperty
    /// indicates ....
    /// </summary>
    public Mtx Value
    {
        get => this.GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }


}