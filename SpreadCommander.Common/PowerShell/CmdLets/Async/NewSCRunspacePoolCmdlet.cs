using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Async
{
    [Cmdlet(VerbsCommon.New, "SCRunspacePool")]
    [OutputType(typeof(RunspacePool))]
    public class NewSCRunspacePoolCmdlet: SCCmdlet
    {
        [Parameter(HelpMessage = "Number of script blocks that in parallel. Default is number of processor cores, up to 16.")]
        [ValidateRange(1, 256)]
        public int? ThrottleLimit { get; set; }

        [Parameter(HelpMessage = "Return closed runspace pool. By default returned runspace pool is open.")]
        public SwitchParameter Closed { get; set; }


        protected override void EndProcessing()
        {
            int maxRunspaceCount = Utils.ValueInRange(ThrottleLimit ?? Math.Max(Environment.ProcessorCount, 16), 1, 256);
            var result           = CheckExternalHost().Engine.CreateHostedRunspacePool(1, maxRunspaceCount, !Closed);
            WriteObject(result);
        }
    }
}
