using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Windows.Forms;
using FlatFiles;
using FlatFiles.TypeMapping;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Import, "DelimitedText")]
    [OutputType(typeof(DataTable))]
    public class ImportDelimitedText: BaseTextImportExportCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the file containing delimited data.")]
        public string FileName { get; set; }

        [Parameter(Position = 1, HelpMessage = "Definition of columns in delimited data.")]
        public TextColumnDefinition[] Columns { get; set; }

        [Parameter(HelpMessage = "Character or characters used to separate the columns.")]
        public string Separator { get; set; }

        [Parameter(HelpMessage = @"Record separator. Default is /r, /n, /r/n when reading and Environment.NewLine when writing.")]
        public string RecordSeparator { get; set; }

        [Parameter(HelpMessage = "Character used to quote records containing special characters.)")]
        public char Quote { get; set; } = '"';

        [Parameter(HelpMessage = "If set - file has no header row.")]
        public SwitchParameter NoHeaderRow { get; set; }

        [Parameter(HelpMessage = "Whether leading and trailing whitespace should be preserved when reading.")]
        public SwitchParameter PreserveWhiteSpace { get; set; }

        [Parameter(HelpMessage = "Culture as format provider.")]
        public string Culture { get; set; }


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

            var schema = new SeparatedValueSchema();

            if (Columns != null)
            {
                foreach (var column in Columns)
                {
                    var definition = CreateColumnDefinition(column);
                    schema.AddColumn(definition);
                }
            }

            var options = new SeparatedValueOptions();

            if (Separator != null)
                options.Separator = Separator;
            if (RecordSeparator != null)
                options.RecordSeparator = RecordSeparator;
            options.Quote = Quote;
            options.IsFirstRecordSchema = !NoHeaderRow;
            options.PreserveWhiteSpace = PreserveWhiteSpace;
            if (!string.IsNullOrWhiteSpace(Culture))
                options.FormatProvider = new CultureInfo(Culture);

            var readerOptions = new FlatFileDataReaderOptions()
            {
                IsDBNullReturned    = true,
                IsNullStringAllowed = true
            };

            using var reader = new StreamReader(File.OpenRead(fileName));
            var csvReader    = Columns != null ? new SeparatedValueReader(reader, schema, options) : new SeparatedValueReader(reader, options);
            var dataReader   = new FlatFileDataReader(csvReader, readerOptions);

            var table = new DataTable("TextData");
            table.Load(dataReader);

            return table;
        }
    }
}
