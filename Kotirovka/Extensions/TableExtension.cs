using Kotirovka.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace Kotirovka.Extensions
{
    static class TableExtension
    {

        public static DataTable ToDataTable()
        {
            DataTable table = new DataTable();
            DataColumn[] cols ={
                                 
                                  new DataColumn("NumCode",typeof(string)),
                                  new DataColumn("CharCode",typeof(string)),
                                  new DataColumn("Nominal",typeof(uint)),
                                  new DataColumn("Name",typeof(string)),
                                  new DataColumn("Value",typeof(decimal)),
                                  new DataColumn("Previous", typeof(decimal))
            };
            table.Columns.AddRange(cols);


            return table;
        }

        public static void Insert(DataTable table,List<Valute> Data)
        {
            foreach(var val in Data)
            {
                var row = table.NewRow();
                row["NumCode"] = val.NumCode;
                row["CharCode"] = val.CharCode;
                row["Nominal"] = val.Nominal;
                row["Name"] = val.Name;
                row["Value"] = val.Value;
                row["Previous"] = val.Previous;
                table.Rows.Add(row);
            }
           
        }
    } 
}