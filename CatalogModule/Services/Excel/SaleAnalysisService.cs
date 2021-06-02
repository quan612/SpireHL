using System;
using System.Collections.Generic;

namespace CatalogModule.Services.Excel
{
    public class SaleAnalysisService : BaseExcelService
    {
        public override string SourceTemplate => ProjectDirectory + "SaleAnalysis.xlsx";
        public override string ExportTemplate
        {
            get
            {
                return "Sale Analysis_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx";
            }
        }

        public SaleAnalysisService() : base()
        {

        }

        protected override void AppendItemsToDataTable<T>(IEnumerable<T> items)
        {
            // not used yet
        }
    }
}
