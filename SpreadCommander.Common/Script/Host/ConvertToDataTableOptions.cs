using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpreadCommander.Common.Script.ScriptHostObject;

namespace SpreadCommander.Common.Script
{
    public class ConvertToDataTableOptions: ConvertToDataReaderOptions
    {
    }

    public partial class ScriptHost
    {
#pragma warning disable CA1822 // Mark members as static
        public DataTable ConvertToDataTable(object dataSource, ConvertToDataReaderOptions options = null)
#pragma warning restore CA1822 // Mark members as static
        {
            options ??= new ConvertToDataReaderOptions();

            var table = ScriptHostObject.GetDataSource(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns, DeedleFrameKeys = options.DeedleFrameKeys });

            return table;
        }
    }
}
