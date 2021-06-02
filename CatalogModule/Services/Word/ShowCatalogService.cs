using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CatalogModule.Services.Word
{
    public class ShowCatalogService : BaseWordService
    {
        public override string SourceTemplate => ProjectDirectory + "ShowCatalogTemplate.docx";
        public override string ExportTemplate
        {
            get
            {
                return "Show Catalog_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".docx";
            }
        }
        public ShowCatalogService()
        {
        }

        protected override void AppendItemsToDataTable<T>(IEnumerable<T> items)
        {
            foreach (var item in items.ToList() as List<SpireItem>)
            {
                DataRow row = DataTableItems.NewRow();

                row["ProductImage"] = item.ProductImage;

                row["PartNo"] = item.PartNo;
                row["IsNew"] = item.InventoryType.Contains("SN") ? "New" : (object)DBNull.Value;

                row["Barcode"] = item.PartNo;

                row["Price1"] = item.Price1;
                row["Price2"] = item.Price2;
                row["Price3"] = item.Price3;

                row["OnHandQty"] = item.OnHandQty;

                if (item.PartNo.Contains("PH"))
                {
                    row["InStock"] = item.OnHandQty > 0 ? "In Stock" : "Pre-order only";
                }
                else
                {
                    row["InStock"] = item.OnHandQty > 0 ? "In Stock" : (object)DBNull.Value;
                }

                row["ArrivalDate"] = !string.IsNullOrEmpty(item.ArrivalDate) && item.OnHandQty < 1 ? item.ArrivalDate : (object)DBNull.Value;

                row["Description"] = item.Description;

                row["Length"] = item.UDFData.Length + "\"";
                row["Width"] = item.UDFData.Width + "\"";
                row["Height"] = item.UDFData.Height + "\"";

                row["PcCtn"] = item.UDFData.PcCtn;

                row["MinOrder"] = item.MinOrder <= 0 ? (object)DBNull.Value : item.MinOrder;
                row["CDNTire"] = item.UDFData.CDNTire;
                row["MOP"] = item.UDFData.MOP == "true" ? "MOP" : (object)DBNull.Value;
                row["OnSale"] = item.Price1 < item.Price3 ? "On Sale" : (object)DBNull.Value;
                DataTableItems.Rows.Add(row);
            }
        }
    }
}
