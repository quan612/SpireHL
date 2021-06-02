using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SpireHL.Core.Extensions
{
    public static class ExcelToObjects
    {
        public static DataTable ToDataTable(this ExcelWorksheet workSheet)
        {
            DataTable dt = new DataTable();
            //create a list to hold the column names
            List<string> columnNames = new List<string>();

            //needed to keep track of empty column headers
            int currentColumn = 1;

            //loop all columns in the sheet and add them to the datatable
            foreach (var cell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                string columnName = cell.Text.Trim();

                //check if the previous header was empty and add it if it was
                if (cell.Start.Column != currentColumn)
                {
                    columnNames.Add("Header_" + currentColumn);
                    dt.Columns.Add("Header_" + currentColumn);
                    currentColumn++;
                }

                //add the column name to the list to count the duplicates
                columnNames.Add(columnName);

                //count the duplicate column names and make them unique to avoid the exception
                //A column named 'Name' already belongs to this DataTable
                int occurrences = columnNames.Count(x => x.Equals(columnName));
                if (occurrences > 1)
                {
                    columnName = columnName + "_" + occurrences;
                }

                //add the column to the datatable
                dt.Columns.Add(columnName);

                currentColumn++;
            }

            //start adding the contents of the excel file to the datatable
            for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
            {
                var row = workSheet.Cells[i, 1, i, workSheet.Dimension.End.Column];
                DataRow newRow = dt.NewRow();

                //loop all cells in the row
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }

                dt.Rows.Add(newRow);
            }

            return dt;
        }

        public static void TrimLastEmptyRows(this ExcelWorksheet worksheet)
        {
            while (worksheet.IsLastRowEmpty())
                worksheet.DeleteRow(worksheet.Dimension.End.Row);
        }

        public static bool IsLastRowEmpty(this ExcelWorksheet worksheet)
        {
            var empties = new List<bool>();

            for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
            {
                var rowEmpty = worksheet.Cells[worksheet.Dimension.End.Row, i].Value == null ? true : false;
                empties.Add(rowEmpty);
            }

            return empties.All(e => e);
        }
    }
}
