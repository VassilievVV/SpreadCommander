﻿using SpreadCommander.Common.SqlScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
    public class DataViewReader : DbDataReader, IListSource
    {
        private readonly DataView _DataSource;
        private IEnumerator _Enumerator;
        private int _RowCounter;
        private readonly CancellationToken _CancelToken;

        public DataViewReader(DataView dataSource): this(dataSource, CancellationToken.None)
        {
        }

        public DataViewReader(DataView dataSource, CancellationToken cancelToken)
        {
            _DataSource  = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _Enumerator  = _DataSource.GetEnumerator();
            _CancelToken = cancelToken;
        }

        public DataViewReader(DataTable table) : this(table.DefaultView)
        {
        }

        public DataViewReader(DataTable table, CancellationToken cancelToken): this(table.DefaultView, cancelToken)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_Enumerator != null)
            {
                if (_Enumerator is IDisposable dispEnumerator)
                    dispEnumerator.Dispose();
                _Enumerator = null;
            }
        }

        public IList GetList() => _DataSource;

        public DbDataReaderResult Result { get; private set; }

        public int MaxRows { get; set; } = -1;

        public bool IgnoreErrors { get; set; }

        public void Cancel()
        {
            switch (Result)
            {
                case DbDataReaderResult.Reading:
                case DbDataReaderResult.FinishedTable:
                    Result = DbDataReaderResult.Cancelled;
                    break;
            }
        }

        //DbDataReader properties and methods.
        public override int Depth => 0;

        public override int FieldCount => _DataSource.Table.Columns.Count;

        public override bool HasRows => (_DataSource?.Count ?? 0) > 0;

        public override bool IsClosed => _Enumerator == null;

        public override int RecordsAffected => -1;

        public override int VisibleFieldCount => FieldCount;

        public bool ContainsListCollection => false;

        public int RowCount => _DataSource?.Count ?? 0;
        public int ProcessedRowCount => _RowCounter;

        public override object this[int ordinal]
        {
            get
            {
                if (_Enumerator.Current == null)
                    return null;

                if (ordinal < 0 || ordinal >= FieldCount)
                    return null;

                object result;
                try
                {
                    result = (_Enumerator.Current as DataRowView)[ordinal];
                }
                catch (Exception)
                {
                    if (IgnoreErrors)
                        result = null;
                    else
                        throw;
                }
                return result;
            }
        }

        public override object this[string name]
        {
            get
            {
                if (_Enumerator.Current == null)
                    return null;

                object result;
                try
                {
                    var ordinal = GetOrdinal(name);
                    if (ordinal < 0)
                        result = null;
                    else
                        result = (_Enumerator.Current as DataRowView)[ordinal];
                }
                catch (Exception)
                {
                    if (IgnoreErrors)
                        result = null;
                    else
                        throw;
                }
                return result;
            }
        }

        public override void Close()
        {
            if (_Enumerator != null)
            {
                if (_Enumerator is IDisposable dispEnumerator)
                    dispEnumerator.Dispose();
                _Enumerator = null;
            }
        }

        public override bool GetBoolean(int ordinal) => Convert.ToBoolean(this[ordinal]);

        public override byte GetByte(int ordinal) => Convert.ToByte(this[ordinal]);

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) =>
            throw new NotImplementedException();

        public override char GetChar(int ordinal) => Convert.ToChar(this[ordinal]);

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) =>
            throw new NotImplementedException();

        public override string GetDataTypeName(int ordinal)
        {
            if (ordinal < 0 || ordinal >= FieldCount)
                return null;

            var result = SpreadCommander.Common.SqlScript.SqlScript.GetColumnDataType(_DataSource.Table.Columns[ordinal].DataType, int.MaxValue);
            return result;
        }

        public override DateTime GetDateTime(int ordinal) => Convert.ToDateTime(this[ordinal]);

        protected override DbDataReader GetDbDataReader(int ordinal) =>
            throw new NotImplementedException();

        public override decimal GetDecimal(int ordinal) => Convert.ToDecimal(this[ordinal]);

        public override double GetDouble(int ordinal) => Convert.ToDouble(this[ordinal]);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override IEnumerator GetEnumerator() => new DbEnumerator(this);

        public override Type GetFieldType(int ordinal)
        {
            if (ordinal < 0 || ordinal >= FieldCount)
                return null;

            return _DataSource.Table.Columns[ordinal].DataType;
        }

        public override float GetFloat(int ordinal) => (float)Convert.ToDouble(this[ordinal]);

        public override Guid GetGuid(int ordinal)
        {
            object value = this[ordinal];
            if (value is Guid guid)
                return guid;

            var strValue = Convert.ToString(value);
            var result = Guid.Parse(strValue);
            return result;
        }

        public override short GetInt16(int ordinal) => Convert.ToInt16(this[ordinal]);

        public override int GetInt32(int ordinal) => Convert.ToInt32(this[ordinal]);

        public override long GetInt64(int ordinal) => Convert.ToInt64(this[ordinal]);

        public override string GetName(int ordinal)
        {
            if (ordinal < 0 || ordinal >= FieldCount)
                return null;

            return _DataSource.Table.Columns[ordinal].ColumnName;
        }

        public override int GetOrdinal(string name)
        {
            return _DataSource.Table.Columns[name]?.Ordinal ?? -1;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Type GetProviderSpecificFieldType(int ordinal) => GetFieldType(ordinal);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override object GetProviderSpecificValue(int ordinal) => this[ordinal];

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetProviderSpecificValues(object[] values) => GetValues(values);

        public override DataTable GetSchemaTable()
        {
            var result = new DataTable("Schema");

            result.Columns.Add("ColumnName", typeof(string));
            result.Columns.Add("ColumnOrdinal", typeof(int));
            result.Columns.Add("ColumnSize", typeof(int));
            result.Columns.Add("DataType", typeof(Type));
            result.Columns.Add("IsUnique", typeof(bool));
            result.Columns.Add("IsKey", typeof(bool));

            for (int i = 0; i < _DataSource.Table.Columns.Count; i++)
            {
                var property = _DataSource.Table.Columns[i];

                var row              = result.NewRow();
                row["ColumnName"]    = property.ColumnName;
                row["ColumnOrdinal"] = i;
                row["ColumnSize"]    = CalcColumnSize(property);
                row["DataType"]      = property.DataType;
                row["IsUnique"]      = false;
                row["IsKey"]         = false;

                result.Rows.Add(row);
            }

            return result;


            int CalcColumnSize(DataColumn propInfo)
            {
                switch (Type.GetTypeCode(propInfo.DataType))
                {
                    case TypeCode.Empty:
                        return 0;
                    case TypeCode.Object:
                        return 0;
                    case TypeCode.DBNull:
                        return 0;
                    case TypeCode.Boolean:
                        return 1;
                    case TypeCode.SByte:
                        return 1;
                    case TypeCode.Byte:
                        return 1;
                    case TypeCode.Int16:
                        return 2;
                    case TypeCode.UInt16:
                        return 2;
                    case TypeCode.Int32:
                        return 4;
                    case TypeCode.UInt32:
                        return 4;
                    case TypeCode.Int64:
                        return 8;
                    case TypeCode.UInt64:
                        return 8;
                    case TypeCode.Single:
                        return 4;
                    case TypeCode.Double:
                        return 8;
                    case TypeCode.Decimal:
                        return 29;
                    case TypeCode.DateTime:
                        return 8;
                    case TypeCode.String:
                    case TypeCode.Char:
                        int colSize = 0;
                        foreach (DataRowView row in _DataSource)
                        {
                            var strValue = Convert.ToString(row[propInfo.ColumnName]);
                            if (strValue != null && strValue.Length > colSize)
                                colSize = strValue.Length;
                        }
                        return Math.Max(colSize, 1);
                    default:
                        return 0;
                }
            }
        }

        public override string GetString(int ordinal) => Convert.ToString(this[ordinal]);

        public override object GetValue(int ordinal) => this[ordinal];

        public override int GetValues(object[] values)
        {
            if (values == null)
                return 0;

            var len = Math.Min(FieldCount, values.Length);
            for (int i = 0; i < len; i++)
                values[i] = this[i];

            return len;
        }

        public override bool IsDBNull(int ordinal)
        {
            var value  = this[ordinal];
            var result = (value == null) || (value == DBNull.Value);
            return result;
        }

        public override bool NextResult() => false;

        public override bool Read()
        {
            if (Result != DbDataReaderResult.Reading)
                return false;

            if (_CancelToken.IsCancellationRequested)
            {
                Result = DbDataReaderResult.Cancelled;
                return false;
            }

            bool result = _Enumerator.MoveNext();
            if (!result)
                Result = DbDataReaderResult.FinishedTable;

            _RowCounter++;
            if (result && MaxRows >= 0 && _RowCounter > MaxRows)
            {
                Result = DbDataReaderResult.MaxRowsReached;
                return false;
            }

            return result;
        }
    }
}
