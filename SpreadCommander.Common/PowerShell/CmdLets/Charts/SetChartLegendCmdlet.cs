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
    [Cmdlet(VerbsCommon.Set, "ChartLegend")]
    public class SetChartLegendCmdlet: BaseChartLegendCmdlet
    {
        [Parameter(HelpMessage = "Legend's name.")]
        public string Name { get; set; }


        protected override void UpdateChart()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var legend = ChartContext.Chart.Legends[Name];
                if (legend == null)
                    throw new Exception($"Cannot find legend '{Name}'.");

                SetupXtraChartLegend(legend);
            }
            else
                SetupXtraChartLegend(ChartContext.Chart.Legend);
        }
    }
}
