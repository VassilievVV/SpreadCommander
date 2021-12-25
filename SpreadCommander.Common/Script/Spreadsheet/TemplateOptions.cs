using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class TemplateOptions: SpreadsheetWithCopyToBookOptions
    {
        [Description("Sheet name that contains template. If not specified - first sheet is using.")]
        public string TemplateSheetName { get; set; }

        [Description("Sheet name for the data source. Unique sheet name will be generated if sheet already exists or in case of multiple data sources")]
        public string SheetName { get; set; }

        [Description("Replace existing sheet if it exists")]
        public bool Replace { get; set; }

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values")]
        public bool IgnoreErrors { get; set; }

        [Description("Merge parameters")]
        public Hashtable Parameters { get; set; }

        [Description("If set - sheet will be removed after generating table. This can be used, for example, together with CopyBook, to copy a table into a Book and then remove sheet.")]
        public bool TemporarySheet { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }
    }

    public partial class SCSpreadsheet
    {
        public SCSpreadsheet OutTemplate(object dataSource, string templateFileName, TemplateOptions options = null)
        {
            var tableDataSource = GetDataSource(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns });

            options ??= new TemplateOptions();
            var spread = options.Spreadsheet?.Workbook ?? Workbook;

            if (string.IsNullOrWhiteSpace(templateFileName))
                throw new Exception("Template file is not specified.");

            var templateFile = Project.Current.MapPath(templateFileName);
            if (!File.Exists(templateFile))
                throw new Exception($"Template file '{templateFileName}' does not exist.");

            IList<IWorkbook> workbooks;

            using (var template = SpreadsheetUtils.CreateWorkbook())
            {
                ExecuteLocked(() => LoadTemplate(template, templateFile, options.TemplateSheetName), options.LockFiles ? LockObject : null);

                template.MailMergeDataSource                    = tableDataSource;
                template.MailMergeDataMember                    = null;
                template.MailMergeOptions.UseTemplateSheetNames = true;
                template.MailMergeParameters.Clear();
                if (options.Parameters != null)
                {
                    foreach (DictionaryEntry keyValuePair in options.Parameters)
                        template.MailMergeParameters.AddParameter(Convert.ToString(keyValuePair.Key), keyValuePair.Value);
                }

                workbooks = template.GenerateMailMergeDocuments();
            }

            ExecuteSynchronized(() => DoWriteWorkbooks(spread, workbooks, options));
            return this;


            static void LoadTemplate(Workbook template, string templateFile, string templateSheet)
            {
                if (string.IsNullOrWhiteSpace(templateSheet))
                {
                    template.LoadDocument(templateFile);
                }
                else
                {
                    using var workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.LoadDocument(templateFile);
                    template.Worksheets[0].CopyFrom(workbook.Worksheets[templateSheet]);
                }
            }
        }

        protected virtual void DoWriteWorkbooks(IWorkbook spreadsheet, IList<IWorkbook> workbooks, TemplateOptions options)
        {
            Worksheet worksheet = null;

            try
            {
                foreach (var mergeWorkbook in workbooks)
                {
                    //Shall be only one worksheet in each merge workbook
                    foreach (var mergeWorksheet in mergeWorkbook.Worksheets)
                    {
                        using (new UsingProcessor(() => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
                        {
                            if (!string.IsNullOrWhiteSpace(options.SheetName))
                            {
                                var sheet = spreadsheet.Worksheets.Where(s => string.Compare(s.Name, options.SheetName, true) == 0).FirstOrDefault();
                                if (sheet != null)
                                {
                                    if (!options.Replace)
                                        throw new Exception($"Cannot create spreadsheet table: sheet '{options.SheetName}' already exists.");

                                    var sheetName = sheet.Name;

                                    worksheet = spreadsheet.Worksheets.Insert(sheet.Index);
                                    spreadsheet.Worksheets.Remove(sheet);

                                    worksheet.Name = sheetName;
                                }
                            }

                            if (worksheet == null)
                            {
                                var newSheetName = !string.IsNullOrWhiteSpace(options.SheetName) ? options.SheetName :
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

                            AddComments(worksheet.GetDataRange(), options.Comment);
                        }

                        CopyRangeToBook(worksheet.GetDataRange(), options);

                        if (options.TemporarySheet)
                        {
                            if (spreadsheet.Worksheets.Count <= 1)
                                spreadsheet.Worksheets.Add();
                            spreadsheet.Worksheets.Remove(worksheet);
                        }
                    }
                }
            }
            finally
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
