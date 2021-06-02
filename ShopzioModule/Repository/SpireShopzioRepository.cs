using Dapper;
using OfficeOpenXml;
using ShopzioModule.Models;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SpireHL.Core.Repository
{
    public class SpireShopzioRepository : BaseInventoryRepository
    {
        public SpireShopzioRepository()
        {

        }

        public List<SpireShopzioItem> GetSpireShopzioItemsFromExcel(string pathToFile, ShopzioCreateOrderUserOptions userOptions)
        {
            var spireItemsFromExcel = ReadDataFromCatalogExcel(pathToFile);

            var sql = BaseCatalogSQL + @" and inv.part_no in ({0})";

            using (var connection = GetConnection())
            {
                connection.Open();
                int num = 0;
                List<string> values = new List<string>();

                var parameters = new DynamicParameters();
                foreach (var item in spireItemsFromExcel)
                {
                    string parameterName = "@parameter" + num;
                    parameters.Add(parameterName, item.PartNo.ToString(), DbType.String);
                    values.Add(parameterName);
                    num++;
                }

                string selectCommandText = string.Format(sql, string.Join(",", values.ToArray()));

                var itemFromDb = connection.Query<SpireShopzioItem>(selectCommandText, parameters);
                foreach (var item in itemFromDb)
                {
                    var current = spireItemsFromExcel.Single(i => i.PartNo == item.PartNo);
                    if (userOptions.IsPriceOverride)
                    {
                        current.CopyPropertiesTo(item, "Price");
                    }

                    if (userOptions.IsQuantityOverride)
                    {
                        current.CopyPropertiesTo(item, "ShopzioOrderQty");
                    }
                }
                return itemFromDb.ToList();
            }
        }

        private List<SpireShopzioExcelItem> ReadDataFromCatalogExcel(string pathToFile)
        {
            FileInfo existingFile = new FileInfo(pathToFile);

            if (!existingFile.Exists)
            {
                throw new Exception($"File {existingFile} is not exist");
            }

            var items = new List<SpireShopzioExcelItem>();

            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                worksheet.TrimLastEmptyRows();
                var dataTable = worksheet.ToDataTable();

                items = dataTable.AsEnumerable().Select(m =>
                {
                    var result = new SpireShopzioExcelItem();

                    result.PartNo = m.Field<string>("PartNo");

                    if (dataTable.Columns.Contains("Price1") && !string.IsNullOrEmpty(m.Field<string>("Price1")))
                    {
                        result.Price1 = Convert.ToDecimal(m.Field<string>("Price1"));
                    }
                    if (dataTable.Columns.Contains("Price2") && !string.IsNullOrEmpty(m.Field<string>("Price2")))
                    {
                        result.Price2 = Convert.ToDecimal(m.Field<string>("Price2"));
                    }
                    if (dataTable.Columns.Contains("Price3") && !string.IsNullOrEmpty(m.Field<string>("Price3")))
                    {
                        result.Price3 = Convert.ToDecimal(m.Field<string>("Price3"));
                    }
                    if (dataTable.Columns.Contains("ShopzioOrderQty") && !string.IsNullOrEmpty(m.Field<string>("ShopzioOrderQty")))
                    {
                        result.ShopzioOrderQty = Convert.ToInt32(m.Field<string>("ShopzioOrderQty"));
                    }

                    return result;
                }).ToList();
            }

            return items;
        }
    }
}
