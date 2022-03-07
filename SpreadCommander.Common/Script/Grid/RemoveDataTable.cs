using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Grid
{
    public partial class SCGrid
    {
        public void RemoveDataTable(string tableName) =>
            ExecuteSynchronized(() => DoRemoveDataTable(tableName));

        protected virtual void DoRemoveDataTable(string tableName)
        {
            var dataSet = DataSet;

            var table = dataSet.Tables[tableName];
            if (table != null)
            {
                for (int i = dataSet.Relations.Count - 1; i >= 0; i--)
                {
                    var relation = dataSet.Relations[i];
                    if (relation.ParentTable == table || relation.ChildTable == table)
                        dataSet.Relations.RemoveAt(i);
                }

                dataSet.Tables.Remove(table);
            }
        }
    }
}
