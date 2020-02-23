using DevExpress.Utils;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartLegend")]
    public class AddChartLegendCmdlet : BaseChartLegendCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Legend's name.")]
        public string Name { get; set; }


        protected override void UpdateChart()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new Exception("Legend name cannot be empty.");

            var legend = new Legend(Name);
            ChartContext.Chart.Legends.Add(legend);

            SetupXtraChartLegend(legend);
        }
    }
}
