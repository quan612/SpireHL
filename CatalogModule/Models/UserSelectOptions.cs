using CatalogModule.Contracts;
using Prism.Mvvm;

namespace CatalogModule.Models
{
    public class UserSelectOptions : BindableBase, IUserDefine
    {
        public bool Price1 { get; set; }
        public bool Price2 { get; set; }
        public bool Price3 { get; set; }
        public bool PcCtn { get; set; }
        public bool UPC { get; set; }
        public bool CubeCtn { get; set; }
        public bool CBMCtn { get; set; }
        public bool VendorCode { get; set; }
        public bool Description { get; set; }
        public bool HS { get; set; }
        public bool MBox { get; set; }
        public bool IBox { get; set; }
        public bool ItemSize { get; set; }
        public bool MinOrder { get; set; }
        public bool CDNTire { get; set; }
        public bool OnOrderQty { get; set; }
        public bool ProductCode { get; set; }
        public bool MOP { get; set; }
        public bool InventoryType { get; set; }
        public bool OnHandQty { get; set; }

        public UserSelectOptions()
        {
            Price1 = true;
            Price2 = true;
            PcCtn = true;
            UPC = true;
            CubeCtn = true;
            CBMCtn = true;
            VendorCode = true;
            Description = true;
            HS = true;
            MBox = true;
            IBox = true;
            ItemSize = true;
            MinOrder = true;
            CDNTire = true;
            OnOrderQty = true;
            ProductCode = true;
            MOP = true;
            InventoryType = true;
            OnHandQty = true;
        }
    }
}