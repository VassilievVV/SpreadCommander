using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Common.Spreadsheet;
using DevExpress.Spreadsheet;
using SpreadCommander.Documents.ViewModels;
using DevExpress.Mvvm;
using System.IO;
using DevExpress.Utils.Animation;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class ImportExportSpreadsheetForm : BaseForm
    {
        #region SheetName
        public class SheetName: BindableBase
        {
            public bool Selected
            {
                get => GetProperty(() => Selected);
                set => SetProperty(() => Selected, value);
            }

            public string Name
            {
                get => GetProperty(() => Name);
                set => SetProperty(() => Name, value);
            }

            public string NewName
            {
                get => GetProperty(() => NewName);
                set => SetProperty(() => NewName, value);
            }

            public int? Index
            {
                get => GetProperty(() => Index);
                set => SetProperty(() => Index, value);
            }
        }
        #endregion

        #region SpreadComboItem
        private class SpreadComboItem
        {
            public string Title         { get; set; }

            public IWorkbook Workbook   { get; set; }

            public override string ToString() => Title;
        }
        #endregion

        public enum ImportExportSpreadsheetMode { Import, Export }

        private readonly ImportExportSpreadsheetMode _Mode;
        private readonly IWorkbook _Workbook;
        private SpreadComboItem _CurrentComboItem;
        private string _CurrentFileName;

        public ImportExportSpreadsheetForm(ImportExportSpreadsheetMode mode, IWorkbook workbook)
        {
            InitializeComponent();
            SpreadsheetViewer.InitializeSpreadsheet();

            _Mode     = mode;
            _Workbook = workbook ?? throw new ArgumentNullException(nameof(workbook));

            switch (_Mode)
            {
                case ImportExportSpreadsheetMode.Import:
                    btnOK.Text = "Import";
                    layoutSpreadsheetName.Text = "Source spreadsheet: ";
                    break;
                case ImportExportSpreadsheetMode.Export:
                    btnOK.Text = "Export";
                    layoutSpreadsheetName.Text = "Target spreadsheet";
                    break;
            }
        }

        private void LoadSheetNamesFromViewer()
        {
            bindingSheetNames.Clear();

            using (new UsingProcessor(() => viewSheetNames.BeginDataUpdate(), () => viewSheetNames.EndDataUpdate()))
            {
                foreach (var sheet in SpreadsheetViewer.Document.Sheets.OfType<Worksheet>())
                {
                    var sheetName = new SheetName()
                    {
                        Name = sheet.Name
                    };
                    bindingSheetNames.Add(sheetName);
                }

                UpdateNewNames();
            }
        }

        private void UpdateNewNames()
        {

            var workbook = _Mode switch
            {
                ImportExportSpreadsheetMode.Import => _Workbook,
                ImportExportSpreadsheetMode.Export => SpreadsheetViewer.Document,
                _                                  => throw new Exception("Incorrect Import/Export mode.")
            };
            var sheetNames =
                (from sheet in workbook.Sheets
                 select sheet.Name).ToList();

            using (new UsingProcessor(() => viewSheetNames.BeginDataUpdate(), () => viewSheetNames.EndDataUpdate()))
            {
                foreach (SheetName sheetName in bindingSheetNames)
                {
                    var name    = !string.IsNullOrWhiteSpace(sheetName.NewName) ? sheetName.NewName : sheetName.Name;
                    var newName = Utils.GetExcelSheetName(name, sheetNames);

                    sheetName.NewName = newName;
                }
            }
        }

        private void LoadSpreadComboItem(SpreadComboItem comboItem)
        {
            using var stream = new MemoryStream(65536);
            comboItem.Workbook.SaveDocument(stream, DocumentFormat.Xlsx);
            stream.Seek(0, SeekOrigin.Begin);
            SpreadsheetViewer.LoadDocument(stream, DocumentFormat.Xlsx);
        }

        private void ImportExportSpreadsheetForm_Load(object sender, EventArgs e)
        {
            comboSpreadsheetName.Properties.Items.Clear();
            using (new UsingProcessor(
                () => comboSpreadsheetName.Properties.BeginUpdate(),
                () => comboSpreadsheetName.Properties.EndUpdate()))
            {
                var spreadsheets = BaseDocumentViewModel.MainDocumentParent.OpenDocuments.Where(doc => doc is ISpreadsheetHolder);
                foreach (var spreadsheet in spreadsheets)
                {
                    var workbook = (spreadsheet as ISpreadsheetHolder).Workbook;
                    if (workbook == _Workbook)
                        continue;

                    var item = new SpreadComboItem()
                    {
                        Title    = Convert.ToString(spreadsheet.Title),
                        Workbook = workbook
                    };
                    comboSpreadsheetName.Properties.Items.Add(item);
                }
            }

            if (_Mode == ImportExportSpreadsheetMode.Export)
            {
                bindingSheetNames.Clear();

                using (new UsingProcessor(() => viewSheetNames.BeginDataUpdate(), () => viewSheetNames.EndDataUpdate()))
                {
                    foreach (var sheet in _Workbook.Sheets.OfType<Worksheet>())
                    {
                        var sheetName = new SheetName()
                        {
                            Name = sheet.Name
                        };
                        bindingSheetNames.Add(sheetName);
                    }

                    UpdateNewNames();
                }
            }
        }

        private void ComboSpreadsheetName_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboSpreadsheetName.SelectedItem is not SpreadComboItem comboItem || comboItem.Workbook == null)
                return;

            if (_Mode == ImportExportSpreadsheetMode.Import)
                bindingSheetNames.Clear();

            _CurrentFileName  = null;
            _CurrentComboItem = comboItem;

            LoadSpreadComboItem(comboItem);

            if (_Mode == ImportExportSpreadsheetMode.Import)
                LoadSheetNamesFromViewer();
        }

        private void ComboSpreadsheetName_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (Convert.ToString(e.Button.Tag) == "OpenSpreadsheet")
            {
                if (xtraOpenFileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                _CurrentComboItem = null;
                _CurrentFileName  = xtraOpenFileDialog.FileName;

                comboSpreadsheetName.Text = _CurrentFileName;
                comboSpreadsheetName.Tag  = null;

                if (_Mode == ImportExportSpreadsheetMode.Import)
                    bindingSheetNames.Clear();

                var fileName = xtraOpenFileDialog.FileName;

                using (new UsingProcessor(
                    () => transitionManager.StartTransition(SpreadsheetViewer), 
                    () => transitionManager.EndTransition()))
                {
                    SpreadsheetViewer.LoadDocument(fileName);
                }

                switch (_Mode)
                {
                    case ImportExportSpreadsheetMode.Import:
                        LoadSheetNamesFromViewer();
                        break;
                    case ImportExportSpreadsheetMode.Export:
                        UpdateNewNames();
                        break;
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            IWorkbook sourceSpread, targetSpread;
            string operation = Enum.GetName(typeof(ImportExportSpreadsheetMode), _Mode).ToLower();

            switch (_Mode)
            {
                case ImportExportSpreadsheetMode.Import:
                    sourceSpread = SpreadsheetViewer.Document;
                    targetSpread = _Workbook;
                    break;
                case ImportExportSpreadsheetMode.Export:
                    sourceSpread = _Workbook;
                    targetSpread = _CurrentComboItem != null ? _CurrentComboItem.Workbook : SpreadsheetViewer.Document;
                    break;
                default:
                    throw new Exception("Incorrect Import/Export mode.");
            }

            var sheetNames = new List<SheetName>();
            foreach (SheetName sheetName in bindingSheetNames)
            {
                if (sheetName.Selected)
                    sheetNames.Add(sheetName);
            }

            if (sheetNames.Count <= 0)
            {
                XtraMessageBox.Show(this, $"Please select sheet(s) to {operation}.", "No selected sheets", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (new UsingProcessor(
                () => transitionManager.StartWaitingIndicator(SpreadsheetViewer, WaitingAnimatorType.Default),
                () => transitionManager.StopWaitingIndicator()))
            {
                foreach (var sheetName in sheetNames)
                {
                    var sourceSheet = sourceSpread.Worksheets[sheetName.Name];
                    var targetSheet = targetSpread.Worksheets.Add(sheetName.NewName);

                    if (sheetName.Index.HasValue)
                        targetSheet.Move(Utils.ValueInRange(sheetName.Index.Value, 0, targetSpread.Sheets.Count - 1));

                    targetSheet.CopyFrom(sourceSheet);
                }

                if (!string.IsNullOrWhiteSpace(_CurrentFileName) && targetSpread == SpreadsheetViewer.Document)
                    SpreadsheetViewer.SaveDocument(_CurrentFileName);
                else if (_CurrentComboItem != null)
                    LoadSpreadComboItem(_CurrentComboItem);
            }

            XtraMessageBox.Show(this, "Sheets are {operation}ed.", "Sheets copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}