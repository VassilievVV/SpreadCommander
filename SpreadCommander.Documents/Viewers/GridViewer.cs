#pragma warning disable CRR0050

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common;
using SpreadCommander.Common.Extensions;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System.IO;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using DevExpress.Spreadsheet;
using DevExpress.DataAccess.Excel;
using DevExpress.XtraGrid.Views.Grid;
using SpreadCommander.Documents.Code;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data;
using SpreadCommander.Common.Code;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Globalization;
using SpreadCommander.Documents.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using SpreadCommander.Documents.Extensions;
using DevExpress.Compression;
using System.Xml.Linq;
using DevExpress.XtraGrid.Extension;
using System.Text.RegularExpressions;

namespace SpreadCommander.Documents.Viewers
{
    public partial class GridViewer : BaseViewer
    {
        public object DataSource { get; protected set; }

        public GridProperties GridProperties { get; }

        public GridViewer()
        {
            Utils.StartProfile("GridViewer");

            InitializeComponent();
            
            GridUtils.InitializeGridView(viewTable);

            //Allow show editors
            viewTable.OptionsBehavior.Editable  = true;
            viewTable.OptionsBehavior.ReadOnly  = true;
            viewTable.OptionsFind.AlwaysVisible = true;

            GridProperties = new GridProperties(viewTable);
        }

        public GridControl GridControl => gridTable;

        public void SetFullMode(bool fullMode)
        {
            viewTable.OptionsView.ShowAutoFilterRow = fullMode;
            viewTable.OptionsView.ShowGroupPanel    = fullMode;
            viewTable.OptionsFind.AlwaysVisible     = fullMode;
        }

        public bool ColumnAutoWidth
        {
            get => viewTable.OptionsView.ColumnAutoWidth;
            set => viewTable.OptionsView.ColumnAutoWidth = value;
        }

        public string PreviewFieldName
        {
            get => viewTable.PreviewFieldName;
            set
            {
                viewTable.PreviewFieldName        = value;
                viewTable.OptionsView.ShowPreview = !string.IsNullOrWhiteSpace(value);
            }
        }

        public GridFormatRuleCollection FormatRules => viewTable.FormatRules;
        public GridColumnCollection Columns         => viewTable.Columns;

        private void Zoom(int? delta)
        {
            int iDelta = delta.HasValue ? viewTable.Appearance.Row.FontSizeDelta + delta.Value : 0;
            iDelta = Utils.ValueInRange(iDelta, -5, 20);

            viewTable.Appearance.FilterPanel.FontSizeDelta = iDelta;
            viewTable.Appearance.Row.FontSizeDelta         = iDelta;
            viewTable.Appearance.HeaderPanel.FontSizeDelta = iDelta;
            viewTable.Appearance.FooterPanel.FontSizeDelta = iDelta;
            viewTable.Appearance.GroupPanel.FontSizeDelta  = iDelta;
        }

        public void ClearDataSource() => AttachDataSource(null);

        public override void AttachDataSource(object dataSource)
        {
            GridControl.DataSource = null;
            viewTable.Columns.Clear();
            viewTable.FormatConditions.Clear();
            viewTable.FormatRules.Clear();

            DataSource = dataSource;
            GridControl.DataSource = DataSource;
            GridControl.RefreshDataSource();

            if (dataSource != null)
            {
                viewTable.PopulateColumns();
                viewTable.BestFitColumns();
                viewTable.UpdateViewColumns();

                switch (dataSource)
                {
                    case DataTable dataTable:
                        InitializeDataTable(viewTable, dataTable);
                        break;
                    case DataView dataView:
                        InitializeDataTable(viewTable, dataView.Table);
                        break;
                }
            }

            viewTable.LayoutChanged();
            viewTable.Invalidate();
        }

        public void RefreshDataSource()
        {
            GridControl.DataSource = null;
            GridControl.DataSource = DataSource;
            GridControl.RefreshDataSource();
        }
        
