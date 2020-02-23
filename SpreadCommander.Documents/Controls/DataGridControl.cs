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
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid;
using SpreadCommander.Common;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using SpreadCommander.Common.SqlScript;
using System.Drawing.Drawing2D;
using DevExpress.Utils;
using SpreadCommander.Documents.Dialogs;
using DevExpress.XtraPrinting;
using System.IO;
using DevExpress.Export;
using System.Globalization;
using SpreadCommander.Common.Parsers.ConsoleScript;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Documents.Viewers;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using DevExpress.XtraGrid.Extension;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using DevExpress.Data;

namespace SpreadCommander.Documents.Controls
{
    public partial class DataGridControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region TableData
        public class TableData
        {
            public TableData(DataTable table)
            {
                DataTable       = table;
                FormatRules     = new List<GridFormatRule>();
                ComputedColumns = new List<ComputedColumn>();
            }

            public DataTable DataTable { get; }

            public string TableName
            {
                get => DataTable?.TableName;
                set { if (DataTable != null) DataTable.TableName = value; }
            }

            public string Description => DataTable != null ? $"{DataTable.Columns.Count:#,##0} columns x {DataTable.Rows.Count:#,##0} rows" : null;

            public List<GridFormatRule> FormatRules     { get; }
            
            public List<ComputedColumn> ComputedColumns { get; }
        }
        #endregion

        #region NavigateLineEventArgs
        public class NavigateLineEventArgs: EventArgs
        {
            public int Line { get; set; }
        }
        #endregion

        public EventHandler<NavigateLineEventArgs> NavigateLine;

        public DataGridControl()
        {
            InitializeComponent();

            navLeft.State = NavigationPaneState.Collapsed;
            GridUtils.InitializeGridView(viewTable);

            //Allow show editors
            viewTable.OptionsBehavior.Editable = true;
            viewTable.OptionsBehavior.ReadOnly = true;
        }

        public void PrintGrid()
        {
            gridTable.Print();
        }

        public void PrintPreviewGrid()
        {
            gridTable.ShowRibbonPrintPreview();
        }

        public ColumnView MainView => viewTable;
        
        public bool DisableTransitions { get; set; }

        private DataSet _DataSet;
        public DataSet DataSet
        {
            get => _DataSet;
            set
            {
                if (_DataSet != null)
                    _DataSet.Tables.CollectionChanged -= DataSetTables_CollectionChanged;
                _DataSet = value;
                if (_DataSet != null)
                    _DataSet.Tables.CollectionChanged += DataSetTables_CollectionChanged;

                bindingTables.Clear();
                foreach (DataTable table in _DataSet.Tables)
                    bindingTables.Add(new TableData(table));
            }
        }

        public DataTable ActiveTable
        {
            get => (bindingTables.Current as TableData)?.DataTable;
            set
            {
                for (int i = 0; i < bindingTables.Count; i++)
                {
                    var data = bindingTables[i] as TableData;
                    if (data?.DataTable == value)
                    {
                        bindingTables.Position = i;
                        break;
                    }
                }
            }
        }

        public bool ActivateNextTable { get; set; }

