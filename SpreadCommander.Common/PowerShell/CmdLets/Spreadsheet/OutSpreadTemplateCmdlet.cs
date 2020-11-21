using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsData.Out, "SpreadTemplate")]
    public class OutSpreadTemplateCmdlet: BaseSpreadsheetWithCopyToBookCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "File with spreadsheet template.")]
        public string TemplateFile { get; set; }

        [Parameter(HelpMessage = "Sheet name for the data source. Unique sheet name will be generated if sheet already exists or in case of multiple data sources")]
        [Alias("Sheet")]
        public string SheetName { get; set; }

        [Parameter(HelpMessage = "Replace existing sheet if it exists")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Merge parameters")]
        public Hashtable Parameters { get; set; }

        [Parameter(HelpMessage = "If set - sheet will be removed after generating table. This can be used, for example, together with CopyBook, to copy a table into a Book and then remove sheet.")]
        public SwitchParameter TemporarySheet { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }
        
        [Parameter(HelpMessage = "If set - new spreadsheet will be generated and sent to the pipe")]
        public SwitchParameter OutputSpreadsheet { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


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
            WriteTable();

            if (PassThru)
                WriteObject(_Output, true);
        }

        protected void WriteTable()
        {
            WriteTable(GetCmdletSpreadsheet());
        }

        protected void WriteTable(IWorkbook spreadsheet)
        {
            var dataSource = GetDataSource(_Output, DataSource, 
                new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns });

            if (string.IsNullOrWhiteSpace(TemplateFile))
                throw new Exception("Template file is not specified.");

            var templateFile = Project.Current.MapPath(TemplateFile);
            if (!File.Exists(templateFile))
                throw new Exception($"Template file '{TemplateFile}' does not exist.");

            IList<IWorkbook> workbooks;
            
            using (var template = SpreadsheetUtils.CreateWorkbook())
            {
                ExecuteLocked(() => template.LoadDocument(templateFile), LockFiles ? LockObject : null);

                template.MailMergeDataSource                    = dataSource;
                template.MailMergeDataMember                    = null;
                template.MailMergeOptions.UseTemplateSheetNames = true;
                template.MailMergeParameters.Clear();
                if (Parameters != null)
                {
                    foreach (DictionaryEntry keyValuePair in Parameters)
                        template.MailMergeParameters.AddParameter(Convert.ToString(keyValuePair.Key), keyValuePair.Value);
                }

                workbooks = template.GenerateMailMergeDocuments();
            }

            ExecuteSynchronized(() => DoWriteWorkbooks(spreadsheet, workbooks));

            if (OutputSpreadsheet)
            {
                foreach (var workbook in workbooks)
                    WriteObject(new SCSpreadsheetContext(workbook));
            }
        }

        protected virtual void DoWriteWorkbooks(IWorkbook spreadsheet, IList<IWorkbook> workbooks)
        {
            Worksheet worksheet = null;

            foreach (var mergeWorkbook in workbooks)
            {
                //Shall be only one worksheet in each merge workbook
                foreach (var mergeWorksheet in mergeWorkbook.Worksheets)
                {
                    using (new UsingProcessor(() => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
                    {
                        if (!string.IsNullOrWhiteSpace(SheetName))
                        {
                            var sheet = spreadsheet.Worksheets.Where(s => string.Compare(s.Name, SheetName, true) == 0).FirstOrDefault();
                            if (sheet != null)
                            {
                                if (!Replace)
                                    throw new Exception($"Cannot create spreadsheet table: sheet '{SheetName}' already exists.");

                                var sheetName = sheet.Name;

                                worksheet = spreadsheet.Worksheets.Insert(sheet.Index);
                                spreadsheet.Worksheets.Remove(sheet);

                                worksheet.Name = sheetName;
                            }
                        }

                        if (worksheet == null)
                        {
                            var newSheetName = !string.IsNullOrWhiteSpace(SheetName) ? SheetName :
                                Utils.AddUniqueString(spreadsheet.Worksheets.Select(sheet => sheet.Name).ToList(),
                                    "Sheet1", StringComparison.CurrentCultureIgnoreCase, false);

                            if (spreadsheet.Worksheets.Count == 1 && IsRangeEmpty(spreadsheet.Worksheets[0].GetUsedRange()))
                            {
                                worksheet = spreadsheet.Worksheets[0];
                                worksheet.Name = newSheetName;
                            }
                            else
                                worksheet = spreadsheet.Worksheets.Add(newSheetName);
                        }

                        worksheet.CopyFrom(mergeWorksheet);

                        AddComments(worksheet.GetDataRange());
                    }

                    CopyRangeToBook(worksheet.GetDataRange());

                    if (TemporarySheet)
                    {
                        if (spreadsheet.Worksheets.Count <= 1)
                            spreadsheet.Worksheets.Add();
                        spreadsheet.Worksheets.Remove(worksheet);
                    }
                }
            }

            if (!OutputSpreadsheet)
            { 
                for (int i = 0; i < workbooks.Count; i++)
                    workbooks[i].Dispose();
            }


            static bool IsRangeEmpty(CellRange range)
            {
                if (range.TopRowIndex == 0 && range.BottomRowIndex == 0 &&
                    range.LeftColumnIndex == 0 && range.RightColumnIndex == 0)
                    return true;
                return false;
            }
        }
    }
}
