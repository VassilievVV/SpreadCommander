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

        public override string GetColumnDataType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return "text";

            return Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean    => "tinyint",
                TypeCode.Byte       => "smallint",
                TypeCode.Char       => "text",
                TypeCode.DateTime   => "datetime",
                TypeCode.Decimal    => "float",
                TypeCode.Double     => "float",
                TypeCode.Int16      => "integer",
                TypeCode.Int32      => "integer",
                TypeCode.Int64      => "bigint",
                TypeCode.Object     => "blob",
                TypeCode.SByte      => "integer",
                TypeCode.Single     => "float",
                TypeCode.String     => "text",
                TypeCode.UInt16     => "integer",
                TypeCode.UInt32     => "bigint",
                TypeCode.UInt64     => "bigint",
                _                   => "blob"
            };
        }

        public override void DropTable(DbConnection connection, string tableSchema, string tableName)
        {
            var tblName = GetQualifiedTableName(tableSchema, tableName);

            using var cmd   = connection.CreateCommand();
            cmd.CommandText = $"drop table if exists {tblName}";
            cmd.ExecuteNonQuery();
        }
    }
}
