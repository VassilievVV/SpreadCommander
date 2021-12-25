using SpreadCommander.Common.Code;
using SpreadCommander.Common.Code.Exporters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SpreadCommander.Common.Script.ScriptHostObject;

namespace SpreadCommander.Common.Script
{
    public class ExportTableToDatabaseOptions
    {
        [Description("Table schema of the table to export data into")]
        public string TableSchema { get; set; }

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Skip auto-generated ID column.")]
        public bool SkipAutoID { get; set; }

        [Description("Ignore errors thrown when getting property values")]
        public bool IgnoreErrors { get; set; }

        [Description("Batch size for exporting data. Also used as interval for progress report")]
        public int? BatchSize { get; set; }

        [Description("If set - progress will be reported in UI")]
        public bool ReportProgress { get; set; }

        [Description("If set and table already exists - it will be dropped and new one created. Table shall have no foreign keys and other constraints that prevent its deleting")]
        public bool Replace { get; set; }

        [Description("Timeout for commands in SQL script")]
        public int? CommandTimeout { get; set; }

        [Description("SQL Script to execute before exporting data.")]
        public string PreScript { get; set; }

        [Description("SQL Script to execute after exporting data.")]
        public string PostScript { get; set; }

        [Description("SQL Script to create table. Optional, use when need to have specific table schema.")]
        public string CreateTableScript { get; set; }

        [Description("If set - do not create new table to export data into.")]
        public bool UseExistingTable { get; set; }
    }

    public partial class ScriptHost
    {
        public void ExportTableToDatabase(object dataSource, DbConnection connection, string tableName, ExportTableToDatabaseOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromDbConnection(connection);
            ExportTableToDatabase(dataSource, conn, tableName, options);
        }

        public void ExportTableToDatabase(object dataSource, string connectionName, string tableName, ExportTableToDatabaseOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
                throw new Exception("ConnectionName cannot be empty.");

            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                ExportTableToDatabase(dataSource, conn, tableName, options);
            }
            finally
            {
                conn.Close();
            }   
        }

        public void ExportTableToDatabase(object dataSource, Connection connection, string tableName, ExportTableToDatabaseOptions options = null)
        {
            options ??= new ExportTableToDatabaseOptions();

            const string progressDescription = "Exporting table into database";

            var dataReader = ScriptHostObject.GetDataSourceReader(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns });

            if ((connection.DbConnection?.State ?? ConnectionState.Closed) != ConnectionState.Open)
                connection.Open();

            if (options.ReportProgress)
                UpdateProgress(ProgressKind.Value, 0, 100, progressDescription);

            if (!string.IsNullOrWhiteSpace(options.PreScript))
                ExecuteScript(connection, options.PreScript, options);

            var exporter = DbExporter.GetDbExporter(connection.FactoryInvariantName);
            if (exporter == null)
                throw new Exception("Cannot configure DB exporter for selected connection.");

            exporter.SkipAutoID = options.SkipAutoID;

            if (options.Replace)
                exporter.DropTable(connection.DbConnection, options.TableSchema, tableName);

            if (!string.IsNullOrWhiteSpace(options.CreateTableScript))
                ExecuteScript(connection, options.CreateTableScript, options);

            if (options.BatchSize.HasValue)
                exporter.BatchSize = options.BatchSize.Value;

            if (options.ReportProgress)
            {
                exporter.Progress += (s, e) =>
                {
                    var percent = (e.Max > 0 && e.Progress <= e.Max) ? Convert.ToInt32(Convert.ToDouble(e.Progress) / Convert.ToDouble(e.Max)) : 50;
                    percent = Utils.ValueInRange(percent, 0, 100);
                    UpdateProgress(ProgressKind.Value, percent, 100, progressDescription);
                };
            }

            try
            {
                bool needCreateTable = !options.UseExistingTable && string.IsNullOrWhiteSpace(options.CreateTableScript);
                exporter.ExportDataTable(connection.DbConnection, dataReader,
                    options.TableSchema, tableName, needCreateTable, CancellationToken.None);
            }
            finally
            {
                if (options.ReportProgress)
                    UpdateProgress(ProgressKind.Value, 0, 100, string.Empty);
            }

            if (!string.IsNullOrWhiteSpace(options.PostScript))
                ExecuteScript(connection, options.PostScript, options);
        }

#pragma warning disable CA1822 // Mark members as static
        protected void ExecuteScript(Connection conn, string scriptText, ExportTableToDatabaseOptions options)
#pragma warning restore CA1822 // Mark members as static
        {
            if (string.IsNullOrWhiteSpace(scriptText))
                return;

            using var dataSet = new DataSet("Data");

            var sqlScript = new SqlScript.SqlScript()
            {
                ScriptText          = scriptText,
                Connection          = conn?.DbConnection,
                ConnectionName      = null,
                DataSet             = dataSet,
                SCDispatcherService = null
            };

            if (options.CommandTimeout.HasValue)
                sqlScript.ScriptParameters.CommandTimeout = options.CommandTimeout.Value;

            var dbConnections = DBConnections.LoadConnections();

            sqlScript.RequestConnection += (s, e) =>
            {
                var requestConnection = dbConnections.FindConnection(e.ConnectionName);
                if (requestConnection == null)
                    return;

                var requestConn = new Connection(requestConnection.Provider, requestConnection.ConnectionString);
                if (requestConn.ConnectionType == null)
                    return;

                requestConn.Open();
                e.Connection = requestConn.DbConnection;
            };

            sqlScript.ExecuteScript();
        }
    }
}
