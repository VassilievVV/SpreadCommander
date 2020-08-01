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
using SpreadCommander.Common.PowerShell.CmdLets.Book;
using SpreadCommander.Common.SqlScript;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlQuery")]
    [OutputType(typeof(DataTable))]
    [OutputType(typeof(DbDataReader))]
    public class InvokeSqlQueryCmdlet : SCCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ConnectionName", Position = 0, HelpMessage = "Name of connection for SQL script")]
        public string ConnectionName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Connection", Position = 0, HelpMessage = "Database connection for SQL script")]
        public DbConnection Connection { get; set; }

        [Parameter(HelpMessage = "SQL Script text")]
        [Alias("ScriptText", "Script", "q")]
        public string Query { get; set; }

        [Parameter(HelpMessage = "Parameters for SQL script")]
        public Hashtable Parameters { get; set; }

        [Parameter(HelpMessage = "Timeout for commands in SQL script")]
        [Alias("Timeout")]
        public int? CommandTimeout { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "If set - messages returned from DBMS will be returned in second object (first object is DataSet with results).")]
        public SwitchParameter OutputMessages { get; set; }

        [Parameter(HelpMessage = "Return DbDataReader instead of DataTable. Can be used to export data into database.")]
        public SwitchParameter AsDataReader { get; set; }


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
            if (string.IsNullOrWhiteSpace(Query))
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

            var cmd         = conn.CreateDbCommand();
            cmd.CommandText = Query;
            if (CommandTimeout.HasValue)
                cmd.CommandTimeout = CommandTimeout.Value;

            DbDataReader reader = null;

            try
            {
                if (Parameters != null)
                {
                    foreach (DictionaryEntry keyPair in Parameters)
                    {
                        var parameterType     = keyPair.Value?.GetType() ?? typeof(string);
                        (var dbType, var len) = ScriptRunParameter.GetColumnDbType(parameterType, int.MaxValue / 2);


                        var param           = cmd.CreateParameter();
                        param.ParameterName = Convert.ToString(keyPair.Key);
                        param.DbType        = dbType;
                        param.Value         = keyPair.Value;
                        if (len > 0)
                            param.Size = len;

                        cmd.Parameters.Add(param);
                    }
                }

                var readerParameters = new DataReaderWrapper.DataReaderWrapperParameters()
                {
                    Columns = this.SelectColumns, 
                    CloseAction = () =>
                    {
                        cmd.Dispose();
                        if (closeConnection)
                            conn?.Close();
                    }
                };
                reader = new DataReaderWrapper(cmd.ExecuteReader(), readerParameters);
                if (AsDataReader)
                    return reader;

                var table = new DataTable("Query");
                table.Load(reader);

                if (closeConnection)
                    conn?.Close();

                return table;
            }
            catch (Exception ex)
            {

                reader?.Dispose();
                cmd?.Dispose();

                if (closeConnection)
                    conn?.Close();

                throw ex;
            }
        }
    }
}
