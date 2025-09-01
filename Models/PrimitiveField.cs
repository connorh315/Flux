using Flux.Models.StreamContainers.StreamInfo.Definitions;

namespace Flux.Models
{
    public class PrimitiveField : FieldValue
    {
        public FieldType FieldType { get; }
        public object    Value     { get; set; }
        public object[]  Values    { get; set; }

        public PrimitiveField(string name, FieldType type, object value) : base(name)
        {
            FieldType = type;
            Value     = value;
        }

        public PrimitiveField(string name, FieldType type, object[] values) : base(name)
        {
            FieldType = type;
            Values    = values;
        }
    }
}
