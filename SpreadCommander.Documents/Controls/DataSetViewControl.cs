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
using SpreadCommander.Documents.Console;
using DevExpress.XtraBars.Ribbon;
using SpreadCommander.Documents.Code;
using DevExpress.XtraBars.Docking2010.Views;
using SpreadCommander.Documents.Viewers;
using System.IO;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Common.Spreadsheet;
using SpreadCommander.Common.Code;
using System.Data.Common;
using SpreadCommander.Documents.Services;
using System.Threading;
using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;

namespace SpreadCommander.Documents.Controls
{
    public partial class DataSetViewControl : DevExpress.XtraEditors.XtraUserControl, IRibbonHolder, IExportSource
    {
        public event EventHandler Modified;
        public event EventHandler<RibbonUpdateRequestEventArgs> RibbonUpdateRequest;

        public DataSetViewControl()
        {
            Utils.StartProfile("DataSetView");

            InitializeComponent();
            UIUtils.ConfigureRibbonBar(ribbonControl);

            HandleCreated += DataSetViewControl_HandleCreated;

            Messenger.Default.Register<SelectDataSetTableMessage>(this, OnSelectDataSetTableMessage);

            Disposed += (s, e) =>
            {
                Messenger.Default.Unregister<SelectDataSetTableMessage>(this);
            };
        }

        RibbonControl IRibbonHolder.Ribbon            => this.ribbonControl;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => this.ribbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => ribbonControl.Visible;
            set
            {
                ribbonControl.Visible   = value;
                ribbonStatusBar.Visible = value;
            }
        }

        private ISpreadsheetHolder _TargetSpreadsheet;
        public ISpreadsheetHolder TargetSpreadsheet
        {
            get => _TargetSpreadsheet;
            set
            {
                _TargetSpreadsheet = value;

                foreach (var document in viewTableViews.Documents)
                    ((TableViewControl)document.Control).TargetSpreadsheet = _TargetSpreadsheet;
            }
        }
        
        public DataSet DataSet { get; private set; }

        public BaseDocument ActiveDocument => viewTableViews.ActiveDocument;
        public TableViewControl ActiveGrid => ActiveDocument?.Control as TableViewControl;

        public void SetDataSet(DataSet dataSet)
        {
            var oldDataSet = DataSet;
            DataSet        = dataSet;
            DataSetChanged(oldDataSet, DataSet);
        }

