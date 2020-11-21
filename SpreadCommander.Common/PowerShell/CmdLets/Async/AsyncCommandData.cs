using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Automation = System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SpreadCommander.Common.PowerShell.CmdLets.Async
{
    public class AsyncCommandData
    {
        internal Task<PSDataCollection<PSObject>> AsyncTask { get; set; }
        internal Automation.PowerShell PowerShell   { get; set; }
        internal bool Processed                     { get; set; }

        public Hashtable InputParameters            { get; set; }

        public PSDataCollection<PSObject> Output    { get; set; }

        public bool HadErrors                       { get; set; }

        public bool TimedOut                        { get; set; }
    }
}
