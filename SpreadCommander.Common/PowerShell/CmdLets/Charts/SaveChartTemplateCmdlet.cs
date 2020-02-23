using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsData.Save, "ChartTemplate")]
    public class SaveChartTemplateCmdlet : BaseChartWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to save image to")]
        [Alias("fn")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output")]
        public SwitchParameter PassThru { get; set; }
        
        [Parameter(HelpMessage = "If set and file already exists - it will be overwritten")]
        [Alias("r")]
        public SwitchParameter Replace { get; set; }


        protected override bool PassThruChartContext => PassThru;


        protected override void UpdateChart()
        {
            WriteChartTemplate();
        }

        protected void WriteChartTemplate()
        {
            var chart = ChartContext?.Chart;
            if (chart == null)
                throw new Exception("Chart is not provided. Please use one of New-Chart cmdlets to create a chart.");

            if (string.IsNullOrWhiteSpace(FileName))
                throw new Exception("Filename for image is not specified.");


            var fileName = Project.Current.MapPath(FileName);
            var dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                throw new Exception($"Directory '{dir}' does not exist.");
            
            if (File.Exists(fileName))
            {
                if (Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{FileName}' already exists.");
            }

            lock (LockObject)
            {
                using var fileStream = new FileStream(fileName, FileMode.CreateNew);
                chart.SaveLayout(fileStream);
            }
        }
    }
}