        protected void DataSetChanged(DataSet oldDataSet, DataSet newDataSet)
        {
            if (oldDataSet == newDataSet)
            {
                if (newDataSet != null)
                    DataSetTables_CollectionChanged(newDataSet, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
                return;
            }

            if (oldDataSet != null)
            {
                oldDataSet.Tables.CollectionChanged    -= DataSetTables_CollectionChanged;
                oldDataSet.Relations.CollectionChanged -= DataSetRelations_CollectionChanged;
            }

            if (newDataSet != null)
            {
                newDataSet.Tables.CollectionChanged    += DataSetTables_CollectionChanged;
                newDataSet.Relations.CollectionChanged += DataSetRelations_CollectionChanged;
                
                DataSetTables_CollectionChanged(newDataSet, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
            }
        }

        private void EnableControls()
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                foreach (var doc in viewTableViews.Documents)
                    doc.Control.Enabled = true;
            }));
        }

        private void DataSetViewControl_HandleCreated(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void DataSetTables_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            var dataTable = (DataTable)e.Element;
            BaseDocument activeDocument = null;

            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (dataTable != null)
                        activeDocument = AddDocument(dataTable);
                    break;
                case CollectionChangeAction.Remove:
                    var docRemove = viewTableViews.Documents.FirstOrDefault(d => ((TableViewControl)d.Control).DataSource == dataTable);
                    var control = docRemove.Control;
                    if (control != null)
                        viewTableViews.RemoveDocument(control);
                    if (!control.IsDisposed)
                        control.Parent = null;
                    control.Dispose();
                    break;
                case CollectionChangeAction.Refresh:
                    var controls = new List<Control>(viewTableViews.Documents.Select(d => d.Control));
                    viewTableViews.Documents.Clear();
                    foreach (var ctrl in controls)
                    {
                        ctrl.Parent = null;
                        ctrl.Dispose();
                    }

                    var dataSet = DataSet;
                    if (dataSet == null)
                        return;

                    foreach (DataTable table in dataSet.Tables)
                        AddDocument(table);

                    if (controls.Count <= 0 && dataSet.Tables.Count <= 0)
                        return;
                    break;
            }

            FireModified();

            if (IsHandleCreated)
                EnableControls();

            if (activeDocument == null && viewTableViews.Documents.Count > 0)
                activeDocument = viewTableViews.Documents[0];

            if (activeDocument?.Control != null)
                viewTableViews.ActivateDocument(activeDocument.Control);
            RequestRibbonUpdate(activeDocument);
            

            BaseDocument AddDocument(DataTable tbl)
            {
                var control = new TableViewControl()
                {
                    DataSource        = tbl,
                    TargetSpreadsheet = this.TargetSpreadsheet, 
                    Enabled           = false   //Prevent auto-focus when TableViewControl will be shown (it auto-focuses Search edit box if it is visible)
                };
                
                var document                     = viewTableViews.AddDocument(control);
                document.Caption                 = tbl.TableName;
                document.ImageOptions.ImageIndex = 0;
                return document;
            }
        }

        private void DataSetRelations_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            var dataSet  = DataSet;
            var relation = (DataRelation)e.Element;

            if (dataSet == null)
                return;

            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (relation != null)
                    {
                        RefreshDataTable(relation.ParentTable);
                        RefreshDataTable(relation.ChildTable);
                    }
                    break;
                case CollectionChangeAction.Remove:
                    if (relation != null)
                    {
                        RefreshDataTable(relation.ParentTable);
                        RefreshDataTable(relation.ChildTable);
                    }
                    break;
                case CollectionChangeAction.Refresh:
                    if (dataSet.Tables.Count <= 0)
                        return; //Do not call FireModified(), nothing was updated.

                    foreach (DataTable dataTable in dataSet.Tables)
                        RefreshDataTable(dataTable);
                    break;
            }

            FireModified();


            void RefreshDataTable(DataTable dataTable)
            {
                if (dataTable == null)
                    return;

                foreach (var view in viewTableViews.Documents.Where(doc => (doc.Control as TableViewControl)?.DataSource == dataTable))
                {
                    ((TableViewControl)view.Control).RefreshDataSource();
                    //((TableViewControl)view.Control).DataSource = null;
                    //((TableViewControl)view.Control).DataSource = dataTable;
                }
            }
        }

        protected virtual void FireModified() =>
            Modified?.Invoke(this, new EventArgs());

        public GridData SaveGridData() =>
            ActiveGrid?.SaveGridData();

        public void ExportGridToSpreadsheet(Stream stream) =>
            ActiveGrid?.ExportGridToSpreadsheet(stream);

        public string[] GetTableNames() =>
            DataSet?.Tables.Cast<DataTable>().Select(t => t.TableName).ToArray();

        public DbDataReader GetDataTable(string tableName)
        {
            var dataSet = DataSet;

            if (dataSet == null)
                return null;

            var dataTable = dataSet.Tables[tableName];
            if (dataTable == null)
                return null;

            return new DataTableReader(dataTable);
        }

        public void ExportToDatabase()
        {
            var tableExporterService = ServiceContainer.Default.GetService<IExportTablesService>();
            tableExporterService.ExportDataTables(this);
        }

        private void RequestRibbonUpdate(BaseDocument document)
        {
            RibbonUpdateRequest?.Invoke(this, new RibbonUpdateRequestEventArgs() { RibbonHolder = document?.Control as IRibbonHolder, IsFloating = document?.IsFloating ?? false });
        }

        private void ViewTableViews_TabMouseActivating(object sender, DocumentCancelEventArgs e)
        {
        }

        private void ViewTableViews_DocumentActivated(object sender, DocumentEventArgs e)
        {
            RequestRibbonUpdate(e.Document);
        }

        private void ViewTableViews_DocumentDeactivated(object sender, DocumentEventArgs e)
        {
            RequestRibbonUpdate(null);

            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = e.Document?.IsFloating ?? false;
        }

        private void DocumentManager_DocumentActivate(object sender, DocumentEventArgs e)
        {
            RequestRibbonUpdate(e.Document);

            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = e.Document?.IsFloating ?? false;
        }

        private void ViewTableViews_Floating(object sender, DocumentEventArgs e)
        {
            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = true;
        }

        private void ViewTableViews_EndFloating(object sender, DocumentEventArgs e)
        {
            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = false;
        }

        private void OnSelectDataSetTableMessage(SelectDataSetTableMessage message)
        {
            if (message.DataSet == null || message.SelectedDataTable == null || message.DataSet != this.DataSet)
                return;

            var docSelect = viewTableViews.Documents.FirstOrDefault(d => ((TableViewControl)d.Control).DataSource == message.SelectedDataTable);
            if (docSelect?.Control != null)
                viewTableViews.ActivateDocument(docSelect.Control);
            RequestRibbonUpdate(docSelect);
        }
    }
}
