using FlatFiles;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Import, "FixedLengthText")]
    [OutputType(typeof(DataTable))]
    public class ImportFixedLengthTextCmdlet: BaseTextImportExportCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the file containing delimited data.")]
        public string FileName { get; set; }

        [Parameter(Position = 1, HelpMessage = "Definition of columns in delimited data.")]
        public TextColumnDefinition[] Columns { get; set; }

        [Parameter(HelpMessage = "Character used to buffer values in a column.")]
        public char FillCharacter { get; set; } = ' ';

        [Parameter(HelpMessage = "When set - Import-FixedLength text will attempt to start reading the next record immediately after the end of the previous record.")]
        public SwitchParameter NoRecordSeparator { get; set; }

        [Parameter(HelpMessage = "Record separator. Default is /r, /n, /r/n when reading and Environment.NewLine when writing.")]
        public string RecordSeparator { get; set; }

        [Parameter(HelpMessage = "If set - file has no header row.")]
        public SwitchParameter NoHeaderRow { get; set; }

        [Parameter(HelpMessage = "Column alignment.")]
        public FixedAlignment Alignment { get; set; }

        [Parameter(HelpMessage = "Culture as format provider.")]
        public string Culture { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Return DbDataReader instead of DataTable. Can be used to export data into database.")]
        public SwitchParameter AsDataReader { get; set; }

        [Parameter(HelpMessage = "Ignore reader errors and return NULL when error is encountered.")]
        public SwitchParameter IgnoreReaderErrors { get; set; }


        protected override void ProcessRecord()
        {
            var result = ImportText();
            WriteObject(result);
        }

        private object ImportText()
        {
            var fileName = Project.Current.MapPath(FileName);
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                throw new FileNotFoundException($"File '{fileName}' not found.");

            var schema = new FixedLengthSchema();

            if (Columns != null)
            {
                foreach (var column in Columns)
                {
                    var definition = CreateColumnDefinition(column);
                    var window     = new Window(column.ColumnLength);

                    if (column.FillCharacter.HasValue)
                        window.FillCharacter = column.FillCharacter.Value;
                    if (column.Alignment.HasValue)
                        window.Alignment = column.Alignment.Value;
                    if (column.TruncationPolicy.HasValue)
                        window.TruncationPolicy = column.TruncationPolicy.Value;

                    schema.AddColumn(definition, window);
                }
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
            if (!string.IsNullOrWhiteSpace(Culture))
                options.FormatProvider = new CultureInfo(Culture);

            var readerOptions = new FlatFileDataReaderOptions()
            {
                IsDBNullReturned = true,
                IsNullStringAllowed = true
            };

            using var reader = new StreamReader(File.OpenRead(fileName));
            var csvReader    = new FixedLengthReader(reader, schema, options);
            var dataReader   = new FlatFileDataReader(csvReader, readerOptions);

            var resultReader = new DataReaderWrapper(dataReader, new DataReaderWrapper.DataReaderWrapperParameters()
            {
                Columns            = this.SelectColumns,
                SkipColumns        = this.SkipColumns,
                IgnoreReaderErrors = this.IgnoreReaderErrors,
                CloseAction = () =>
                {
                    reader.Dispose();
                }
            });

            if (AsDataReader)
                return resultReader;
            else
            {
                var table = new DataTable("TextData");
                table.Load(resultReader);

                return table;
            }
        }
    }
}
