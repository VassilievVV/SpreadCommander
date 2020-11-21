using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Globalization;
using SpreadCommander.Common;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace SpreadCommander.Common.SqlScript
{
	#region DbDataReaderResult
	public enum DbDataReaderResult
	{
		Reading,
		FinishedTable,
		Finished,
		MaxRowsReached,
		Cancelled
	}
	#endregion

	#region SqlScriptDataReader
	public class SqlScriptDataReader : DbDataReader
	{
		private DbDataReader _Reader;
		private int _RowCounter;
		private readonly CancellationToken? _CancelToken;

		public SqlScriptDataReader(DbDataReader reader, CancellationToken? cancelToken = null,
			bool onlyVisibleFields = false)
		{
			_Reader = reader ?? throw new ArgumentNullException(nameof(reader));
			OnlyVisibleFields = onlyVisibleFields;
			_CancelToken      = cancelToken;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (_Reader != null)
			{
				_Reader.Dispose();
				_Reader = null;
			}
		}

		public DbDataReader Reader
		{
			get { return _Reader; }
		}

		public int MaxRows { get; set; } = -1;

		public bool OnlyVisibleFields { get; set; }

		public DbDataReaderResult Result { get; private set; }

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
		public override int Depth
		{
			get { return _Reader.Depth; }
		}

		public override int FieldCount
		{
			get { return !OnlyVisibleFields ? _Reader.FieldCount : _Reader.VisibleFieldCount; }
		}

		public override bool HasRows
		{
			get { return _Reader.HasRows; }
		}

		public override bool IsClosed
		{
			get { return _Reader.IsClosed; }
		}

		public override int RecordsAffected
		{
			get { return _Reader.RecordsAffected; }
		}

		public override int VisibleFieldCount
		{
			get { return _Reader.VisibleFieldCount; }
		}

		public override object this[int ordinal]
		{
			get { return _Reader[ordinal]; }
		}

		public override object this[string name]
		{
			get { return _Reader[name]; }
		}

		public override void Close()
		{
			_Reader.Close();
		}

		public override bool GetBoolean(int ordinal)
		{
			return _Reader.GetBoolean(ordinal);
		}

		public override byte GetByte(int ordinal)
		{
			return _Reader.GetByte(ordinal);
		}

		public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			return _Reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
		}

		public override char GetChar(int ordinal)
		{
			return _Reader.GetChar(ordinal);
		}

		public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			return _Reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
		}

		public override string GetDataTypeName(int ordinal)
		{
			return _Reader.GetDataTypeName(ordinal);
		}

		public override DateTime GetDateTime(int ordinal)
		{
			return _Reader.GetDateTime(ordinal);
		}

		protected override DbDataReader GetDbDataReader(int ordinal)
		{
			return _Reader.GetData(ordinal);
		}

		public override decimal GetDecimal(int ordinal)
		{
			return _Reader.GetDecimal(ordinal);
		}

		public override double GetDouble(int ordinal)
		{
			return _Reader.GetDouble(ordinal);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override IEnumerator GetEnumerator()
		{
			return _Reader.GetEnumerator();
		}

		public override Type GetFieldType(int ordinal)
		{
			return _Reader.GetFieldType(ordinal);
		}

		public override float GetFloat(int ordinal)
		{
			return _Reader.GetFloat(ordinal);
		}

		public override Guid GetGuid(int ordinal)
		{
			return _Reader.GetGuid(ordinal);
		}

		public override short GetInt16(int ordinal)
		{
			return _Reader.GetInt16(ordinal);
		}

		public override int GetInt32(int ordinal)
		{
			return _Reader.GetInt32(ordinal);
		}

		public override long GetInt64(int ordinal)
		{
			return _Reader.GetInt64(ordinal);
		}

		public override string GetName(int ordinal)
		{
			return _Reader.GetName(ordinal);
		}

		public override int GetOrdinal(string name)
		{
			return _Reader.GetOrdinal(name);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Type GetProviderSpecificFieldType(int ordinal)
		{
			return _Reader.GetProviderSpecificFieldType(ordinal);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override object GetProviderSpecificValue(int ordinal)
		{
			return _Reader.GetProviderSpecificValue(ordinal);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetProviderSpecificValues(object[] values)
		{
			return _Reader.GetProviderSpecificValues(values);
		}

		public override DataTable GetSchemaTable()
		{
			return _Reader.GetSchemaTable();
		}

		public override string GetString(int ordinal)
		{
			return _Reader.GetString(ordinal);
		}

		public override object GetValue(int ordinal)
		{
			return _Reader.GetValue(ordinal);
		}

		public override int GetValues(object[] values)
		{
			return _Reader.GetValues(values);
		}

		public override bool IsDBNull(int ordinal)
		{
			return _Reader.IsDBNull(ordinal);
		}

		public override bool NextResult()
		{
			if (Result == DbDataReaderResult.Cancelled)
				return false;

			if (_CancelToken != null && _CancelToken.Value.IsCancellationRequested)
			{
				Result = DbDataReaderResult.Cancelled;
				return false;
			}

			_RowCounter = 0;
			Result = DbDataReaderResult.Reading;

			bool result = _Reader.NextResult();
			if (!result)
				Result = DbDataReaderResult.Finished;

			return result;
		}

		public override bool Read()
		{
			if (Result != DbDataReaderResult.Reading)
				return false;

			if (_CancelToken != null && _CancelToken.Value.IsCancellationRequested)
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
	#endregion
}