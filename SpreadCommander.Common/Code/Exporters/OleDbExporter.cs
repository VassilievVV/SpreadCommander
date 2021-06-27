using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code.Exporters
{
    public class OleDbExporter: DbExporter
    {
        public override string Name => "OLEDB Exporter";

        public override object CreateConnectionStringBuilder()
        {
            return new OleDbConnectionStringBuilder();
        }

        public override DbConnection CreateConnection(object connectionStringBuilder)
        {
            if (connectionStringBuilder is not OleDbConnectionStringBuilder oleDbConnectionStringBuilder)
                throw new ArgumentException("Invalid connection string builder");

            var connStr = oleDbConnectionStringBuilder.ConnectionString;
            var result  = new OleDbConnection();
            Connection.SetConnectionString(result, connStr);
            return result;
        }

        public override void FillInsertCommandParameters(DbCommand cmdInsert, DbDataReader table)
        {
            if (cmdInsert is not OleDbCommand oleDbCommand)
                throw new ArgumentException("Command is not OLEDB command.");

            var parameterID       = oleDbCommand.CreateParameter();
            parameterID.OleDbType = OleDbType.Integer;
            cmdInsert.Parameters.Add(parameterID);

            for (int i = 0; i < table.FieldCount; i++)
            {
                var parameter = oleDbCommand.CreateParameter();
                (var dbType, var maxSize) = GetColumnOleDbType(table.GetFieldType(i), GetColumnMaxLength(table, i));
                parameter.OleDbType = dbType;
                if (maxSize > 0)
                    parameter.Size = maxSize;
                cmdInsert.Parameters.Add(parameter);
            }
        }

        public static (OleDbType dbType, int len) GetColumnOleDbType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return (OleDbType.VarWChar, 50);

            return Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean  => (OleDbType.Boolean, 0),
                TypeCode.Byte     => (OleDbType.TinyInt, 0),
                TypeCode.Char     => (OleDbType.VarWChar, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.DateTime => (OleDbType.Date, 0),
                TypeCode.Decimal  => (OleDbType.Double, 0),
                TypeCode.Double   => (OleDbType.Double, 0),
                TypeCode.Int16    => (OleDbType.SmallInt, 0),
                TypeCode.Int32    => (OleDbType.Integer, 0),
                TypeCode.Int64    => (OleDbType.BigInt, 0),
                TypeCode.Object   => (OleDbType.Binary, int.MaxValue),
                TypeCode.SByte    => (OleDbType.SmallInt, 0),
                TypeCode.Single   => (OleDbType.Double, 0),
                TypeCode.String   => (OleDbType.VarWChar, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.UInt16   => (OleDbType.UnsignedSmallInt, 0),
                TypeCode.UInt32   => (OleDbType.UnsignedInt, 0),
                TypeCode.UInt64   => (OleDbType.UnsignedBigInt, 0),
                _                 => (OleDbType.Binary, int.MaxValue),
            };
        }
    }
}
