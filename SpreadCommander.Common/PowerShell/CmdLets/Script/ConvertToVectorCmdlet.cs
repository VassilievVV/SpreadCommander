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
    [Cmdlet(VerbsData.ConvertTo, "Vector")]
    public class ConvertToVectorCmdlet : SCCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Data source for data tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "Column or field name to convert into vector.")]
        [Alias("Column")]
        public string ColumnName { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values.")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Item type of objects in result array.")]
        [Alias("Type")]
        public Type ItemType { get; set; }

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
            var list   = ConvertList();
            var result = Array.CreateInstance(ItemType, list.Count);
            list.CopyTo(result, 0);

            WriteObject(result, EnumerateCollection);
        }

        protected IList ConvertList()
        {
            var resultType = typeof(List<>).MakeGenericType(ItemType);
            var result = Activator.CreateInstance(resultType) as IList;

            using var reader = GetDataSourceReader(_Input, DataSource, new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns });
            if (reader == null)
                return result;

            int columnIndex = -1;
            for (int i = 0; i < reader.VisibleFieldCount; i++)
            {
                if (string.Compare(reader.GetName(i), ColumnName, true) == 0)
                {
                    columnIndex = i;
                    break;
                }
            }
            if (columnIndex < 0)
                throw new Exception($"Cannot find column '{ColumnName}'.");

            while (reader.Read())
            {
                var value = reader.GetValue(columnIndex);
                value     = Utils.ChangeType(ItemType, value);
                result.Add(value);
            }

            return result;
        }
    }
}
