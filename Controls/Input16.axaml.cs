using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Flux
{
    public class Input16 : TemplatedControl
    {
        public static readonly StyledProperty<string> InputLabelProperty =
            AvaloniaProperty.Register<Input16, string>(nameof(InputLabel), "Input:");

        public string InputLabel
        {
            get => GetValue(InputLabelProperty);
            set => SetValue(InputLabelProperty, value);
        }

        public static readonly StyledProperty<InputControlType> InputTypeProperty =
            AvaloniaProperty.Register<Input16, InputControlType>(nameof(InputType), InputControlType.String);

        public InputControlType InputType
        {
            get => GetValue(InputTypeProperty);
            set => SetValue(InputTypeProperty, value);
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
        public static readonly DirectProperty<Input16, string> Value1Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(
                nameof(Value1), o => o.Value1, (o, v) => o.Value1 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value1 { get => _value1; set { if (_value1 != value) { Validate(value); SetAndRaise(Value1Property, ref _value1, value); } } }

        private string _value2 = "";
        public static readonly DirectProperty<Input16, string> Value2Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value2), o => o.Value2, (o, v) => o.Value2 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value2 { get => _value2; set { if (_value2 != value) { Validate(value); SetAndRaise(Value2Property, ref _value2, value); } } }

        private string _value3 = "";
        public static readonly DirectProperty<Input16, string> Value3Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value3), o => o.Value3, (o, v) => o.Value3 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value3 { get => _value3; set { if (_value3 != value) { Validate(value); SetAndRaise(Value3Property, ref _value3, value); } } }

        private string _value4 = "";
        public static readonly DirectProperty<Input16, string> Value4Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value4), o => o.Value4, (o, v) => o.Value4 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value4 { get => _value4; set { if (_value4 != value) { Validate(value); SetAndRaise(Value4Property, ref _value4, value); } } }

        private string _value5 = "";
        public static readonly DirectProperty<Input16, string> Value5Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value5), o => o.Value5, (o, v) => o.Value5 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value5 { get => _value5; set { if (_value5 != value) { Validate(value); SetAndRaise(Value5Property, ref _value5, value); } } }

        private string _value6 = "";
        public static readonly DirectProperty<Input16, string> Value6Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value6), o => o.Value6, (o, v) => o.Value6 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value6 { get => _value6; set { if (_value6 != value) { Validate(value); SetAndRaise(Value6Property, ref _value6, value); } } }

        private string _value7 = "";
        public static readonly DirectProperty<Input16, string> Value7Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value7), o => o.Value7, (o, v) => o.Value7 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value7 { get => _value7; set { if (_value7 != value) { Validate(value); SetAndRaise(Value7Property, ref _value7, value); } } }

        private string _value8 = "";
        public static readonly DirectProperty<Input16, string> Value8Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value8), o => o.Value8, (o, v) => o.Value8 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value8 { get => _value8; set { if (_value8 != value) { Validate(value); SetAndRaise(Value8Property, ref _value8, value); } } }

        private string _value9 = "";
        public static readonly DirectProperty<Input16, string> Value9Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value9), o => o.Value9, (o, v) => o.Value9 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value9 { get => _value9; set { if (_value9 != value) { Validate(value); SetAndRaise(Value9Property, ref _value9, value); } } }

        private string _value10 = "";
        public static readonly DirectProperty<Input16, string> Value10Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value10), o => o.Value10, (o, v) => o.Value10 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value10 { get => _value10; set { if (_value10 != value) { Validate(value); SetAndRaise(Value10Property, ref _value10, value); } } }

        private string _value11 = "";
        public static readonly DirectProperty<Input16, string> Value11Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value11), o => o.Value11, (o, v) => o.Value11 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value11 { get => _value11; set { if (_value11 != value) { Validate(value); SetAndRaise(Value11Property, ref _value11, value); } } }

        private string _value12 = "";
        public static readonly DirectProperty<Input16, string> Value12Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value12), o => o.Value12, (o, v) => o.Value12 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value12 { get => _value12; set { if (_value12 != value) { Validate(value); SetAndRaise(Value12Property, ref _value12, value); } } }

        private string _value13 = "";
        public static readonly DirectProperty<Input16, string> Value13Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value13), o => o.Value13, (o, v) => o.Value13 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value13 { get => _value13; set { if (_value13 != value) { Validate(value); SetAndRaise(Value13Property, ref _value13, value); } } }

        private string _value14 = "";
        public static readonly DirectProperty<Input16, string> Value14Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value14), o => o.Value14, (o, v) => o.Value14 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value14 { get => _value14; set { if (_value14 != value) { Validate(value); SetAndRaise(Value14Property, ref _value14, value); } } }

        private string _value15 = "";
        public static readonly DirectProperty<Input16, string> Value15Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value15), o => o.Value15, (o, v) => o.Value15 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value15 { get => _value15; set { if (_value15 != value) { Validate(value); SetAndRaise(Value15Property, ref _value15, value); } } }

        private string _value16 = "";
        public static readonly DirectProperty<Input16, string> Value16Property =
            AvaloniaProperty.RegisterDirect<Input16, string>(nameof(Value16), o => o.Value16, (o, v) => o.Value16 = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);
        public string Value16 { get => _value16; set { if (_value16 != value) { Validate(value); SetAndRaise(Value16Property, ref _value16, value); } } }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            for (int i = 1; i <= 16; i++)
            {
                var tb = e.NameScope.Find<TextBox>($"PART_Textbox{i}");
                if (tb != null)
                    tb.LostFocus += (_, __) => ForceValidate(i);
            }
        }

        private void ForceValidate(int index)
        {
            var values = new[] { Value1, Value2, Value3, Value4, Value5, Value6, Value7, Value8,
                         Value9, Value10, Value11, Value12, Value13, Value14, Value15, Value16 };

            if (index < 1 || index > 16) return;

            string original = values[index - 1];

            switch (index)
            {
                case 1: Value1 = "0"; Value1 = original; break;
                case 2: Value2 = "0"; Value2 = original; break;
                case 3: Value3 = "0"; Value3 = original; break;
                case 4: Value4 = "0"; Value4 = original; break;
                case 5: Value5 = "0"; Value5 = original; break;
                case 6: Value6 = "0"; Value6 = original; break;
                case 7: Value7 = "0"; Value7 = original; break;
                case 8: Value8 = "0"; Value8 = original; break;
                case 9: Value9 = "0"; Value9 = original; break;
                case 10: Value10 = "0"; Value10 = original; break;
                case 11: Value11 = "0"; Value11 = original; break;
                case 12: Value12 = "0"; Value12 = original; break;
                case 13: Value13 = "0"; Value13 = original; break;
                case 14: Value14 = "0"; Value14 = original; break;
                case 15: Value15 = "0"; Value15 = original; break;
                case 16: Value16 = "0"; Value16 = original; break;
            }
        }
    }
}