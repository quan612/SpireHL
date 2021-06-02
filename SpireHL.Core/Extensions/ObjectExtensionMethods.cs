using System;

/// <summary>
/// Copy properties from self to target, using the Property Name
/// </summary>
namespace SpireHL.Core.Extensions
{
    public static class ObjectExtensionMethods
    {
        public static void CopyPropertiesTo(this object self, object target, string propertyToCopy)
        {
            var fromProperties = self.GetType().GetProperties();
            var toProperties = target.GetType().GetProperties();

            foreach (var fromProp in fromProperties)
            {
                foreach (var toProp in toProperties)
                {
                    //if(fromProp.Name.Contains(propertyToCopy) && fromProp.Name == toProp.Name && fromProp.PropertyType == toProp.PropertyType)
                    if (fromProp.Name.Contains(propertyToCopy) && fromProp.Name == toProp.Name)
                    {
                        if (fromProp.GetValue(self) != null && !string.IsNullOrEmpty(fromProp.GetValue(self).ToString()))
                        {
                            if (fromProp.PropertyType == typeof(Nullable<Int32>))
                            {
                                var val = (Int32)fromProp.GetValue(self);
                                if (val != 0)
                                {
                                    toProp.SetValue(target, val);
                                    break;
                                }
                            }

                            if (fromProp.PropertyType == typeof(Nullable<Decimal>))
                            {
                                var val = (decimal)fromProp.GetValue(self);
                                if (val != decimal.Zero)
                                {
                                    toProp.SetValue(target, val);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
