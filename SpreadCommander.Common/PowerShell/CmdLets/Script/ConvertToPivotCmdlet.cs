using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.SqlScript;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    #region PivotAggregateFunction
    public enum PivotAggregateFunction
    {
        First,
        Last,
        Count,
        Min,
        Max,
        Sum,
        Avg        
    }
    #endregion

    [Cmdlet(VerbsData.ConvertTo, "Pivot")]
    [OutputType(typeof(DataTable))]
    public class ConvertToPivotCmdlet : SCCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for data tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Columns from data source that will remain in pivot table.")]
        public string[] PrimaryColumns { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Column that will be expanded into multiple columns.")]
        public string PivotColumn { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Column that contains values for new columns.")]
        [Alias("PivotValues", "Values")]
        public string PivotValueColumn { get; set; }

        [Parameter(Position = 3, HelpMessage = "Aggregate function for PivotValue.")]
        public PivotAggregateFunction? AggregateFunction { get; set; }

        [Parameter(HelpMessage = "Data source.")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

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
            if (string.IsNullOrWhiteSpace(PivotColumn))
                throw new ArgumentNullException(nameof(PivotColumn));
            if (string.IsNullOrWhiteSpace(PivotValueColumn))
                throw new ArgumentException(nameof(PivotValueColumn));

            using var reader = GetDataSourceReader(_Input, DataSource, new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns });
            if (reader == null)
                throw new Exception("Input data are not provided.");

            using var sourceTable = new DataTable("Source");
            sourceTable.Load(reader);

            var primaryColumns   = PrimaryColumns.Select(pc => sourceTable.Columns[pc]).ToArray();
            var pivotColumn      = sourceTable.Columns[PivotColumn];
            var pivotValueColumn = sourceTable.Columns[PivotValueColumn];

            if (primaryColumns.Length <= 0 || primaryColumns.Contains(null))
                throw new ArgumentException("Cannot find one or more of primary columns.");
            if (pivotColumn == null)
                throw new ArgumentException($"Cannot find column '{PivotColumn}'.");
            if (pivotValueColumn == null)
                throw new ArgumentException($"Cannot find column '{PivotValueColumn}'.");

            var result = Pivot(sourceTable, primaryColumns, pivotColumn, pivotValueColumn, AggregateFunction ?? PivotAggregateFunction.First);
            return result;
        }

        private static DataTable Pivot(DataTable dt, DataColumn[] primaryColumns,
            DataColumn pivotColumn, DataColumn pivotValue, PivotAggregateFunction aggregateFunction)
        {
            string pivotColumnName = pivotColumn.ColumnName;
            string pivotValueName  = pivotValue.ColumnName;

            string[] pkColumnNames = primaryColumns
                .Select(c => c.ColumnName)
                .ToArray();

            var result = dt.DefaultView.ToTable("Pivot", true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();

            dt.AsEnumerable()
                .Select(row => Convert.ToString(row[pivotColumnName]))
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .Distinct()
                .OrderBy(str => str, StringLogicalComparer.DefaultComparer)
                .ToList()
                .ForEach(c => result.Columns.Add(c, pivotValue.DataType));

            var dictValues = new Dictionary<(DataRow Row, string ColumnName), List<object>>();

            foreach (DataRow row in dt.Rows)
            {
                var pivotColumnValue = row[pivotValueName];
                if (pivotColumnValue == null || pivotColumnValue == DBNull.Value)
                    continue;

                // find row to update
                var aggRow = result.Rows.Find(
                    pkColumnNames
                        .Select(c => row[c])
                        .ToArray());

                var columnName = Convert.ToString(row[pivotColumnName]);

                if (dictValues.ContainsKey((aggRow, columnName)))
                    dictValues[(aggRow, columnName)].Add(pivotColumnValue);
                else
                    dictValues.Add((aggRow, columnName), new List<object>() { pivotColumnValue });
            }

            foreach (var keyValuePair in dictValues)
            {
                var aggValue = CalculateAggregate(aggregateFunction, keyValuePair.Value);
                keyValuePair.Key.Row[keyValuePair.Key.ColumnName] = aggValue;
            }

            return result;
        }

        private static object CalculateAggregate(PivotAggregateFunction aggregateFunction, List<object> values)
        {
            if (values.Count <= 0)
                return DBNull.Value;

            var result = aggregateFunction switch
            {
                PivotAggregateFunction.First => values.FirstOrDefault(),
                PivotAggregateFunction.Last  => values.LastOrDefault(),
                PivotAggregateFunction.Count => values.Count,
                PivotAggregateFunction.Min   => values.Min(),
                PivotAggregateFunction.Max   => values.Max(),
                PivotAggregateFunction.Sum   => (object)values.Cast<double>().Sum(),
                PivotAggregateFunction.Avg   => (object)values.Cast<double>().Average(),
                _                            => values.FirstOrDefault()
            };
            return result ?? DBNull.Value;
        }
    }
}