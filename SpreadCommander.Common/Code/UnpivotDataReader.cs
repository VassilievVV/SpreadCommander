using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using DevExpress.XtraSpreadsheet.Model;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
    public class UnpivotDataReader : DbDataReader
    {
        #region Parameters
        public class Parameters
        {
            public IDataReader DataSource           { get; set; }
            public string[] PrimaryColumns          { get; set; }
            public string[] IgnoreColumns           { get; set; }
            public string UnPivotColumnName         { get; set; }
            public Type UnpivotColumnType           { get; set; }
            public string UnPivotValueColumnName    { get; set; }
            public Type UnPivotValueType            { get; set; }
            public bool IgnoreErrors                { get; set; }
            public object[] SkipValues              { get; set; }
        }
        #endregion

        private int _RowCounter;
        private readonly CancellationToken _CancelToken;

        private IDataReader DataSource          { get; set; }
        private string UnPivotColumnName        { get; set; }
        private Type UnpivotColumnType          { get; set; }
        private string UnPivotValueColumnName   { get; set; }
        private Type UnPivotValueType           { get; set; }
        private object[] SkipValues             { get; set; }
        private string[] DataSourceColumns      { get; set; }
        private int[] DataSourceOrdinals        { get; set; }
        private string[] PivotColumns           { get; set; }
        private int[] PivotOrdinals             { get; set; }
        private int UnpivotIndex = -1;

        public UnpivotDataReader(Parameters parameters) : this(parameters, CancellationToken.None)
        {
        }

        public UnpivotDataReader(Parameters parameters, CancellationToken cancelToken)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            _CancelToken = cancelToken;

            Initialize(parameters);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            DataSource?.Dispose();
            DataSource = null;
        }

        private static int IndexOfString(IEnumerable<string> strings, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (strings == null)
                return -1;

            int index = -1;
            foreach (var str in strings)
            {
                index++;
                if (string.Compare(str, value, comparison) == 0)
                    return index;
            }
            return -1;
        }

        private static bool ContainsString(IEnumerable<string> strings, string value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (strings == null)
                return false;

            int index = IndexOfString(strings, value, comparison);
            return (index >= 0);
        }

        protected virtual void Initialize(Parameters parameters)
        {
            if (parameters.PrimaryColumns == null || parameters.PrimaryColumns.Length <= 0)
                throw new ArgumentNullException(nameof(parameters.PrimaryColumns));
            if (string.IsNullOrWhiteSpace(parameters.UnPivotColumnName))
                throw new ArgumentNullException(nameof(parameters.UnPivotColumnName));
            if (string.IsNullOrWhiteSpace(parameters.UnPivotValueColumnName))
                throw new ArgumentException(nameof(parameters.UnPivotValueColumnName));
            if (parameters.UnPivotValueType == null)
                throw new ArgumentNullException(nameof(parameters.UnPivotValueType));
            if (parameters.DataSource == null)
                throw new ArgumentNullException(nameof(parameters.DataSource));

            DataSource = parameters.DataSource;
            UnPivotColumnName      = parameters.UnPivotColumnName;
            UnpivotColumnType      = parameters.UnpivotColumnType ?? typeof(string);
            UnPivotValueColumnName = parameters.UnPivotValueColumnName;
            UnPivotValueType       = parameters.UnPivotValueType;

            if (parameters.SkipValues != null)
            {
                SkipValues = new object[parameters.SkipValues.Length];
                for (int i = 0; i < parameters.SkipValues.Length; i++)
                {
                    object skipValue = parameters.SkipValues[i];
                    if (skipValue is PSObject psSkipValue)
                        SkipValues[i] = psSkipValue.BaseObject;
                    else
                        SkipValues[i] = skipValue;
                }
            }

            var schema = parameters.DataSource.GetSchemaTable();

            var dataSourceColumns = new List<string>();
            dataSourceColumns.AddRange(parameters.PrimaryColumns);
            var dataSourceOrdinals = new int[dataSourceColumns.Count];
            for (int i = 0; i < dataSourceColumns.Count; i++)
            {
                dataSourceOrdinals[i] = parameters.DataSource.GetOrdinal(dataSourceColumns[i]);
                if (dataSourceOrdinals[i] < 0)
                    throw new Exception($"Cannot find column '{dataSourceColumns[i]}'.");
            }

            var pivotColumns  = new List<string>();
            var pivotOrdinals = new List<int> ();
            foreach (DataRow row in schema.Rows)
            {
                var columnName    = Convert.ToString(row["ColumnName"]);
                var columnOrdinal = Convert.ToInt32(row["ColumnOrdinal"]);
                if (!ContainsString(dataSourceColumns, columnName) && !ContainsString(parameters.IgnoreColumns, columnName))
                {
                    pivotColumns.Add(columnName);
                    pivotOrdinals.Add(columnOrdinal);
                }
            }

            DataSourceColumns  = dataSourceColumns.ToArray();
            DataSourceOrdinals = dataSourceOrdinals;
            PivotColumns       = pivotColumns.ToArray();
            PivotOrdinals      = pivotOrdinals.ToArray();
        }

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

        public override int FieldCount => DataSourceColumns.Length + 2;

        public override bool HasRows
        {
            get
            {
                if (DataSource is DbDataReader dbReader)
                    return dbReader.HasRows;
                return true;    //Assume that there are rows if result is not known
            }
        }

        public override bool IsClosed => DataSource == null;

        public override int RecordsAffected => -1;

        public override int VisibleFieldCount => FieldCount;

        public int ProcessedRowCount => _RowCounter;

        public override object this[int ordinal]
        {
            get
            {
                if (Result != DbDataReaderResult.Reading)
                    throw new InvalidOperationException("DataReader is not open");

                if (ordinal < 0 || ordinal >= FieldCount)
                    throw new ArgumentException("Invalid ordinal", nameof(ordinal));

                object result;
                try
                {
                    if (ordinal < DataSourceColumns.Length)
                    {
                        int dbOrdinal = DataSourceOrdinals[ordinal];
                        result = DataSource[dbOrdinal];
                    }
                    else if (ordinal == DataSourceColumns.Length)
                    {
                        if (UnpivotIndex >= 0 && UnpivotIndex < PivotColumns.Length)
                            result = PivotColumns[UnpivotIndex];
                        else
                            result = null;
                    }
                    else if (ordinal == DataSourceColumns.Length + 1)
                    {
                        if (UnpivotIndex >= 0 && UnpivotIndex < PivotColumns.Length)
                        {
                            result = DBNull.Value;
                            object value = DataSource[PivotOrdinals[UnpivotIndex]];
                            if (value != null && value != DBNull.Value)
                            {
                                try
                                {
                                    result = Utils.ConvertValue(UnPivotValueType, value, DBNull.Value);
                                }
                                catch (Exception)
                                {
                                    if (IgnoreErrors)
                                        result = DBNull.Value;
                                    else
                                        throw;
                                }
                            }
                        }
                        else
                            result = DBNull.Value;
                    }
                    else
                        throw new ArgumentException(nameof(ordinal));
                }
                catch (Exception)
                {
                    if (IgnoreErrors)
                        result = DBNull.Value;
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
                var ordinal = GetOrdinal(name);
                var result  = this[ordinal];
                return result;
            }
        }

        public override void Close()
        {
            DataSource?.Close();
            DataSource?.Dispose();
            DataSource = null;
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

            var type = GetFieldType(ordinal);
            if (type == null)
                return null;

            var result = SpreadCommander.Common.SqlScript.SqlScript.GetColumnDataType(type, int.MaxValue);
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

            if (ordinal < DataSourceOrdinals.Length)
                return DataSource.GetFieldType(DataSourceOrdinals[ordinal]);

            if (ordinal == DataSourceOrdinals.Length)
                return UnpivotColumnType ?? typeof(string);
            if (ordinal == DataSourceOrdinals.Length + 1)
                return UnPivotValueType;

            return null;
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
            if (ordinal < 0 || ordinal >= FieldCount)
                return null;

            if (ordinal < DataSourceColumns.Length)
                return DataSourceColumns[ordinal];

            if (ordinal == DataSourceColumns.Length)
                return UnPivotColumnName;

            if (ordinal == DataSourceColumns.Length + 1)
                return UnPivotValueColumnName;

            return null;
        }

        public override int GetOrdinal(string name)
        {
            var ordinal = IndexOfString(DataSourceColumns, name);
            if (ordinal >= 0)
                return ordinal;
            if (string.Compare(name, UnPivotColumnName, true) == 0)
                return DataSourceColumns.Length;
            if (string.Compare(name, UnPivotValueColumnName, true) == 0)
                return DataSourceColumns.Length + 1;
            throw new ArgumentException("Invalid column name");
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

            for (int i = 0; i < DataSourceColumns.Length; i++)
            {
                var fieldType = GetFieldType(i);

                var row              = result.NewRow();
                row["ColumnName"]    = DataSourceColumns[i];
                row["ColumnOrdinal"] = i;
                row["ColumnSize"]    = CalcColumnSize(fieldType);
                row["DataType"]      = fieldType;
                row["IsUnique"]      = false;
                row["IsKey"]         = false;

                result.Rows.Add(row);
            }

            var rowUnPivotColumn              = result.NewRow();
            rowUnPivotColumn["ColumnName"]    = UnPivotColumnName;
            rowUnPivotColumn["ColumnOrdinal"] = DataSourceColumns.Length;
            rowUnPivotColumn["ColumnSize"]    = CalcColumnSize(UnpivotColumnType ?? typeof(string));
            rowUnPivotColumn["DataType"]      = UnpivotColumnType ?? typeof(string);
            rowUnPivotColumn["IsUnique"]      = false;
            rowUnPivotColumn["IsKey"]         = false;
            result.Rows.Add(rowUnPivotColumn);

            var rowUnPivotValue              = result.NewRow();
            rowUnPivotValue["ColumnName"]    = UnPivotValueColumnName;
            rowUnPivotValue["ColumnOrdinal"] = DataSourceColumns.Length + 1;
            rowUnPivotValue["ColumnSize"]    = CalcColumnSize(UnPivotValueType);
            rowUnPivotValue["DataType"]      = UnPivotValueType;
            rowUnPivotValue["IsUnique"]      = false;
            rowUnPivotValue["IsKey"]         = false;
            result.Rows.Add(rowUnPivotValue);

            return result;


            static int CalcColumnSize(Type type)
            {
                return (Type.GetTypeCode(type)) switch
                {
                    TypeCode.Empty                   => 0,
                    TypeCode.Object                  => 0,
                    TypeCode.DBNull                  => 0,
                    TypeCode.Boolean                 => 1,
                    TypeCode.SByte                   => 1,
                    TypeCode.Byte                    => 1,
                    TypeCode.Int16                   => 2,
                    TypeCode.UInt16                  => 2,
                    TypeCode.Int32                   => 4,
                    TypeCode.UInt32                  => 4,
                    TypeCode.Int64                   => 8,
                    TypeCode.UInt64                  => 8,
                    TypeCode.Single                  => 4,
                    TypeCode.Double                  => 8,
                    TypeCode.Decimal                 => 29,
                    TypeCode.DateTime                => 8,
                    TypeCode.String or TypeCode.Char => int.MaxValue / 2,
                    _                                => 0,
                };
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
            var value = this[ordinal];
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

            bool result = true;
            if (UnpivotIndex < 0)
                result = DataSource?.Read() ?? false;

            do
            {
                UnpivotIndex++;
                if (UnpivotIndex >= PivotColumns.Length)
                {
                    UnpivotIndex = 0;

                    result = DataSource?.Read() ?? false;
                    if (!result)
                    {
                        Result = DbDataReaderResult.FinishedTable;
                        return false;
                    }
                }

                _RowCounter++;
                if (result && MaxRows >= 0 && _RowCounter > MaxRows)
                {
                    Result = DbDataReaderResult.MaxRowsReached;
                    return false;
                }

                object value = DataSource[PivotOrdinals[UnpivotIndex]];
                if (value != null && value != DBNull.Value)
                {
                    try
                    {
                        value = Utils.ConvertValue(UnPivotValueType, value, DBNull.Value);
                    }
                    catch (Exception)
                    {
                        if (IgnoreErrors)
                            value = DBNull.Value;
                        else
                            throw;
                    }
                }
                result = AcceptValue(value);
            } while (!result);

            return result;


            bool AcceptValue(object value)
            {
                if (SkipValues == null || SkipValues.Length <= 0)
                    return true;

                for (int i = 0; i < SkipValues.Length; i++)
                {
                    object skipValue = SkipValues[i];
                    if (skipValue == null || skipValue == DBNull.Value)
                    {
                        if (value == null || value == DBNull.Value)
                            return false;
                    }
                    else if (value != null && value != DBNull.Value)
                    {
                        var valueType     = value.GetType();
                        var skipValueType = skipValue.GetType();

                        bool isEqual = false;

                        if (IsNumericType(valueType) && IsNumericType(skipValueType))
                            isEqual = Convert.ToDouble(value).Equals(Convert.ToDouble(skipValue));
                        else if (IsStringType(valueType) && IsStringType(skipValueType))
                            isEqual = Convert.ToString(value).Equals(Convert.ToString(skipValue));
                        else if (IsDateTimeType(valueType) && IsDateTimeType(skipValueType))
                            isEqual = Convert.ToDateTime(value).Equals(Convert.ToDateTime(skipValue));

                        if (isEqual)
                            return false;
                    }
                }

                return true;
            }


            static bool IsNumericType(Type type)
            {
                return (Type.GetTypeCode(type)) switch
                {
                    TypeCode.Boolean or 
                    TypeCode.Char => false,
                    TypeCode.SByte or 
                    TypeCode.Byte or 
                    TypeCode.Int16 or 
                    TypeCode.UInt16 or 
                    TypeCode.Int32 or 
                    TypeCode.UInt32 or 
                    TypeCode.Int64 or 
                    TypeCode.UInt64 or 
                    TypeCode.Single or 
                    TypeCode.Double or 
                    TypeCode.Decimal => true,
                    TypeCode.DateTime or 
                    TypeCode.String => false,
                    _ => false,
                };
            }

            static bool IsStringType(Type type)
            {
                return (Type.GetTypeCode(type)) switch
                {
                    TypeCode.Boolean => false,
                    TypeCode.Char => true,
                    TypeCode.SByte or 
                    TypeCode.Byte or 
                    TypeCode.Int16 or 
                    TypeCode.UInt16 or 
                    TypeCode.Int32 or 
                    TypeCode.UInt32 or 
                    TypeCode.Int64 or 
                    TypeCode.UInt64 or 
                    TypeCode.Single or 
                    TypeCode.Double or 
                    TypeCode.Decimal or 
                    TypeCode.DateTime => false,
                    TypeCode.String => true,
                    _ => false,
                };
            }

            static bool IsDateTimeType(Type type)
            {
                return (Type.GetTypeCode(type)) switch
                {
                    TypeCode.Boolean => false,
                    TypeCode.Char or
                    TypeCode.SByte or
                    TypeCode.Byte or
                    TypeCode.Int16 or
                    TypeCode.UInt16 or
                    TypeCode.Int32 or
                    TypeCode.UInt32 or
                    TypeCode.Int64 or
                    TypeCode.UInt64 or
                    TypeCode.Single or
                    TypeCode.Double or
                    TypeCode.Decimal => false,
                    TypeCode.DateTime => true,
                    TypeCode.String => false,
                    _ => false,
                };
            }
        }
    }
}
