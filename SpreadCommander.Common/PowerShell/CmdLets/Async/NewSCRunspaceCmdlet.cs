using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Async
{
    [Cmdlet(VerbsCommon.New, "SCRunspace")]
    [OutputType(typeof(Runspace))]
    public class NewSCRunspaceCmdlet: SCCmdlet
    {
        [Parameter(HelpMessage = "Return closed runspace. By default returned runspace is open.")]
        public SwitchParameter Closed { get; set; }


        protected override void EndProcessing()
        {
            var result = CheckExternalHost().Engine.CreateHostedRunspace(!Closed);
            WriteObject(result);
        }
    }
}
