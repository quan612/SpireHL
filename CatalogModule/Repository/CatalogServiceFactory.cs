using CatalogModule.Contracts;
using CatalogModule.Enums;
using CatalogModule.Services.Excel;
using CatalogModule.Services.Word;
using System;

namespace CatalogModule.Repository
{
    public class CatalogServiceFactory
    {
        public static IWordCatalogService GetWordCatalogService(WordCatalogType catalogType, IUserDefine userDefine)
        {
            switch (catalogType)
            {
                case WordCatalogType.ShowCatalog:
                    return new ShowCatalogService();
                case WordCatalogType.InternalCatalog:
                    return new InternalCatalogService(userDefine);
                case WordCatalogType.SimpleCanadaCustomer:
                    return new SimpleCanadaCatalogService();
                default:
                    throw new Exception("Invalid Word Catalog Service");
            }
        }

        public static IExcelCatalogService GetExcelCatalogService(ExcelCatalogType catalogType, IUserDefine userDefine)
        {
            switch (catalogType)
            {
                case ExcelCatalogType.OrderSheet:
                    return new OrderSheetService();
                case ExcelCatalogType.OrderSheetOnhandProductCodeUPC:
                    return new OrderSheetOnhandProductCodeUPCService();
                default:
                    throw new Exception("Invalid Excel Catalog Service");
            }
        }

        public static IExcelCatalogService GetExcelSaleOrderCatalogService(ExcelSaleOrderCatalogType catalogType)
        {

            switch (catalogType)
            {
                case ExcelSaleOrderCatalogType.SaleOrder:
                    return new SaleOrderService();
                //case ExcelCatalogType.SaleOrderOnhandProductCodeUPC:
                //    return new SaleOrderOnhandProductCodeUPCService();
                default:
                    throw new Exception("Invalid Excel Catalog Service");
            }
        }

        public static IExcelCatalogService GetExcelSaleAnalysisCatalogService()
        {
            return new SaleAnalysisService();
            //switch (catalogType)
            //{
            //    case ExcelSaleOrderCatalogType.SaleOrder:
            //        return new SaleOrderService();
            //    //case ExcelCatalogType.SaleOrderOnhandProductCodeUPC:
            //    //    return new SaleOrderOnhandProductCodeUPCService();
            //    default:
            //        throw new Exception("Invalid Excel Catalog Service");
            //}
        }
    }
}
