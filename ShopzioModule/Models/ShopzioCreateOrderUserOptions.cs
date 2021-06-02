namespace ShopzioModule.Models
{
    public class ShopzioCreateOrderUserOptions
    {
        public string CustomerName { get; set; }
        public bool IsPriceOverride { get; set; }
        public bool IsQuantityOverride { get; set; }
    }
}
