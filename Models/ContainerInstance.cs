using System.Collections.Generic;

namespace Flux.Models
{
    public class ContainerInstance(string name, int fieldCount, int componentCount) : FieldValue(name)
    {
        public FieldValue[] Fields { get; set; } = new FieldValue[fieldCount];

        public ContainerList[] Components { get; set; } = new ContainerList[componentCount];

        private Dictionary<string, FieldValue> _fieldLookup { get; set; } = [];

        private int _fieldOffset = 0;
        private int _componentOffset = 0;

        public void AddField(FieldValue field)
        {
            _fieldLookup[field.Name] = field;
            Fields[_fieldOffset++] = field;
        }

        public void AddComponent(ContainerList component)
        {
            Components[_componentOffset++] = component;
        }

        public FieldValue Get(string name)
        {
            _fieldLookup.TryGetValue(name, out var field);

            return field;
        }
    }
}