        private void DataSetTables_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    var data = new TableData((DataTable)e.Element);
                    bindingTables.Add(data);
                    if (ActivateNextTable)
                    {
                        ActiveTable = data.DataTable;
                        ActivateNextTable = false;
                    }
                    break;
                case CollectionChangeAction.Remove:
                    int i = bindingTables.Count - 1;
                    while (i >= 0)
                    {
                        if (bindingTables[i] == (DataTable)e.Element)
                            bindingTables.RemoveAt(i);

                        i--;
                    }
                    break;
                case CollectionChangeAction.Refresh:
                    bindingTables.Clear();
                    ActivateNextTable = false;
                    if (_DataSet != null)
                    {
                        foreach (DataTable table in _DataSet.Tables)
                        {
                            var dataTable = new TableData(table);
                            bindingTables.Add(dataTable);
                        }
                    }
                    break;
            }
        }

        public static void RegisterSplitContainer(GridSplitContainer container)
        {
            if (container == null)
                return;

            var grid = Utils.FindTypedParentControl<DataGridControl>(container);
            if (grid == null)
                return;

            container.SplitViewShown   += grid.DataGridSplitContainer_SplitViewShown;
            container.SplitViewCreated += grid.DataGridSplitContainer_SplitViewCreated;
            container.SplitViewHidden  += grid.DataGridSplitContainer_SplitViewHidden;
        }

        public static void UnregisterSplitContainer(GridSplitContainer container)
        {
            if (container == null)
                return;

            var grid = Utils.FindTypedParentControl<DataGridControl>(container);
            if (grid == null)
                return;

            container.SplitViewShown   -= grid.DataGridSplitContainer_SplitViewShown;
            container.SplitViewCreated -= grid.DataGridSplitContainer_SplitViewCreated;
            container.SplitViewHidden  -= grid.DataGridSplitContainer_SplitViewHidden;
        }

        private void DataGridSplitContainer_SplitViewCreated(object sender, EventArgs e)
        {
            if (!(sender is GridSplitContainer container))
                return;

            container.SplitChildGrid.UseEmbeddedNavigator = gridTable.UseEmbeddedNavigator;
        }

        private void DataGridSplitContainer_SplitViewHidden(object sender, EventArgs e)
        {
            if (sender is GridSplitContainer container && container.Grid == gridTable)
                gridTable.RemoveSplitContainer();
        }

        private void DataGridSplitContainer_SplitViewShown(object sender, EventArgs e)
        {
        }

        private void ViewTable_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Value1 is string && e.Value2 is string)
            {
                e.Result = StringLogicalComparer.Compare(Convert.ToString(e.Value1), Convert.ToString(e.Value2));
                e.Handled = true;
            }
        }

        private void ViewTable_CustomColumnGroup(object sender, CustomColumnSortEventArgs e)
        {
            ViewTable_CustomColumnSort(sender, e);
        }

        private void GridTable_ViewRegistered(object sender, ViewOperationEventArgs e)
        {
            if (e.View is ColumnView view)
            {
                view.UpdateViewColumns();
                view.CustomColumnSort  += ViewTable_CustomColumnSort;

                if (view is GridView gridView)
                {
                    gridView.CustomColumnGroup += ViewTable_CustomColumnGroup;
                    gridView.CustomDrawCell    += ViewTable_CustomDrawCell;
                    gridView.CustomRowCellEdit += ViewTable_CustomRowCellEdit;
                    gridView.ShowingEditor     += ViewTable_ShowingEditor;
                }
            }
        }

        private void GridTable_ViewRemoved(object sender, ViewOperationEventArgs e)
        {
            if (e.View is ColumnView view)
            {
                view.CustomColumnSort -= ViewTable_CustomColumnSort;

                if (view is GridView gridView)
                {
                    gridView.CustomColumnGroup -= ViewTable_CustomColumnGroup;
                    gridView.CustomDrawCell    -= ViewTable_CustomDrawCell;
                    gridView.CustomRowCellEdit -= ViewTable_CustomRowCellEdit;
                    gridView.ShowingEditor     -= ViewTable_ShowingEditor;
                }
            }
        }

        private void BindingTables_CurrentChanged(object sender, EventArgs e)
        {
            var disableTransitions = DisableTransitions;

            using (new UsingProcessor(
                () => { if (!disableTransitions) transitionManager.StartTransition(gridTable); gridTable.Enabled = false; },
                () => { gridTable.Enabled = true; if (!disableTransitions) transitionManager.EndTransition(); }))
            {
                gridTable.DataSource = null;
                viewTable.Columns.Clear();

                var tableData = (TableData)bindingTables.Current;
                var dataTable = tableData?.DataTable;
                gridTable.DataSource = dataTable;
                gridTable.RefreshDataSource();

                viewTable.FormatConditions.Clear();
                viewTable.FormatRules.Clear();
                viewTable.UpdateViewColumns();
                if (viewTable.OptionsView.BestFitMaxRowCount <= 0 || viewTable.OptionsView.BestFitMaxRowCount > 1000)
                    viewTable.BestFitMaxRowCount = 100;

                if (dataTable != null)
                {
                    viewTable.BestFitColumns();
                    viewTable.UpdateViewColumns();   //Reduce max width
                    viewTable.FormatRules.Clear();

                    if (tableData.FormatRules.Count <= 0)   //if rules were loaded and may be modified - use these rules
                    {
                        var formatConditions = new List<ConsoleCommands.BaseCommand>();

                        foreach (var propKey in dataTable.ExtendedProperties.Keys)
                        {
                            if (propKey is string && ((string)propKey).StartsWith("Format", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var propValue = dataTable.ExtendedProperties[propKey];
                                if (propValue != null)
                                {
                                    var propFormatConditions = SpreadCommander.Common.ScriptEngines.ConsoleCommands.FormatCondition.Parse(Convert.ToString(propValue), true);
                                    formatConditions.AddRange(propFormatConditions);
                                }
                            }
                        }

                        GridViewer.ApplyGridFormatRules(viewTable, formatConditions);
                        tableData.FormatRules.AddRange(viewTable.FormatRules);
                    }
                    else
                        viewTable.FormatRules.AddRange(tableData.FormatRules);

                    if (tableData.ComputedColumns.Count <= 0)
                    {
                        foreach (var propKey in dataTable.ExtendedProperties.Keys)
                        {
                            if (propKey is string && ((string)propKey).StartsWith("ComputedColumn", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var propValue = dataTable.ExtendedProperties[propKey];
                                if (propValue != null)
                                {
                                    var propComputedColumns = SpreadCommander.Common.ScriptEngines.ConsoleCommands.ComputedColumn.Parse(Convert.ToString(propValue), true);
                                    tableData.ComputedColumns.AddRange(propComputedColumns);
                                }
                            }
                        }
                    }
                    for (int i = viewTable.Columns.Count - 1; i >= 0; i --)
                    {
                        if (viewTable.Columns[i].UnboundType != UnboundColumnType.Bound)
                            viewTable.Columns.RemoveAt(i);
                    }
                    foreach (var computedColumn in tableData.ComputedColumns)
                    {
                        var caption = computedColumn.ColumnName;

                        var newColumn                       = viewTable.Columns.AddVisible(caption);
                        newColumn.Name                      = caption;
                        newColumn.Caption                   = caption;
                        newColumn.UnboundType               = ConvertUnboudColumnType(computedColumn.ReturnType);
                        newColumn.UnboundExpression         = computedColumn.Expression;
                        newColumn.ShowUnboundExpressionMenu = true;
                        newColumn.Visible                   = true;
                        newColumn.Width                     = 100;
                    }
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
                e.DisplayText          = "<NULL>";
            }
            else if (e.CellValue is byte[])
            {
                e.Appearance.ForeColor = AreColorsNear(viewTable.Appearance.Row.ForeColor, Color.Gray) ? Color.LightGray : Color.Gray;
                e.DisplayText          = GetBlobDisplayText((byte[])e.CellValue);
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
            if (data != null && data != DBNull.Value && data is byte[])
            {
                using var editor = new BlobEditor((byte[])data);
                editor.ShowDialog(this);
            }
        }

        private void RepositoryItemBlobEditor_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.Value != null && e.Value != DBNull.Value && e.Value is byte[])
                e.DisplayText = GetBlobDisplayText((byte[])e.Value);
            else
                e.DisplayText = GetBlobDisplayText(null);
        }

        private void RepositoryItemBlobEditor_FormatEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        {
            if (e.Value != null && e.Value != DBNull.Value && e.Value is byte[])
                e.Value = GetBlobDisplayText((byte[])e.Value);
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

        private void RepositoryItemStringEditor_FormatEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        {
            //Do nothing
        }

        public void ExportGridToSpreadsheet(Stream stream)
        {
            var tableData = (TableData)bindingTables.Current;

            var options = new XlsxExportOptionsEx()
            {
                ExportMode                                   = XlsxExportMode.SingleFile,
                RawDataMode                                  = false,
                SheetName                                    = tableData?.TableName ?? "Table",
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

        public GridData SaveGridData()
        {
            var result = GridViewer.SaveGridViewData(viewTable);
            return result;
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

        private void BarCopyWithHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(true, true);
        }

        private void BarCopyWithoutHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(true, false);
        }

        private void BarCopyPlainDataWithHeaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(false, true);
        }

        private void BarCopyPlainDataWithoutheaders_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CopyData(false, false);
        }

        private void ViewTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && Control.ModifierKeys == Keys.None)
            {
                var hitInfo = viewTable.CalcHitInfo(e.X, e.Y);
                if (hitInfo.InDataRow)
                    popupGrid.ShowPopup(Control.MousePosition);
            }
        }

        public void DeleteActiveTable()
        {
            var activeTable = ActiveTable;
            if (activeTable?.DataSet == null)
            {
                XtraMessageBox.Show(this, "Select data table to be deleted.", "No selected table", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (XtraMessageBox.Show(this, "Do you want to delete selected table?", "Confirm deleting table",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var dataSet = activeTable.DataSet;
            if (dataSet == null)
                return;

            for (int i = dataSet.Relations.Count - 1; i >= 0; i--)
            {
                var relation = dataSet.Relations[i];
                if (relation.ParentTable == activeTable || relation.ChildTable == activeTable)
                    dataSet.Relations.RemoveAt(i);
            }

            dataSet.Tables.Remove(activeTable);
        }

        public void ShowFormatRulesManager()
        {
            if (viewTable.Columns.Count > 0)
                viewTable.Columns[0].ShowFormatRulesManager();
        }

        private void BarMenuFormatting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowFormatRulesManager();
        }
    }
}