        public static void InitializeDataTable(GridView gridView, DataTable dataTable)
        {
            if (dataTable == null)
                return;
            
            var formatConditions = new List<ConsoleCommands.BaseCommand>();
            foreach (var propKey in dataTable.ExtendedProperties.Keys)
            {
                if (propKey is string str && str.StartsWith("Format", StringComparison.InvariantCultureIgnoreCase))
                {
                    var propValue = dataTable.ExtendedProperties[propKey];
                    if (propValue != null)
                    {
                        var propFormatConditions = SpreadCommander.Common.ScriptEngines.ConsoleCommands.FormatCondition.Parse(Convert.ToString(propValue), true);
                        formatConditions.AddRange(propFormatConditions);
                    }
                }
            }
            GridViewer.ApplyGridFormatRules(gridView, formatConditions);

            var computedColumns = new List<ConsoleCommands.ComputedColumn>();
            foreach (var propKey in dataTable.ExtendedProperties.Keys)
            {
                if (propKey is string str && str.StartsWith("ComputedColumn", StringComparison.InvariantCultureIgnoreCase))
                {
                    var propValue = dataTable.ExtendedProperties[propKey];
                    if (propValue != null)
                    {
                        var propComputedColumns = SpreadCommander.Common.ScriptEngines.ConsoleCommands.ComputedColumn.Parse(Convert.ToString(propValue), true);
                        computedColumns.AddRange(propComputedColumns);
                    }
                }
            }

            for (int i = gridView.Columns.Count - 1; i >= 0; i--)
            {
                if (gridView.Columns[i].UnboundType != UnboundColumnType.Bound)
                    gridView.Columns.RemoveAt(i);
            }
            foreach (var computedColumn in computedColumns)
            {
                var caption = computedColumn.ColumnName;

                var newColumn                       = gridView.Columns.AddVisible(caption);
                newColumn.Name                      = caption;
                newColumn.Caption                   = caption;
                newColumn.UnboundType               = ConvertUnboudColumnType(computedColumn.ReturnType);
                newColumn.UnboundExpression         = computedColumn.Expression;
                newColumn.ShowUnboundExpressionMenu = true;
                newColumn.Visible                   = true;
                newColumn.Width                     = 100;
            }

            const string reSortDesc = @"(?i)\s+desc$";

            gridView.ClearGrouping();
            gridView.ClearSorting();

            var groupBy = Convert.ToString(dataTable.ExtendedProperties["Table_GroupBy"]);
            var orderBy = Convert.ToString(dataTable.ExtendedProperties["Table_OrderBy"]);
            if (!string.IsNullOrWhiteSpace(groupBy) || !string.IsNullOrWhiteSpace(orderBy))
            {
                using (new UsingProcessor(() => gridView.BeginSort(), () => gridView.EndSort()))
                {
                    var sortInfos        = new List<GridColumnSortInfo>();
                    int groupColumnCount = 0;

                    if (!string.IsNullOrWhiteSpace(groupBy))
                    {
                        var groups = Utils.SplitString(groupBy, ',');

                        foreach (var group in groups)
                        {
                            if (string.IsNullOrWhiteSpace(group))
                                continue;

                            var sortAscending = true;
                            var colName       = group;
                            if (Regex.IsMatch(colName, reSortDesc))
                            {
                                colName = Regex.Replace(colName, reSortDesc, string.Empty);
                                sortAscending = false;
                            }

                            var column = gridView.Columns.ColumnByFieldName(colName);
                            if (column != null)
                            {
                                var colSortInfo = new GridColumnSortInfo(column, sortAscending ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending);
                                sortInfos.Add(colSortInfo);
                                groupColumnCount++;
                            }
                        }
                    }


                    if (!string.IsNullOrWhiteSpace(orderBy))
                    {
                        var orders = Utils.SplitString(orderBy, ',');

                        foreach (var order in orders)
                        {
                            if (string.IsNullOrWhiteSpace(order))
                                continue;

                            var sortAscending = true;
                            var colName       = order;
                            if (Regex.IsMatch(colName, reSortDesc))
                            {
                                colName = Regex.Replace(colName, reSortDesc, string.Empty);
                                sortAscending = false;
                            }

                            var column = gridView.Columns.ColumnByFieldName(colName);
                            if (column != null)
                            {
                                var colSortInfo = new GridColumnSortInfo(column, sortAscending ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending);
                                sortInfos.Add(colSortInfo);
                            }
                        }
                    }

                    gridView.SortInfo.ClearAndAddRange(sortInfos.ToArray(), groupColumnCount);
                }
            }


            static UnboundColumnType ConvertUnboudColumnType(ComputedColumn.ComputedColumnType returnType)
            {
                return returnType switch
                {
                    ComputedColumn.ComputedColumnType.String   => UnboundColumnType.String,
                    ComputedColumn.ComputedColumnType.Integer  => UnboundColumnType.Integer,
                    ComputedColumn.ComputedColumnType.Decimal  => UnboundColumnType.Decimal,
                    ComputedColumn.ComputedColumnType.DateTime => UnboundColumnType.DateTime,
                    ComputedColumn.ComputedColumnType.Boolean  => UnboundColumnType.Boolean,
                    _                                          => UnboundColumnType.String
                };
            }
        }

