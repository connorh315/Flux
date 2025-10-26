using Flux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models
{
    public class MemberObject : FieldValue
    {
        private Dictionary<string, FieldValue> FieldLookup { get; set; } = new();

        public FieldValue[] Fields { get; set; }

        public ObjectList[] Components { get; set; }

        public bool ClassObject = false;

        public MemberObject Params { get; set; }

        public MemberObject(string name, int fieldCount, int componentCount) : base(name)
        {
            Fields = new FieldValue[fieldCount];
            Components = new ObjectList[componentCount];
        }

        private int fieldOffset = 0;
        public void AddField(FieldValue field)
        {
            FieldLookup[field.Name] = field;
            Fields[fieldOffset++] = field;
        }

        private int componentOffset = 0;
        public void AddComponent(ObjectList component)
        {
            Components[componentOffset++] = component;
        }

        public FieldValue Get(string name)
        {
            FieldLookup.TryGetValue(name, out var field);

            return field;
        }
    }
}
