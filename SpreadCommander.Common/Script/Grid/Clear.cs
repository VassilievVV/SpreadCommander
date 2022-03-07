using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Grid
{
    public partial class SCGrid
    {
        public void Clear() =>
            ExecuteSynchronized(() => DoClear());

        protected virtual void DoClear()
        {
            DataSet.Clear();
        }
    }
}