        public override void ClearFind()
        {
            viewTable.ApplyFindFilter(null);
        }

        public override void Find(string value)
        {
            viewTable.ApplyFindFilter(value);
        }

        public void LoadFile(string fileName) =>
            LoadFile(fileName, null, null);

        public override async void LoadFile(string fileName, Dictionary<string, string> parameters, List<BaseCommand> commands)
        {
            DataSource             = null;
            GridControl.DataSource = null;
            viewTable.Columns.Clear();

            SpreadDataSource dataSource = null;

            try
            {
                await Task.Run(() =>
                {
                    var ext = Path.GetExtension(fileName)?.ToLower();

                    var options = new CsvSourceOptions()
                    {
                        DetectEncoding = true,
                        DetectNewlineType = true,
                        DetectValueSeparator = true,
                        TrimBlanks = true,
                        UseFirstRowAsHeader = true,
                        ValueSeparator = ext == ".txt" ? '\t' : ','
                    };

                    dataSource = new SpreadDataSource()
                    {
                        SourceOptions = options,
                        StreamDocumentFormat = ExcelDocumentFormat.Csv
                    };

                    using (var stream = new FileStream(fileName, FileMode.Open))
                    {
                        dataSource.Stream = stream;
                        dataSource.Fill();
                    }

                    viewTable.UpdateViewColumns();
                }).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            AttachDataSource(dataSource);

            if (commands != null && commands.Count > 0)
                ApplyGridFormatRules(viewTable, commands);
            else
            {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP05 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                var fileFormat = $"{fileName}.scgrid";  //Add .scgrid extension to existing one, i.e. produce .csv.scgrid
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP05 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
                if (File.Exists(fileFormat))
                    LoadGridData(fileFormat);
            }
        }

        //Save only formatting, do not change data file (.csv or .txt)
        public void SaveFile(string fileName)
        {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP05 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
            var fileFormat = $"{fileName}.scgrid";	//Add .scgrid extension to existing one, i.e. produce .csv.scgrid
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP05 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
            SaveGridData(fileFormat);
        }

        public override void Print()
        {
            GridControl.Print();
        }

        public void PrintPreview()
        {
            GridControl.ShowRibbonPrintPreview();
        }

        public override void Zoom100()
        {
            Zoom(null);
        }

        public override void ZoomIn()
        {
            Zoom(-1);
        }

        public override void ZoomOut()
        {
            Zoom(+1);
        }

        public override bool SupportDataSource => true;
        public override bool SupportFind       => true;
        public override bool SupportPrint      => true;
        public override bool SupportZoom       => false;

        private void ViewTable_CustomColumnGroup(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            ViewTable_CustomColumnSort(sender, e);
        }

        private void ViewTable_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Value1 is string && e.Value2 is string)
            {
                e.Result  = StringLogicalComparer.Compare(Convert.ToString(e.Value1), Convert.ToString(e.Value2));
                e.Handled = true;
            }
        }

