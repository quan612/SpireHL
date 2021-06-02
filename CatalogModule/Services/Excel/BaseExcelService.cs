using CatalogModule.Contracts;
using CatalogModule.Models;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using SpireHL.Core.Repository;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CatalogModule.Services.Excel
{
    public abstract class BaseExcelService : IExcelCatalogService
    {
        private const string CatalogExportPathParams = "CatalogExportPath";
        protected string ProjectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Templates\";
        ExcelEngine excelEngine;

        public DataTable DataTableItems { get; set; }

        public abstract string SourceTemplate { get; }
        public abstract string ExportTemplate { get; }

        public IUserDefine UserDefine { get; }

        abstract protected void AppendItemsToDataTable<T>(IEnumerable<T> items);
        //abstract protected void AppendItemsToDataTable<T>(IEnumerable<T> items);
        public BaseExcelService()
        {

        }

        public BaseExcelService(IUserDefine userDefine) : this()
        {
            UserDefine = userDefine;
        }

        public virtual string MakeCatalog<T>(IEnumerable<T> items)
        {
            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.
            //Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();

            excelEngine.Excel.DefaultVersion = ExcelVersion.Xlsx;
            //Open an existing spreadsheet which will be used as a template for generating the new spreadsheet.
            //After opening, the workbook object represents the complete in-memory object model of the template spreadsheet.
            IWorkbook workbook;

            // var fileName = ProjectDirectory + "SaleOrder.xlsx";

            workbook = excelEngine.Excel.Workbooks.Open(SourceTemplate);
            //The first worksheet object in the worksheets collection is accessed.
            IWorksheet sheet1 = workbook.Worksheets[0];

            //Create Template Marker Processor
            ITemplateMarkersProcessor marker = workbook.CreateTemplateMarkersProcessor();

            // modify this items with image
            // data table here
            DataTableItems = items.ListToDataTable();
            AppendItemsToDataTable(items.ToList());
            marker.AddVariable("SpireItem", DataTableItems);
            ///marker.AddVariable("SpireItem", items.ToList());

            //Process the markers in the template.
            marker.ApplyMarkers();

            workbook.Version = ExcelVersion.Xlsx;

            string savedFile = string.Empty;
            try
            {
                savedFile = SaveFile(workbook);
                //Message box confirmation to view the created spreadsheet.
            }
            catch (Exception)
            {
                throw;
            }
            return savedFile;
        }

        public virtual string MakeCatalogFromObjectCollectionUsingTemplateMaker<T>(IEnumerable<T> items)
        {
            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.
            //Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();

            excelEngine.Excel.DefaultVersion = ExcelVersion.Xlsx;
            //Open an existing spreadsheet which will be used as a template for generating the new spreadsheet.
            //After opening, the workbook object represents the complete in-memory object model of the template spreadsheet.
            IWorkbook workbook;

            // var fileName = ProjectDirectory + "SaleOrder.xlsx";

            workbook = excelEngine.Excel.Workbooks.Open(SourceTemplate);
            //The first worksheet object in the worksheets collection is accessed.
            IWorksheet sheet1 = workbook.Worksheets[0];

            //Create Template Marker Processor
            ITemplateMarkersProcessor marker = workbook.CreateTemplateMarkersProcessor();
            var saleAnalysisData = BuildCustomObjectForSaleAnalysis(items);
            marker.AddVariable("SpireItem", saleAnalysisData.ToList());

            //Process the markers in the template.
            marker.ApplyMarkers();

            workbook.Version = ExcelVersion.Xlsx;

            string savedFile = string.Empty;
            try
            {
                savedFile = SaveFile(workbook);
                //Message box confirmation to view the created spreadsheet.
            }
            catch (Exception)
            {
                throw;
            }
            return savedFile;
        }

        private List<SpireAnalysisItem> BuildCustomObjectForSaleAnalysis<T>(IEnumerable<T> items)
        {
            var result = new List<SpireAnalysisItem>();
            foreach (var item in items.ToList() as List<SpireItem>)
            {
                var currentPeriodDateTime = item.SaleAnalysisData.Max(l => l.SaleAnalysisYearEnd);
                var currentPeriodData = item.SaleAnalysisData.Where(e => e.SaleAnalysisYearEnd == currentPeriodDateTime);

                // https://www.tradegecko.com/blog/inventory-management/how-to-calculate-beginning-inventory

                var revenue = currentPeriodData.Select(x => x.SaleAnalysisTotalSell).Sum(e => e.Value);
                var cogs = currentPeriodData.Select(x => x.SaleAnalysisTotalCost).Sum(e => e.Value);

                var totalQtySaleForThisPeriod = currentPeriodData.Select(x => x.SaleAnalysisQty).Sum(e => e.Value);
                var beginningQty = totalQtySaleForThisPeriod + item.OnHandQty;

                // does not need purchase qty as onHandQty already incurred the purchase qty during this period
                var beginningInventory = beginningQty * item.Price1;
                var endingInventory = item.OnHandQty * item.Price1;
                var avgInventory = (beginningInventory + endingInventory) / 2;

                var inventoryTurnOver = revenue / avgInventory;
                var daysSaleOfInventory = (1 / inventoryTurnOver) * 365;

                result.Add(new SpireAnalysisItem()
                {
                    PartNo = item.PartNo,
                    OnHandQty = item.OnHandQty,
                    BackorderQty = item.BackorderQty,
                    CommittedQty = item.CommittedQty,
                    ReorderQty = item.ReorderQty,
                    CurrentCost = item.CurrentCost,
                    AverageCost = item.AverageCost,
                    LastSaleDate = item.LastSaleDate,
                    Price1 = item.Price1,
                    Price3 = item.Price3,

                    SpireSaleAnalysisSummary = new SpireSaleAnalysisSummary()
                    {
                        SaleAnalysisFiscalYear = currentPeriodDateTime,
                        Revenue = revenue,
                        COGS = cogs,
                        TotalQtySaleForThisPeriod = totalQtySaleForThisPeriod,
                        BeginningQty = beginningQty,
                        BeginningInventory = beginningInventory,
                        EndingInventory = endingInventory,
                        AverageInventory = avgInventory,
                        InventoryTurnOver = inventoryTurnOver,
                        DaysSaleOfInventory = (Int32)daysSaleOfInventory
                    },
                    ProductImageExcel = item.ProductImageExcel
                });
            }

            return result;
        }
        public virtual string MakeCatalogFromObjectCollectionUsingImportData<T>(IEnumerable<T> items)
        {
            excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            string fileName = string.Empty;

            ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions();
            importDataOptions.NestedDataLayoutOptions = ExcelNestedDataLayoutOptions.Merge;
            importDataOptions.NestedDataGroupOptions = ExcelNestedDataGroupOptions.Collapse;
            importDataOptions.CollapseLevel = 1;

            importDataOptions.FirstRow = 4;
            importDataOptions.FirstColumn = 1;
            //importDataOptions.IncludeHeader = false;
            //importDataOptions.PreserveTypes = false;

            var saleAnalysisData = BuildCustomObjectForSaleAnalysis(items);
            worksheet.ImportData(saleAnalysisData.ToList(), importDataOptions);

            Syncfusion.XlsIO.IStyle tableHeader = workbook.Styles.Add("TableHeaderStyle");
            worksheet["A4:C4"].CellStyle = tableHeader;
            worksheet["A1:C1"].CellStyle.Font.Bold = true;
            worksheet.UsedRange.AutofitColumns();

            workbook.Version = ExcelVersion.Xlsx;

            string savedFile = string.Empty;
            try
            {
                savedFile = SaveFile(workbook);
                //Message box confirmation to view the created spreadsheet.
            }
            catch (Exception)
            {
                throw;
            }

            excelEngine.Dispose();
            return savedFile;
        }

        #region Helper methods
        private string SaveFile(IWorkbook workbook)
        {
            var configs = ModuleConfigs.GetConfigs(CatalogConstants.Module, CatalogConstants.Section);
            var path = configs.Find(e => e.ParameterName == CatalogExportPathParams).ParameterValue;
            workbook.SaveAs(path + @"\" + ExportTemplate);
            workbook.Close();
            return path;
        }

        #endregion
    }
}
