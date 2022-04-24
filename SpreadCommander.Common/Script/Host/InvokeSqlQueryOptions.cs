using SpreadCommander.Common.Code;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public class InvokeSqlQueryOptions
    {
        [Description("Parameters for SQL script")]
        public Hashtable Parameters { get; set; }

        [Description("Timeout for commands in SQL script")]
        public int? CommandTimeout { get; set; }

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }
    }

    public partial class ScriptHost
    {
        public DataTable InvokeSqlQuery(string connectionName, string query, InvokeSqlQueryOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                var result = InvokeSqlQuery(GetDbConnection(connectionName), query, options);
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public DbDataReader InvokeSqlQueryAsDataReader(string connectionName, string query, InvokeSqlQueryOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                var result = InvokeSqlQueryAsDataReader(GetDbConnection(connectionName), query, options);
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public Deedle.Frame<int, string> InvokeSqlQueryAsDeedleFrame(string connectionName, string query, InvokeSqlQueryOptions options = null)
        {
            var conn = ConnectionFactory.CreateFromString(connectionName);
            try
            {
                var result = InvokeSqlQueryAsDeedleFrame(GetDbConnection(connectionName), query, options);
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable InvokeSqlQuery(DbConnection connection, string query, InvokeSqlQueryOptions options = null)
        {
            using var reader = InvokeSqlQueryAsDataReader(connection, query, options);
            if (reader == null)
                return null;

            var table = new DataTable("Query");
            table.Load(reader);

            return table;
        }

#pragma warning disable CA1822 // Mark members as static
        public DbDataReader InvokeSqlQueryAsDataReader(DbConnection connection, string query, InvokeSqlQueryOptions options = null)
#pragma warning restore CA1822 // Mark members as static
        {
            options ??= new InvokeSqlQueryOptions();

            if (string.IsNullOrWhiteSpace(query))
                throw new Exception("Cannot load script text.");

            if ((connection?.State ?? ConnectionState.Closed) != ConnectionState.Open)
                connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = query;
            if (options.CommandTimeout.HasValue)
                cmd.CommandTimeout = options.CommandTimeout.Value;

            DbDataReader reader = null;

            try
            {
                if (options.Parameters != null)
                {
                    foreach (DictionaryEntry keyPair in options.Parameters)
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
                    Columns     = options.SelectColumns,
                    SkipColumns = options.SkipColumns,
                    CloseAction = () =>
                    {
                        cmd.Dispose();
                    }
                };
                reader = new DataReaderWrapper(cmd.ExecuteReader(), readerParameters);
                return reader;
            }
            catch (Exception)
            {

                reader?.Dispose();
                cmd?.Dispose();

                throw;
            }
        }

        public Deedle.Frame<int, string> InvokeSqlQueryAsDeedleFrame(DbConnection connection, string query, InvokeSqlQueryOptions options = null)
        {
            using var reader = InvokeSqlQueryAsDataReader(connection, query, options);
            if (reader == null)
                return null;

            var frame = Deedle.Frame.ReadReader(reader);
            return frame;
        }
    }
}
