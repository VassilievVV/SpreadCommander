using System;
using System.Collections.Generic;
using System.Data;
using System.Management.Automation;
using System.Text;
using System.Linq;
using SpreadCommander.Common.Code;
using System.Data.Common;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsData.ConvertTo, "UnPivot")]
    [OutputType(typeof(DataTable))]
    [OutputType(typeof(DbDataReader))]
    public class ConvertToUnPivotCmdlet : SCCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for data tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Columns from data source that will remain in un-pivot table.")]
        [Alias("Primary")]
        public string[] PrimaryColumns { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Column name for headers of un-pivoting columns.")]
        public string UnpivotColumnName { get; set; }

        [Parameter(HelpMessage = "Type of values in Unpivot column. Default is string.")]
        [PSDefaultValue(Value = typeof(string))]
        public Type UnpivotColumnType { get; set; } = typeof(string);

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Column name for values of un-pivoting columns.")]
        public string UnpivotValueColumnName { get; set; }

        [Parameter(Mandatory = true, Position = 3, HelpMessage = "Type of values in un-pivot column.")]
        public Type UnpivotValueType { get; set; }

        [Parameter(HelpMessage = "Columns that are neither primary columns nor un-pivot columns.")]
        [Alias("Ignore")]
        public string[] IgnoreColumns { get; set; }

        [Parameter(HelpMessage = "Data source.")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Skip values from data source. For example NULL or 0 can be skipped. if $null is specified - DBNull is skipping too.")]
        public object[] SkipValues { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values.")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Return DbDataReader instead of DataTable. Can be used to export data into database.")]
        public SwitchParameter AsDataReader { get; set; }


        private readonly List<PSObject> _Input = new ();

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
            WriteObject(result);
        }

        protected object ConvertList()
        {
            if (PrimaryColumns == null || PrimaryColumns.Length <= 0)
                throw new ArgumentNullException(nameof(PrimaryColumns));
            if (string.IsNullOrWhiteSpace(UnpivotColumnName))
                throw new ArgumentNullException(nameof(UnpivotColumnName));
            if (string.IsNullOrWhiteSpace(UnpivotValueColumnName))
                throw new ArgumentException(nameof(UnpivotValueColumnName));
            if (UnpivotValueType == null)
                throw new ArgumentNullException(nameof(UnpivotValueType));

            var reader = GetDataSourceReader(_Input, DataSource, 
                new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns });
            if (reader == null)
                throw new Exception("Input data are not provided.");

            try
            {
                var readerParameters = new UnpivotDataReader.Parameters()
                {
                    DataSource             = reader,
                    PrimaryColumns         = this.PrimaryColumns,
                    IgnoreColumns          = this.IgnoreColumns,
                    IgnoreErrors           = this.IgnoreErrors,
                    UnPivotColumnName      = this.UnpivotColumnName,
                    UnpivotColumnType      = this.UnpivotColumnType,
                    UnPivotValueColumnName = this.UnpivotValueColumnName,
                    UnPivotValueType       = this.UnpivotValueType,
                    SkipValues             = this.SkipValues
                };

                var result = new UnpivotDataReader(readerParameters);
                if (AsDataReader)
                    return result;

                var tblResult = new DataTable("UnpivotData");
                tblResult.Load(result);
                result.Close();
                result.Dispose();

                return tblResult;
            }
            catch (Exception)
            {
                reader.Close();
                reader.Dispose();
                throw;
            }
        }
    }
}
