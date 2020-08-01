using DevExpress.XtraRichEdit.Commands.Internal;
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
            public string[] Columns { get; set; }

            public Action CloseAction { get; set; }
        }
        #endregion

        private int _RowCounter;
        private readonly IDataReader _Reader;
        private readonly Action _CloseAction;
        private readonly CancellationToken _CancelToken;

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
            _CloseAction = parameters?.CloseAction;
        }

        protected override void Dispose(bool disposing)
        {
            Close();
            _Reader?.Dispose();

            base.Dispose(disposing);
        }

        protected void InitializeColumnMap(DataReaderWrapperParameters parameters)
        {
            string[] columns = parameters?.Columns;
            if ((columns?.Length ?? 0) <= 0)
            {
                columns = new string[_Reader.FieldCount];
                for (int i = 0; i < columns.Length; i++)
                    columns[i] = _Reader.GetName(i);
            }

            for (int i = 0; i < columns.Length; i++)
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
                var result        = _Reader[columnOrdinal];
                return result;
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

            bool hasColumnOrdinal = initSchema.Columns.Contains("ColumnOrdinal");

            var result = initSchema.Clone();
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
                switch (Type.GetTypeCode(GetFieldType(ordinal)))
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
                        return int.MaxValue;
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
