using CatalogModule.Contracts;
using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CatalogModule.Services.Word
{
    public class InternalCatalogService : BaseWordService
    {
        public override string SourceTemplate => ProjectDirectory + "InternalCatalogTemplate.docx";
        public override string ExportTemplate
        {
            get
            {
                return "Internal Catalog_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".docx";
            }
        }
        public InternalCatalogService(IUserDefine userDefine) : base(userDefine)
        {
        }

        protected override void AppendItemsToDataTable<T>(IEnumerable<T> items)
        {
            foreach (var item in items.ToList() as List<SpireItem>)
            {
                DataRow row = DataTableItems.NewRow();

                row["ProductImage"] = item.ProductImage;
                row["PartNo"] = item.PartNo;
                row["Barcode"] = item.PartNo;

                row["Price1"] = UserDefine.Price1 ? item.Price1 : (object)DBNull.Value;
                row["Price2"] = UserDefine.Price2 ? item.Price2 : (object)DBNull.Value;
                row["Price3"] = UserDefine.Price3 ? item.Price3 : (object)DBNull.Value;

                row["PcCtn"] = UserDefine.PcCtn ? item.UDFData.PcCtn : (object)DBNull.Value;
                row["UPC"] = UserDefine.UPC ? item.UPC : (object)DBNull.Value;

                row["CubeCtn"] = UserDefine.CubeCtn ? item.UDFData.CubeCtn : (object)DBNull.Value;
                row["CBMCtn"] = UserDefine.CBMCtn ? item.UDFData.CBMCtn : (object)DBNull.Value;
                row["VendorCode"] = UserDefine.VendorCode ? item.VendorCode : (object)DBNull.Value;

                row["Description"] = UserDefine.Description && item.Description != null ?
                    item.Description.Trim() : (object)DBNull.Value;

                row["HS"] = UserDefine.HS && item.UDFData.HS != null ?
                    item.UDFData.HS.Trim() : (object)DBNull.Value;

                row["MBoxL"] = UserDefine.MBox ? item.UDFData.MBoxL : (object)DBNull.Value;
                row["MBoxW"] = UserDefine.MBox ? item.UDFData.MBoxW : (object)DBNull.Value;
                row["MBoxH"] = UserDefine.MBox ? item.UDFData.MBoxH : (object)DBNull.Value;
                row["IBoxL"] = UserDefine.IBox ? item.UDFData.IBoxL : (object)DBNull.Value;
                row["IBoxW"] = UserDefine.IBox ? item.UDFData.IBoxW : (object)DBNull.Value;
                row["IBoxH"] = UserDefine.IBox ? item.UDFData.IBoxH : (object)DBNull.Value;
                row["Length"] = UserDefine.ItemSize ? item.UDFData.Length + "\"" : (object)DBNull.Value;
                row["Width"] = UserDefine.ItemSize ? item.UDFData.Width + "\"" : (object)DBNull.Value;
                row["Height"] = UserDefine.ItemSize ? item.UDFData.Height + "\"" : (object)DBNull.Value;

                row["MinOrder"] = UserDefine.MinOrder && item.MinOrder >= 0 ? item.MinOrder : (object)DBNull.Value;
                row["CDNTire"] = UserDefine.CDNTire ? item.UDFData.CDNTire : (object)DBNull.Value;
                row["PurchaseQty"] = UserDefine.OnOrderQty ? item.PurchaseQty : (object)DBNull.Value;

                row["ProductCode"] = UserDefine.ProductCode ? item.ProductCode : (object)DBNull.Value;
                row["MOP"] = UserDefine.MOP && item.UDFData.MOP == "true" ? "MOP" : (object)DBNull.Value;
                row["InventoryType"] = UserDefine.InventoryType ? item.InventoryType : (object)DBNull.Value;
                row["OnHandQty"] = UserDefine.OnHandQty ? item.OnHandQty : (object)DBNull.Value;

                row["OnSale"] = item.Price1 < item.Price3 ? "On Sale" : (object)DBNull.Value;
                DataTableItems.Rows.Add(row);
            }
        }
    }
}
