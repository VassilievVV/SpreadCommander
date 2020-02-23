using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Charts.Native;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	[Cmdlet(VerbsCommon.Set, "ChartAxis")]
	public class SetChartAxisCmdlet: BaseAxisCmdlet
	{
		protected override AxisBase GetAxis()
		{
			var result = GetPrimaryAxis(ChartContext.Chart.Diagram, AxisType);
			return result;
		}
	}
}
