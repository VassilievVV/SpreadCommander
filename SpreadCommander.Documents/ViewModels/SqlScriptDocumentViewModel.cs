using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using SpreadCommander.Documents.Services;
using System.Data.Common;
using SpreadCommander.Common.SqlScript;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DevExpress.Spreadsheet;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common;
using CommonCode = SpreadCommander.Common.Code;
using System.Drawing;
using System.Text.RegularExpressions;
using SpreadCommander.Common.Spreadsheet;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Data.SQLite;
using SpreadCommander.Common.DBMS.SQLite;
using MySqlConnector;

namespace SpreadCommander.Documents.ViewModels
{
    [POCOViewModel]
    public partial class SqlScriptDocumentViewModel: BaseDocumentViewModel, ISpreadsheetHolder
    {
        public const string ViewName = "SqlScriptDocument";

        #region ICallback
        public interface ICallback
        {
            string CommandText { get; }
            string ScriptText  { get; }
            void ClearSqlMessages();
            void AddSqlMessage(SqlMessage message);
            void LoadFromFile(string fileName);
            void SaveToFile(string fileName);
            void ScriptModified();
            void DisableTransitions(bool value);

            void ResetModifiedAll();
            void ResetModifiedBook();
            void ResetModifiedSpreadsheet();
            void ResetModifiedGrid();
        }
        #endregion

        #region TableAlias
        private class TableAlias
        {
            public string Alias		{ get; set; }

            public string Database	{ get; set; }

            public string Schema	{ get; set; }

            public string Table		{ get; set; }
        }
        #endregion

        public SqlScriptDocumentViewModel()
        {
        }

        public static SqlScriptDocumentViewModel Create() =>
            ViewModelSource.Create<SqlScriptDocumentViewModel>(() => new SqlScriptDocumentViewModel());

        public override string DefaultExt   => "sql";
        public override string FileFilter   => "SQL script (*.sql)|*.sql";
        public override string DocumentType => ViewName;
        public override bool Modified
        {
            get => base.Modified;
            set
            {
                if (base.Modified != value)
                {
                    base.Modified = value;
                    if (value)
                        Callback?.ScriptModified();
                }
            }
        }

        protected IDbConnectionEditorService DbConnectionEditor         => this.GetService<IDbConnectionEditorService>();
        protected IDbSchemaViewerService DbSchemaViewer                 => this.GetService<IDbSchemaViewerService>();
        protected ISqlExecutionPlanViewer SqlExecutionPlanViewer        => this.GetService<ISqlExecutionPlanViewer>();
        protected ISQLiteExecutionPlanViewer SQLiteExecutionPlanViewer  => this.GetService<ISQLiteExecutionPlanViewer>();
        protected IMySqlExecutionPlanViewer MySqlExecutionPlanViewer    => this.GetService<IMySqlExecutionPlanViewer>();

        public ICallback Callback				        { get; set; }

        public virtual string ConnectionName	        { get; set; }
        public virtual CommonCode.Connection Connection	{ get; set; }
        public virtual DataSet DataSet			        { get; set; }

        IWorkbook ISpreadsheetHolder.Workbook => Workbook;

        public IRichEditDocumentServer BookServer       { get; set; }
        public Document Document => BookServer?.Document;
        public IWorkbook Workbook                       { get; set; }
        public DataSet GridDataSet                      { get; set; }
        public IFileViewer FileViewer                   { get; set; }


        private CancellationTokenSource _CancelScriptTokenSource;

        private SelectedDbConnection _SelectedDbConnection;

        public bool CanChangeConnection() => Connection == null || (Connection?.State == ConnectionState.Open || Connection?.State == ConnectionState.Closed);
        public async Task ChangeConnection()
        {
            var newConnection = DbConnectionEditor.SelectConnection();
            if (newConnection != null)
            {
                _SelectedDbConnection = newConnection;

                ConnectionName        = _SelectedDbConnection.ConnectionName;
                Connection            = _SelectedDbConnection.Connection;

                try
                {
                    if (await CheckConnection() == null)
                    {
                        ConnectionName = null;
                        Connection     = null;
                    }
                }
                catch (Exception ex)
                {
                    ConnectionName = null;
                    Connection     = null;

                    throw ex;
                }
            }

            ConnectionStateChanged();
        }

        public bool CanShowSchemas() => //Connection?.DbConnection != null && Connection.DbConnection.State == ConnectionState.Open;
            true;
        public async void ShowSchemas()
        {
            await LoadInitialConnection(Callback?.ScriptText);
            if (Connection?.DbConnection == null || Connection.DbConnection.State != ConnectionState.Open)
                return;

            DbSchemaViewer.Show(Connection?.DbConnection);
        }

