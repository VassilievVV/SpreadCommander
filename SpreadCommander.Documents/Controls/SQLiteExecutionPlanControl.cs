using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Common;
using DevExpress.XtraCharts;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using System.Drawing.Drawing2D;
using DevExpress.Utils.Menu;
using DevExpress.Utils;
using System.Reflection;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common.Code;
using DevExpress.XtraTreeList.Extension;

namespace SpreadCommander.Documents.Controls
{
    public partial class SQLiteExecutionPlanControl: DevExpress.XtraEditors.XtraUserControl
    {
        private DataTable			_ExecutionPlanTable;
        private bool				_Updating;

        public SQLiteExecutionPlanControl()
        {
            InitializeComponent();
        }

        private void SQLiteExecutionPlanControl_Load(object sender, EventArgs e)
        {
            if (Parent is XtraDialogForm frm)
                frm.MakeXtraDialogFormResizeableWithDelay();
        }

        public void InitializeCommand(object state)
        {
            bindingExecutionPlan.DataSource = null;

            if (state == null || !(state is DataTable))
                return;

            _ExecutionPlanTable = (DataTable)state;
            bindingExecutionPlan.DataSource = _ExecutionPlanTable;

            if (_ExecutionPlanTable != null)
                treePlan.ExpandAll();
        }

        private void PmnuExpandAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            treePlan.ExpandAll();
        }

        private void PmnuCollapseAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            treePlan.CollapseAll();
        }

        private void PmnuTreeView_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_Updating)
                return;

            treePlan.ParentFieldName = pmnuTreeView.Down ? "parent" : null;
            treePlan.RefreshDataSource();
        }

        private void PmnuShowWholeStatements_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_Updating)
                return;

            treePlan.OptionsView.AutoCalcPreviewLineCount = pmnuShowWholeStatements.Down;
        }

        private void PopupGridPlan_BeforePopup(object sender, CancelEventArgs e)
        {
            using (new UsingProcessor(() => _Updating = true, () => _Updating = false))
            {
                pmnuTreeView.Down            = !string.IsNullOrEmpty(treePlan.ParentFieldName);
                pmnuShowWholeStatements.Down = treePlan.OptionsView.AutoCalcPreviewLineCount;
            }
        }

        private void TreePlan_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                popupGridPlan.ShowPopup(Control.MousePosition);
        }

        private void PmnuManageFormatRules_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (treePlan.Columns.Count <= 0)
                return;

            treePlan.Columns[0].ShowFormatRulesManager();
        }
    }
}