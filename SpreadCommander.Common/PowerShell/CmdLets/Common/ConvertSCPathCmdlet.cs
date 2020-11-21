using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Common
{
    [Cmdlet(VerbsData.Convert, "SCPath")]
    public class ConvertSCPathCmdlet: SCCmdlet
    {
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, HelpMessage = "Paths to convert. '~' is project's path.")]
        public string[] Paths { get; set; }


        protected override void ProcessRecord()
        {
            if (Paths == null || Paths.Length <= 0)
                return;

            foreach (var path in Paths)
            {
                var newPath = Project.Current.MapPath(path);
                WriteObject(newPath);
            }
        }
    }
}
