using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CatalogModule.Services.Word
{
    public class SimpleCanadaCatalogService : BaseWordService
    {
        public override string SourceTemplate => ProjectDirectory + "SimpleCanadaTemplate.docx";
        public override string ExportTemplate
        {
            get
            {
                return "SimpleCanada_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".docx";
            }
        }
        public SimpleCanadaCatalogService()
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

                DataTableItems.Rows.Add(row);
            }
        }
    }
}
