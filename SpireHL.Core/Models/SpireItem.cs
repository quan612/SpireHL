using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpireHL.Core.Models
{
    public interface ISpireItem
    { }
    public class SpireItem : BindableBaseWithValidation, ISpireItem
    {
        public int Id { get; set; }
        public string Whse { get; set; }
        public string PartNo { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }

        private decimal? _price1;
        [Required]
        public decimal? Price1
        {
            get { return _price1; }
            set { SetProperty(ref _price1, value); }
        }

        private decimal? _price2;
        [Required]
        public decimal? Price2
        {
            get { return _price2; }
            set { SetProperty(ref _price2, value); }
        }

        private decimal? _price3;
        [Required]
        public decimal? Price3
        {
            get { return _price3; }
            set { SetProperty(ref _price3, value); }
        }
        public decimal? Weight { get; set; }
        public int OnHandQty { get; set; }
        public int BackorderQty { get; set; }
        public int CommittedQty { get; set; }
        public int ReorderQty { get; set; }
        public string CurrentCost { get; set; }
        public string AverageCost { get; set; }
        public string InStock { get; set; }
        public int PurchaseQty { get; set; }
        public string InventoryType { get; set; }
        public string IsNew { get; set; }
        public string ArrivalDate { get; set; }
        public int? MinOrder { get; set; }
        public UDF UDFData { get; set; }
        public string ProductImage { get; set; }
        public byte[] ProductImageExcel { get; set; }
        public string ProductCode { get; set; }
        public string VendorCode { get; set; }
        public string UPC { get; set; }
        public string OnSale { get; set; }

        public DateTime LastSaleDate { get; set; }

        // for querying in db first, and then referencing in another class for Spire Items 
        public List<SpireSaleAnalysisData> SaleAnalysisData { get; set; }
    }
}






