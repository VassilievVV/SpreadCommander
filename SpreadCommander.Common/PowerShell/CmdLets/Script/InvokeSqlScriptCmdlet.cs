using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.SqlScript;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlScript")]
    [OutputType(typeof(DataSet))]
    [OutputType(typeof(object))]
    public class InvokeSqlScriptCmdlet: SCCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ConnectionName", Position = 0, HelpMessage = "Name of connection for SQL script")]
        public string ConnectionName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Connection", Position = 0, HelpMessage = "Database connection for SQL script")]
        public DbConnection Connection { get; set; }

        [Parameter(Position = 1, HelpMessage = "Name of file containing SQL script")]
        public string ScriptFile { get; set; }

        [Parameter(HelpMessage = "SQL Script text")]
        [Alias("ScriptText", "Script", "q")]
        public string Query { get; set; }

        [Parameter(HelpMessage = "Parameters for SQL script")]
        public Hashtable Parameters { get; set; }

        [Parameter(HelpMessage = "Timeout for commands in SQL script")]
        [Alias("Timeout")]
        public int? CommandTimeout { get; set; }
        
        [Parameter(HelpMessage = "If set - messages returned from DBMS will be returned in second object (first object is DataSet with results).")]
        public SwitchParameter OutputMessages { get; set; }

        [Parameter(HelpMessage = "When set - returns value from first column and first row of the first table or $null if there is no such value. Query is executing completely and all data retrieved from server.")]
        [Alias("s")]
        public SwitchParameter Scalar { get; set; }


        private readonly StringBuilder _Messages = new StringBuilder();

        protected override void ProcessRecord()
        {
            _Messages.Clear();
            
            var result = ExecuteScript();
            WriteObject(result);

            if (OutputMessages)
                WriteObject(_Messages.ToString());
        }

        public object ExecuteScript()
        {
            string scriptText = Query;
            if (string.IsNullOrWhiteSpace(scriptText))
            {
                lock (LockObject)
                {
                    var scriptFile = Project.Current.MapPath(ScriptFile);
                    if (!string.IsNullOrWhiteSpace(scriptFile) && File.Exists(scriptFile))
                    {
                        using var reader = File.OpenText(scriptFile);
                        scriptText = reader.ReadToEnd();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(scriptText))
                throw new Exception("Cannot load script text.");

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

            var dataSet = new DataSet("Data");

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

            sqlScript.ConnectionChanged   += (s, e) => { };
            sqlScript.ReportScriptMessage += (s, e) => { ScriptMessageOutput(e.Message); };
            sqlScript.RequestConnection   += (s, e) =>
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

            try
            {
                if (Parameters != null)
                {
                    foreach (DictionaryEntry keyPair in Parameters)
                    {
                        var parameterType = keyPair.Value?.GetType() ?? typeof(string);

                        var parameter = new ScriptRunParameter()
                        {
                            Name   = Convert.ToString(keyPair.Key),
                            Type   = parameterType,
                            DbType = ScriptRunParameter.GetColumnDbType(parameterType),
                            Value  = keyPair.Value
                        };

                        sqlScript.ScriptParameters.CommandParameters.Add(parameter);
                    }
                }

                sqlScript.ExecuteScript();

                if (closeConnection)
                    conn?.Close();
            }
            /*
            catch (DbException ex)
            {
                ScriptMessageOutput(new SqlMessage(ex));
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    var errorMessage = innerEx is DbException ? new SqlMessage((DbException)innerEx) : new SqlMessage(SqlMessage.SqlMessageType.Error, innerEx.Message);
                    ScriptMessageOutput(errorMessage);
                }
            }
            catch (Exception ex)
            {
                ScriptMessageOutput(new SqlMessage(SqlMessage.SqlMessageType.Error, ex.Message));
            }
            */
            catch (Exception)
            {
                if (closeConnection)
                    conn?.Close();

                throw;
            }

            object result = !Scalar ? dataSet : null;

            if (Scalar)
            {
                if (dataSet != null && dataSet.Tables.Count > 0 &&
                    dataSet.Tables[0].Columns.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    result = dataSet.Tables[0].Rows[0][0];

                if (result == DBNull.Value)
                    result = null;
            }

            return result;
        }

        public void ScriptMessageOutput(SqlMessage message)
        {
            string messageText = null;

            switch (message.MessageType)
            {
                case SqlMessage.SqlMessageType.Message:
                    messageText = message.Message;
                    break;
                case SqlMessage.SqlMessageType.Error:
                    messageText = $"Error: {message.Message}{Environment.NewLine}";
                    break;
                case SqlMessage.SqlMessageType.Log:
                    messageText = $"Log: {message.Message}{Environment.NewLine}";
                    break;
            }

            if (!string.IsNullOrWhiteSpace(messageText))
                _Messages.AppendLine(messageText);
        }
    }
}
