using System;
using System.Collections.Generic;
using System.Data;
using System.Management.Automation;
using System.Text;
using System.Linq;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsData.ConvertTo, "UnPivot")]
    [OutputType(typeof(DataTable))]
    public class ConvertToUnPivotCmdlet : SCCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for data tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Columns from data source that will remain in un-pivot table.")]
        [Alias("Primary")]
        public string[] PrimaryColumns { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Column name for headers of un-pivoting columns.")]
        public string UnPivotColumnName { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Column name for values of un-pivoting columns.")]
        public string UnPivotValueColumnName { get; set; }

        [Parameter(Mandatory = true, Position = 3, HelpMessage = "Type of values in un-pivot column.")]
        public Type UnPivotValueType { get; set; }

        [Parameter(HelpMessage = "Columns that are neither primary columns nor un-pivot columns.")]
        [Alias("Ignore")]
        public string[] IgnoreColumns { get; set; }

        [Parameter(HelpMessage = "Data source.")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values.")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "If set - objects will be sending individually into pipeline.")]
        public SwitchParameter EnumerateCollection { get; set; }


        private readonly List<PSObject> _Input = new List<PSObject>();

        protected override void BeginProcessing()
        {
            _Input.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Input.Add(obj);
        }

        protected override void EndProcessing()
        {
            var result = ConvertList();
            WriteObject(result, EnumerateCollection);
        }

        protected DataTable ConvertList()
        {
            if (PrimaryColumns == null || PrimaryColumns.Length <= 0)
                throw new ArgumentNullException(nameof(PrimaryColumns));
            if (string.IsNullOrWhiteSpace(UnPivotColumnName))
                throw new ArgumentNullException(nameof(UnPivotColumnName));
            if (string.IsNullOrWhiteSpace(UnPivotValueColumnName))
                throw new ArgumentException(nameof(UnPivotValueColumnName));
            if (UnPivotValueType == null)
                throw new ArgumentNullException(nameof(UnPivotValueType));

            using var reader = GetDataSourceReader(_Input, DataSource, IgnoreErrors);
            if (reader == null)
                throw new Exception("Input data are not provided.");

            using var sourceTable = new DataTable("Source");
            sourceTable.Load(reader);

            var primaryColumns = PrimaryColumns.Select(pc => sourceTable.Columns[pc]).ToArray();

            if (primaryColumns.Length <= 0 || primaryColumns.Contains(null))
                throw new ArgumentException("Cannot find one or more of primary columns.");

            var ignoreColumns = IgnoreColumns?.Select(ic => sourceTable.Columns[ic]).ToArray() ?? new DataColumn[0];

            var result = UnPivot(sourceTable, primaryColumns, ignoreColumns, 
                UnPivotColumnName, UnPivotValueColumnName, UnPivotValueType, IgnoreErrors);
            return result;
        }

        private static DataTable UnPivot(DataTable dt, DataColumn[] primaryColumns, DataColumn[] ignoreColumns,
            string unPivotColumnName, string unPivotValueColumnName, Type unPivotValueType,
            bool ignoreErrors)
        {
            string[] pkColumnNames = primaryColumns
                .Select(c => c.ColumnName)
                .ToArray();

            var result = new DataTable("Pivot");
            foreach (var column in primaryColumns)
                result.Columns.Add(column.ColumnName, column.DataType);
            result.Columns.Add(unPivotColumnName, typeof(string));
            result.Columns.Add(unPivotValueColumnName, unPivotValueType);

            var unPivotColumns = new List<DataColumn>();
            foreach (DataColumn column in dt.Columns)
            {
                if (!primaryColumns.Contains(column) && !ignoreColumns.Contains(column))
                    unPivotColumns.Add(column);
            }

            foreach (DataRow row in dt.Rows)
            {
                foreach (var column in unPivotColumns)
                {
                    var newRow = result.NewRow();

                    foreach (var primaryColumn in primaryColumns)
                        newRow[primaryColumn.ColumnName] = row[primaryColumn.ColumnName];

                    newRow[unPivotColumnName] = column.ColumnName;

                    var value = row[column.ColumnName];
                    try
                    {
                        if (value is string && string.IsNullOrWhiteSpace((string)value) && unPivotValueType != typeof(string))
                            value = DBNull.Value;

                        if (value != null && value != DBNull.Value)
                            newRow[unPivotValueColumnName] = Utils.ChangeType(unPivotValueType, value, DBNull.Value);
                        else
                            newRow[unPivotValueColumnName] = DBNull.Value;
                    }
                    catch (Exception ex)
                    {
                        if (ignoreErrors)
                            newRow[unPivotValueColumnName] = DBNull.Value;
                        else
                            throw ex;
                    }

                    result.Rows.Add(newRow);
                }
            }

            return result;
        }
    }
}