        public bool CanExecute() => _CancelScriptTokenSource == null;
        public async Task Execute()
        {
            var connection = await CheckConnection();
            if (connection == null)
                return;

            ClearAllOutput();

            var scriptText = Callback.CommandText;
            ExecuteCommandText(scriptText, false);
        }

        public void ExecuteCommandText(string scriptText, bool consoleCommand)
        {
            if (string.IsNullOrWhiteSpace(scriptText))
            {
                MessageService.ShowMessage("Please enter script", "Script is empty", MessageButton.OK);
                return;
            }

            if (DataSet == null || !consoleCommand)
            {
                //Console command are executing into current dataset.
                DataSet = new DataSet("Script");
                Callback?.ClearSqlMessages();
            }

            _CancelScriptTokenSource = new CancellationTokenSource();

            ConnectionStateChanged();

            Task.Factory.StartNew(() => ExecuteScript(scriptText), _CancelScriptTokenSource.Token, 
                TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public bool CanCancel() => _CancelScriptTokenSource != null;
        public void Cancel()
        {
            _CancelScriptTokenSource?.Cancel();
        }

        /*
        public bool CanShowQueryInfo()
        {
            var dbConnection = Connection?.DbConnection;
            if (dbConnection == null || !(dbConnection.State == ConnectionState.Open || dbConnection.State == ConnectionState.Closed))
                return false;

            return (dbConnection is SqlConnection || dbConnection is SQLiteConnection || dbConnection is MySqlConnection);
        }
        */
        public bool CanShowQueryInfo() => true;
        
        public async Task ShowQueryInfo()
        {
            await LoadInitialConnection(Callback?.ScriptText);

            var connection = await CheckConnection();
            if (connection == null)
                return;

            var dbConnection = Connection?.DbConnection;
            if (dbConnection == null || !(dbConnection.State == ConnectionState.Open || dbConnection.State == ConnectionState.Closed))
                return;

            if (!(dbConnection is SqlConnection || dbConnection is SQLiteConnection || dbConnection is MySqlConnection))
            {
                MessageService.ShowMessage("Query Info is supported only for MS SQL Server, MySQL and SQLite database.", "Unsupported DBMS", MessageButton.OK);
                return;
            }

            var scriptText = Callback.CommandText;
            if (string.IsNullOrWhiteSpace(scriptText))
            {
                MessageService.ShowMessage("Please enter script", "Script is empty", MessageButton.OK);
                return;
            }

#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
            if (connection is SqlConnection && scriptText.IndexOf("showplan", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                MessageService.ShowMessage("To show execution plan script shall not contain word 'showplan'.", "Invalid script", MessageButton.OK);
                return;
            }
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found

            var script = new SqlScript(scriptText);
            if (script.Commands.Count < 0)
            {
                MessageService.ShowMessage("Cannot determine commands in script", "No commands", MessageButton.OK);
                return;
            }
            if (script.Commands.Count > 1)
                MessageService.ShowMessage("Multiple commands in script. Showing execution plan for first command.", "Multiple commands", MessageButton.OK);

            var firstCommand    = script.Commands[0].Text;
            var firstSubCommand = Utils.SplitString(firstCommand, ';')[0];

            try
            {
                if (connection is SqlConnection sqlConnection)
                    await ShowSqlExecutionPlan(sqlConnection, firstCommand, CancellationToken.None).ConfigureAwait(true);
                else if (connection is SQLiteConnection sqliteConnection)
                    await ShowSQLiteExecutionPlan(sqliteConnection, firstSubCommand, CancellationToken.None).ConfigureAwait(true);
                else if (connection is MySqlConnection mySqlConnection)
                    await ShowMySqlExecutionPlan(mySqlConnection, firstSubCommand, CancellationToken.None).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private async Task ShowSqlExecutionPlan(SqlConnection sqlConnection, string cmdText, CancellationToken cancellationToken)
        {
            try
            {
                DataTable tableExecutionPlan = null;

                await Task.Run(() =>
                {
                    try
                    {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                        using (var cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "set showplan_all on";
                            cmd.ExecuteNonQuery();
                        }
#pragma warning disable CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found

                        if (cancellationToken.IsCancellationRequested)
                            return;

                        using (var cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = cmdText;
                            using SqlDataReader reader = cmd.ExecuteReader();
                            using LoadAdapter adapter = new LoadAdapter();
                            tableExecutionPlan = new DataTable("ExecutionPlan");
                            adapter.FillSchema(tableExecutionPlan, SchemaType.Source, reader);
                            adapter.Fill(tableExecutionPlan, reader);
                        }
                    }
                    finally
                    {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                        using var cmd = sqlConnection.CreateCommand();
                        cmd.CommandText = "set showplan_all off";
                        cmd.ExecuteNonQuery();
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
                    }
                }).ConfigureAwait(true);

                if (tableExecutionPlan != null)
                    SqlExecutionPlanViewer.ShowExecutionPlan(tableExecutionPlan);
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage($"Cannot generate execution plant: {ex.Message}", "Error", MessageButton.OK);
                return;
            }
        }
        
        private async Task ShowSQLiteExecutionPlan(SQLiteConnection sqliteConnection, string cmdText, CancellationToken cancellationToken)
        {
            try
            {
                DataTable tableExecutionPlan = null;

                await Task.Run(() =>
                {
                    try
                    {
                        using var sqliteConnection2 = new SQLiteConnection();
                        var sqliteConnectionStringBuilder = new SQLiteConnectionStringBuilder(sqliteConnection.ConnectionString)
                        {
                            ReadOnly = true
                        };
                        sqliteConnection2.ConnectionString = sqliteConnectionStringBuilder.ConnectionString;
                        sqliteConnection2.Open();
                        sqliteConnection2.BindSCFunctions();

                        if (cancellationToken.IsCancellationRequested)
                            return;

                        using var cmd = sqliteConnection.CreateCommand();
                        cmd.CommandText = $"explain query plan\r\n{cmdText}";
                        using DbDataReader reader = cmd.ExecuteReader();
                        using LoadAdapter adapter = new LoadAdapter();
                        tableExecutionPlan = new DataTable("ExecutionPlan");
                        //adapter.FillSchema(tableExecutionPlan, SchemaType.Source, reader);
                        adapter.Fill(tableExecutionPlan, reader);
                    }
                    finally
                    {
                    }
                }).ConfigureAwait(true);

                if (tableExecutionPlan != null)
                    SQLiteExecutionPlanViewer.ShowExecutionPlan(tableExecutionPlan);
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage($"Cannot generate execution plant: {ex.Message}", "Error", MessageButton.OK);
                return;
            }
        }

        private async Task ShowMySqlExecutionPlan(MySqlConnection mySqlConnection, string cmdText, CancellationToken cancellationToken)
        {
            try
            {
                DataTable tableExecutionPlan = null;

                await Task.Run(() =>
                {
                    try
                    {
                        using var mySqlConnection2 = new MySqlConnection() { ConnectionString = mySqlConnection.ConnectionString };
                        mySqlConnection2.Open();

                        if (cancellationToken.IsCancellationRequested)
                            return;

                        using var cmd = mySqlConnection.CreateCommand();
                        cmd.CommandText = $"set session transaction read only;\r\nexplain\r\n{cmdText}";
                        using DbDataReader reader = cmd.ExecuteReader();
                        using LoadAdapter adapter = new LoadAdapter();
                        tableExecutionPlan = new DataTable("ExecutionPlan");
                        //adapter.FillSchema(tableExecutionPlan, SchemaType.Source, reader);
                        adapter.Fill(tableExecutionPlan, reader);
                    }
                    finally
                    {
                    }
                }).ConfigureAwait(true);

                if (tableExecutionPlan != null)
                    MySqlExecutionPlanViewer.ShowExecutionPlan(tableExecutionPlan);
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage($"Cannot generate execution plant: {ex.Message}", "Error", MessageButton.OK);
                return;
            }
        }

        private void ConnectionStateChanged()
        {
            RaiseCanExecuteChanged(() => ChangeConnection());
            RaiseCanExecuteChanged(() => ShowSchemas());
            RaiseCanExecuteChanged(() => ShowQueryInfo());
            RaiseCanExecuteChanged(() => Execute());
            RaiseCanExecuteChanged(() => Cancel());
        }

        private async Task<DbConnection> CheckConnection(bool silent = false)
        {
            if (Connection == null)
            {
                await LoadInitialConnection(Callback?.ScriptText);

                if (!silent && Connection == null)
                    await ChangeConnection();
            }

            var connection = Connection;
            if (connection == null)
                return null;

            switch (connection.State)
            {
                case ConnectionState.Closed:
                    connection.Open();
                    break;
                case ConnectionState.Open:
                    break;
                case ConnectionState.Connecting:
                    //Should not happen, connection is open synchronously (in own thread)
                    throw new Exception("Database connection is already in use.");
                case ConnectionState.Executing:
                    throw new Exception("Database connection is already in use.");
                case ConnectionState.Fetching:
                    throw new Exception("Database connection is already in use.");
                case ConnectionState.Broken:
                    connection.Close();
                    throw new Exception("Database connection is broken and was closed.");
            }

            return connection.DbConnection;
        }

        private void ReportScriptMessage(SqlMessage message)
        {
            if (Callback != null)
                SCDispatcherService.BeginInvoke(() => Callback?.AddSqlMessage(message));
        }

        protected void ExecuteScript(string scriptText)
        {
            var sqlScript = new SqlScript()
            {
                ScriptText          = scriptText,
                Connection          = Connection?.DbConnection,
                ConnectionName      = ConnectionName,
                DataSet             = this.DataSet, 
                SCDispatcherService = this.SCDispatcherService,
                CancelScriptToken   = _CancelScriptTokenSource.Token
            };

            var connections = DBConnections.LoadConnections();

            sqlScript.ConnectionChanged   += (s, e) => { };
            sqlScript.ReportScriptMessage += (s, e) => { ReportScriptMessage(e.Message); };
            sqlScript.RequestConnection   += (s, e) =>
            {
                var connection = connections.FindConnection(e.ConnectionName);
                if (connection == null)
                    return;

                var conn = new CommonCode.Connection(connection.Provider, connection.ConnectionString);
                if (conn.ConnectionType == null)
                    return;

                conn.Open();
                e.Connection = conn.DbConnection;
            };

            try
            {
                sqlScript.ExecuteScript();
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                    ReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, innerEx.Message));
            }
            catch (Exception ex)
            {
                ReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, ex.Message));
            }

            SCDispatcherService.BeginInvoke(() =>
            {
                _CancelScriptTokenSource?.Dispose();
                _CancelScriptTokenSource = null;

                ConnectionStateChanged();

                AlertService.Show("Script is executed", "Script execution finished");
            });
        }

        public bool CanExportData => (DataSet != null) && (DataSet?.Tables.Count > 0);

        public override DbDataReader GetDataTable(string tableName)
        {
            var dataSet = DataSet;

            if (dataSet == null)
                return null;

            return new DataTableReader(dataSet.Tables[tableName]);
        }

        public override void LoadFromFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            Callback.LoadFromFile(fileName);
            base.LoadFromFile(fileName);

            LoadInitialConnectionFromFile(fileName);
        }

