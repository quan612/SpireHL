using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CatalogModule.Services.Excel
{
    public class SaleOrderService : BaseExcelService
    {
        public override string SourceTemplate => ProjectDirectory + "SaleOrder.xlsx";
        public override string ExportTemplate
        {
            get
            {
                return "Sale Order_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".xlsx";
            }
        }

        public SaleOrderService() : base()
        {

        }

        protected override void AppendItemsToDataTable<T>(IEnumerable<T> items)
        {
            DataTableItems.Columns.Remove("ProductImage");
            DataTableItems.Columns.Add("ProductImage", typeof(byte[]));

            DataTableItems.Columns.Remove("Length");
            DataTableItems.Columns.Remove("Width");
            DataTableItems.Columns.Remove("Height");
            DataTableItems.Columns.Add("Dimension", typeof(string));

            foreach (var item in items.ToList() as List<SpireSaleOrderItem>)
            {
                DataRow row = DataTableItems.NewRow();

                if (!string.IsNullOrEmpty(item.ProductImage))
                {
                    row["ProductImage"] = File.ReadAllBytes(item.ProductImage);
                }

                row["PartNo"] = item.PartNo;
                row["UnitPrice"] = item.UnitPrice;
                //row["Price2"] = item.Price2;
                //row["Price3"] = item.Price3;

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
                row["Dimension"] = item.UDFData.Length + "\" x " + item.UDFData.Width + "\" x " + item.UDFData.Height + "\"";
                row["OrderQty"] = item.OrderQty;
                DataTableItems.Rows.Add(row);
            }
        }
    }
}
