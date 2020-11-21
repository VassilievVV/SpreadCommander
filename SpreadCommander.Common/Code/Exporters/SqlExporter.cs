using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpreadCommander.Common;
using SpreadCommander.Common.SqlScript;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Code.Exporters
{
    public class SqlExporter: DbExporter
    {
        #region BulkData
        private class BulkData
        {
            public CancellationToken CancellationToken { get; set; }

            public DbDataReader Table { get; set; }
        }
        #endregion

        public override string Name => "MS SQL Server Exporter";
        
        public override object CreateConnectionStringBuilder()
        {
            return new SqlConnectionStringBuilder();
        }

        public override DbConnection CreateConnection(object connectionStringBuilder)
        {
            if (connectionStringBuilder is SqlConnectionStringBuilder sqlConnectionStringBuilder)
            {
                var connStr = sqlConnectionStringBuilder.ConnectionString;
                return new SqlConnection(connStr);
            }
            else if (connectionStringBuilder is SqlConnectionStringBuilderLight sqlConnectionStringBuilderLight)
            {
                var connStr = sqlConnectionStringBuilderLight.ConnectionString;
                return new SqlConnection(connStr);
            }
            else
                throw new ArgumentException("Invalid connection string builder");
        }

        public override string QuoteString(string value)
        {
            return Utils.QuoteString(value, "[");
        }

        public override string GetCreateTableCommandText(DbConnection connection, DbDataReader table, string tableSchema, string tableName)
        {
            var result = new StringBuilder();

            var tblName = GetQualifiedTableName(tableSchema, tableName);

            var columnID = GetUniqueColumnName(table, "ID");

            //Table name should be already quoted, quoting rules are different for different databases, in SQL Server it can be dbo.MyTable and [dbo.MyTable] is not correct
            result.AppendLine($"create table {tblName} (");
            result.AppendLine($"  {columnID} integer identity(1, 1) primary key, ");

            for (int i = 0; i < table.FieldCount; i++)
                result.AppendLine($"  {QuoteString(table.GetName(i))} {GetColumnDataType(table.GetFieldType(i), GetColumnMaxLength(table, i))}, ");

            //Remove last ", "
            result.Length -= (2 + Environment.NewLine.Length);
            result.AppendLine().Append(')');

            return result.ToString();
        }

        private static readonly Dictionary<SqlBulkCopy, BulkData> _BulkData = new Dictionary<SqlBulkCopy, BulkData>();

        public override void ExportDataTable(DbConnection connection, DbDataReader table,
            string tableSchema, string tableName, bool needCreateTable,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            if (needCreateTable)
                CreateTable(connection, table, tableSchema, tableName);

            if (cancellationToken.IsCancellationRequested)
                return;

            var count = table is IList listTable ? listTable.Count : -1;
            if (count < 0 && table is IListSource listSource)
            {
                var list = listSource.GetList();
                if (list != null)
                    count = list.Count;
            }

            var tblName = GetQualifiedTableName(tableSchema, tableName);

            int batchSize = BatchSize ?? 1000;

            ReportProgress(0, count);

            using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection))
            {
                try
                {
                    _BulkData[bulkCopy] = new BulkData() { CancellationToken = cancellationToken, Table = table };

                    bulkCopy.DestinationTableName = tblName;
                    bulkCopy.BatchSize            = batchSize;
                    //bulkCopy.NotifyAfter = BatchCopyNotifyAfter;
                    bulkCopy.SqlRowsCopied       += BulkCopy_SqlRowsCopied; ;

                    for (int i = 0; i < table.FieldCount; i++)
                        bulkCopy.ColumnMappings.Add(i, table.GetName(i));

                    bulkCopy.WriteToServer(table);
                }
                finally
                {
                    if (_BulkData.ContainsKey(bulkCopy))
                        _BulkData.Remove(bulkCopy);
                }
            }

            ReportProgress(count, count);
        }

        private void BulkCopy_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            if (sender is SqlBulkCopy bulkCopy && _BulkData.ContainsKey(bulkCopy))
            {
                var bulkData = _BulkData[bulkCopy];
                if (bulkData.CancellationToken.IsCancellationRequested)
                    e.Abort = true;
                else
                {
                    var count = bulkData.Table is IList list ? list.Count : -1;
                    if (count < 0 && bulkData.Table is IListSource listSource)
                    {
                        var list2 = listSource.GetList();
                        if (list2 != null)
                            count = list2.Count;
                    }

                    ReportProgress(e.RowsCopied, count);
                }
            }
        }

        public override string GetColumnDataType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return "uniqueidentifier";

            return base.GetColumnDataType(dataType, maxLength);
        }

        public static (SqlDbType dbType, int len) GetColumnSqlDbType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return (SqlDbType.UniqueIdentifier, 50);

            return Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean  => (SqlDbType.Bit, 0),
                TypeCode.Byte     => (SqlDbType.TinyInt, 0),
                TypeCode.Char     => (SqlDbType.NVarChar, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.DateTime => (SqlDbType.DateTime, 0),
                TypeCode.Decimal  => (SqlDbType.Float, 0),
                TypeCode.Double   => (SqlDbType.Float, 0),
                TypeCode.Int16    => (SqlDbType.Int, 0),
                TypeCode.Int32    => (SqlDbType.Int, 0),
                TypeCode.Int64    => (SqlDbType.BigInt, 0),
                TypeCode.Object   => (SqlDbType.Binary, int.MaxValue),
                TypeCode.SByte    => (SqlDbType.SmallInt, 0),
                TypeCode.Single   => (SqlDbType.Float, 0),
                TypeCode.String   => (SqlDbType.NVarChar, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.UInt16   => (SqlDbType.Int, 0),
                TypeCode.UInt32   => (SqlDbType.BigInt, 0),
                TypeCode.UInt64   => (SqlDbType.BigInt, 0),
                _                 => (SqlDbType.Binary, int.MaxValue),
            };
        }

        public override void DropTable(DbConnection connection, string tableSchema, string tableName)
        {
            var tblName = GetQualifiedTableName(tableSchema, tableName);

            using var cmd = connection.CreateCommand();
            cmd.CommandText =
$@"if (object_id({Utils.QuoteString(tblName, "'")}) is not null)
    drop table {tblName}";
            cmd.ExecuteNonQuery();
        }
    }
}
