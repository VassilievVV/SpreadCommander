using SpreadCommander.Common.SqlScript;
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
    public class ObjectDataReader<T> : DbDataReader, IListSource
    {
        private readonly object _DataSource;
        private IEnumerator<T> _Enumerator;
        private int _RowCounter;
        private readonly CancellationToken _CancelToken;
        private readonly List<PropertyInfo> _Properties = new List<PropertyInfo>();

        public ObjectDataReader(IEnumerable<T> dataSource) : this(dataSource, CancellationToken.None)
        {
        }

        public ObjectDataReader(IEnumerable<T> dataSource, CancellationToken cancelToken)
        {
            _DataSource  = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _Enumerator  = dataSource.GetEnumerator();
            _CancelToken = cancelToken;

            var properties = typeof(T).GetProperties();
            _Properties.AddRange(properties);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_Enumerator != null)
            {
                _Enumerator.Dispose();
                _Enumerator = null;
            }
        }

        public IList GetList() => _DataSource as IList;

        public IEnumerator<T> Enumerator => _Enumerator;

        public IList<T> List => _Enumerator as IList<T>;
        public IList SimpleList => _Enumerator as IList;

        public DbDataReaderResult Result { get; private set; }

        public int MaxRows { get; set; } = -1;

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

        public override int FieldCount => _Properties.Count;

        public override bool HasRows => (GetList()?.Count ?? 1) > 0;

        public override bool IsClosed => _Enumerator == null;

        public override int RecordsAffected => 0;

        public override int VisibleFieldCount => FieldCount;

        public bool ContainsListCollection => false;

        public override object this[int ordinal]
        {
            get
            {
                if (_Enumerator.Current == null)
                    return null;

                if (ordinal < 0 || ordinal >= _Properties.Count)
                    return null;

                return _Properties[ordinal].GetValue(_Enumerator.Current);
            }
        }

        public override object this[string name] => this[GetOrdinal(name)];

        public override void Close()
        {
            if (_Enumerator != null)
            {
                _Enumerator.Dispose();
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
            if (ordinal < 0 || ordinal >= _Properties.Count)
                return null;

            var result = SpreadCommander.Common.SqlScript.SqlScript.GetColumnDataType(_Properties[ordinal].PropertyType, int.MaxValue);
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
            if (ordinal < 0 || ordinal >= _Properties.Count)
                return null;

            return _Properties[ordinal].PropertyType;		
        }

        public override float GetFloat(int ordinal) => (float)Convert.ToDouble(this[ordinal]);

        public override Guid GetGuid(int ordinal)
        {
            object value = this[ordinal];
            if (value is Guid guid)
                return guid;

            var strValue = Convert.ToString(value);
            var result   = Guid.Parse(strValue);
            return result;
        }

        public override short GetInt16(int ordinal) => Convert.ToInt16(this[ordinal]);

        public override int GetInt32(int ordinal) => Convert.ToInt32(this[ordinal]);

        public override long GetInt64(int ordinal) => Convert.ToInt64(this[ordinal]);
        
        public override string GetName(int ordinal)
        {
            if (ordinal < 0 || ordinal >= _Properties.Count)
                return null;

            return _Properties[ordinal].Name;
        }

        public override int GetOrdinal(string name)
        {
            for (int i = 0; i < _Properties.Count; i++)
            {
                var property = _Properties[i];
                if (property.Name == name)
                    return i;
            }
            return -1;
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

            for (int i = 0; i < _Properties.Count; i++)
            {
                var property = _Properties[i];

                var row = result.NewRow();
                row["ColumnName"]    = property.Name;
                row["ColumnOrdinal"] = i;
                row["ColumnSize"]    = CalcColumnSize(property);
                row["DataType"]      = property.PropertyType;
                row["IsUnique"]      = false;
                row["IsKey"]         = false;

                result.Rows.Add(row);
            }

            return result;


            int CalcColumnSize(PropertyInfo propInfo)
            {
                switch (Type.GetTypeCode(propInfo.PropertyType))
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
                        if (_DataSource is IList list)
                        {
                            int colSize = 0;
                            foreach (var item in list)
                            {
                                var strValue = Convert.ToString(propInfo.GetValue(item));
                                if (strValue != null && strValue.Length > colSize)
                                    colSize = strValue.Length;
                            }
                            return Math.Max(colSize, 1);
                        }
                        else
                            return int.MaxValue;
                    default:
                        return 0;
                }
            }
        }

        public override string GetString(int ordinal) => Convert.ToString(this[ordinal]);

        public override object GetValue(int ordinal)
        {
            var value  = this[ordinal];
            var result = (value == null) || (value == DBNull.Value);
            return result;
        }

        public override int GetValues(object[] values)
        {
            if (values == null)
                return 0;

            var len = Math.Min(_Properties.Count, values.Length);
            for (int i = 0; i < len; i++)
                values[i] = this[i];

            return len;			
        }

        public override bool IsDBNull(int ordinal) => this[ordinal] == null;

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
