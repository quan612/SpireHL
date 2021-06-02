using SpireHL.Core.Converters;
using System.ComponentModel;

namespace CatalogModule.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum WordCatalogType
    {
        [Description("Show Catalog")]
        ShowCatalog,

        [Description("Simple Canada Customer Catalog")]
        SimpleCanadaCustomer,

        [Description("Internal Catalog")]
        InternalCatalog
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ExcelCatalogType
    {
        [Description("Order Sheet")]
        OrderSheet,

        [Description("Order with Onhand, Product Code, UPC")]
        OrderSheetOnhandProductCodeUPC
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ExcelSaleOrderCatalogType
    {
        [Description("Sale Order")]
        SaleOrder,

        //[Description("Order with Onhand, Product Code, UPC")]
        //SaleOrderOnhandProductCodeUPC
    }
}
