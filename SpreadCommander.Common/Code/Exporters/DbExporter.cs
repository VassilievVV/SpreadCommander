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
using SpreadCommander.Common;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Code.Exporters
{
    public class DbExporter
    {
        #region ExportProgressEventArgs
        public class ExportProgressEventArgs: EventArgs
        {
            public long Progress { get; set; }
            public long Max { get; set; }
        }
        #endregion

        public int? BatchSize { get; set; }

        public event EventHandler<ExportProgressEventArgs> Progress;

        public virtual object CreateConnectionStringBuilder()
        {
            return null;
        }

        public virtual DbConnection CreateConnection(object connectionStringBuilder)
        {
            return null;
        }

        public virtual string Name => "DB Exporter";

        public static DbExporter GetDbExporter(string providerName)
        {
            if (string.Compare(providerName, ConnectionFactory.SqlClientFactoryLightName, true) == 0)
                return new SqlLightExporter();
            if (string.Compare(providerName, ConnectionFactory.SqlClientFactoryName, true) == 0)
                return new SqlExporter();
            if (string.Compare(providerName, ConnectionFactory.OleDbFactoryName, true) == 0)
                return new OleDbExporter();
            if (string.Compare(providerName, ConnectionFactory.OdbcFactoryName, true) == 0)
                return new OdbcExporter();
            if (string.Compare(providerName, ConnectionFactory.MySqlFactoryName, true) == 0)
                return new MySqlExporter();
            if (string.Compare(providerName, ConnectionFactory.SQLiteFactoryName, true) == 0)
                return new SQLiteExporter();

            return null;
        }

        protected string GetUniqueColumnName(DbDataReader table, string columnName)
        {
            var columnNames = new List<string>();
            for (int i = 0; i < table.FieldCount; i++)
                columnNames.Add(table.GetName(i));
            return Utils.AddUniqueString(columnNames, columnName, StringComparison.CurrentCultureIgnoreCase, false);
        }

        protected virtual int GetColumnMaxLength(DbDataReader reader, int ordinal) =>
            GetColumnMaxLength(reader, reader.GetName(ordinal));

        protected virtual int GetColumnMaxLength(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            if (ordinal < 0)
                return 0;

            var fieldType = reader.GetFieldType(ordinal);
            var fieldTypeCode = Type.GetTypeCode(fieldType);

            try
            {
                var schema = reader.GetSchemaTable();
                if (schema == null || schema.Rows.Count <= 0)
                    return GetSizeFromType(fieldTypeCode);

                if (schema.Columns["ColumnName"] == null || schema.Columns["ColumnSize"] == null)
                    return GetSizeFromType(fieldTypeCode);

                foreach (DataRow row in schema.Rows)
                {
                    var colName = Convert.ToString(row["ColumnName"]);
                    if (string.Compare(colName, columnName, true) == 0)
                        return Convert.ToInt32(row["ColumnSize"]);
                }

                return GetSizeFromType(fieldTypeCode);
            }
            catch (Exception)
            {
                return GetSizeFromType(fieldTypeCode);
            }


            static int GetSizeFromType(TypeCode typeCode)
            {
                return typeCode switch
                {
                    TypeCode.Empty    => 0,
                    TypeCode.Object   => 0,
                    TypeCode.DBNull   => 0,
                    TypeCode.Boolean  => 1,
                    TypeCode.Char     => int.MaxValue,
                    TypeCode.SByte    => 1,
                    TypeCode.Byte     => 1,
                    TypeCode.Int16    => 2,
                    TypeCode.UInt16   => 2,
                    TypeCode.Int32    => 4,
                    TypeCode.UInt32   => 4,
                    TypeCode.Int64    => 8,
                    TypeCode.UInt64   => 8,
                    TypeCode.Single   => 4,
                    TypeCode.Double   => 8,
                    TypeCode.Decimal  => 29,
                    TypeCode.DateTime => 8,
                    TypeCode.String   => int.MaxValue,
                    _                 => 0
                };
            }
        }

        public virtual string GetCreateTableCommandText(DbConnection connection, DbDataReader table, string tableSchema, string tableName)
        {
            var result = new StringBuilder();

            var tblName = GetQualifiedTableName(tableSchema, tableName);

            var columnID = GetUniqueColumnName(table, "ID");

            //Table name should be already quoted, quoting rules are different for different databases, in SQL Server it can be dbo.MyTable and [dbo.MyTable] is not correct
            result.AppendLine($"create table {tblName} (");
            result.AppendLine($"  {columnID} integer primary key, ");

            for (int i = 0; i < table.FieldCount; i++)
                result.AppendLine($"  {QuoteString(table.GetName(i))} {GetColumnDataType(table.GetFieldType(i), GetColumnMaxLength(table, i))}, ");

            //Remove last ", "
            result.Length -= (2 + Environment.NewLine.Length);
            result.AppendLine().Append(")");

            return result.ToString();
        }

        public virtual string GetInsertCommandText(DbConnection connection, DbDataReader table, string tableSchema, string tableName)
        {
            var result = new StringBuilder();

            var tblName = GetQualifiedTableName(tableSchema, tableName);

            var columnID = GetUniqueColumnName(table, "ID");

            result.AppendLine($"insert into {tblName} ({columnID}, ");

            for (int i = 0; i < table.FieldCount; i++)
                result.Append($"{QuoteString(table.GetName(i))}, ");

            //Remove last ", "
            result.Length -= 2;

            result.AppendLine(")").Append("values (?, ");   //first column is ID

            for (int i = 0; i < table.FieldCount; i++)
                result.Append("?, ");

            //Remove last ", "
            result.Length -= 2;

            result.Append(")");

            return result.ToString();
        }

        public virtual string GetQualifiedTableName(string tableSchema, string tableName)
        {
            var result = QuoteString(tableName);
            if (!string.IsNullOrWhiteSpace(tableSchema))
                result = $"{QuoteString(tableSchema)}.{result}";
            return result;
        }

        public virtual void CreateTable(DbConnection connection, DbDataReader table, string tableSchema, string tableName)
        {
            using var cmdCreateTable = connection.CreateCommand();
            cmdCreateTable.CommandText = GetCreateTableCommandText(connection, table, tableSchema, tableName);
            cmdCreateTable.ExecuteNonQuery();
        }

        public virtual void ExportDataTable(DbConnection connection, DbDataReader table,
            string tableSchema, string tableName, bool needCreateTable, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            int progressInterval = BatchSize ?? 100;

            if (needCreateTable)
                CreateTable(connection, table, tableSchema, tableName);

            if (cancellationToken.IsCancellationRequested)
                return;

            var transaction = connection.BeginTransaction();
            try
            {
                using (var cmdInsert = connection.CreateCommand())
                {
                    cmdInsert.Transaction = transaction;
                    cmdInsert.CommandText = GetInsertCommandText(connection, table, tableSchema, tableName);

                    FillInsertCommandParameters(cmdInsert, table);

                    cmdInsert.Prepare();

                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var count = table is IList list ? list.Count : -1;
                    if (count < 0 && table is IListSource listSource)
                    {
                        var list2 = listSource.GetList();
                        if (list2 != null)
                            count = list2.Count;
                    }

                    ReportProgress(0, count);

                    int rowCounter = 0;
                    while (table.Read())
                    {
                        cmdInsert.Parameters[0].Value = ++rowCounter;   //Column ID
                        for (int i = 0; i < table.FieldCount; i++)
                        {
                            object value = table.GetValue(i);
                            if (value != null && value is Guid)
                                value = value.ToString();
                            cmdInsert.Parameters[i + 1].Value = value;
                        }

                        cmdInsert.ExecuteNonQuery();

                        //Report progress for every 100 rows
                        if (rowCounter % progressInterval == 0)
                            ReportProgress(rowCounter, count);

                        if (cancellationToken.IsCancellationRequested)
                            return;
                    }

                    ReportProgress(count, count);
                }

                transaction.Commit();
                transaction.Dispose();
            }
            catch (Exception)
            {
                transaction.Rollback();
                transaction.Dispose();
                
                throw;
            }
        }

        public virtual void FillInsertCommandParameters(DbCommand cmdInsert, DbDataReader table)
        {
            var parameterID    = cmdInsert.CreateParameter();
            parameterID.DbType = DbType.Int32;
            cmdInsert.Parameters.Add(parameterID);

            for (int i = 0; i < table.FieldCount; i++)
            {
                var parameter = cmdInsert.CreateParameter();
                (var dbType, var maxSize) = GetColumnDbType(table.GetFieldType(i), GetColumnMaxLength(table, i));
                parameter.DbType = dbType;
                if (maxSize > 0)
                    parameter.Size = maxSize;
                cmdInsert.Parameters.Add(parameter);
            }
        }

        public virtual string QuoteString(string value)
        {
            return Utils.QuoteString(value, "\"");
        }

        public virtual string GetColumnDataType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return "nvarchar(50)";

            return Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean  => "bit",
                TypeCode.Byte     => "integer",
                //TypeCode.Char     => maxLength > 1000 ? "nvarchar(max)" : $"nchar({maxLength})",
                TypeCode.Char     => maxLength > 1000 ? "text" : $"nchar({maxLength})",
                TypeCode.DateTime => "datetime",
                TypeCode.Decimal  => "float",
                TypeCode.Double   => "float",
                TypeCode.Int16    => "integer",
                TypeCode.Int32    => "integer",
                TypeCode.Int64    => "bigint",
                TypeCode.Object   => "varbinary(max)",
                TypeCode.SByte    => "integer",
                TypeCode.Single   => "float",
                //TypeCode.String   => $"nvarchar({(maxLength > 1000 ? "max" : maxLength.ToString())})",
                TypeCode.String   => maxLength > 1000 ? "text" : $"nvarchar({maxLength})",
                TypeCode.UInt16   => "integer",
                TypeCode.UInt32   => "bigint",
                TypeCode.UInt64   => "bigint",
                _                 => "varbinary(max)"
            };
        }

        public virtual (DbType dbType, int len) GetColumnDbType(Type dataType, int maxLength)
        {
            if (dataType == typeof(Guid))
                return (DbType.String, 50);

            return Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean  => (DbType.Boolean, 0),
                TypeCode.Byte     => (DbType.Byte, 0),
                TypeCode.Char     => (DbType.String, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.DateTime => (DbType.DateTime, 0),
                TypeCode.Decimal  => (DbType.Double, 0),
                TypeCode.Double   => (DbType.Double, 0),
                TypeCode.Int16    => (DbType.Int16, 0),
                TypeCode.Int32    => (DbType.Int32, 0),
                TypeCode.Int64    => (DbType.Int64, 0),
                TypeCode.Object   => (DbType.Binary, int.MaxValue),
                TypeCode.SByte    => (DbType.Int16, 0),
                TypeCode.Single   => (DbType.Double, 0),
                TypeCode.String   => (DbType.String, maxLength > 1000 ? int.MaxValue / 2 : maxLength),
                TypeCode.UInt16   => (DbType.UInt16, 0),
                TypeCode.UInt32   => (DbType.UInt32, 0),
                TypeCode.UInt64   => (DbType.UInt64, 0),
                _                 => (DbType.Binary, int.MaxValue)
            };
        }

        protected virtual void ReportProgress(long progress, long maxValue)
        {
            Progress?.Invoke(this, new ExportProgressEventArgs() { Progress = progress, Max = maxValue });
        }

        public virtual void DropTable(DbConnection connection, string tableSchema, string tableName)
        {
            try
            {
                var tblName = GetQualifiedTableName(tableSchema, tableName);

                var sql = $"DROP TABLE {tblName}";
                using var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                //Ignore errors, table name not exist
            }
        }
    }
}
