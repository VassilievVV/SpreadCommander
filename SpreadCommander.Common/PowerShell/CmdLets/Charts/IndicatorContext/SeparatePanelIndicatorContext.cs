using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class SeparatePanelIndicatorContext: BaseIndicatorContext
	{
		[Parameter(HelpMessage = "Y-axis that is used to plot the current indicator.")]
		public string AxisY { get; set; }

		[Parameter(Mandatory = true, HelpMessage = "Pane, used to plot the separate pane indicator")]
		public string Pane { get; set; }


		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is SeparatePaneIndicator separateIndicator)
			{
				if (!string.IsNullOrWhiteSpace(AxisY))
				{
					var axisY = (chartContext.Chart.Diagram as XYDiagram)?.FindAxisYByName(AxisY) ??
						throw new Exception($"Cannot find axis '{AxisY}' or indicator is not supported on this chart.");

					separateIndicator.AxisY = axisY;
				}

				if (!string.IsNullOrWhiteSpace(Pane))
				{
					var pane = (chartContext.Chart.Diagram as XYDiagram)?.Panes[Pane] ??
						throw new Exception($"Cannot find pane '{Pane}' or indicator is not supported on this chart.");

					separateIndicator.Pane = pane;
				}
			}
		}
	}
}
