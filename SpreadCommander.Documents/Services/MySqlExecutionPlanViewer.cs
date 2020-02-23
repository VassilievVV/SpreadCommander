using DevExpress.XtraEditors;
using SpreadCommander.Documents.Controls;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Documents.Viewers;
using SpreadCommander.Documents.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class MySqlExecutionPlanViewer : IMySqlExecutionPlanViewer
    {
        public IWin32Window _Owner;

        public MySqlExecutionPlanViewer()
        {
        }

        public MySqlExecutionPlanViewer(IWin32Window owner) : this()
        {
            _Owner = owner;
        }

        public void ShowExecutionPlan(DataTable tablePlan)
        {
            var control = new GridViewer();
            control.AttachDataSource(tablePlan);

            var colRows = control.Columns["rows"];
            if (colRows != null)
            {
                var ruleRows = control.FormatRules.AddDataBar(colRows);
                (ruleRows.Rule as FormatConditionRuleDataBar).PredefinedName = "Blue Gradient";
            }

            var colFiltered = control.Columns["filtered"];
            if (colFiltered != null)
            {
                var ruleFiltered = control.FormatRules.AddDataBar(colFiltered);
                (ruleFiltered.Rule as FormatConditionRuleDataBar).PredefinedName = "Blue Gradient";
            }

            var colExtra = control.Columns["Extra"];
            if (colExtra != null)
                control.Columns.Remove(colExtra);

            control.ColumnAutoWidth  = true;
            control.PreviewFieldName = "Extra";

            control.Load += (s, e) =>
            {
                var ctrl = s as Control;
                if (ctrl.Parent is XtraDialogForm frm)
                    frm.MakeXtraDialogFormResizeableWithDelay();
            };

            XtraDialog.Show(_Owner, control, "Execution plan", MessageBoxButtons.OK);
        }
    }
}
