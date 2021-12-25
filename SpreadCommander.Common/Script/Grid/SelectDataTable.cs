using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Grid
{
    public partial class SCGrid
    {
        public SCGrid SelectDataTable(string tableName)
        {
            ExecuteSynchronized(() => DoSelectDataTable(tableName));
            return this;
        }

        protected virtual void DoSelectDataTable(string tableName)
        {
            var dataSet = DataSet;

            var table = dataSet.Tables[tableName];
            if (table != null)
                Messenger.Default.Send(new SelectDataSetTableMessage() { DataSet = dataSet, SelectedDataTable = table });
        }
    }
}
