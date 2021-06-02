using System;
using System.Linq;

namespace EComModule.FieldDelimited
{
    public class Deserializer
    {
        public T ToObject<T>(string[] records)
        {
            var type = typeof(T);
            var obj = Activator.CreateInstance(type);

            for (int i = 0; i < records.Length; i++)
            {
                var props = (from p in type.GetProperties()
                             let attr = (DelimitedFieldAttribute)p.GetCustomAttributes(typeof(DelimitedFieldAttribute), true)
                                 .FirstOrDefault()
                             where attr.Order == i + 1
                             select p).FirstOrDefault();
                if (props != null)
                    props.SetValue(obj, Convert.ChangeType(records[i], props.PropertyType));
            }

            return (T)obj;
        }
    }
}
