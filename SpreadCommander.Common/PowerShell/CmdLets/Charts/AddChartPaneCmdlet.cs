using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartPane")]
    public class AddChartPaneCmdlet : BaseXYDiagramPaneCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Pane's name.")]
        public string Name { get; set; }


        protected override XYDiagramPaneBase GetPane()
        {
            if (!(ChartContext.Chart.Diagram is XYDiagram diagram))
                return null;

            var pane = new XYDiagramPane(Name);
            diagram.Panes.Add(pane);

            return pane;
        }
    }
}
