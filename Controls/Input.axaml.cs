using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Flux
{

    public enum InputControlType
    {
        String,
        Byte,
        Int,
        Long,
        Float,
        Short,
    }

    public class Input : TemplatedControl
    {

        /// <summary>
        /// InputLabel StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<string> InputLabelProperty =
            AvaloniaProperty.Register<Input, string>(nameof(InputLabel), "Input:");

        /// <summary>
        /// Gets or sets the InputLabel property. This StyledProperty 
        /// indicates ....
        /// </summary>
        public string InputLabel
        {
            get => GetValue(InputLabelProperty);
            set => SetValue(InputLabelProperty, value);
        }

        private string _value = "";

        public static readonly DirectProperty<Input, string> ValueProperty =
            AvaloniaProperty.RegisterDirect<Input, string>(
                nameof(Value),
                o => o.Value,
                (o, v) => o.Value = v,
                defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

        public string Value
        {
            get => _value;
            set
            {
                if (value == _value) return;

                if (!string.IsNullOrEmpty(value))
                {
                    switch (InputType)
                    {
                        case InputControlType.Byte:
                            if (!byte.TryParse(value, out _))
                                throw new DataValidationException($"Byte value only (0-255)");
                            break;

                        case InputControlType.Int:
                            if (!int.TryParse(value, out _))
                                throw new DataValidationException($"Integer value only ({int.MinValue} to {int.MaxValue})");
                            break;

                        case InputControlType.Long:
                            if (!long.TryParse(value, out _))
                                throw new DataValidationException($"Long value only ({long.MinValue} to {long.MaxValue})");
                            break;

                        case InputControlType.Float:
                            if (!float.TryParse(value, out _))
                                throw new DataValidationException($"Float value only");
                            break;

                        case InputControlType.Short:
                            if (!short.TryParse(value, out _))
                                throw new DataValidationException($"Short value only");
                            break;
                    }
                }

                //if (NumericValue && !string.IsNullOrEmpty(value))
                //{
                //    if (FloatValue)
                //    {
                //        // Allow float input
                //        if (!float.TryParse(value, out _))
                //            throw new DataValidationException("Numeric value only");
                //    }
                //    else
                //    {
                //        // Only allow integer input
                //        if (!int.TryParse(value, out _)) // TODO: Might be a better idea to leverage on this, rather than setting a max value, set a value type, where it can be byte, short, int, long, etc.
                //            throw new DataValidationException("Integer input only");
                //    }
                //}

                SetAndRaise(ValueProperty, ref _value, value);
            }
        }


        /// <summary>
        /// InputType StyledProperty definition
        /// </summary>
        public static readonly StyledProperty<InputControlType> InputTypeProperty =
            AvaloniaProperty.Register<Input, InputControlType>(nameof(InputType), InputControlType.String);

        /// <summary>
        /// Gets or sets the InputType property. This StyledProperty 
        /// indicates ....
        /// </summary>
        public InputControlType InputType
        {
            get => GetValue(InputTypeProperty);
            set => SetValue(InputTypeProperty, value);
        }

        /// <summary>
        /// This is so so so stupid that I have to do this. Why is there nothing better than this in Avalonia anyway????
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_LostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not TextBox textbox) return;

            // Basically, this forces the input to be valid, either by using the last appropriate value, or best case scenario it just updates back to itself again.
            string original = Value;
            Value = "00000";
            Value = original;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var tb = e.NameScope.Find<TextBox>("PART_Textbox");
            tb.LostFocus += TextBox_LostFocus;
        }
    }
}