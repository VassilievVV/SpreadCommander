using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Extensions
{
    public static class DataSetExtensions
    {
        public static void ConvertColumnType(this DataTable dt, string columnName, Type newType)
        {
            using DataColumn dc = new DataColumn(Guid.NewGuid().ToString(), newType);
            // Add the new column which has the new type, and move it to the ordinal of the old column
            int ordinal = dt.Columns[columnName].Ordinal;
            dt.Columns.Add(dc);
            dc.SetOrdinal(ordinal);

            // Get and convert the values of the old column, and insert them into the new
            foreach (DataRow dr in dt.Rows)
            {
                var value = dr[columnName];
                if (value != DBNull.Value)
                    dr[dc.ColumnName] = Utils.ChangeType(newType, value);
            }

            // Remove the old column
            dt.Columns.Remove(columnName);

            // Give the new column the old column's name
            dc.ColumnName = columnName;
        }

        public static void ConvertColumnType(this DataColumn column, Type newType)
        {
            var dt = column.Table;
            var columnName = column.ColumnName;

            using DataColumn dc = new DataColumn(Guid.NewGuid().ToString(), newType);
            // Add the new column which has the new type, and move it to the ordinal of the old column
            int ordinal = column.Ordinal;
            dt.Columns.Add(dc);
            dc.SetOrdinal(ordinal);

            // Get and convert the values of the old column, and insert them into the new
            foreach (DataRow dr in dt.Rows)
            {
                var value = dr[column];
                if (value != DBNull.Value)
                    dr[dc.ColumnName] = Utils.ChangeType(newType, value, DBNull.Value);
            }

            // Remove the old column
            dt.Columns.Remove(column);

            // Give the new column the old column's name
            dc.ColumnName = columnName;
        }
    }
}
