using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	[Cmdlet(VerbsCommon.Add, "ChartScaleBreak")]
	public class AddChartScaleBreakCmdlet: BaseChartWithContextCmdlet
	{
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Axis type - X or Y.")]
		public ChartAxisType AxisType { get; set; }

		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Name of the axis.")]
		public string AxisName { get; set; }

		[Parameter(Mandatory = true, Position = 2, HelpMessage = "Name of custom axis label.")]
		public string Name { get; set; }

		[Parameter(Mandatory = true, Position = 3, HelpMessage = "First edge of a scale break.")]
		public object Edge1 { get; set; }

		[Parameter(Mandatory = true, Position = 4, HelpMessage = "Second edge of a scale break.")]
		public object Edge2 { get; set; }


		protected override void UpdateChart()
		{
			AxisBase axis;

			if (!string.IsNullOrWhiteSpace(AxisName))
			{
				axis = BaseAxisCmdlet.GetSecondaryAxis(ChartContext.Chart.Diagram, AxisType, AxisName);
				if (axis == null)
					throw new Exception($"Cannot find axis '{AxisName}'.");
			}
			else
			{
				axis = BaseAxisCmdlet.GetPrimaryAxis(ChartContext.Chart.Diagram, AxisType);
				if (axis == null)
					throw new Exception("Cannot find primary axis.");
			}

			if (axis is not Axis axis2D)
				throw new Exception("Only 2D axis except SwiftPlot support scale breaks.");

			var scaleBreak = new ScaleBreak();
			if (!string.IsNullOrWhiteSpace(Name))
				scaleBreak.Name = Name;

			scaleBreak.Edge1 = Edge1;
			scaleBreak.Edge2 = Edge2;

			scaleBreak.Visible = true;

			axis2D.ScaleBreaks.Add(scaleBreak);
		}
	}
}
