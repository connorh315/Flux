using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Models
{
    public enum FieldType
    {
        Char,
        Short,
        Int,
        Int64,
        Float,
        String,
        Colour3,
        NuHSpecial,
        Vec3,
        Vec4,
        Mtx,
        Ptr,
        ClassObject,
        ClassObjectRef,
        Half,
        Data,
        Unk,
        HalfVec3,
        UChar,
        Colour4
    }

    public class PrimitiveField : FieldValue
    {
        public FieldType FieldType { get; }
        public object Value { get; set; }
        public object[] Values { get; set; }
        public PrimitiveField(string name, FieldType type, object value) : base(name)
        {
            FieldType = type;
            Value = value;
        }

        public PrimitiveField(string name, FieldType type, object[] values) : base(name)
        {
            FieldType = type;
            Values = values;
        }
    }
}
