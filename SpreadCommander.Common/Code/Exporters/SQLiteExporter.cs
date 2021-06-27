using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code.Exporters
{
    public class SQLiteExporter: DbExporter
    {
        public override string Name => "SQLite Exporter";

        public override object CreateConnectionStringBuilder()
        {
            return new SQLiteConnectionStringBuilder();
        }

        public override DbConnection CreateConnection(object connectionStringBuilder)
        {
            if (connectionStringBuilder is not SQLiteConnectionStringBuilder sqliteConnectionStringBuilder)
                throw new ArgumentException("Invalid connection string builder");

            var connStr = sqliteConnectionStringBuilder.ConnectionString;
            var result  = new SQLiteConnection();
            Connection.SetConnectionString(result, connStr);
            return result;
        }

        public override string QuoteString(string value)
        {
            return Utils.QuoteString(value, "[");
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
