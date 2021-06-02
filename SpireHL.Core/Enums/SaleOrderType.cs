using SpireHL.Core.Converters;
using System.ComponentModel;

namespace SpireHL.Core.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SaleOrderType
    {
        [Description("Sale History - Invoiced")]
        SaleHistory,

        [Description("Sale Order")]
        SaleOrder,
    }
}
