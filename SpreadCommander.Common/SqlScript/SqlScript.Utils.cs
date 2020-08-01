using DevExpress.Mvvm;
using MySqlConnector;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Common.SqlScript
{
    public partial class SqlScript
    {
        private static void FixDataTypes(DataTable tbl)
        {
            var columns = new List<DataColumn>(tbl.Columns.OfType<DataColumn>());
            foreach (DataColumn col in columns)
                FixColumnType(col);

            static bool IsTypeCode(TypeCode value, params TypeCode[] codes)
            {
                var result = codes.Contains(value);
                return result;
            }

            static void FixColumnType(DataColumn column)
            {
                if (column.DataType == typeof(object))
                {
                    Type colType = null;

                    foreach (DataRow row in column.Table.Rows)
                    {
                        var value = row[column];

                        if (value == null || value == DBNull.Value)
                            continue;

                        var valType = value.GetType();
                        if (colType == null )
                            colType = valType;
                        else if (colType != valType)
                        {
                            var colTypeCode = Type.GetTypeCode(colType);
                            var valTypeCode = Type.GetTypeCode(valType);

                            switch (colTypeCode)
                            {
                                case TypeCode.Empty:
                                    colType = valType;
                                    break;
                                case TypeCode.Object:
                                    colType = typeof(object);
                                    break;
                                case TypeCode.DBNull:
                                    colType = valType;
                                    break;
                                case TypeCode.Boolean:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Char:
                                    if (IsTypeCode(valTypeCode, TypeCode.String, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.SByte:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte))
                                        colType = typeof(short);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Byte:
                                    if (IsTypeCode(valTypeCode, TypeCode.SByte))
                                        colType = typeof(short);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Int16:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte))
                                        colType = typeof(short);
                                    else if (IsTypeCode(valTypeCode, TypeCode.UInt16))
                                        colType = typeof(int);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.UInt16:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte))
                                        colType = typeof(int);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int16))
                                        colType = typeof(int);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Int32:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16))
                                        colType = typeof(int);
                                    else if (IsTypeCode(valTypeCode, TypeCode.UInt32))
                                        colType = typeof(long);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.UInt32:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16))
                                        colType = typeof(long);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int32))
                                        colType = typeof(long);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Int64:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32))
                                        colType = typeof(long);
                                    else if (IsTypeCode(valTypeCode, TypeCode.UInt64))  //anyway leave unsigned
                                        colType = typeof(long);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.UInt64:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32))
                                        colType = typeof(long);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Int64))  //anyway leave unsigned
                                        colType = typeof(long);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Single:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64))
                                        colType = typeof(float);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Double))
                                        colType = typeof(double);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Double:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single))
                                        colType = typeof(double);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Decimal, TypeCode.DateTime))
                                        colType = valType;
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.Decimal:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.DateTime))
                                        colType = typeof(decimal);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.DateTime:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal))
                                        colType = typeof(decimal);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Char, TypeCode.String))
                                        colType = typeof(string);
                                    else if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                    break;
                                case TypeCode.String:
                                    if (IsTypeCode(valTypeCode, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32,
                                        TypeCode.Int64, TypeCode.UInt64, TypeCode.Single, TypeCode.Double, TypeCode.Decimal, TypeCode.DateTime, TypeCode.Char))
                                        colType = typeof(string);
                                    if (IsTypeCode(valTypeCode, TypeCode.Object))
                                        colType = typeof(object);
                                        break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (colType != null && colType != column.DataType)
                        column.ConvertColumnType(colType);
                }
            }
        }

        public static DbParameter CloneDbParameter(DbParameter parameter)
        {
            var result = (DbParameter)Activator.CreateInstance(parameter.GetType());

            result.ParameterName = parameter.ParameterName;
            result.DbType        = parameter.DbType;
            result.Size          = parameter.Size;
            result.Direction     = parameter.Direction;
            result.Precision     = parameter.Precision;
            result.Scale         = parameter.Scale;
            result.SourceColumn  = parameter.SourceColumn;
            result.SourceVersion = parameter.SourceVersion;
            result.Value         = parameter.Value;

            if (result is SqlParameter sqlParam)
                sqlParam.IsNullable = ((SqlParameter)parameter).IsNullable;

            return result;
        }

        public static DbParameter CreateDbParameter(DbCommand dbCommand, string name, Type type, object value)
        {
            var result = dbCommand.CreateParameter();

            int maxLen = 0;
            if (value is string str)
                maxLen = value != null ? str.Length : 0;
            else if (value is byte[] bValue)
                maxLen = value != null ? bValue.Length : 0;

            (var dbType, var len) = GetColumnDbType(type, maxLen);

            result.ParameterName = name;
            result.DbType        = dbType;
            result.Direction     = ParameterDirection.Input;
            result.Size          = len;
            result.Value         = value ?? DBNull.Value;

            if (result is SqlParameter sqlParam)
                sqlParam.IsNullable = true;

            return result;
        }

        public static string UpdateDbParameterName(string initialName)
        {
            if (string.IsNullOrWhiteSpace(initialName))
                throw new ArgumentException("Parameter name cannot be empty", nameof(initialName));

            var result = new StringBuilder();
            for (int i = 0; i < initialName.Length; i++)
            {
                var c = initialName[i];

                if (i == 0 && (c == '@' || c == ':'))
                    result.Append(c);
                else if (char.IsLetterOrDigit(c))
                    result.Append(c);
                else
                    result.Append('_');
            }

            return result.ToString();
        }

        public static DbParameter CreateDbParameter(DbCommand dbCommand, ScriptRunParameter parameter)
        {
            var result = dbCommand.CreateParameter();

            int maxLen = 0;
            if (parameter.Value is string str)
                maxLen = parameter.Value != null ? str.Length : 0;
            else if (parameter.Value is byte[] bValue)
                maxLen = parameter.Value != null ? bValue.Length : 0;

            var parameterName = UpdateDbParameterName(parameter.Name);
            if (dbCommand is SqlCommand && !string.IsNullOrWhiteSpace(parameterName) &&
                !parameterName.StartsWith("@"))
                parameterName = $"@{parameterName}";

            result.ParameterName = parameterName;
            result.DbType        = parameter.DbType;
            result.Direction     = parameter.Direction;
            result.Value         = parameter.Value ?? DBNull.Value;
            if (maxLen > 0)
                result.Size      = maxLen;

            if (result is SqlParameter sqlParam)
                sqlParam.IsNullable = true;

            return result;
        }

        public static string GetColumnDataType(Type dataType, int maxLength)
        {
            if (dataType == null)
                return "integer";

            if (dataType == typeof(Guid))
                return "uniqueidentifier";

            return Type.GetTypeCode(dataType) switch
            {
                TypeCode.Boolean  => "bit",
                TypeCode.Byte     => "integer",
                TypeCode.Char     => maxLength > 1000 ? "nvarchar(max)" : $"nchar({maxLength})",
                TypeCode.DateTime => "datetime",
                TypeCode.Decimal  => "float",
                TypeCode.Double   => "float",
                TypeCode.Int16    => "integer",
                TypeCode.Int32    => "integer",
                TypeCode.Int64    => "bigint",
                TypeCode.Object   => "varbinary(max)",
                TypeCode.SByte    => "integer",
                TypeCode.Single   => "float",
                TypeCode.String   => $"nvarchar({(maxLength > 1000 ? "max" : maxLength.ToString())})",
                TypeCode.UInt16   => "integer",
                TypeCode.UInt32   => "bigint",
                TypeCode.UInt64   => "bigint",
                _                 => "varbinary(max)"
            };
        }

        public static (DbType dbType, int len) GetColumnDbType(Type dataType, int maxLength)
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
    }
}
