using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	[Cmdlet(VerbsCommon.Set, "ChartDefaultPane")]
	public class SetChartDefaultPaneCmdlet: BaseXYDiagramPaneCmdlet
	{
		protected override XYDiagramPaneBase GetPane()
		{
			return (ChartContext.Chart.Diagram as XYDiagram)?.DefaultPane;
		}
	}
}
