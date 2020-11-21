using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.IO;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using SpreadCommander.Common;
using System.Linq;
using SpreadCommander.Common.Code;
using System.Data.Common;
using SpreadCommander.Common.SqlScript;
using System.ComponentModel;
using SpreadCommander.Documents.Services;
using System.Threading;
using System.Threading.Tasks;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Spreadsheet;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Documents.ViewModels
{
    public class SpreadsheetDocumentViewModel: BaseDocumentViewModel, ISpreadsheetHolder
    {
        public const string ViewName = "SpreadsheetDocument";

        #region ICallback
        public interface ICallback
        {
            void LoadFromStream(Stream stream);
            void SaveToStream(Stream stream);
            object InvokeFunction(Func<object> method);
            void SpreadsheetModified();
            void CloseEditor();
            void BeginWait();
            void EndWait();

            IWorkbook Workbook { get; }
        }
        #endregion

        #region SpreadParametersData
        public class SpreadParametersData: BaseDocumentViewModel.ParametersData
        {
            public Stream SpreadsheetStream { get; set; }
        }
        #endregion

        public SpreadsheetDocumentViewModel()
        {
        }

        public static SpreadsheetDocumentViewModel Create() =>
            ViewModelSource.Create<SpreadsheetDocumentViewModel>(() => new SpreadsheetDocumentViewModel());

        public ICallback Callback { get; set; }

        public IWorkbook Workbook => Callback?.Workbook;

        public override string DefaultExt   => "xlsx";
        public override string FileFilter   => "Excel Workbook (*.xlsx)|*.xlsx|Excel Macro-Enabled Workbook (*.xlsm)|*.xlsm|Excel 97-2003 Workbook (*.xls)|*.xls|Text (Tab delimited) (*.txt)|*.txt|CSV (Comma delimited) (*.csv)|*.csv";
        public override string DocumentType => ViewName;
        
        public override bool Modified
        {
            get => base.Modified;
            set
            {
                if (base.Modified != value)
                {
                    base.Modified = value;
                    if (value)
                        Callback?.SpreadsheetModified();
                }
            }
        }

        public void LoadFromStream(Stream stream)
        {
            Callback?.LoadFromStream(stream);
        }

        public void MergeWorkbooks(bool deleteCurrentSheets, params IWorkbook[] workbooks)
        {
            var rootWorkbook = Callback?.Workbook;
            if (rootWorkbook == null)
                throw new NullReferenceException("Workbook is not loaded yet.");

            var sheetCount = rootWorkbook.Sheets.Count;

            rootWorkbook.Append(workbooks);

            if (deleteCurrentSheets)
            {
                for (int i = 0; i < sheetCount; i++)
                    if (rootWorkbook.Worksheets.Count > 0)
                        rootWorkbook.Worksheets.RemoveAt(0);
            }
        }

        public Table AppendDataTable(DataTable table)
        {
            var rootWorkbook = Callback?.Workbook;
            if (rootWorkbook == null)
                throw new NullReferenceException("Workbook is not loaded yet.");

            var sheetName = Utils.GetExcelSheetName(table.TableName, rootWorkbook.Worksheets.Select(sheet => sheet.Name).ToList());
            var worksheet = rootWorkbook.Worksheets.Add(sheetName);
            worksheet.FreezeRows(0);

            var options = new DataSourceImportOptions()
            {
                ImportFormulas = true,
                FormulaCulture = CultureInfo.InvariantCulture
            };

            var tableName = SpreadsheetUtils.GetUniqueTableName(rootWorkbook, sheetName);

            worksheet.Import(table, true, 0, 0, options);

            var result = worksheet.Tables.Add(worksheet.GetDataRange(), tableName, true);
            return result;
        }

        public Table AppendDataSource(object dataSource, string sheetName = "Table")
        {
            var rootWorkbook = Callback?.Workbook;
            if (rootWorkbook == null)
                throw new NullReferenceException("Workbook is not loaded yet.");

            var result = SpreadsheetUtils.AppendDataSource(rootWorkbook, dataSource, sheetName);
            return result;
        }

        public Table AppendDataSource(Worksheet worksheet, object dataSource)
        {
            var rootWorkbook = Callback?.Workbook;
            if (rootWorkbook == null)
                throw new NullReferenceException("Workbook is not loaded yet.");

            var result = SpreadsheetUtils.AppendDataSource(worksheet, dataSource);
            return result;
        }

        public void DeleteFirstSheet()
        {
            var rootWorkbook = Callback?.Workbook;
            if (rootWorkbook == null)
                throw new NullReferenceException("Workbook is not loaded yet.");
            if (rootWorkbook.Worksheets.Count > 1)	//Do not remove single sheet
                rootWorkbook.Worksheets.RemoveAt(0);
        }

        public override string[] GetTableNames()
        {
            var rootWorkbook = Callback?.Workbook;
            if (rootWorkbook == null)
                return null;

            var result = SpreadsheetUtils.GetTableNames(rootWorkbook);
            return result.ToArray();
        }

        public override DbDataReader GetDataTable(string tableName)
        {
            var result = (DbDataReader)Callback.InvokeFunction(() =>
            {
                var rootWorkbook = Callback?.Workbook;
                if (rootWorkbook == null || string.IsNullOrWhiteSpace(tableName))
                    return null;

                var dbTable = SpreadsheetUtils.GetTableDataReader(rootWorkbook, tableName);
                return dbTable;
            });
            return result;
        }

        public void ExportToDatabase()
        {
            TableExporterService.ExportDataTables(this);
        }

        protected Table GetSelectedTable()
        {
            var workbook = Callback?.Workbook;
            if (workbook == null)
            {
                MessageService.ShowMessage("Cannot find workbook.", "No workbook", MessageButton.OK, MessageIcon.Error);
                return null;
            }

            var selectedTable = workbook.GetSelectedTable();
            if (selectedTable == null)
            {
                MessageService.ShowMessage("Cannot find active table.", "No table", MessageButton.OK, MessageIcon.Error);
                return null;
            }

            return selectedTable;
        }

        public void ShowSpreadsheetTemplateEditor()
        {
            var selectedTable = GetSelectedTable();
            if (selectedTable == null)
                return;

            var dataTable = selectedTable.ExportToDataTable();
            SpreadsheetTemplateService.EditSpreadsheetTemplate(dataTable);
        }

        public void ShowBookTemplateEditor()
        {
            var selectedTable = GetSelectedTable();
            if (selectedTable == null)
                return;

            var dataTable = selectedTable.ExportToDataTable();
            BookTemplateService.EditBookTemplate(dataTable);
        }

        public void SelectTable()
        {
            if (Workbook?.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var table = sheet.Selection.GetRangeTable() ?? throw new Exception("Please select range inside a table.");
            sheet.Selection = table.Range;
        }

        public void SelectTableData()
        {
            if (Workbook?.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var table = sheet.Selection.GetRangeTable() ?? throw new Exception("Please select range inside a table.");
            sheet.Selection = table.DataRange;
        }

        public void ExpandSelectionToRows()
        {
            if (Workbook?.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection   = sheet.Selection.ExpandToTableRows();
            sheet.Selection = selection;
        }

        public void ExpandSelectionToColumns()
        {
            if (Workbook?.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection   = sheet.Selection.ExpandToTableColumn();
            sheet.Selection = selection;
        }

        public void CopySelectionToRows()
        {
            if (Workbook?.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection;
            selection.CopyToTableRows();
        }

        public void CopySelectionToColumns()
        {
            if (Workbook?.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection;
            selection.CopyToTableColumns();
        }

        public override void LoadFromFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            Workbook.LoadDocument(fileName);
            base.LoadFromFile(fileName);
        }

        public override void SaveToFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            Workbook.SaveDocument(fileName, DocumentFormat.Xlsx);
            base.SaveToFile(fileName);
        }

        public void ExportSheets()
        {
            var workbook = Workbook ?? throw new NullReferenceException("Workbook is not loaded yet.");
            ImportExportSpreadSheetsService.ExportSheets(workbook);
        }

        public void ImportSheets()
        {
            var workbook = Workbook ?? throw new NullReferenceException("Workbook is not loaded yet.");
            ImportExportSpreadSheetsService.ImportSheets(workbook);
        }
        
        public void CloneSpreadsheet()
        {
            var workbook = Workbook ?? throw new NullReferenceException("Workbook is not loaded yet.");
            var modified = workbook.Modified;

            using (new UsingProcessor(() => Callback.BeginWait(), () => Callback.EndWait()))
            {
                using var stream = new MemoryStream(65536);
                workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                stream.Seek(0, SeekOrigin.Begin);
                workbook.Modified = modified;

                var newSpreadsheetModel = AddNewSpreadsheetModel();
                newSpreadsheetModel.LoadFromStream(stream);
                newSpreadsheetModel.Modified = true;
            }
        }
    }
}