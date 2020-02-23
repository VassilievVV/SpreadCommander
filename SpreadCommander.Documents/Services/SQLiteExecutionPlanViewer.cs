using DevExpress.XtraEditors;
using SpreadCommander.Documents.Controls;
using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class SQLiteExecutionPlanViewer : ISQLiteExecutionPlanViewer
    {
        public IWin32Window _Owner;

        public SQLiteExecutionPlanViewer()
        {
        }

        public SQLiteExecutionPlanViewer(IWin32Window owner) : this()
        {
            _Owner = owner;
        }

        public void ShowExecutionPlan(DataTable tablePlan)
        {
            var control = new SQLiteExecutionPlanControl();
            control.InitializeCommand(tablePlan);

            XtraDialog.Show(_Owner, control, "Execution plan", MessageBoxButtons.OK);
        }
    }
}
