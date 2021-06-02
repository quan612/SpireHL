using Dapper;
using EComModule.Models.Lowes;
using SpireHL.Core.Models;
using SpireHL.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EComModule.Repository
{
    public class EComRepository : BaseInventoryRepository
    {
        public EComRepository()
        {

        }

        public List<LowesCsv> GetItemsWeight(List<LowesCsv> lowesList)
        {
            var listOfItemNo = from item in lowesList select item.PKG_CUSTOM1;
            var itemsFromdb = GetSpireItemsBasedOnSubQuery(listOfItemNo.ToList());
            var customList = new List<LowesCsv>();

            foreach (var lowes in lowesList)
            {
                var orderQty = Convert.ToInt32(lowes.PKG_CUSTOM5);
                SpireItem current;
                try
                {
                    current = itemsFromdb.FirstOrDefault(item => lowes.PKG_CUSTOM1 == item.PartNo);
                }
                catch (Exception)
                {
                    throw;
                }
                if (current == null)
                {
                    throw new Exception($"Cannot find item {lowes.PKG_CUSTOM1}");
                }

                lowes.PKG_WEIGHT_ACTUAL = current.Weight.ToString();

                if (orderQty > 1)
                {
                    decimal d = 2;

                    // case when in 2 boxes
                    if (!string.IsNullOrEmpty(current.UDFData.BoxSize) && current.UDFData.BoxSize.Contains("TWO BOXES"))
                    {
                        var numberOfItem = 2;

                        for (int i = 1; i <= numberOfItem; i++)
                        {
                            customList.Add(lowes);
                        }

                        if (current.Weight >= d)
                        {
                            for (int i = 1; i <= orderQty; i++)
                            {
                                customList.Add(lowes);
                            }
                        }
                    }

                    // case when not in two boxes
                    else
                    {
                        if (current.Weight >= d)
                        {
                            for (int i = 1; i <= orderQty; i++)
                            {
                                customList.Add(lowes);
                            }
                        }

                        if (current.Weight < 1)
                        {
                            var numberOfBoxes = Math.Round((decimal)orderQty / 2);
                            for (int i = 1; i <= numberOfBoxes; i++)
                            {
                                customList.Add(lowes);
                            }
                        }

                        if (current.Weight > 1 && current.Weight < d)
                        {
                            customList.Add(lowes);
                        }
                    }

                }

                if (orderQty == 1) //&& (!string.IsNullOrEmpty(current.UDFData.BoxSize) && !current.UDFData.BoxSize.Contains("TWO BOXES")))
                {
                    // case when in two boxes
                    if (!string.IsNullOrEmpty(current.UDFData.BoxSize) && current.UDFData.BoxSize.Contains("TWO BOXES"))
                    {
                        var numberOfItem = 2;

                        for (int i = 1; i <= numberOfItem; i++)
                        {
                            customList.Add(lowes);
                        }
                    }
                    // // case when NOT in two boxes
                    else
                    {
                        customList.Add(lowes);
                    }

                }

            }

            return customList;
        }

        public List<LowesCsv> DuplicateLowesRecordsForShipping(List<LowesCsv> lowesList)
        {
            var listOfItemNo = from item in lowesList select item.PKG_CUSTOM1;
            var itemsFromdb = GetSpireItemsBasedOnSubQuery(listOfItemNo.ToList());

            foreach (var lowes in lowesList)
            {
                var currentItemFromDb = itemsFromdb.Single(item => lowes.PKG_CUSTOM1 == item.PartNo);

                //lowes.PKG_WEIGHT_ACTUAL = current.Weight.ToString();
            }

            return lowesList;
        }

        public object GetPropValue(object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        private List<SpireItem> GetSpireItemsBasedOnSubQuery(List<string> items)
        {
            var sql = BaseCatalogSQL + @" and inv.part_no in ({0})";

            using (var connection = GetConnection())
            {
                connection.Open();
                int num = 0;
                List<string> values = new List<string>();

                var parameters = new DynamicParameters();
                foreach (var item in items)
                {
                    string parameterName = "@parameter" + num;
                    parameters.Add(parameterName, item.ToString(), DbType.String);
                    values.Add(parameterName);
                    num++;
                }

                string selectCommandText = string.Format(sql, string.Join(",", values.ToArray()));

                var itemFromDb = connection.Query<SpireItem>(selectCommandText, parameters);
                var test = itemFromDb.Select(e => e.PartNo);
                var diff = items.Except(test);
                return itemFromDb.ToList();
            }
        }

        //public List<SpireShopzioItem> GetSpireShopzioItemsFromExcel(string pathToFile, ShopzioCreateOrderUserOptions userOptions)
        //{
        //    var spireItemsFromExcel = ReadDataFromCatalogExcel(pathToFile);

        //    var sql = BaseCatalogSQL + @" and inv.part_no in ({0})";

        //    using (var connection = new NpgsqlConnection(HilineConnectionString))
        //    {
        //        connection.Open();
        //        int num = 0;
        //        List<string> values = new List<string>();

        //        var parameters = new DynamicParameters();
        //        foreach (var item in spireItemsFromExcel)
        //        {
        //            string parameterName = "@parameter" + num;
        //            parameters.Add(parameterName, item.PartNo.ToString(), DbType.String);
        //            values.Add(parameterName);
        //            num++;
        //        }

        //        string selectCommandText = string.Format(sql, string.Join(",", values.ToArray()));

        //        var itemFromDb = connection.Query<SpireShopzioItem>(selectCommandText, parameters);
        //        foreach (var item in itemFromDb)
        //        {
        //            var current = spireItemsFromExcel.Single(i => i.PartNo == item.PartNo);
        //            if (userOptions.IsPriceOverride)
        //            {
        //                current.CopyPropertiesTo(item, "Price");
        //            }

        //            if (userOptions.IsQuantityOverride)
        //            {
        //                current.CopyPropertiesTo(item, "ShopzioOrderQty");
        //            }
        //        }
        //        return itemFromDb.ToList();
        //    }
        //}

        //private List<SpireShopzioExcelItem> ReadDataFromCatalogExcel(string pathToFile)
        //{
        //    FileInfo existingFile = new FileInfo(pathToFile);

        //    if (!existingFile.Exists)
        //    {
        //        throw new Exception($"File {existingFile} is not exist");
        //    }

        //    var items = new List<SpireShopzioExcelItem>();

        //    using (ExcelPackage package = new ExcelPackage(existingFile))
        //    {
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
        //        worksheet.TrimLastEmptyRows();
        //        var dataTable = worksheet.ToDataTable();

        //        items = dataTable.AsEnumerable().Select(m =>
        //        {
        //            var result = new SpireShopzioExcelItem();

        //            result.PartNo = m.Field<string>("PartNo");

        //            if (dataTable.Columns.Contains("Price1") && !string.IsNullOrEmpty(m.Field<string>("Price1")))
        //            {
        //                result.Price1 = Convert.ToDecimal(m.Field<string>("Price1"));
        //            }
        //            if (dataTable.Columns.Contains("Price2") && !string.IsNullOrEmpty(m.Field<string>("Price2")))
        //            {
        //                result.Price2 = Convert.ToDecimal(m.Field<string>("Price2"));
        //            }
        //            if (dataTable.Columns.Contains("Price3") && !string.IsNullOrEmpty(m.Field<string>("Price3")))
        //            {
        //                result.Price3 = Convert.ToDecimal(m.Field<string>("Price3"));
        //            }
        //            if (dataTable.Columns.Contains("ShopzioOrderQty") && !string.IsNullOrEmpty(m.Field<string>("ShopzioOrderQty")))
        //            {
        //                result.ShopzioOrderQty = Convert.ToInt32(m.Field<string>("ShopzioOrderQty"));
        //            }

        //            return result;
        //        }).ToList();
        //    }

        //    return items;
        //}
    }
}
