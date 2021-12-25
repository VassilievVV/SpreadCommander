using DevExpress.XtraRichEdit.Commands.Internal;
using DevExpress.XtraSpellChecker;
using Fizzler;
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
    public class DataReaderWrapper : DbDataReader, IListSource
    {
        #region DataReaderWrapperParameters
        public class DataReaderWrapperParameters
        {
            public string[] Columns         { get; set; }

            public string[] SkipColumns     { get; set; }

            public Action CloseAction       { get; set; }

            public bool IgnoreReaderErrors  { get; set; }
        }
        #endregion

        private int _RowCounter;
        private readonly IDataReader _Reader;
        private readonly Action _CloseAction;
        private readonly CancellationToken _CancelToken;
        private readonly bool _IgnoreReaderErrors;

        private Dictionary<int, int> ColumnMap        { get; } = new Dictionary<int, int>();
        private Dictionary<int, int> ColumnMapReverse { get; } = new Dictionary<int, int>();

        public DataReaderWrapper(IDataReader reader):
            this (reader, null, CancellationToken.None)
        {
        }

        public DataReaderWrapper(IDataReader reader, DataReaderWrapperParameters parameters) : 
            this(reader, parameters, CancellationToken.None)
        {
        }

        public DataReaderWrapper(IDataReader reader, DataReaderWrapperParameters parameters, CancellationToken cancelToken)
        {
            _Reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _CancelToken = cancelToken;

            InitializeColumnMap(parameters);
            _CloseAction        = parameters?.CloseAction;
            _IgnoreReaderErrors = parameters?.IgnoreReaderErrors ?? false;
        }

        protected override void Dispose(bool disposing)
        {
            Close();
            _Reader?.Dispose();

            base.Dispose(disposing);
        }

        protected void InitializeColumnMap(DataReaderWrapperParameters parameters)
        {
            var columns = new List<string>();
            if (parameters?.Columns != null)
                columns.AddRange(parameters.Columns);

            if (columns.Count <= 0)
            {
                for (int i = 0; i < _Reader.FieldCount; i++)
                    columns.Add(_Reader.GetName(i));
            }

            if ((parameters.SkipColumns?.Length ?? 0) > 0)
            {
                var dictSkip = new StringNoCaseDictionary<int>();
                
                foreach (string skipColumn in parameters.SkipColumns)
                {
                    if (!dictSkip.ContainsKey(skipColumn))
                        dictSkip.Add(skipColumn, 1);
                    else
                        dictSkip[skipColumn]++;
                }

                for (int i = columns.Count-1; i >= 0; i--)
                {
                    if (dictSkip.ContainsKey(columns[i]))
                        columns.RemoveAt(i);
                }
            }

            for (int i = 0; i < columns.Count; i++)
            {
                string columnName = columns[i];
                int ordinal = _Reader.GetOrdinal(columnName);
                if (ordinal < 0)
                    throw new Exception($"Cannot find column '{columnName}'");
                ColumnMap.Add(i, ordinal);
                ColumnMapReverse.Add(ordinal, i);
            }
        }

        public IList GetList() => (_Reader as IListSource)?.GetList();

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

        public override int FieldCount => ColumnMap.Count;

        public override bool HasRows => (_Reader as DbDataReader)?.HasRows ?? true;

        public override bool IsClosed => _Reader.IsClosed;

        public override int RecordsAffected => _Reader.RecordsAffected;

        public override int VisibleFieldCount => FieldCount;

        public bool ContainsListCollection => false;

        public int ProcessedRowCount => _RowCounter;

        public override object this[int ordinal]
        {
            get
            {
                var columnOrdinal = ColumnMap[ordinal];
                try
                {
                    var result = _Reader[columnOrdinal];
                    return result;
                }
                catch (Exception)
                {
                    if (_IgnoreReaderErrors)
                        return DBNull.Value;
                    throw;
                }
            }
        }

        public override object this[string name]
        {
            get => _Reader[name];
        }

        public override void Close()
        {
            _Reader?.Close();
            _CloseAction?.Invoke();
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
            var columnOrdinal = ColumnMap[ordinal];
            var result        = _Reader.GetDataTypeName(columnOrdinal);
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
            var columnOrdinal = ColumnMap[ordinal];
            var result        = _Reader.GetFieldType(columnOrdinal);
            return result;
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
            var columnOrdinal = ColumnMap[ordinal];
            var result        = _Reader.GetName(columnOrdinal);
            return result;
        }

        public override int GetOrdinal(string name)
        {
            var ordinal = _Reader.GetOrdinal(name);
            var result  = ColumnMapReverse[ordinal];
            return result;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Type GetProviderSpecificFieldType(int ordinal)
        {
            var columnOrdinal = ColumnMap[ordinal];
            var result        = GetFieldType(columnOrdinal);
            return result;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override object GetProviderSpecificValue(int ordinal) => this[ordinal];

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetProviderSpecificValues(object[] values) => GetValues(values);

        public override DataTable GetSchemaTable()
        {
            var initSchema = _Reader.GetSchemaTable();
            if (initSchema == null || !initSchema.Columns.Contains("ColumnName"))
            {
                var customSchema = GenerateCustomSchema();
                return customSchema;
            }

            var columnNames = new List<string>();
            for (int i = 0; i < FieldCount; i++)
                columnNames.Add(GetName(i));

            var colColumnName = initSchema.Columns["ColumnName"];

            initSchema.PrimaryKey = new DataColumn[] { colColumnName };

            var rows = new List<DataRow>();
            foreach (var columnName in columnNames)
            {
                var row = initSchema.Rows.Find(columnName);
                if (row == null)
                    throw new Exception($"Cannot find column '{columnName}'.");

                rows.Add(row);
            }

            var result = initSchema.Clone();
            bool hasColumnOrdinal = result.Columns.Contains("ColumnOrdinal");
            if (hasColumnOrdinal)
                result.Columns["ColumnOrdinal"].ReadOnly = false;
            for (int i = 0; i < rows.Count; i++)
            {
                var row = result.Rows.Add(rows[i].ItemArray);
                if (hasColumnOrdinal)
                    row["ColumnOrdinal"] = i;
            }

            return result;


            DataTable GenerateCustomSchema()
            {
                var result = new DataTable("Schema");

                result.Columns.Add("ColumnName", typeof(string));
                result.Columns.Add("ColumnOrdinal", typeof(int));
                result.Columns.Add("ColumnSize", typeof(int));
                result.Columns.Add("DataType", typeof(Type));
                result.Columns.Add("IsUnique", typeof(bool));
                result.Columns.Add("IsKey", typeof(bool));

                for (int i = 0; i < FieldCount; i++)
                {
                    var row              = result.NewRow();
                    row["ColumnName"]    = GetName(i);
                    row["ColumnOrdinal"] = i;
                    row["ColumnSize"]    = CalcColumnSize(i);
                    row["DataType"]      = GetFieldType(i);
                    row["IsUnique"]      = false;
                    row["IsKey"]         = false;

                    result.Rows.Add(row);
                }

                return result;
            }

            int CalcColumnSize(int ordinal)
            {
                return (Type.GetTypeCode(GetFieldType(ordinal))) switch
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
                    TypeCode.String or TypeCode.Char => int.MaxValue,
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

            bool result = _Reader.Read();
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
