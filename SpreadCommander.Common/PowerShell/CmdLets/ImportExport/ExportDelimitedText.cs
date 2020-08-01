using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Windows.Forms;
using DevExpress.DataAccess.Sql;
using FlatFiles;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Export, "DelimitedText")]
    public class ExportDelimitedText : BaseTextImportExportCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of the file containing delimited data.")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "Whether to allow overwrite file or no.")]
        public SwitchParameter Overwrite { get; set; }

        [Parameter(Position = 1, HelpMessage = "Definition of columns in delimited data.")]
        public TextColumnDefinition[] Columns { get; set; }

        [Parameter(HelpMessage = "Character or characters used to separate the columns.")]
        public string Separator { get; set; }

        [Parameter(HelpMessage = @"Record separator. Default is /r, /n, /r/n when reading and Environment.NewLine when writing.")]
        public string RecordSeparator { get; set; }

        [Parameter(HelpMessage = "Character used to quote records containing special characters.)")]
        public char Quote { get; set; } = '"';

        [Parameter(HelpMessage = "How to handle quoting values.")]
        public QuoteBehavior QuoteBehavior { get; set; }

        [Parameter(HelpMessage = "If set - file has no header row.")]
        public SwitchParameter NoHeaderRow { get; set; }

        [Parameter(HelpMessage = "Whether leading and trailing whitespace should be preserved when reading.")]
        public SwitchParameter PreserveWhiteSpace { get; set; }

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

            using var dataReader = GetDataSourceReader(_Output, DataSource, new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns });

            var schema = new SeparatedValueSchema();

            int colCount = Columns != null ? Columns.Length : dataReader.FieldCount;
            int[] columnIndexes = new int[colCount];

            if (Columns != null)
            {
                for (int i = 0; i < Columns.Length; i++)
                {
                    var column = Columns[i];

                    int columnIndex = dataReader.GetOrdinal(column.ColumnName);
                    if (columnIndex < 0)
                        throw new Exception($"Cannot find column '{column.ColumnName}'.");

                    columnIndexes[i] = columnIndex;

                    var definition = CreateColumnDefinition(column);
                    schema.AddColumn(definition);
                }
            }
            else
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    columnIndexes[i] = i;

                    var column = new TextColumnDefinition()
                    {
                        ColumnName = dataReader.GetName(i),
                        ColumnType = dataReader.GetFieldType(i)
                    };
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
            options.QuoteBehavior = QuoteBehavior;

            using var writer = new StreamWriter(File.OpenWrite(fileName));
            var csvWriter    = new SeparatedValueWriter(writer, schema, options);

            int columnCount = schema.ColumnDefinitions.Count;
            object[] values = new object[columnCount];

            bool isHeader = options.IsFirstRecordSchema;

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
