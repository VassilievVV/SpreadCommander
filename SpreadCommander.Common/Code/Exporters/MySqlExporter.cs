using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code.Exporters
{
    public class MySqlExporter: DbExporter
    {
        public override string Name => "MySQL Exporter";

        public override object CreateConnectionStringBuilder()
        {
            return new MySqlConnectionStringBuilder();
        }

        public override DbConnection CreateConnection(object connectionStringBuilder)
        {
            if (connectionStringBuilder is not MySqlConnectionStringBuilder mySqlConnectionStringBuilder)
                throw new ArgumentException("Invalid connection string builder");

            var connStr = mySqlConnectionStringBuilder.ConnectionString;
            var result  = new MySqlConnection();
            Connection.SetConnectionString(result, connStr);
            return result;
        }

        public override string QuoteString(string value)
        {
            return Utils.QuoteString(value, "`");
        }

        public override void DropTable(DbConnection connection, string tableSchema, string tableName)
        {
            var tblName = GetQualifiedTableName(tableSchema, tableName);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"drop table if exists {tblName}";
            cmd.ExecuteNonQuery();
        }
    }
}
