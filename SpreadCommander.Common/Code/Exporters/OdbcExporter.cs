using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code.Exporters
{
    public class OdbcExporter: DbExporter
    {
        public override string Name => "ODBC Exporter";

        public override object CreateConnectionStringBuilder()
        {
            return new OdbcConnectionStringBuilder();
        }

        public override DbConnection CreateConnection(object connectionStringBuilder)
        {
            if (connectionStringBuilder is not OdbcConnectionStringBuilder odbcConnectionStringBuilder)
                throw new ArgumentException("Invalid connection string builder");

            var connStr = odbcConnectionStringBuilder.ConnectionString;
            return new OdbcConnection(connStr);
        }

        public override void FillInsertCommandParameters(DbCommand cmdInsert, DbDataReader table)
        {
            if (cmdInsert is not OdbcCommand odbcCommand)
                throw new ArgumentException("Command is not ODBC command.");

            var parameterID      = odbcCommand.CreateParameter();
            parameterID.OdbcType = OdbcType.Int;
            cmdInsert.Parameters.Add(parameterID);

            for (int i = 0; i < table.FieldCount; i++)
            {
                var parameter = odbcCommand.CreateParameter();
                (var dbType, var maxSize) = GetColumnOdbcType(table.GetFieldType(i), GetColumnMaxLength(table, i));
                parameter.OdbcType = dbType;
                if (maxSize > 0)
                    parameter.Size = maxSize;
                cmdInsert.Parameters.Add(parameter);
            }
        }

        public static (OdbcType dbType, int len) GetColumnOdbcType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return (OdbcType.NVarChar, 50);

            return Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean  => (OdbcType.Bit, 0),
                TypeCode.Byte     => (OdbcType.TinyInt, 0),
                TypeCode.Char     => (OdbcType.NVarChar, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.DateTime => (OdbcType.DateTime, 0),
                TypeCode.Decimal  => (OdbcType.Double, 0),
                TypeCode.Double   => (OdbcType.Double, 0),
                TypeCode.Int16    => (OdbcType.SmallInt, 0),
                TypeCode.Int32    => (OdbcType.Int, 0),
                TypeCode.Int64    => (OdbcType.BigInt, 0),
                TypeCode.Object   => (OdbcType.Binary, int.MaxValue),
                TypeCode.SByte    => (OdbcType.SmallInt, 0),
                TypeCode.Single   => (OdbcType.Double, 0),
                TypeCode.String   => (OdbcType.NVarChar, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.UInt16   => (OdbcType.Int, 0),
                TypeCode.UInt32   => (OdbcType.BigInt, 0),
                TypeCode.UInt64   => (OdbcType.BigInt, 0),
                _                 => (OdbcType.Binary, int.MaxValue),
            };
        }
    }
}
