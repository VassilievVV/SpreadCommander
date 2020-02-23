using DevExpress.Charts.Native;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	[Cmdlet(VerbsCommon.Add, "ChartAxis")]
	public class AddChartAxisCmdlet: BaseAxisCmdlet
	{
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Name of the axis.")]
		public string Name { get; set; }

		protected override AxisBase GetAxis()
		{
			var result = GetSecondaryAxis(ChartContext.Chart.Diagram, AxisType, Name);
			return result;
		}
	}
}
