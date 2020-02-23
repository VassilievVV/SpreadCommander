using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Heap
{
    [Cmdlet(VerbsData.Out, "FileViewer")]
    public class OutFileViewerCmdlet: SCCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of file to preview")]
        public string FileName { get; set; }


        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                throw new Exception("Filename is not provided.");

            var fileName = Project.Current.MapPath(FileName);
            if (!File.Exists(fileName))
                throw new Exception($"File '{FileName}' does not exist.");

            PreviewFile(fileName);
        }
    }
}
