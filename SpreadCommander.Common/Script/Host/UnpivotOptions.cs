using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpreadCommander.Common.Script.ScriptHostObject;

namespace SpreadCommander.Common.Script
{
    public class UnpivotOptions
    {
        [Description("Columns that are neither primary columns nor un-pivot columns.")]
        public string[] IgnoreColumns { get; set; }

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Skip values from data source. For example NULL or 0 can be skipped. if $null is specified - DBNull is skipping too.")]
        public object[] SkipValues { get; set; }

        [Description("Ignore errors thrown when getting property values.")]
        public bool IgnoreErrors { get; set; }

        [Description("Deedle frame keys.")]
        public string[] DeedleFrameKeys { get; set; }
    }

    public partial class ScriptHost
    {
        public DataTable Unpivot(object dataSource, string[] primaryColumns, string unpivotColumnName, Type unpivotColumnType,
            string unpivotValueColumnName, Type unpivotValueType, UnpivotOptions options = null)
        {
            using var reader = UnpivotAsDataReader(dataSource, primaryColumns, unpivotColumnName, unpivotColumnType, 
                unpivotValueColumnName, unpivotValueType, options);
    
            var tblResult = new DataTable("UnpivotData");
            tblResult.Load(reader);
            reader.Close();

            return tblResult;
        }

#pragma warning disable CA1822 // Mark members as static
        public DbDataReader UnpivotAsDataReader(object dataSource, string[] primaryColumns, string unpivotColumnName, Type unpivotColumnType,
#pragma warning restore CA1822 // Mark members as static
            string unpivotValueColumnName, Type unpivotValueType, UnpivotOptions options = null)
        {
            options ??= new UnpivotOptions();

            if (primaryColumns == null || primaryColumns.Length <= 0)
                throw new ArgumentNullException(nameof(primaryColumns));
            if (string.IsNullOrWhiteSpace(unpivotColumnName))
                throw new ArgumentNullException(nameof(unpivotColumnName));
            if (string.IsNullOrWhiteSpace(unpivotValueColumnName))
                throw new ArgumentException(nameof(unpivotValueColumnName));
            if (unpivotValueType == null)
                throw new ArgumentNullException(nameof(unpivotValueType));

            var reader = ScriptHostObject.GetDataSourceReader(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns, DeedleFrameKeys = options.DeedleFrameKeys });
            if (reader == null)
                throw new Exception("Input data are not provided.");

            try
            {
                var readerParameters = new UnpivotDataReader.Parameters()
                {
                    DataSource             = reader,
                    PrimaryColumns         = primaryColumns,
                    IgnoreColumns          = options.IgnoreColumns,
                    IgnoreErrors           = options.IgnoreErrors,
                    UnPivotColumnName      = unpivotColumnName,
                    UnpivotColumnType      = unpivotColumnType,
                    UnPivotValueColumnName = unpivotValueColumnName,
                    UnPivotValueType       = unpivotValueType,
                    SkipValues             = options.SkipValues
                };

                var result = new UnpivotDataReader(readerParameters);
                return result;
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