        public override void SaveToFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            Callback.SaveToFile(fileName);
            base.SaveToFile(fileName);
        }

        private Task LoadInitialConnectionFromFile(string fileName)
        {
            if (Connection != null)
                return Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                return Task.CompletedTask;

            try
            {
                string scriptText = File.ReadAllText(fileName);
                var result        = LoadInitialConnection(scriptText);
                return result;
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }

        private Task LoadInitialConnection(string scriptText)
        {
            if (Connection != null)
                return Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(scriptText))
                return Task.CompletedTask;

            var result = Task.Run(() =>
            {
                try
                {
                    var script = new SqlScript(scriptText);
                    if (script.Commands.Count <= 0)
                        return;

                    var cmdConnection = script.Commands[0].Commands.OfType<SpreadCommander.Common.ScriptEngines.ConsoleCommands.Connection>().FirstOrDefault();
                    if (cmdConnection == null || string.IsNullOrWhiteSpace(cmdConnection.Name))
                        return;

                    var connections = DBConnections.LoadConnections();
                    var connection  = connections.FindConnection(cmdConnection.Name);
                    if (connection == null)
                        return;

                    var conn = new CommonCode.Connection(connection.Provider, connection.ConnectionString);
                    if (conn.ConnectionType == null)
                        return;

                    conn.Open();

                    SCDispatcherService.Invoke(() =>
                    {
                        Connection     = conn;
                        ConnectionName = cmdConnection.Name;

                        ConnectionStateChanged();
                    });
                }
                catch (Exception)
                {
                    //Do nothing
                }
            });

            return result;
        }

        public virtual void ClearBook()
        {
            var doc = Document;
            if (doc == null)
                return;

            doc.Text = string.Empty;

            Callback?.ResetModifiedBook();
        }

        public virtual void ClearSpreadsheet()
        {
            var workbook = Workbook;
            if (workbook == null)
                return;

            workbook.Modified = false;
            workbook.CreateNewDocument();

            Callback?.ResetModifiedSpreadsheet();
        }

        public virtual void ClearGrid()
        {
            var dataSet = GridDataSet;
            if (dataSet == null)
                return;

            dataSet.Clear();
            dataSet.Reset();

            Callback?.ResetModifiedGrid();
        }

        public virtual void ClearAllOutput()
        {
            ClearBook();
            ClearSpreadsheet();
            ClearGrid();

            Callback?.ResetModifiedAll();
        }

    }
}