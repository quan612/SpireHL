using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

/// <summary>
/// Extensions for List in linq
/// </summary>
namespace SpireHL.Core.Extensions
{
    public static class Lists
    {
        public static DataTable ListToDataTable<T>(this IEnumerable<T> lst, string tableName = "")
        {
            var currentDT = CreateTable<T>(tableName);
            
            // Notes: only in case we want to set value for rows, but currently we do it manually on each service
            //Type entType = typeof(T);
            //PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //foreach (T item in lst)
            //{
            //    DataRow row = currentDT.NewRow();
            //    foreach (PropertyDescriptor prop in properties)
            //    {
            //        // if current prop is a user defined class, 
            //        if (prop.PropertyType.Assembly.FullName.Equals(entType.Assembly.FullName) && prop.PropertyType.IsClass)
            //        {
            //            if (prop.PropertyType.Assembly.FullName.Equals(entType.Assembly.FullName) && prop.PropertyType.IsClass)
            //            {
            //                PropertyDescriptorCollection subProperties = TypeDescriptor.GetProperties(prop.PropertyType);

            //                // get value for sub class of current item
            //                var nestedItemObject = item.GetType().GetProperty(prop.Name);
            //                var nestedItemtValue = nestedItemObject.GetValue(item, null);

            //                foreach (PropertyDescriptor subProp in subProperties)
            //                {

            //                    SetRowValue(row, subProp, nestedItemtValue);
            //                }
            //            }
            //        }
            //        else 
            //        {
            //            SetRowValue(row, prop, item);
            //        }

            //    }
            //    currentDT.Rows.Add(row);
            //}

            return currentDT;
        }

        //private static void SetRowValue<T>(DataRow row, PropertyDescriptor prop, T item)
        //{

        //    if (prop.PropertyType == typeof(Nullable<decimal>) || prop.PropertyType == typeof(Nullable<int>) || prop.PropertyType == typeof(Nullable<Int64>))
        //    {
        //        if (prop.GetValue(item) == null)
        //            row[prop.Name] = 0;
        //        else
        //            row[prop.Name] = prop.GetValue(item);
        //    }
        //    else
        //        row[prop.Name] = prop.GetValue(item);
        //}

        public static DataTable CreateTable<T>(string tableName)
        {
            Type entType = typeof(T);

            DataTable tbl = new DataTable(tableName);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);

            foreach (PropertyDescriptor prop in properties)
            {
                // if current prop is a user defined class
                if (prop.PropertyType.Assembly.FullName.Equals(entType.Assembly.FullName) && prop.PropertyType.IsClass)
                {
                    PropertyDescriptorCollection subProperties = TypeDescriptor.GetProperties(prop.PropertyType);
                    foreach (PropertyDescriptor subProp in subProperties)
                    {
                        AddTableColumns(subProp, tbl);
                    }
                }
                else
                {
                    AddTableColumns(prop, tbl);
                }
            }
            
            return tbl;
        }

        private static void AddTableColumns(PropertyDescriptor prop, DataTable tbl)        
        {
            if (prop.PropertyType == typeof(Nullable<decimal>))
                tbl.Columns.Add(prop.Name, typeof(decimal));
            else if (prop.PropertyType == typeof(Nullable<int>))
                tbl.Columns.Add(prop.Name, typeof(int));
            else if (prop.PropertyType == typeof(Nullable<Int64>))
                tbl.Columns.Add(prop.Name, typeof(Int64));
            else
                tbl.Columns.Add(prop.Name, prop.PropertyType);
        }
    }
}
