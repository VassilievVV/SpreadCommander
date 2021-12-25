using SpreadCommander.Common.Code;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public class InvokeSqlScriptOptions
    {
        [Description("Parameters for SQL script")]
        public Hashtable Parameters { get; set; }

        [Description("Timeout for commands in SQL script")]
        public int? CommandTimeout { get; set; }

        [Description("When set - returns value from first column and first row of the first table or $null if there is no such value. Query is executing completely and all data retrieved from server.")]
        public bool Scalar { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }
    }

    public partial class ScriptHost
    {
        public object InvokeSqlScriptScalar(string connectionName, string query, InvokeSqlScriptOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                var result = InvokeSqlScriptScalar(GetDbConnection(connectionName), query, options);
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public object InvokeSqlScriptScalarFromFile(string connectionName, string fileName, InvokeSqlScriptOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                var result = InvokeSqlScriptScalarFromFile(GetDbConnection(connectionName), fileName, options);
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet InvokeSqlScriptFromFile(string connectionName, string fileName, InvokeSqlScriptOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                var result = InvokeSqlScriptFromFile(GetDbConnection(connectionName), fileName, options);
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet InvokeSqlScript(string connectionName, string query, InvokeSqlScriptOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                var result = InvokeSqlScript(conn.DbConnection, query, options);
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public object InvokeSqlScriptScalar(DbConnection connection, string query, InvokeSqlScriptOptions options = null)
        {
            var dataSet = InvokeSqlScript(connection, query, options);
            
            object result = null;

            if (dataSet != null && dataSet.Tables.Count > 0 &&
                dataSet.Tables[0].Columns.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                result = dataSet.Tables[0].Rows[0][0];

            if (result == DBNull.Value)
                result = null;

            return result;
        }

        public object InvokeSqlScriptScalarFromFile(DbConnection connection, string fileName, InvokeSqlScriptOptions options = null)
        {
            var dataSet = InvokeSqlScriptFromFile(connection, fileName, options);

            object result = null;

            if (dataSet != null && dataSet.Tables.Count > 0 &&
                dataSet.Tables[0].Columns.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                result = dataSet.Tables[0].Rows[0][0];

            if (result == DBNull.Value)
                result = null;

            return result;
        }

        public DataSet InvokeSqlScriptFromFile(DbConnection connection, string fileName, InvokeSqlScriptOptions options = null)
        {
            var scriptFile    = Project.Current.MapPath(fileName);
            string scriptText = null;
            if (!string.IsNullOrWhiteSpace(scriptFile) && File.Exists(scriptFile))
            {
                ScriptHostObject.ExecuteLocked(() =>
                {
                    using var reader = File.OpenText(scriptFile);
                    scriptText       = reader.ReadToEnd();
                }, options?.LockFiles ?? false ? ScriptHostObject.LockObject : null);
            }

            var result = InvokeSqlScript(connection, scriptText, options);
            return result;
        }

#pragma warning disable CA1822 // Mark members as static
        public DataSet InvokeSqlScript(DbConnection connection, string query, InvokeSqlScriptOptions options = null)
#pragma warning restore CA1822 // Mark members as static
        {
            options ??= new InvokeSqlScriptOptions();

            string scriptText = query;

            if (string.IsNullOrWhiteSpace(scriptText))
                throw new Exception("Cannot load script text.");

            if ((connection?.State ?? ConnectionState.Closed) != ConnectionState.Open)
                connection.Open();

            var dataSet = new DataSet("Data");

            var sqlScript = new SqlScript.SqlScript()
            {
                ScriptText          = scriptText,
                Connection          = connection,
                ConnectionName      = null,
                DataSet             = dataSet,
                SCDispatcherService = null
            };

            if (options.CommandTimeout.HasValue)
                sqlScript.ScriptParameters.CommandTimeout = options.CommandTimeout.Value;

            var dbConnections = DBConnections.LoadConnections();

            sqlScript.ConnectionChanged += (s, e) => { };
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

            if (options.Parameters != null)
            {
                foreach (DictionaryEntry keyPair in options.Parameters)
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

            return dataSet;
        }
    }
}
