using SpreadCommander.Common.Code;
using SpreadCommander.Common.Code.Exporters;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Export, "TableToDatabase")]
    public class ExportTableToDatabaseCmdlet : SCCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ConnectionName", Position = 0, HelpMessage = "Name of connection for SQL script")]
        public string ConnectionName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Connection", Position = 0, HelpMessage = "Database connection for SQL script")]
        public DbConnection Connection { get; set; }

        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "Table schema of the table to export data into")]
        public string TableSchema { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Name of the table to export data into")]
        public string TableName { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Skip auto-generated ID column.")]
        public SwitchParameter SkipAutoID { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Batch size for exporting data. Also used as interval for progress report")]
        public int? BatchSize { get; set; }

        [Parameter(HelpMessage = "If set - progress will be reported in UI")]
        [Alias("Progress")]
        public SwitchParameter ReportProgress { get; set; }

        [Parameter(HelpMessage = "If set and table already exists - it will be dropped and new one created. Table shall have no foreign keys and other constraints that prevent its deleting")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Timeout for commands in SQL script")]
        [Alias("Timeout")]
        public int? CommandTimeout { get; set; }

        [Parameter(HelpMessage = "SQL Script to execute before exporting data.")]
        public string PreScript { get; set; }

        [Parameter(HelpMessage = "SQL Script to execute after exporting data.")]
        public string PostScript { get; set; }

        [Parameter(HelpMessage = "SQL Script to create table. Optional, use when need to have specific table schema.")]
        [Alias("SqlCreateTable")]
        public string CreateTableScript { get; set; }

        [Parameter(HelpMessage = "If set - do not create new table to export data into.")]
        public SwitchParameter UseExistingTable { get; set; }


        private readonly List<PSObject> _Output = new ();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Output.Add(obj);
        }

        protected override void EndProcessing()
        {
            ExportTable();

            if (PassThru)
                WriteObject(_Output, true);
        }

        protected void ExportTable()
        {
            const string progressActivity    = "Export to DB";
            const string progressDescription = "Exporting table into database";

            if (string.IsNullOrWhiteSpace(ConnectionName))
                throw new Exception("ConnectionName cannot be empty.");

            using var dataReader = GetDataSourceReader(_Output, DataSource, 
                new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns, SkipAutoID = this.SkipAutoID });

            Connection conn = null;
            bool closeConnection = false;
            if (Connection != null)
                conn = ConnectionFactory.CreateFromDbConnection(Connection);
            else if (!string.IsNullOrWhiteSpace(ConnectionName))
            {
                conn = ConnectionFactory.CreateFromString(ConnectionName);
                closeConnection = true;
            }
            else
                throw new Exception("Cannot determine connection - provide either Connection or ConnectionName.");

            if ((conn.DbConnection?.State ?? ConnectionState.Closed) != ConnectionState.Open)
                conn.Open();

            try
            {               
                int activityID = 0;

                if (ReportProgress)
                {
                    activityID = GetNextProgressActivityID();
                    
                    var progress = new ProgressRecord(activityID, progressActivity, progressDescription)
                    {
                        RecordType = ProgressRecordType.Processing,
                        PercentComplete = 0
                    };

                    WriteProgress(progress);
                }

                if (!string.IsNullOrWhiteSpace(PreScript))
                    ExecuteScript(conn, PreScript);

                var exporter = DbExporter.GetDbExporter(conn.FactoryInvariantName);
                if (exporter == null)
                    throw new Exception("Cannot configure DB exporter for selected connection.");

                exporter.SkipAutoID = SkipAutoID;

                if (Replace)
                    exporter.DropTable(conn.DbConnection, TableSchema, TableName);

                if (!string.IsNullOrWhiteSpace(CreateTableScript))
                    ExecuteScript(conn, CreateTableScript);

                if (BatchSize.HasValue)
                    exporter.BatchSize = BatchSize.Value;

                if (ReportProgress)
                {
                    exporter.Progress += (s, e) =>
                    {
                        var percent = (e.Max > 0 && e.Progress <= e.Max) ? Convert.ToInt32(Convert.ToDouble(e.Progress) / Convert.ToDouble(e.Max)) : 50;
                        percent = Utils.ValueInRange(percent, 0, 100);
                        var progress = new ProgressRecord(activityID, progressActivity, progressDescription)
                        {
                            RecordType = ProgressRecordType.Processing,
                            PercentComplete = percent
                        };

                        WriteProgress(progress);
                    };
                }

                try
                {
                    bool needCreateTable = !UseExistingTable && string.IsNullOrWhiteSpace(CreateTableScript);
                    exporter.ExportDataTable(conn.DbConnection, dataReader, 
                        TableSchema, TableName, needCreateTable, CancellationToken.None);
                }
                finally
                {
                    if (ReportProgress)
                    {
                        var progress = new ProgressRecord(activityID, progressActivity, progressDescription)
                        {
                            RecordType = ProgressRecordType.Completed,
                            PercentComplete = 0
                        };

                        WriteProgress(progress);
                    }
                }

                if (!string.IsNullOrWhiteSpace(PostScript))
                    ExecuteScript(conn, PostScript);
            }
            catch (Exception)
            {
                if (closeConnection)
                    conn?.Close();

                throw;
            }
        }

        private void ExecuteScript(Connection conn, string scriptText)
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

            if (CommandTimeout.HasValue)
                sqlScript.ScriptParameters.CommandTimeout = CommandTimeout.Value;

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
