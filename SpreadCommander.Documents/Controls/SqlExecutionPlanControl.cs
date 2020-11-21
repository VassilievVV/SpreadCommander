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
    public partial class SqlExecutionPlanControl: DevExpress.XtraEditors.XtraUserControl
    {
        private DataTable			_ExecutionPlanTable;
        private bool				_Updating;

        public SqlExecutionPlanControl()
        {
            InitializeComponent();
        }

        private void SqlExecutionPlanControl_Load(object sender, EventArgs e)
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

        public void Print()
        {
            Printing.PrintControl(this, treePlan);
        }

        private void TreePlan_GetPreviewText(object sender, DevExpress.XtraTreeList.GetPreviewTextEventArgs e)
        {
            try
            {
                if (treePlan.GetDataRecordByNode(e.Node) is DataRowView row)
                {
                    if (row["TYPE"] as string != "PLAN_ROW")
                    {
                        if (treePlan.OptionsView.AutoCalcPreviewLineCount)
                            e.PreviewText = Utils.TrimString(row["StmtText"] as string);
                        else
                        {
                            string text = Utils.TrimString(row["StmtText"] as string);
                            if (!string.IsNullOrEmpty(text))
                            {
                                StringBuilder str = new StringBuilder(text);
                                str  = str.Replace('\r', ' ').Replace('\n', ' ');
                                text = str.ToString();
                            }
                            e.PreviewText = text;
                        }
                    }
                    else
                        e.PreviewText = $"{Utils.TrimString(row["LogicalOp"] as string)} ({Utils.TrimString(row["Argument"] as string)})";
                }
            }
            catch (Exception)
            {
            }
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

            treePlan.ParentFieldName = pmnuTreeView.Down ? "Parent" : null;
            treePlan.RefreshDataSource();
        }

        private void PmnuShowWholeStatements_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_Updating)
                return;

            treePlan.OptionsView.AutoCalcPreviewLineCount = pmnuShowWholeStatements.Down;
        }

        private void BarManageFormatRules_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (treePlan.Columns.Count <= 0)
                return;

            treePlan.Columns[0].ShowFormatRulesManager();
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
    }
}