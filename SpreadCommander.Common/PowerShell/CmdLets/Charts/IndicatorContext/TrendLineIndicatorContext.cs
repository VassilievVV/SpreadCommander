using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class TrendLineIndicatorContext: FinancialIndicatorContext
	{
		[Parameter(HelpMessage = "Whether the Trend Line is extrapolated to infinity.")]
		public SwitchParameter DoNotExtrapolateToInfinity { get; set; }


		public override Indicator CreateIndicator()
		{
			return new TrendLine();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is TrendLine trendLine)
			{
				trendLine.ExtrapolateToInfinity = !DoNotExtrapolateToInfinity;
			}
		}
	}
}
