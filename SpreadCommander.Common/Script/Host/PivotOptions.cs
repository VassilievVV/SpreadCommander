using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpreadCommander.Common.Script.ScriptHostObject;

namespace SpreadCommander.Common.Script
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

    public class PivotOptions
    {
        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values.")]
        public bool IgnoreErrors { get; set; }

        [Description("Deedle frame keys.")]
        public string[] DeedleFrameKeys { get; set; }
    }

    public partial class ScriptHost
    {
#pragma warning disable CA1822 // Mark members as static
        public DataTable Pivot(object dataSource, string[] primaryColumns, string pivotColumn, string pivotValueColumn,
#pragma warning restore CA1822 // Mark members as static
            PivotAggregateFunction AggregateFunction = PivotAggregateFunction.First, PivotOptions options = null)
        {
            options ??= new PivotOptions();

            if (primaryColumns == null || primaryColumns.Length <= 0)
                throw new ArgumentNullException(nameof(primaryColumns));
            if (string.IsNullOrWhiteSpace(pivotColumn))
                throw new ArgumentNullException(nameof(pivotColumn));
            if (string.IsNullOrWhiteSpace(pivotValueColumn))
                throw new ArgumentException(nameof(pivotValueColumn));

            using var reader = ScriptHostObject.GetDataSourceReader(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns, DeedleFrameKeys = options.DeedleFrameKeys });
            if (reader == null)
                throw new Exception("Input data are not provided.");

            using var sourceTable = new DataTable("Source");
            sourceTable.Load(reader);

            var primColumns    = primaryColumns.Select(pc => sourceTable.Columns[pc]).ToArray();
            var pivColumn      = sourceTable.Columns[pivotColumn];
            var pivValueColumn = sourceTable.Columns[pivotValueColumn];

            if (primColumns.Length <= 0 || primColumns.Contains(null))
                throw new ArgumentException("Cannot find one or more of primary columns.");
            if (pivColumn == null)
                throw new ArgumentException($"Cannot find column '{pivotColumn}'.");
            if (pivValueColumn == null)
                throw new ArgumentException($"Cannot find column '{pivotValueColumn}'.");

            var result = PivotToTable(sourceTable, primColumns, pivColumn, pivValueColumn, AggregateFunction);
            return result;
        }

        private static DataTable PivotToTable(DataTable dt, DataColumn[] primaryColumns,
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
                PivotAggregateFunction.Last => values.LastOrDefault(),
                PivotAggregateFunction.Count => values.Count,
                PivotAggregateFunction.Min => values.Min(),
                PivotAggregateFunction.Max => values.Max(),
                PivotAggregateFunction.Sum => (object)values.Cast<double>().Sum(),
                PivotAggregateFunction.Avg => (object)values.Cast<double>().Average(),
                _ => values.FirstOrDefault()
            };
            return result ?? DBNull.Value;
        }
    }
}
