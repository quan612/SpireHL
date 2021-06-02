using Dapper;
using Npgsql;
using OfficeOpenXml;
using SpireHL.Core.Contracts;
using SpireHL.Core.Extensions;
using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SpireHL.Core.Repository
{
    public class InventoryRepository : BaseInventoryRepository
    {
        public InventoryRepository()
        {

        }

        public List<SpireItem> GetSpireItemsFromExcel(string pathToFile)
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

                var itemFromDb = connection.Query<SpireItem>(selectCommandText, parameters);
                foreach (var item in itemFromDb)
                {
                    var current = spireItemsFromExcel.Single(i => i.PartNo == item.PartNo);
                    current.CopyPropertiesTo(item, "Price");
                    current.CopyPropertiesTo(item, "ShopzioOrderQty");
                }
                return itemFromDb.ToList();
            }
        }

        private List<SpireExcelItem> ReadDataFromCatalogExcel(string pathToFile)
        {
            FileInfo existingFile = new FileInfo(pathToFile);

            if (!existingFile.Exists)
            {
                throw new Exception($"File {existingFile} is not exist");
            }

            List<SpireExcelItem> items = new List<SpireExcelItem>();

            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                worksheet.TrimLastEmptyRows();
                var dataTable = worksheet.ToDataTable();

                items = dataTable.AsEnumerable().Select(m =>
                {
                    var result = new SpireExcelItem();

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

                    return result;
                }).ToList();
            }

            return items;
        }

        public List<SpireItem> GetSpireItemsFromDatabase(IInventory inventoryType)
        {
            var sql = BaseCatalogSQL + inventoryType.Query;

            using (var connection = GetConnection())
            {
                connection.Open();
                var result = connection.Query<SpireItem>(sql);
                return result.ToList();
            }
        }

        public List<SpireItem> GetSpireItemsSaleAnalysisFromDatabase(IInventory inventoryType)
        {
            var itemsSql = BaseCatalogSQL + inventoryType.Query;
            var analysisSql = BaseSaleAnalysisSQL;

            using (var connection = GetConnection())
            {
                connection.Open();
                var saleAnalysisData = connection.Query<SpireSaleAnalysisData>(analysisSql);
                var spireItems = connection.Query<SpireItem>(itemsSql);

                foreach (var item in spireItems)
                {
                    item.ProductImageExcel = File.ReadAllBytes(item.ProductImage);
                    item.SaleAnalysisData = saleAnalysisData
                        .OrderByDescending(e => e.SaleAnalysisYearEnd)
                        .Where(el => el.PartNo == item.PartNo)
                        .ToList();
                }
                return spireItems.ToList();
            }
        }
    }
}