        public void ExportGridToSpreadsheet(Stream stream)
        {
            var options = new XlsxExportOptionsEx()
            {
                ExportMode                                   = XlsxExportMode.SingleFile,
                RawDataMode                                  = false,
                SheetName                                    = "Table",
                ShowGridLines                                = true,
                TextExportMode                               = TextExportMode.Value,

                ExportType                                   = ExportType.DataAware,

                AllowBandHeaderCellMerge                     = DefaultBoolean.False,
                AllowCellMerge                               = DefaultBoolean.False,
                AllowCombinedBandAndColumnHeaderCellMerge    = DefaultBoolean.False,
                AllowConditionalFormatting                   = DefaultBoolean.True,
                AllowFixedColumnHeaderPanel                  = DefaultBoolean.True,
                AllowFixedColumns                            = DefaultBoolean.True,
                AllowGrouping                                = DefaultBoolean.True,
                AllowHyperLinks                              = DefaultBoolean.True,
                AllowLookupValues                            = DefaultBoolean.True,
                AllowSortingAndFiltering                     = DefaultBoolean.True,
                AllowSparklines                              = DefaultBoolean.True,
                ApplyFormattingToEntireColumn                = DefaultBoolean.True,    //?
                AutoCalcConditionalFormattingIconSetMinValue = DefaultBoolean.True,
                BandedLayoutMode                             = BandedLayoutMode.LinearBandsAndColumns,
                CalcTotalSummaryOnCompositeRange             = true,
                DocumentCulture                              = CultureInfo.CurrentCulture,
                GroupState                                   = GroupState.CollapseAll,
                LayoutMode                                   = LayoutMode.Table,  //customize
                ShowBandHeaders                              = DefaultBoolean.True,
                ShowColumnHeaders                            = DefaultBoolean.True,
                ShowGroupSummaries                           = DefaultBoolean.True,
                ShowPageTitle                                = DefaultBoolean.True,
                ShowTotalSummaries                           = DefaultBoolean.True,
                SummaryCountBlankCells                       = false, //customize
                SuppressEmptyStrings                         = true,
                UnboundExpressionExportMode                  = UnboundExpressionExportMode.AsFormula    //customize
            };

            options.DocumentOptions.Application = Parameters.ApplicationName;
            options.DocumentOptions.Author = Environment.UserDomainName ?? Environment.UserName;

            viewTable.ExportToXlsx(stream, options);
        }

        public void CopyData(bool useFormatting, bool copyHeaders)
        {
            viewTable.OptionsClipboard.ClipboardMode     = useFormatting ? ClipboardMode.Formatted : ClipboardMode.PlainText;
            viewTable.OptionsClipboard.CopyColumnHeaders = copyHeaders ? DefaultBoolean.True : DefaultBoolean.False;

            try
            {
                viewTable.CopyToClipboard();
            }
            finally
            {
                viewTable.OptionsClipboard.ClipboardMode     = ClipboardMode.Formatted;
                viewTable.OptionsClipboard.CopyColumnHeaders = DefaultBoolean.True;
            }
        }

        public void ShowFormatRulesManager()
        {
            if (viewTable.Columns.Count > 0)
                viewTable.Columns[0].ShowFormatRulesManager();
        }
        
        public void ShowComputedColumnsEditor()
        {
            using var frm = new UnboundExpressionEditor(viewTable);
            frm.ShowDialog(this);
        }

