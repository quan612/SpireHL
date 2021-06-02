/// <summary>
/// This is used for user selection
/// </summary>
namespace CatalogModule.Contracts
{
    public interface IUserDefine
    {
        bool Price1 { get; set; }
        bool Price2 { get; set; }
        bool Price3 { get; set; }
        bool PcCtn { get; set; }
        bool UPC { get; set; }
        bool CubeCtn { get; set; }
        bool CBMCtn { get; set; }
        bool VendorCode { get; set; }
        bool Description { get; set; }
        bool HS { get; set; }
        bool MBox { get; set; }
        bool IBox { get; set; }
        bool ItemSize { get; set; }
        bool MinOrder { get; set; }
        bool CDNTire { get; set; }
        bool OnOrderQty { get; set; }
        bool ProductCode { get; set; }
        bool MOP { get; set; }
        bool InventoryType { get; set; }
        bool OnHandQty { get; set; }
    }
}
