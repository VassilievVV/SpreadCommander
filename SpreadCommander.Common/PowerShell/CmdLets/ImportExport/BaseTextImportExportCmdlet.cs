using FlatFiles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    public class BaseTextImportExportCmdlet: SCCmdlet
    {
        #region TextColumnDefinition
        public class TextColumnDefinition
        {
            public string ColumnName                { get; set; }

            public Type ColumnType                  { get; set; } = typeof(string);

            public int ColumnLength                 { get; set; }

            public string Caption                   { get; set; }

            public bool IsNullable                  { get; set; } = true;

            public object DefaultValue              { get; set; }

            public string Culture                   { get; set; }

            public NumberStyles? NumberStyles       { get; set; }

            public string InputFormat               { get; set; }

            public string OutputFormat              { get; set; }

            public string TrueString                { get; set; }

            public string FalseString               { get; set; }

            public bool AllowTrailing               { get; set; }

            public bool Trim                        { get; set; } = true;

            public char? FillCharacter              { get; set; }

            public FixedAlignment? Alignment        { get; set; }

            public OverflowTruncationPolicy? TruncationPolicy { get; set; } = OverflowTruncationPolicy.TruncateTrailing;
        }
        #endregion

        protected IColumnDefinition CreateColumnDefinition(TextColumnDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("Column definition is not provided.");

            string columnName = definition.Caption ?? definition.ColumnName;
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException($"{nameof(definition.ColumnName)} cannot be empty.");

            if (definition.ColumnType == null)
                return new IgnoredColumn(columnName);

            switch (Type.GetTypeCode(definition.ColumnType))
            {
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                    throw new ArgumentException($"Invalid column type: {definition.ColumnType.FullName}");
                case TypeCode.Boolean:
                    var boolColumn = new BooleanColumn(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.TrueString != null)
                        boolColumn.TrueString = definition.TrueString;
                    if (definition.FalseString != null)
                        boolColumn.FalseString = definition.FalseString;
                    if (definition.DefaultValue != null)
                        boolColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    return boolColumn;
                case TypeCode.Char:
                    var charColumn = new CharColumn(columnName)
                    {
                        IsNullable    = definition.IsNullable,
                        AllowTrailing = definition.AllowTrailing
                    };
                    return charColumn;
                case TypeCode.SByte:
                    var sbyteColumn = new SByteColumn(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        sbyteColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        sbyteColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        sbyteColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        sbyteColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return sbyteColumn;
                case TypeCode.Byte:
                    var byteColumn = new ByteColumn(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        byteColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        byteColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        byteColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        byteColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return byteColumn;
                case TypeCode.Int16:
                    var shortColumn = new Int16Column(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        shortColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        shortColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        shortColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        shortColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return shortColumn;
                case TypeCode.UInt16:
                    var ushortColumn = new UInt16Column(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        ushortColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        ushortColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        ushortColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        ushortColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return ushortColumn;
                case TypeCode.Int32:
                    var intColumn = new Int32Column(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        intColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        intColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        intColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        intColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return intColumn;
                case TypeCode.UInt32:
                    var uintColumn = new UInt32Column(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        uintColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        uintColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        uintColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        uintColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return uintColumn;
                case TypeCode.Int64:
                    var longColumn = new Int64Column(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        longColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        longColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        longColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        longColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return longColumn;
                case TypeCode.UInt64:
                    var ulongColumn = new UInt64Column(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        ulongColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        ulongColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        ulongColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        ulongColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return ulongColumn;
                case TypeCode.Single:
                    var singleColumn = new SingleColumn(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        singleColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        singleColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        singleColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        singleColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return singleColumn;
                case TypeCode.Double:
                    var doubleColumn = new DoubleColumn(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        doubleColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        doubleColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        doubleColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        doubleColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return doubleColumn;
                case TypeCode.Decimal:
                    var decimalColumn = new DecimalColumn(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (definition.NumberStyles.HasValue)
                        decimalColumn.NumberStyles = definition.NumberStyles.Value;
                    if (definition.DefaultValue != null)
                        decimalColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        decimalColumn.OutputFormat = definition.OutputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        decimalColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return decimalColumn;
                case TypeCode.DateTime:
                    var datetimeColumn = new DateTimeColumn(columnName)
                    {
                        IsNullable = definition.IsNullable
                    };
                    if (!string.IsNullOrWhiteSpace(definition.InputFormat))
                        datetimeColumn.InputFormat = definition.InputFormat;
                    if (!string.IsNullOrWhiteSpace(definition.OutputFormat))
                        datetimeColumn.OutputFormat = definition.OutputFormat;
                    if (definition.DefaultValue != null)
                        datetimeColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(definition.Culture))
                        datetimeColumn.FormatProvider = new CultureInfo(definition.Culture);
                    return datetimeColumn;
                case TypeCode.String:
                    var stringColumn = new StringColumn(columnName)
                    {
                        IsNullable = definition.IsNullable,
                        Trim       = definition.Trim
                    };
                    if (definition.DefaultValue != null)
                        stringColumn.DefaultValue = DefaultValue.Use(definition.DefaultValue);
                    return stringColumn;
                default:
                    throw new ArgumentException("Invalid column type for text import/export.");
            }
        }
    }
}
