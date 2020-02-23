#pragma warning disable CRR0050

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
    public class PSObjectDataReader : DbDataReader, IListSource
    {
        #region PSPropertyInfo
        protected class PSPropertyInfo
        {
            public string Name { get; set; }
            public string TypeName { get; set; }
            public Type Type { get; set; }
        }
        #endregion

        private readonly IList<PSObject> _DataSource;
        private IEnumerator<PSObject> _Enumerator;
        private int _RowCounter;
        private readonly CancellationToken _CancelToken;
        private readonly List<PSPropertyInfo> _Properties = new List<PSPropertyInfo>();

        public bool IgnoreErrors { get; set; }

        public PSObjectDataReader(IList<PSObject> dataSource) : this(dataSource, CancellationToken.None)
        {
        }

        public PSObjectDataReader(IList<PSObject> dataSource, CancellationToken cancelToken)
        {
            _DataSource  = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _Enumerator  = dataSource.AsEnumerable().GetEnumerator();
            _CancelToken = cancelToken;

            InitializeProperties();
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

        protected void InitializeProperties()
        {
            _Properties.Clear();

            var properties = new StringNoCaseDictionary<PSPropertyInfo>();
            var dataTables = new List<DataTable>();

            foreach (var obj in _DataSource)
            {
                if (obj == null)
                    continue;

                if (obj.BaseObject is DataRow row)
                {
                    if (dataTables.Contains(row.Table))
                        continue;

                    foreach (DataColumn column in row.Table.Columns)
                    {
                        var propInfo = new PSPropertyInfo()
                        {
                            Name     = column.ColumnName,
                            TypeName = column.DataType.FullName,
                            Type     = column.DataType
                        };

                        properties.Add(propInfo.Name, propInfo);
                        _Properties.Add(propInfo);
                    }

                    dataTables.Add(row.Table);
                }
                else if (obj.BaseObject is DataRowView rowView)
                {
                    if (dataTables.Contains(rowView.DataView.Table))
                        continue;

                    foreach (DataColumn column in rowView.DataView.Table.Columns)
                    {
                        var propInfo = new PSPropertyInfo()
                        {
                            Name     = column.ColumnName,
                            TypeName = column.DataType.FullName,
                            Type     = column.DataType
                        };

                        properties.Add(propInfo.Name, propInfo);
                        _Properties.Add(propInfo);
                    }

                    dataTables.Add(rowView.DataView.Table);
                }
                else
                {
                    foreach (var property in obj.Properties)
                    {
                        if (property == null || string.IsNullOrWhiteSpace(property.Name) ||
                            !property.IsGettable || string.IsNullOrWhiteSpace(property.TypeNameOfValue))
                            continue;

                        if ((property.MemberType & (PSMemberTypes.Property | PSMemberTypes.CodeProperty | PSMemberTypes.ScriptMethod | PSMemberTypes.NoteProperty)) == 0)
                            continue;

                        Type propType = null;
                        try
                        {
                            propType = Type.GetType(property.TypeNameOfValue, false, true);
                        }
                        catch (Exception)
                        {
                            propType = null;
                        }

                        if (propType == null)
                            continue;
                        else if (!properties.ContainsKey(property.Name))
                        {
                            var propInfo = new PSPropertyInfo()
                            {
                                Name     = property.Name,
                                TypeName = property.TypeNameOfValue,
                                Type     = propType
                            };

                            properties.Add(property.Name, propInfo);
                            _Properties.Add(propInfo);
                        }
                        else
                        {
                            var propInfo = properties[property.Name];
                            if (propInfo.Type != propType)
                            {
                                if (IsNumericType(propInfo.Type) && IsNumericType(propType))
                                {
                                    //If both are numeric - use double for column
                                    propInfo.TypeName = "System.Double";
                                    propInfo.Type     = typeof(double);
                                }
                                else if (IsStringType(propInfo.Type) && IsStringType(propType))
                                {
                                    //If both are strings - use string for column
                                    propInfo.TypeName = "System.String";
                                    propInfo.Type     = typeof(string);
                                }
                                else if ((IsNumericType(propInfo.Type) || IsStringType(propInfo.Type)) &&
                                    (IsNumericType(propType) || IsStringType(propType)))
                                {
                                    //Both are either string or number - use string for column
                                    propInfo.TypeName = "System.String";
                                    propInfo.Type     = typeof(string);
                                }
                                else
                                {
                                    //One or both are nor numeric neither string - skip this property
                                    propInfo.TypeName = null;
                                    propInfo.Type     = null;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = _Properties.Count-1; i >= 0; i--)
            {
                var property = _Properties[i];
                if (property.Type == null)
                {
                    _Properties.RemoveAt(i);
                    continue;
                }

                //Allow only properties of base type
                switch (Type.GetTypeCode(property.Type))
                {
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                    case TypeCode.Object:
                        break;
                    case TypeCode.Empty:
                    //case TypeCode.Object:
                    case TypeCode.DBNull:
                    default:
                        _Properties.RemoveAt(i);
                        break;
                }
            }


            static bool IsNumericType(Type type)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                        return false;
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                    case TypeCode.DateTime:
                    case TypeCode.String:
                        return false;
                    case TypeCode.Empty:
                    case TypeCode.Object:
                    case TypeCode.DBNull:
                    default:
                        return false;
                }
            }

            static bool IsStringType(Type type)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return false;
                    case TypeCode.Char:
                        return true;
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.DateTime:
                        return false;
                    case TypeCode.String:
                        return true;
                    case TypeCode.Empty:
                    case TypeCode.Object:
                    case TypeCode.DBNull:
                    default:
                        return false;
                }
            }
        }

        public IList GetList() => _DataSource as IList;

        public IEnumerator<PSObject> Enumerator => _Enumerator;

        public IList<PSObject> List => _Enumerator as IList<PSObject>;
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

        protected PSPropertyInfo FindProperty(string name)
        {
            for (int i = 0; i < _Properties.Count; i++)
            {
                var property = _Properties[i];
                if (string.Compare(property.Name, name, true) == 0)
                    return property;
            }
            return null;
        }

        protected object GetPropertyValue(PSObject obj, string propertyName)
        {
            if (obj == null)
                return null;

            foreach (var property in obj.Properties)
            {
                if (string.Compare(property.Name, propertyName, true) == 0)
                {
                    try
                    {
                        var value = property.Value;
                        if (value is PSObject psValue)
                            return psValue.BaseObject;
                        return property.Value;
                    }
                    catch (Exception ex)
                    {
                        if (IgnoreErrors)
                            return null;
                        else
                            throw ex;
                    }
                }
            }

            return null;
        }

        //DbDataReader properties and methods.
        public override int Depth => 0;

        public override int FieldCount => _Properties.Count;

        public override bool HasRows => (GetList()?.Count ?? 0) > 0;

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

                return GetPropertyValue(_Enumerator.Current, _Properties[ordinal].Name) ?? DBNull.Value;
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

            var result = SpreadCommander.Common.SqlScript.SqlScript.GetColumnDataType(_Properties[ordinal].Type, int.MaxValue);
            return result;
        }

        public override DateTime GetDateTime(int ordinal) => Convert.ToDateTime(this[ordinal]);

        protected override DbDataReader GetDbDataReader(int ordinal) =>
            throw new NotImplementedException();

        public override decimal GetDecimal(int ordinal) => Convert.ToDecimal(this[ordinal]);

        public override double GetDouble(int ordinal) => Convert.ToDouble(this[ordinal]);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override IEnumerator GetEnumerator() => _Enumerator;

        public override Type GetFieldType(int ordinal)
        {
            if (ordinal < 0 || ordinal >= _Properties.Count)
                return null;

            return _Properties[ordinal].Type;
        }

        public override float GetFloat(int ordinal) => (float)Convert.ToDouble(this[ordinal]);

        public override Guid GetGuid(int ordinal)
        {
            object value = this[ordinal];
            if (value is Guid)
                return (Guid)value;

            var strValue = Convert.ToString(value);
            var result = Guid.Parse(strValue);
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

                var row              = result.NewRow();
                row["ColumnName"]    = property.Name;
                row["ColumnOrdinal"] = i;
                row["ColumnSize"]    = CalcColumnSize(property);
                row["DataType"]      = property.Type;
                row["IsUnique"]      = false;
                row["IsKey"]         = false;

                result.Rows.Add(row);
            }

            return result;


            int CalcColumnSize(PSPropertyInfo property)
            {
                if (property.Type == null)
                    return 0;

                switch (Type.GetTypeCode(property.Type))
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
                        if (_DataSource != null)
                        {
                            int colSize = 0;
                            foreach (var item in _DataSource)
                            {
                                var itemProperty = GetPropertyValue(item, property.Name);

                                var strValue = Convert.ToString(itemProperty);
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

        public override object GetValue(int ordinal) => this[ordinal];

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

            if (_CancelToken != null && _CancelToken.IsCancellationRequested)
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
