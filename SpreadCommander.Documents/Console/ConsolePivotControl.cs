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
using SpreadCommander.Documents.Code;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraPivotGrid;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.ViewModels;
using System.IO;
using DevExpress.Utils;
using DevExpress.Utils.Animation;
using SpreadCommander.Common;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsolePivotControl : ConsoleCustomControl, IRibbonHolder, PivotDocumentViewModel.IPivotCallback
    {
        public ConsolePivotControl()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);
        }

        public override string Caption                                => "Pivot";
        public override DevExpress.Utils.Svg.SvgImage CaptionSvgImage => images["pivot"];

        private PivotDocumentViewModel PivotViewModel => (PivotDocumentViewModel)ViewModel;

        private void ConsolePivotControl_Load(object sender, EventArgs e)
        {
            PivotGrid.FieldsCustomization(navPageCustomization);
        }

        private void GalleryDataSources_ItemClick(object sender, GalleryItemClickEventArgs e)
        {
            if (e.Item.Tag is not DataTable table)
                return;

            ActiveTable = table;
            TableName   = table.TableName;
        }

        protected override void ViewModelChanged()
        {
            var model = PivotViewModel;
            model.PivotCallback = this;
            model.SetPivotDataSource(PivotGrid);
        }

        protected override void DataSetChanged(DataSet oldDataSet, DataSet newDataSet)
        {
            base.DataSetChanged(oldDataSet, newDataSet);

            if (oldDataSet != null)
                oldDataSet.Tables.CollectionChanged -= DataSetTables_CollectionChanged;

            if (newDataSet != null)
                newDataSet.Tables.CollectionChanged += DataSetTables_CollectionChanged;

            DataSetTables_CollectionChanged(newDataSet?.Tables, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
        }

        RibbonControl IRibbonHolder.Ribbon            => Ribbon;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => RibbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => Ribbon.Visible;
            set
            {
                Ribbon.Visible = value;
                RibbonStatusBar.Visible = value;
            }
        }

        private byte[] _PivotLayout;
        public void LoadPivot(Stream stream)
        {
            var initPosition = stream.Position;
            int len          = (int)(stream.Length - stream.Position);

            _PivotLayout = new byte[len];
            stream.Read(_PivotLayout, 0, len);

            stream.Position = initPosition;
            PivotGrid.RestoreLayoutFromStream(stream);
        }

        public void SavePivot(Stream stream)
        {
            PivotGrid.SaveLayoutToStream(stream);
        }

        public string TableName { get; set; }

        public DataTable ActiveTable
        {
            get => PivotGrid.DataSource as DataTable;
            set
            {
                if (PivotGrid.DataSource == value)
                    return;

                PivotGrid.DataSource = value;

                RetrieveFields();
                FireModified(true);
            }
        }

        private void RetrieveFields()
        {
            using var stream = new MemoryStream();
            if (PivotGrid.Fields.Count <= 0 && _PivotLayout != null)
                stream.Write(_PivotLayout, 0, _PivotLayout.Length);
            else
                SavePivot(stream);
            stream.Seek(0, SeekOrigin.Begin);

            PivotGrid.RetrieveFields();

            LoadPivot(stream);
        }

        public object CreateDrillDownDataSource() => PivotGrid.CreateDrillDownDataSource();
        public object CreateSummaryDataSource()   => PivotGrid.CreateSummaryDataSource();

        private void DataSetTables_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            var items = galleryDataSources.Gallery.Groups[0].Items;
            var table = (DataTable)e.Element;

            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (table == null)
                        return;

                    AddGalleryItem(table);

                    if (ActiveTable == null && string.Compare(table.TableName, TableName, true) == 0)
                    {
                        ActiveTable = table;
                        TableName = table.TableName;
                    }
                    break;
                case CollectionChangeAction.Remove:
                    int i = items.Count - 1;
                    while (i >= 0)
                    {
                        if (items[i].Tag == table)
                            items.RemoveAt(i);

                        i--;
                    }

                    if (ActiveTable == table)
                    {
                        ActiveTable = null;
                        TableName = null;
                    }
                    break;
                case CollectionChangeAction.Refresh:
                    ActiveTable = null;
                    items.Clear();

                    var dataSet = DataSet;
                    if (dataSet != null)
                    {
                        var tableName = TableName;

                        foreach (DataTable tbl in dataSet.Tables)
                            AddGalleryItem(tbl);

                        if (!string.IsNullOrWhiteSpace(tableName))
                        {
                            var dataTable = dataSet.Tables[tableName];
                            {
                                ActiveTable = dataTable;
                                TableName   = tableName;
                            }
                        }
                    }
                    break;
            }

            FireModified(true);


            void AddGalleryItem(DataTable tbl)
            {
                if (tbl == null)
                    return;

                var galleryItem = new GalleryItem()
                {
                    Caption = tbl.TableName,
                    Tag     = tbl
                };
                galleryItem.ImageOptions.SvgImage = images32["table"];
                items.Add(galleryItem);

                if (ActiveTable == null && TableName == null)
                {
                    ActiveTable = tbl;
                    TableName   = tbl.TableName;
                }
            }
        }

        private void BarLoadTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dlgOpen.InitialDirectory))
                dlgOpen.InitialDirectory = Project.Current.ProjectPath;

            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return;

            PivotViewModel.LoadPivot(dlgOpen.FileName);
        }

        private void BarSaveTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dlgSave.InitialDirectory))
                dlgSave.InitialDirectory = Project.Current.ProjectPath;

            if (dlgSave.ShowDialog(this) != DialogResult.OK)
                return;

            PivotViewModel.SavePivot(dlgSave.FileName);
        }

        private void BarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PivotGrid.Print();
        }

        private void BarPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PivotGrid.ShowRibbonPrintPreview();
        }

        private void BarFormatConditions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var field = PivotGrid.GetFieldByArea(PivotArea.DataArea, 0);
            if (field != null)
                field.ShowFormatRulesManager();
        }

        private void PivotGrid_CustomFieldSort(object sender, PivotGridCustomFieldSortEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Value1 is string && e.Value2 is string)
            {
                e.Result  = StringLogicalComparer.Compare(Convert.ToString(e.Value1), Convert.ToString(e.Value2));
                e.Handled = true;
            }
        }

        private void BarCustomization_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (PivotGrid.CustomizationForm?.Visible ?? false)
                PivotGrid.HideCustomization();
            else
                PivotGrid.ShowCustomization();
        }

        private void BarExportSummary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (new UsingProcessor(
                () => transitionManager.StartWaitingIndicator(PivotGrid, WaitingAnimatorType.Default),
                () => transitionManager.StopWaitingIndicator()))
            {
                var model = (PivotDocumentViewModel)ViewModel;

                using var dataSource = PivotGrid.CreateSummaryDataSource();
                model.ExportToSpreadsheet(dataSource);
            }
        }

        private void BarExportDrillDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*
            var selectedCell = PivotGrid.Cells.FocusedCell;
            if (selectedCell == null || selectedCell.X < 0 || selectedCell.Y < 0)
            {
                XtraMessageBox.Show(this, "Please select cell in pivot grid.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var model = (PivotDocumentViewModel)ViewModel;
            using (var dataSource = PivotGrid.CreateDrillDownDataSource(selectedCell.X, selectedCell.Y))
                model.ExportToSpreadsheet(dataSource);
            */

            var table = ActiveTable;
            if (table == null)
            {
                XtraMessageBox.Show(this, "Please select source table.", "No source table", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (new UsingProcessor(
                () => transitionManager.StartWaitingIndicator(PivotGrid, WaitingAnimatorType.Default),
                () => transitionManager.StopWaitingIndicator()))
            {
                var drillDownIndexes = new Dictionary<int, int>();
                foreach (var cell in PivotGrid.Cells.MultiSelection.SelectedCells)
                {
                    var dataSource = PivotGrid.CreateDrillDownDataSource(cell.X, cell.Y);
                    for (int i = 0; i < dataSource.RowCount; i++)
                    {
                        var index = dataSource[i].ListSourceRowIndex;
                        if (drillDownIndexes.ContainsKey(index))
                            drillDownIndexes[index]++;
                        else
                            drillDownIndexes.Add(index, 1);
                    }
                }

                var indexes = drillDownIndexes.Keys.ToList();
                indexes.Sort();

                var rows = new List<DataRow>();
                foreach (var index in indexes)
                    rows.Add(table.Rows[index]);

                DataTable tblRows;
                if (rows.Count > 0)
                    tblRows = rows.AsEnumerable().CopyToDataTable();
                else
                    tblRows = table.Clone();    //Does not copy data

                var model = (PivotDocumentViewModel)ViewModel;
                model.ExportToSpreadsheet(tblRows);
            }
        }

        private void BarRetrieveFields_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RetrieveFields();
            FireModified(true);
        }

        private void PivotGrid_GridLayout(object sender, EventArgs e)
        {
            FireModified(true);
        }

        private void BarLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barLayout.Down)
            {
                PivotGrid.OptionsView.RowTotalsLocation       = PivotRowTotalsLocation.Tree;
                PivotGrid.OptionsBehavior.HorizontalScrolling = PivotGridScrolling.CellsArea;
            }
            else
            {
                PivotGrid.OptionsView.RowTotalsLocation       = PivotRowTotalsLocation.Far;
                PivotGrid.OptionsBehavior.HorizontalScrolling = PivotGridScrolling.Control;
            }
        }
    }
}
