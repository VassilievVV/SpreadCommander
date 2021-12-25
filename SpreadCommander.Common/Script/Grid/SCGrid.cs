using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Grid
{
    public partial class SCGrid: ScriptHostObject
    {
        protected internal DataSet DataSet { get; }

        public SCGrid(ScriptHost host) : base(host)
        {
            DataSet = host.Engine.GridDataSet;
        }
    }
}