        private void BarCopyWithHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(true, true);
        }

        private void BarCopyWithoutHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(true, false);
        }

        private void BarCopyPlainWithHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(false, true);
        }

        private void BarCopyPlainWithoutHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(false, false);
        }

        private void ViewTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && Control.ModifierKeys == Keys.None)
            {
                var hitInfo = viewTable.CalcHitInfo(e.X, e.Y);
                if (hitInfo.InDataRow)
                    popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        private void BarMenuFormatting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowFormatRulesManager();
        }

        private static string GetBlobDisplayText(byte[] value)
        {
            string mimeType = WinAPI.FindMimeFromData((byte[])value);
            return mimeType != null ? $"<BLOB>: {mimeType}" : "<BLOB>";
        }

        private void ViewTable_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (!viewTable.IsValidRowHandle(e.RowHandle) || e.RowHandle < 0)
                return;

            if (e.CellValue == null || e.CellValue == DBNull.Value)
            {
                e.Appearance.ForeColor = AreColorsNear(viewTable.Appearance.Row.ForeColor, Color.Gray) ? Color.LightGray : Color.Gray;
                e.DisplayText = "<NULL>";
            }
            else if (e.CellValue is byte[] v)
            {
                e.Appearance.ForeColor = AreColorsNear(viewTable.Appearance.Row.ForeColor, Color.Gray) ? Color.LightGray : Color.Gray;
                e.DisplayText = GetBlobDisplayText(v);
            }


            static bool AreColorsNear(Color color1, Color color2)
            {
                return (Math.Abs(color1.B - color2.B) + Math.Abs(color1.G - color2.G) + Math.Abs(color1.R - color2.R)) < 10;
            }
        }

        private void ViewTable_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.CellValue == null || e.CellValue == DBNull.Value)
                return;

            if (e.CellValue is byte[])
                e.RepositoryItem = repositoryItemBlobEditor;
            else if (e.CellValue is string)
                e.RepositoryItem = repositoryItemStringEditor;
            else if (e.CellValue is DateTime)
                e.RepositoryItem = repositoryItemDateEditor;
        }

        private void ViewTable_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (!(sender is GridView view))
                return;

            Type columnType = view.FocusedColumn?.ColumnType;
            if (columnType == typeof(byte[]) || columnType == typeof(string))
            {
            }
            else
                e.Cancel = true;
        }

        private void RepositoryItemBlobEditor_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!(sender is ButtonEdit))
                return;

            object data = ((ButtonEdit)sender).EditValue;
            if (data != null && data != DBNull.Value && data is byte[] v)
            {
                using var editor = new BlobEditor(v);
                editor.ShowDialog(this);
            }
        }

        private void RepositoryItemBlobEditor_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.Value != null && e.Value != DBNull.Value && e.Value is byte[] v)
                e.DisplayText = GetBlobDisplayText(v);
            else
                e.DisplayText = GetBlobDisplayText(null);
        }

        private void RepositoryItemBlobEditor_FormatEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        {
            if (e.Value != null && e.Value != DBNull.Value && e.Value is byte[] v)
                e.Value = GetBlobDisplayText(v);
            else
                e.Value = GetBlobDisplayText(null);
        }

        private void RepositoryItemStringEditor_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            object value = viewTable.EditingValue;
            string str = value != null ? value.ToString() : string.Empty;

            using var memo = new MemoEditor()
            {
                Caption = "String value",
                MemoText = str
            };
            memo.ShowDialog(this);
        }

        private void RepositoryItemStringEditor_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            e.DisplayText = e.Value != null ? e.Value.ToString() : string.Empty;
        }

        private void ViewTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Control.IsKeyLocked(Keys.Scroll))
                return;

            var view = sender as GridView;
            switch (e.KeyData)
            {
                case Keys.Left:
                    view.LeftCoord -= 20;
                    e.Handled = true;
                    break;
                case Keys.Right:
                    view.LeftCoord += 20;
                    e.Handled = true;
                    break;
                case Keys.Down:
                    view.TopRowIndex += 1;
                    e.Handled = true;
                    break;
                case Keys.Up:
                    view.TopRowIndex -= 1;
                    e.Handled = true;
                    break;
            }
        }

        private void ViewTable_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys.HasFlag(Keys.Shift))
            {
                (sender as GridView).LeftCoord -= e.Delta;
                ((DXMouseEventArgs)e).Handled = true;
            }
        }

        private void GridTable_ViewRegistered(object sender, ViewOperationEventArgs e)
        {
            if (e.View.DataSource is DataView detailView && e.View is GridView gridView)
                InitializeDataTable(gridView, detailView.Table);
        }
    }
}
