using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public partial class ScriptHost
    {
#pragma warning disable CA1822 // Mark members as static
        public void GCCollect()
#pragma warning restore CA1822 // Mark members as static
        {
            GC.Collect();
        }
    }
}
