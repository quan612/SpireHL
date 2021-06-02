/// <summary>
/// To query sale order and sale history items
/// </summary>
namespace SpireHL.Core.Models
{
    public class SpireSaleOrderItem : SpireItem
    {
        public string OrderNo { get; set; }
        public int OrderQty { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
