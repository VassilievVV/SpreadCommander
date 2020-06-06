using FlatFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Management.Automation;
using System.Text;
using SpreadCommander.Common.Code;
using System.Globalization;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Export, "FixedLengthText")]
    public class ExportFixedLengthText : BaseTextImportExportCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of the file containing delimited data.")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "Whether to allow overwrite file or no.")]
        public SwitchParameter Overwrite { get; set; }

        [Parameter(Position = 1, HelpMessage = "Definition of columns in delimited data.")]
        public TextColumnDefinition[] Columns { get; set; }

        [Parameter(HelpMessage = "Character used to buffer values in a column.")]
        public char FillCharacter { get; set; } = ' ';

        [Parameter(HelpMessage = "When set - Import-FixedLength text will attempt to start reading the next record immediately after the end of the previous record.")]
        public SwitchParameter NoRecordSeparator { get; set; }

        [Parameter(HelpMessage = @"Record separator. Default is /r, /n, /r/n when reading and Environment.NewLine when writing.")]
        public string RecordSeparator { get; set; }

        [Parameter(HelpMessage = "If set - file has no header row.")]
        public SwitchParameter NoHeaderRow { get; set; }

        [Parameter(HelpMessage = "Column alignment.")]
        public FixedAlignment Alignment { get; set; }

        [Parameter(HelpMessage = "Truncation policy")]
        public OverflowTruncationPolicy TruncationPolicy { get; set; } = OverflowTruncationPolicy.TruncateTrailing;

        [Parameter(HelpMessage = "Culture as format provider.")]
        public string Culture { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output")]
        public SwitchParameter PassThru { get; set; }


        private readonly List<PSObject> _Output = new List<PSObject>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Output.Add(obj);
        }

        protected override void EndProcessing()
        {
            ExportText();

            if (PassThru)
                WriteObject(_Output, true);
        }


        private void ExportText()
        {
            var fileName = Project.Current.MapPath(FileName);

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("Filename is not provided.");

            if (!Overwrite && File.Exists(fileName))
                throw new Exception($"File '{FileName}' already exists.");

            if (Overwrite && File.Exists(fileName))
                File.Delete(fileName);

            using var dataReader = GetDataSourceReader(_Output, DataSource, IgnoreErrors);

            var schema = new FixedLengthSchema();

            if (Columns == null)
                throw new ArgumentNullException(nameof(Columns), "Fixed-length import/export requires column schema.");

            int[] columnIndexes = new int[Columns.Length];

            for (int i = 0; i < Columns.Length; i++)
            {
                var column = Columns[i];

                int columnIndex = dataReader.GetOrdinal(column.ColumnName);
                if (columnIndex < 0)
                    throw new Exception($"Cannot find column '{column.ColumnName}'.");

                columnIndexes[i] = columnIndex;

                var definition = CreateColumnDefinition(column);
                var window = new Window(column.ColumnLength);

                if (column.FillCharacter.HasValue)
                    window.FillCharacter = column.FillCharacter.Value;
                if (column.Alignment.HasValue)
                    window.Alignment = column.Alignment.Value;
                if (column.TruncationPolicy.HasValue)
                    window.TruncationPolicy = column.TruncationPolicy.Value;

                schema.AddColumn(definition, window);
            }

#pragma warning disable IDE0017 // Simplify object initialization
            var options = new FixedLengthOptions();
#pragma warning restore IDE0017 // Simplify object initialization

            options.FillCharacter = FillCharacter;
            options.HasRecordSeparator = !NoRecordSeparator;
            if (RecordSeparator != null)
                options.RecordSeparator = RecordSeparator;
            options.IsFirstRecordHeader = !NoHeaderRow;
            options.Alignment = Alignment;
            options.TruncationPolicy = TruncationPolicy;
            if (!string.IsNullOrWhiteSpace(Culture))
                options.FormatProvider = new CultureInfo(Culture);

            using var writer = new StreamWriter(File.OpenWrite(fileName));
            var csvWriter = new FixedLengthWriter(writer, schema, options);

            int columnCount = schema.ColumnDefinitions.Count;
            object[] values = new object[columnCount];

            bool isHeader = options.IsFirstRecordHeader;

            while (dataReader.Read())
            {
                for (int i = 0; i < columnCount; i++)
                {
                    object value = dataReader.GetValue(columnIndexes[i]);
                    if (value == DBNull.Value)
                        value = null;

                    if (!isHeader)
                        value = Utils.ChangeType(schema.ColumnDefinitions[i].ColumnType, value, null);
                    values[i] = value;

                    isHeader = false;
                }

                csvWriter.Write(values);
            }
        }
    }
}
