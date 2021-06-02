using System;

namespace CatalogModule.Models
{
    public class SpireAnalysisItem
    {

        public string PartNo { get; set; }
        public int OnHandQty { get; set; }
        public int BackorderQty { get; set; }
        public int CommittedQty { get; set; }
        public int ReorderQty { get; set; }
        public string CurrentCost { get; set; }
        public string AverageCost { get; set; }
        public DateTime LastSaleDate { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price3 { get; set; }

        //public List<SpireSaleAnalysisSummary> SpireSaleAnalysisSummary { get; set; }
        public SpireSaleAnalysisSummary SpireSaleAnalysisSummary { get; set; }

        public byte[] ProductImageExcel { get; set; }
    }
    public class SpireSaleAnalysisSummary
    {
        public DateTime SaleAnalysisFiscalYear { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? COGS { get; set; }
        public decimal? TotalQtySaleForThisPeriod { get; set; }
        public decimal? BeginningQty { get; set; }
        public decimal? BeginningInventory { get; set; }
        public decimal? EndingInventory { get; set; }
        public decimal? AverageInventory { get; set; }
        public decimal? InventoryTurnOver { get; set; }

        public int DaysSaleOfInventory { get; set; }
    }
}






