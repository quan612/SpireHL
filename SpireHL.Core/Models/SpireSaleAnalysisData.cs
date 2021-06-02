using Newtonsoft.Json;
using System;

/// <summary>
/// To read the value from repository, mapped with JSon props
/// </summary>
namespace SpireHL.Core.Models
{
    public class SpireSaleAnalysisData
    {
        [JsonProperty("part_no")]
        public string PartNo { get; set; }
        [JsonProperty("year_end")]
        public DateTime SaleAnalysisYearEnd { get; set; }
        [JsonProperty("period")]
        public int? SaleAnalysisPeriod { get; set; }
        [JsonProperty("qty")]
        public decimal? SaleAnalysisQty { get; set; }
        [JsonProperty("total_sell")]
        public decimal? SaleAnalysisTotalSell { get; set; }
        [JsonProperty("total_cost")]
        public decimal? SaleAnalysisTotalCost { get; set; }
    }
}
