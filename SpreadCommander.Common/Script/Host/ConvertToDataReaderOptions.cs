using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpreadCommander.Common.Script.ScriptHostObject;

namespace SpreadCommander.Common.Script
{
    public class ConvertToDataReaderOptions
    {
        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values.")]
        public bool IgnoreErrors { get; set; }
    }

    public partial class ScriptHost
    {
#pragma warning disable CA1822 // Mark members as static
        public DbDataReader ConvertToDataReader(object dataSource, ConvertToDataReaderOptions options = null)
#pragma warning restore CA1822 // Mark members as static
        {
            options ??= new ConvertToDataReaderOptions();

            var reader = ScriptHostObject.GetDataSourceReader(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns });

            return reader;
        }
    }
}
