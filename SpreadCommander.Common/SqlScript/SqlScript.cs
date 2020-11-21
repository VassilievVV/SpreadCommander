using DevExpress.Mvvm;
using MySqlConnector;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Common.SqlScript
{
    public partial class SqlScript
    {
        #region ReportScriptMessageArgs
        public class ReportScriptMessageArgs: EventArgs
        {
            public SqlMessage Message { get; set; }
        }
        #endregion

        #region ConnectionChangedEventArgs
        public class ConnectionChangedEventArgs: EventArgs
        {
            public DbConnection Connection { get; set; }
        }
        #endregion

        #region ConnectionStateChangedEventArgs
        public class ConnectionStateChangedEventArgs : EventArgs
        {
            public DbConnection Connection { get; set; }
        }
        #endregion

        #region RequestConnectionEventArgs
        public class RequestConnectionEventArgs : EventArgs
        {
            public string ConnectionName   { get; set; }
            public DbConnection Connection { get; set; }
        }
        #endregion

        private SqlScriptCommand _CurrentCommand;
        private int ErrorLineOffset => _CurrentCommand != null ? Math.Max(_CurrentCommand.StartLineNumber - 1, 0) : 0;

        public List<SqlScriptCommand> Commands					{ get; private set; } = new List<SqlScriptCommand>();
        public DbConnection Connection							{ get; set; }
        public string ConnectionName							{ get; set; }
        public DataSet DataSet									{ get; set; }
        //public IDispatcherService DispatcherService				{ get; set; }
        public ISCDispatcherService SCDispatcherService         { get; set; }
        public CancellationToken CancelScriptToken				{ get; set; }
        public ScriptRunParameters ScriptParameters				{ get; private set; } = new ScriptRunParameters();

        private string _ScriptText;
        public string ScriptText
        {
            get => _ScriptText;
            set => ParseScriptText(value);
        }

        public event EventHandler<ReportScriptMessageArgs> ReportScriptMessage;
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<RequestConnectionEventArgs> RequestConnection;

        public SqlScript()
        {
        }

        public SqlScript(string scriptText)
        {
            ParseScriptText(scriptText);
        }

        protected virtual void OnReportScriptMessage(SqlMessage message) =>
            ReportScriptMessage?.Invoke(this, new ReportScriptMessageArgs() { Message = message });

        protected virtual void OnConnectionChanged(DbConnection connection) =>
            ConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs() { Connection = connection });

        protected virtual void OnConnectionStateChanged(DbConnection connection) =>
            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs() { Connection = connection });

        protected virtual DbConnection OnRequestConnection(string connectionName)
        {
            var args = new RequestConnectionEventArgs()
            {
                ConnectionName = connectionName
            };
            RequestConnection?.Invoke(this, args);
            return args.Connection;
        }


        public void ParseScriptText(string scriptText)
        {
            _ScriptText = scriptText;
            Commands = SplitScript(scriptText);
        }

        private void PrepareConnection(DbConnection connection)
        {
            if (connection == null)
                return;

            _CurrentCommand = null;

            if (connection is SqlConnection sqlConnection)
            {
                sqlConnection.InfoMessage += SqlConnection_InfoMessage;
                sqlConnection.StatisticsEnabled = true;
                sqlConnection.ResetStatistics();
            }
            else if (connection is OleDbConnection oleDbConnection)
            {
                oleDbConnection.InfoMessage += OleDbConnection_InfoMessage;
            }
            else if (connection is OdbcConnection odbcConnection)
            {
                odbcConnection.InfoMessage += OdbcConnection_InfoMessage;
            }

            /*
#pragma warning disable CS0618 // Type or member is obsolete
            else if (connection is OracleConnection oracleConnection)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                oracleConnection.InfoMessage += OracleConnection_InfoMessage;
            }
            */
            else if (connection is MySqlConnection mySqlConnection)
            {
                mySqlConnection.InfoMessage += MySqlConnection_InfoMessage;
            }
            else if (connection is SQLiteConnection)
            {
                //Do nothing, no InfoMessage event
            }
        }

        private void UnprepareConnection(DbConnection connection)
        {
            if (connection == null)
                return;

            if (connection is SqlConnection sqlConnection)
            {
                sqlConnection.InfoMessage -= SqlConnection_InfoMessage;

                if (sqlConnection.StatisticsEnabled)
                {
                    var statistics = sqlConnection.RetrieveStatistics();
                    var stats = new StringBuilder();
                    stats.AppendLine("Statistics:");
                    stats.AppendLine("-----------");

                    foreach (DictionaryEntry entry in statistics)
                        stats.AppendLine(string.Format("{0,-20}: {1}", entry.Key, entry.Value));

                    OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Log, stats.ToString()));
                }

                sqlConnection.StatisticsEnabled = false;
                //Leave statistics as is, do not reset it
            }
            else if (connection is OleDbConnection oleDbConnection)
            {
                oleDbConnection.InfoMessage -= OleDbConnection_InfoMessage;
            }
            else if (connection is OdbcConnection odbcConnection)
            {
                odbcConnection.InfoMessage -= OdbcConnection_InfoMessage;
            }
            /*
#pragma warning disable CS0618 // Type or member is obsolete
            else if (connection is OracleConnection oracleConnection)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                oracleConnection.InfoMessage -= OracleConnection_InfoMessage;
            }
            */
            else if (connection is MySqlConnection mySqlConnection)
            {
                mySqlConnection.InfoMessage -= MySqlConnection_InfoMessage;
            }
            else if (connection is SQLiteConnection)
            {
                //Do nothing, no InfoMessage event
            }

            if (connection != this.Connection)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private void SqlConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (e.Errors != null && e.Errors.Count > 0)
            {
                foreach (SqlError error in e.Errors)
                {
                    OnReportScriptMessage(new SqlMessage()
                    {
                        MessageType = SqlMessage.SqlMessageType.Message,
                        Message     = error.Message,
                        Line        = error.LineNumber + ErrorLineOffset,
                        HResult     = error.Number,
                        ErrorCode   = error.Class,
                        Source      = error.Source,
                        State       = error.State.ToString()
                    });
                }
            }
            else
            {
                OnReportScriptMessage(new SqlMessage()
                {
                    MessageType = SqlMessage.SqlMessageType.Message,
                    Message     = e.Message,
                    Source      = e.Source,
                    Line        = ErrorLineOffset + 1
                });
            }
        }

        private void OleDbConnection_InfoMessage(object sender, OleDbInfoMessageEventArgs e)
        {
            if (e.Errors != null && e.Errors.Count > 0)
            {
                foreach (OleDbError error in e.Errors)
                {
                    OnReportScriptMessage(new SqlMessage()
                    {
                        MessageType = SqlMessage.SqlMessageType.Message,
                        Message     = error.Message,
                        HResult     = error.NativeError,
                        Source      = error.Source,
                        State       = error.SQLState,
                        Line        = ErrorLineOffset + 1
                    });
                }
            }
            else
            {
                OnReportScriptMessage(new SqlMessage()
                {
                    MessageType = SqlMessage.SqlMessageType.Message,
                    HResult     = e.ErrorCode,
                    Message     = e.Message,
                    Source      = e.Source,
                    Line        = ErrorLineOffset + 1
                });
            }
        }

        private void OdbcConnection_InfoMessage(object sender, OdbcInfoMessageEventArgs e)
        {
            if (e.Errors != null && e.Errors.Count > 0)
            {
                foreach (OdbcError error in e.Errors)
                {
                    OnReportScriptMessage(new SqlMessage()
                    {
                        MessageType = SqlMessage.SqlMessageType.Message,
                        Message     = error.Message,
                        HResult     = error.NativeError,
                        Source      = error.Source,
                        State       = error.SQLState,
                        Line        = ErrorLineOffset + 1
                    });
                }
            }
            else
            {
                OnReportScriptMessage(new SqlMessage()
                {
                    MessageType = SqlMessage.SqlMessageType.Message,
                    Message     = e.Message,
                    Line        = ErrorLineOffset + 1
                });
            }
        }

        /*
        private void OracleConnection_InfoMessage(object sender, OracleInfoMessageEventArgs e)
        {
            OnReportScriptMessage(new SqlMessage()
            {
                MessageType = SqlMessage.SqlMessageType.Message,
                Message     = e.Message,
                ErrorCode   = e.Code,
                Line        = ErrorLineOffset + 1
            });
        }
        */

        private void MySqlConnection_InfoMessage(object sender, MySqlInfoMessageEventArgs e)
        {
            if (e.Errors != null && e.Errors.Count > 0)
            {
                foreach (MySqlError error in e.Errors)
                {
                    OnReportScriptMessage(new SqlMessage()
                    {
                        MessageType = SqlMessage.SqlMessageType.Message,
                        Message     = error.Message,
                        HResult     = error.Code,
                        State       = error.Level,
                        Line        = ErrorLineOffset + 1
                    });
                }
            }
            else
            {
                OnReportScriptMessage(new SqlMessage()
                {
                    MessageType = SqlMessage.SqlMessageType.Message,
                    Message     = "Unknown error",
                    Line        = ErrorLineOffset + 1
                });
            }      
        }

        private void Adapter_FillError(object sender, FillErrorEventArgs e)
        {
            e.Continue = false;
            OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot fetch data: {e.Errors}" != null ? e.Errors.Message : "unknown error.") { Line = ErrorLineOffset + 1 });
        }

        private void UpdateDataSetAfterScriptExecution(DataSet dataSet)
        {
            if (dataSet == null)
                return;

            var relations = new List<Relation>();
            int relationCounter = 0;

            foreach (var command in Commands)
            {
                var commandRelations = command.GetDataRelations();
                relations.AddRange(commandRelations);
            }

            foreach (var relation in relations)
            {
                var table1 = dataSet.Tables[relation.ParentTableName];
                if (table1 == null)
                {
                    OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot create relation - '{table1}' is not found."));
                    continue;
                }

                var table2 = dataSet.Tables[relation.ChildTableName];
                if (table2 == null)
                {
                    OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot create relation - '{table1}' is not found."));
                    continue;
                }

                if (relation.ParentColumnNames == null || relation.ChildColumnNames == null ||
                    relation.ParentColumnNames.Count != relation.ChildColumnNames.Count)
                {
                    OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot create relation '{relation.ParentTableName}'-'{relation.ChildTableName}' - column count is different."));
                    continue;
                }

                var columns1 = new List<DataColumn>();
                foreach (var columnName in relation.ParentColumnNames)
                {
                    var column1 = table1.Columns[columnName];
                    if (column1 == null)
                    {
                        OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot create relation '{relation.ParentTableName}'-'{relation.ChildTableName}' - cannot find column '{columnName}'"));
                        break;
                    }
                    columns1.Add(column1);
                }

                var columns2 = new List<DataColumn>();
                foreach (var columnName in relation.ChildColumnNames)
                {
                    var column2 = table2.Columns[columnName];
                    if (column2 == null)
                    {
                        OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot create relation '{relation.ParentTableName}'-'{relation.ChildTableName}' - cannot find column '{columnName}'"));
                        break;
                    }
                    columns2.Add(column2);
                }

                if (columns1.Count != relation.ParentColumnNames.Count ||
                    columns2.Count != relation.ChildColumnNames.Count)
                    continue;

                try
                {
                    var relationName = !string.IsNullOrWhiteSpace(relation.RelationName) ? relation.RelationName : $"Relation{++relationCounter}";
                    dataSet.Relations.Add(relationName, columns1.ToArray(), columns2.ToArray());
                }
                catch (Exception ex)
                {
                    OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot create relation '{relation.ParentTableName}'-'{relation.ChildTableName}' - {ex.Message}"));
                    continue;
                }
            }
        }

        public void ExecuteScript()
        {
            var cancelToken       = CancelScriptToken;

            var connection        = Connection;
            var connectionName    = ConnectionName;
            var dataSet           = DataSet;
            var dispatcherService = SCDispatcherService;

            var initialConnection     = connection;
            var initialConnectionName = connectionName;

            if (dataSet == null)
                throw new Exception("Dataset is not provider");

            var tableCounter          = GetStartTableCounter(dataSet);
            var tableNameCounter      = 0;
            int commandCounter        = 0;
            var stopWatch             = new Stopwatch();
            var postProcessTableTasks = new List<Task>();

            stopWatch.Start();

            PrepareConnection(connection);
            try
            {
                foreach (var command in Commands)
                {
                    if (cancelToken.IsCancellationRequested)
                        return;

                    var newConnectionName = command.GetConnectionName();
                    if (!string.IsNullOrWhiteSpace(newConnectionName))
                    {
                        DbConnection newConnection;
                        if (string.Compare(newConnectionName, initialConnectionName, true) == 0 && initialConnection != null)
                            newConnection = initialConnection;
                        else
                            newConnection = OnRequestConnection(newConnectionName) ?? throw new Exception($"Connection '{newConnectionName}' is not available");

                        UnprepareConnection(connection);
                        if (connection != initialConnection)
                        {
                            connection.Close();
                            connection.Dispose();
                        }

                        connection = newConnection;
                        PrepareConnection(connection);

                        OnConnectionChanged(connection);

                        OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Log, $"Connection changed to \"{newConnectionName}\""));
                    }

                    commandCounter++;
                    var commandTables   = command.GetTables();
                    var formatting      = command.GetTableFormatting();
                    var computedColumns = command.GetTableComputedColumns();

                    using var dbCommand = connection.CreateCommand();
                    //_CurrentDbCommand = dbCommand;
                    tableNameCounter = 0;

                    dbCommand.CommandTimeout = ScriptParameters.CommandTimeout;
                    dbCommand.CommandText = command.Text;

                    if (command.AllParameters)
                    {
                        foreach (var parameter in ScriptParameters.CommandParameters)
                        {
                            var dbParameter = CreateDbParameter(dbCommand, parameter);
                            dbCommand.Parameters.Add(dbParameter);
                        }
                    }
                    else
                    {
                        foreach (var parameterName in command.Parameters)
                        {
                            var parameter = ScriptParameters.FindDbParameter(parameterName);
                            if (parameter == null)
                            {
                                OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, $"Cannot find parameter '{parameterName}'"));
                                break;
                            }

                            var dbParameter = CreateDbParameter(dbCommand, parameter);
                            dbCommand.Parameters.Add(dbParameter);
                        }
                    }

                    var readerTask = Task.Run<DbDataReader>(() => dbCommand.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancelToken));
                    readerTask.Wait(cancelToken);
                    var reader = readerTask.Result;

                    using var scriptReader = new SqlScriptDataReader(reader, cancelToken, true);
                    do
                    {
                        if (cancelToken.IsCancellationRequested)
                            return;

                        var dataTable = new DataTable("Table");
                        string tableName = null;

                        using var adapter = new LoadAdapter();
                        adapter.FillError += Adapter_FillError;
                        try
                        {
                            adapter.FillSchema(dataTable, SchemaType.Source, scriptReader);
                            adapter.Fill(dataTable, scriptReader);
                            int recordsAffected = scriptReader.RecordsAffected;

                            if (dataTable.Columns.Count > 0)
                            {
                                var cmdTable = (commandTables != null && tableNameCounter < commandTables.Length) ?
                                    commandTables[tableNameCounter] : null;

                                tableName = cmdTable?.TableName ?? $"Table {tableCounter}";
                                tableNameCounter++;
                                tableCounter++;
                                dataTable.TableName = tableName;

                                if (cmdTable != null)
                                {
                                    foreach (var tableProperty in cmdTable.Properties)
                                        dataTable.ExtendedProperties[$"Table_{tableProperty.Key}"] = tableProperty.Value;
                                }

                                var tableFormattings = formatting.Where(x => string.Compare(x.TableName, tableName, true) == 0);
                                int formatCounter = 0;
                                foreach (var tableFormatting in tableFormattings)
                                    dataTable.ExtendedProperties[$"Format_{++formatCounter}"] = tableFormatting.AsString();

                                var tableComputedColumns = computedColumns.Where(x => string.Compare(x.TableName, tableName, true) == 0);
                                var computedColumnCounter = 0;
                                foreach (var computedColumn in tableComputedColumns)
                                    dataTable.ExtendedProperties[$"ComputedColumn_{++computedColumnCounter}"] = computedColumn.AsString();

                                var prevTaskPostProcessTable = postProcessTableTasks.LastOrDefault();
                                var taskPostProcessTable = AppendTable(dataSet, dataTable, prevTaskPostProcessTable);

                                postProcessTableTasks.Add(taskPostProcessTable);
                            }
                            else
                            {
                                dataTable = null;

                                if (recordsAffected >= 0)
                                    OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Log, $"{recordsAffected} records affected."));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Cannot fetch data in table {tableName}: {ex.Message}");
                        }
                        finally
                        {
                            adapter.FillError -= Adapter_FillError;
                        }
                    } while (scriptReader.NextResult());
                }
            }
            //throw exceptions
            catch (DbException)
            {
                //OnReportScriptMessage(new SqlMessage(ex));
                throw;
            }
            catch (Exception)
            {
                //OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Error, ex.Message));
                throw;
            }
            finally
            {
                UnprepareConnection(connection);

                if (postProcessTableTasks.Count > 0)
                    Task.WaitAll(postProcessTableTasks.ToArray());

                stopWatch.Stop();

                OnReportScriptMessage(new SqlMessage(SqlMessage.SqlMessageType.Log, $"Script is executed in {stopWatch.Elapsed:hh\\:mm\\:ss\\.ff}"));

                ExecuteAction(() =>
                {
                    UpdateDataSetAfterScriptExecution(dataSet);
                });
            }


            void ExecuteAction(Action function)
            {
                if (dispatcherService != null)
                    dispatcherService.BeginInvoke(function);
                else
                    lock (this)
                        function();
            }

            static int GetStartTableCounter(DataSet ds)
            {
                int result = 0;
                if (ds.Tables.Count <= 0)
                    return result;

                var reTableName = new Regex(@"(?i)Table (?<Counter>\d+)");

                foreach (DataTable table in ds.Tables)
                {
                    var match = reTableName.Match(table.TableName);
                    if (!match.Success)
                        continue;

                    int counter = Convert.ToInt32(match.Groups["Counter"].Value);
                    if (counter >= result)
                        result = counter + 1;
                }

                return result;
            }

            Task AppendTable(DataSet dataSet, DataTable dataTable, Task prevTaskPostProcessTable)
            {
                var result = Task.Run(() => DoAppendTable(dataSet, dataTable, prevTaskPostProcessTable));
                return result;
            }

            void DoAppendTable(DataSet dataSet, DataTable dataTable, Task prevTaskPostProcessTable)
            {
                FixDataTypes(dataTable);

                //Ensure that tables are output (attached to DataSet) in correct order
                if (prevTaskPostProcessTable != null)
                    prevTaskPostProcessTable.Wait();

                ExecuteAction(() =>
                {
                    dataSet.Tables.Add(dataTable);
                });
            }
        }
    }
}
