using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsCommon.Get, "SCHost")]
    public class GetSCHostCmdlet: SCCmdlet
    {
        protected override void EndProcessing()
        {
            var host = CheckExternalHost();
            WriteObject(host.SCHost);
        }
    }
}
