using System;

namespace EComModule.FieldDelimited
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DelimitedFieldAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
