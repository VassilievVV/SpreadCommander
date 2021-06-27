using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Messages
{
    public class SelectDataSetTableMessage: BaseMessage
    {
        public DataSet DataSet { get; set; }

        public DataTable SelectedDataTable { get; set; }
    }
}
