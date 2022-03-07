using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using FlatFiles;
using FlatFiles.TypeMapping;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Import, "DelimitedText")]
    [OutputType(typeof(DataTable))]
    [OutputType(typeof(DbDataReader))]
    public class ImportDelimitedTextCmdlet: BaseTextImportExportCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the file containing delimited data.")]
        public string FileName { get; set; }

        [Parameter(Position = 1, HelpMessage = "Definition of columns in delimited data.")]
        public TextColumnDefinition[] Columns { get; set; }

        [Parameter(HelpMessage = "Character or characters used to separate the columns.")]
        public string Separator { get; set; }

        [Parameter(HelpMessage = "Record separator. Default is /r, /n, /r/n when reading and Environment.NewLine when writing.")]
        public string RecordSeparator { get; set; }

        [Parameter(HelpMessage = "Character used to quote records containing special characters.)")]
        public char Quote { get; set; } = '"';

        [Parameter(HelpMessage = "If set - file has no header row.")]
        public SwitchParameter NoHeaderRow { get; set; }

        [Parameter(HelpMessage = "Whether leading and trailing whitespace should be preserved when reading.")]
        public SwitchParameter PreserveWhiteSpace { get; set; }

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

            if (Columns == null && NoHeaderRow)
                throw new Exception($"{nameof(NoHeaderRow)} requires providing {nameof(Columns)} schema.");

            var schema = new DelimitedSchema();

            if (Columns != null)
            {
                foreach (var column in Columns)
                {
                    var definition = CreateColumnDefinition(column);
                    schema.AddColumn(definition);
                }
            }

            var options = new DelimitedOptions();

            if (Separator != null)
                options.Separator = Separator;
            if (RecordSeparator != null)
                options.RecordSeparator = RecordSeparator;
            options.Quote               = Quote;
            options.IsFirstRecordSchema = !NoHeaderRow;
            options.PreserveWhiteSpace  = PreserveWhiteSpace;
            if (!string.IsNullOrWhiteSpace(Culture))
                options.FormatProvider = new CultureInfo(Culture);

            var readerOptions = new FlatFileDataReaderOptions()
            {
                IsDBNullReturned    = true,
                IsNullStringAllowed = true
            };

            var reader     = new StreamReader(File.OpenRead(fileName));
            var csvReader  = Columns != null ? new DelimitedReader(reader, schema, options) : new DelimitedReader(reader, options);
            var dataReader = new FlatFileDataReader(csvReader, readerOptions);

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
