using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Flux
{

    public class Input4 : TemplatedControl
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

        private void Validate(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                switch (InputType)
                {
                    case InputControlType.Byte:
                        if (!byte.TryParse(input, out _))
                            throw new DataValidationException($"Byte value only (0-255)");
                        break;

                    case InputControlType.Int:
                        if (!int.TryParse(input, out _))
                            throw new DataValidationException($"Integer value only ({int.MinValue} to {int.MaxValue})");
                        break;

                    case InputControlType.Long:
                        if (!long.TryParse(input, out _))
                            throw new DataValidationException($"Long value only ({long.MinValue} to {long.MaxValue})");
                        break;

                    case InputControlType.Float:
                        if (!float.TryParse(input, out _))
                            throw new DataValidationException($"Float value only");
                        break;
                }
            }
        }

        private string _value1 = "";

        public static readonly DirectProperty<Input4, string> Value1Property =
            AvaloniaProperty.RegisterDirect<Input4, string>(
                nameof(Value1),
                o => o.Value1,
                (o, v) => o.Value1 = v,
                defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

        public string Value1
        {
            get => _value1;
            set
            {
                if (value == _value1) return;

                Validate(value);

                SetAndRaise(Value1Property, ref _value1, value);
            }
        }

        private string _value2 = "";

        public static readonly DirectProperty<Input4, string> Value2Property =
            AvaloniaProperty.RegisterDirect<Input4, string>(
                nameof(Value2),
                o => o.Value2,
                (o, v) => o.Value2 = v,
                defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

        public string Value2
        {
            get => _value2;
            set
            {
                if (value == _value2) return;

                Validate(value);

                SetAndRaise(Value2Property, ref _value2, value);
            }
        }

        private string _value3 = "";

        public static readonly DirectProperty<Input4, string> Value3Property =
            AvaloniaProperty.RegisterDirect<Input4, string>(
                nameof(Value3),
                o => o.Value3,
                (o, v) => o.Value3 = v,
                defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

        public string Value3
        {
            get => _value3;
            set
            {
                if (value == _value3) return;

                Validate(value);

                SetAndRaise(Value3Property, ref _value3, value);
            }
        }

        private string _value4 = "";

        public static readonly DirectProperty<Input4, string> Value4Property =
            AvaloniaProperty.RegisterDirect<Input4, string>(
                nameof(Value4),
                o => o.Value4,
                (o, v) => o.Value4 = v,
                defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

        public string Value4
        {
            get => _value4;
            set
            {
                if (value == _value4) return;

                Validate(value);

                SetAndRaise(Value4Property, ref _value4, value);
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


        private void TextBox1_LostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not TextBox textbox) return;

            // Basically, this forces the input to be valid, either by using the last appropriate value, or best case scenario it just updates back to itself again.
            string original = Value1;
            Value1 = "00000";
            Value1 = original;
        }

        private void TextBox2_LostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not TextBox textbox) return;

            // Basically, this forces the input to be valid, either by using the last appropriate value, or best case scenario it just updates back to itself again.
            string original = Value2;
            Value2 = "00000";
            Value2 = original;
        }

        private void TextBox3_LostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not TextBox textbox) return;

            // Basically, this forces the input to be valid, either by using the last appropriate value, or best case scenario it just updates back to itself again.
            string original = Value3;
            Value3 = "00000";
            Value3 = original;
        }

        private void TextBox4_LostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not TextBox textbox) return;

            // Basically, this forces the input to be valid, either by using the last appropriate value, or best case scenario it just updates back to itself again.
            string original = Value4;
            Value4 = "00000";
            Value4 = original;
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

            var tb4 = e.NameScope.Find<TextBox>("PART_Textbox4");
            tb4.LostFocus += TextBox3_LostFocus;
        }
    }
}