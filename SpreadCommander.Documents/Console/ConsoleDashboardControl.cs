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
using SpreadCommander.Documents.ViewModels;
using DevExpress.DashboardCommon;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleDashboardControl : ConsoleCustomControl, IRibbonHolder, DashboardDocumentViewModel.IDashboardCallback
    {
        public ConsoleDashboardControl()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);

            this.DashboardDesigner.Initialized += DashboardDesigner_Initialized;
        }

        public override string Caption => "Dash";
        public override DevExpress.Utils.Svg.SvgImage CaptionSvgImage => images["dash"];

        RibbonControl IRibbonHolder.Ribbon => Ribbon;
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

        protected DashboardDocumentViewModel DashboardViewModel => (DashboardDocumentViewModel)ViewModel;

        public void DashboardModified()
        {
            FireModified(true);
        }

        public void ReloadData()
        {
            if (_DesignerInitialized)
                DashboardDesigner.ReloadData();
        }

        public Dashboard Dashboard => DashboardDesigner.Dashboard;

        protected override void ViewModelChanged()
        {
            var model = DashboardViewModel;
            if (model != null)
                model.DashboardCallback = this;
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

        private void DataSetTables_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            var table = (DataTable)e.Element;
            var dashboard = DashboardDesigner.Dashboard;

            var dictItemDataSources = new StringNoCaseDictionary<List<DataDashboardItem>>();
            StoreItemDataSources();

            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (table == null)
                        return;

                    var existingDataSources = dashboard.DataSources.Where(ds => (string.Compare(ds.Name, table.TableName, true) == 0)).ToList();
                    foreach (var dsRemove in existingDataSources)
                        RemoveDataSoure(dsRemove);

                    var objDataSource = new DashboardObjectDataSource(table.TableName, table);
                    dashboard.DataSources.Add(objDataSource);
                    break;
                case CollectionChangeAction.Remove:
                    var dataSources = dashboard.DataSources.Where(ds => (ds.Data == table)).ToList();
                    foreach (var dsRemove in dataSources)
                        RemoveDataSoure(dsRemove);
                    break;
                case CollectionChangeAction.Refresh:
                    var existingDataSources2 = dashboard.DataSources.ToList();
                    foreach (var dsRemove in existingDataSources2)
                        RemoveDataSoure(dsRemove);

                    var dataSet = DataSet;
                    if (dataSet != null)
                    {
                        foreach (DataTable tbl in dataSet.Tables)
                        {
                            var newDataSource = new DashboardObjectDataSource(tbl.TableName, tbl);
                            dashboard.DataSources.Add(newDataSource);
                        }
                    }
                    break;
            }

            ReloadItemDataSources();
            ReloadData();

            FireModified(true);
            
            
            void RemoveDataSoure(IDashboardDataSource dsRemove)
            {
                int dsItemCount = dashboard.Items.OfType<DataDashboardItem>().Where(i => string.Compare(i.DataSource?.Name, dsRemove.Name, true) == 0).Count();
                if (dsItemCount <= 0)   //Do not remove datasource if it has assigned items. Later it can be replaced with new datasource that has DataTable.
                    dashboard.DataSources.Remove(dsRemove);
                else
                    dsRemove.Data = null;
            }

            void StoreItemDataSources()
            {
                foreach (var item in dashboard.Items.OfType<DataDashboardItem>())
                {
                    var dataSourceName = item.DataSource?.Name;
                    if (!string.IsNullOrWhiteSpace(dataSourceName))
                    {
                        if (dictItemDataSources.ContainsKey(dataSourceName))
                        {
                            var listControls = dictItemDataSources[dataSourceName];
                            listControls.Add(item);
                        }
                        else
                        {
                            var listControls = new List<DataDashboardItem>
                            {
                                item
                            };
                            dictItemDataSources.Add(dataSourceName, listControls);
                        }
                    }
                }
            }
            
            void ReloadItemDataSources()
            {
                foreach (var keyValuePair in dictItemDataSources)
                {
                    var tableSource = DataSet.Tables[keyValuePair.Key];
                    if (tableSource == null)
                        continue;

                    var dataSource = dashboard.DataSources.Where(s => string.Compare(s.Name, tableSource.TableName, true) == 0).FirstOrDefault();
                    if (dataSource != null)
                    {
                        foreach (var item in keyValuePair.Value)
                        {
                            if (item.DataSource != dataSource)
                                item.DataSource = dataSource;
                        }
                    }
                }
            }
        }

        private void BarLoadTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return;

            DashboardViewModel.LoadDashboard(dlgOpen.FileName);
        }

        private void BarSaveTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgSave.ShowDialog(this) != DialogResult.OK)
                return;

            DashboardViewModel.SaveDashboard(dlgSave.FileName);
        }

        private void DashboardDesigner_DataLoading(object sender, DataLoadingEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.DataSourceName))
                return;

            var table = DataSet?.Tables[e.DataSourceName];
            if (table != null)
                e.Data = table;
        }

        private void DashboardDesigner_AsyncDataLoading(object sender, DataLoadingEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.DataSourceName))
                return;

            var table = DataSet?.Tables[e.DataSourceName];
            if (table != null)
                e.Data = table;
        }

        private bool _DesignerInitialized;
        private void DashboardDesigner_Initialized(object sender, EventArgs e)
        {
            _DesignerInitialized = true;
        }
    }
}
