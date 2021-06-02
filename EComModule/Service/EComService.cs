using EComModule.FieldDelimited;
using EComModule.Models.Lowes;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SpireHL.Core.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EComModule.Service
{
    public class EComService
    {
        private const string LowesCanadaExportPathParams = "LowesCanadaExportPath";
        private readonly char[] quotes = { '\"', ' ' };
        private Deserializer deserializer;
        public EComService()
        {
            deserializer = new Deserializer();
        }
        public List<LowesCsv> ReadLowesCsvOrderFile(string pathToFile)
        {
            var lineArray = File.ReadAllLines(pathToFile).Skip(1);
            var lowesList = new List<LowesCsv>();
            foreach (var line in lineArray)
            {
                var current = line
                .Split(',')
                .Select(s => s.Trim(quotes).Replace("\\\"", "\""))
                .ToArray();

                var record = deserializer.ToObject<LowesCsv>(current);
                lowesList.Add(record);
            }

            var lowesFilter = lowesList.Where(el => el.PKG_CUSTOM4.StartsWith("LLP"));
            return lowesFilter.ToList();
        }

        public List<LowesMVKFShippingOrder> BuildLowesMVKFTemplate(List<LowesCsv> lowesItems)
        {
            var lowesMVKF = new List<LowesMVKFShippingOrder>();
            foreach (var item in lowesItems)
            {
                var shippingItem = new LowesMVKFShippingOrder()
                {
                    OrderId = item.PKG_PACKAGE_ID,
                    Attention = item.SHPTO_NAME,
                    CustName = item.SHPTO_NAME,
                    AddressLine1 = item.SHPTO_ADDRESS_1,
                    AddressLine2 = item.SHPTO_ADDRESS_2,
                    AddressLine3 = item.SHPTO_ADDRESS_3,
                    City = item.SHPTO_CITY,
                    Province = item.SHPTO_STATE_PROV,
                    PostalCode = item.SHPTO_POSTAL_CODE,
                    Country = item.SHPTO_COUNTRY_ID,
                    Phone = item.SHPTO_TELEPHONE,
                    WGT = item.PKG_WEIGHT_ACTUAL,
                    Reference = item.PKG_CUSTOM4
                };

                lowesMVKF.Add(shippingItem);
            }

            return lowesMVKF;
        }
        public string MakeLowesShippingMVKF(List<LowesMVKFShippingOrder> lowesMVKF, string sourceFile="")
        {
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            AddExcelStyle(workSheet);
            BuildLowesShippingOrderHeader(workSheet);
            AppendData(workSheet, lowesMVKF);
            workSheet.DefaultColWidth = 20.0;

            string sourceFileName=string.Empty;
            if(!string.IsNullOrEmpty(sourceFile))
            {
                sourceFileName = Path.GetFileNameWithoutExtension(sourceFile);
            }
            else
            {
                sourceFileName = DateTime.Now.ToString("yyyy-MM-dd");
            }
            
            string fileName = "SHIPOrderFile-Lowes_MVKF - " + sourceFileName + ".xlsx";

            var configs = ModuleConfigs.GetConfigs("Inventory", "Catalog");
            var lowesCaPath = configs.Find(e => e.ParameterName == LowesCanadaExportPathParams).ParameterValue + @"\";

            var fullPath = lowesCaPath + fileName;

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            // Create excel file on physical disk 
            FileStream objFileStrm = File.Create(fullPath);
            objFileStrm.Close();

            // Write content to excel file 
            File.WriteAllBytes(fullPath, excel.GetAsByteArray());
            //Close Excel package
            excel.Dispose();

            return fullPath;
        }

        private void AddExcelStyle(ExcelWorksheet workSheet)
        {
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            // Setting the properties
            // of the first row
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
        }
        private void BuildLowesShippingOrderHeader(ExcelWorksheet workSheet)
        {
            // Header of the Excel sheet
            workSheet.Cells[1, 1].Value = @"S/P/I";
            workSheet.Cells[1, 2].Value = @"I/A/Q";
            workSheet.Cells[1, 3].Value = @"OrderID/SHIP CODE";
            workSheet.Cells[1, 4].Value = "CARRIER";
            workSheet.Cells[1, 5].Value = "Attention";
            workSheet.Cells[1, 6].Value = "CUST Name";
            workSheet.Cells[1, 7].Value = "Address Line 1";
            workSheet.Cells[1, 8].Value = "Address Line 2";
            workSheet.Cells[1, 9].Value = "Address Line 3";
            workSheet.Cells[1, 10].Value = "City";
            workSheet.Cells[1, 11].Value = "Province";
            workSheet.Cells[1, 12].Value = "PostalCode";
            workSheet.Cells[1, 13].Value = "Country";
            workSheet.Cells[1, 14].Value = "Phone";
            workSheet.Cells[1, 15].Value = "SERVICE Air=Express GND=Ground";
            workSheet.Cells[1, 16].Value = "CUST PACKAGE P=EXPRESS PACK L=EXPRESS LETTER U=CUST OWN PACK";
            workSheet.Cells[1, 17].Value = "PAYMENT TERMS (P-PRE PAID, C-COLLECT, 3-THIRD PARTY)";
            workSheet.Cells[1, 18].Value = "PCS.";
            workSheet.Cells[1, 19].Value = "WGT.";
            workSheet.Cells[1, 20].Value = "Reference";
            workSheet.Cells[1, 21].Value = "Reference2";
            workSheet.Cells[1, 22].Value = "Reference3";
            workSheet.Cells[1, 23].Value = "NOTES";
            workSheet.Cells[1, 24].Value = "Printer ID";
        }

        private void AppendData(ExcelWorksheet workSheet, List<LowesMVKFShippingOrder> lowesMVKF)
        {
            int recordIndex = 2;

            foreach (var item in lowesMVKF)
            {
                workSheet.Cells[recordIndex, 1].Value = item.SPI;
                workSheet.Cells[recordIndex, 2].Value = item.IAQ;
                workSheet.Cells[recordIndex, 3].Value = item.OrderId;
                workSheet.Cells[recordIndex, 4].Value = item.Carrier;
                workSheet.Cells[recordIndex, 5].Value = item.Attention;
                workSheet.Cells[recordIndex, 6].Value = item.CustName;
                workSheet.Cells[recordIndex, 7].Value = item.AddressLine1;
                workSheet.Cells[recordIndex, 8].Value = item.AddressLine2;
                workSheet.Cells[recordIndex, 9].Value = item.AddressLine3;
                workSheet.Cells[recordIndex, 10].Value = item.City;
                workSheet.Cells[recordIndex, 11].Value = item.Province;
                workSheet.Cells[recordIndex, 12].Value = item.PostalCode;
                workSheet.Cells[recordIndex, 13].Value = item.Country;
                workSheet.Cells[recordIndex, 14].Value = item.Phone;
                workSheet.Cells[recordIndex, 15].Value = item.Service;
                workSheet.Cells[recordIndex, 16].Value = item.CustPackage;
                workSheet.Cells[recordIndex, 17].Value = item.PaymentTerms;
                workSheet.Cells[recordIndex, 18].Value = item.PCS;
                workSheet.Cells[recordIndex, 19].Value = item.WGT;
                workSheet.Cells[recordIndex, 20].Value = item.Reference;
                workSheet.Cells[recordIndex, 21].Value = item.Reference2;
                workSheet.Cells[recordIndex, 22].Value = item.Reference3;
                workSheet.Cells[recordIndex, 23].Value = item.Notes;
                workSheet.Cells[recordIndex, 24].Value = item.PrinterId;

                recordIndex++;
            }
        }
    }
}
