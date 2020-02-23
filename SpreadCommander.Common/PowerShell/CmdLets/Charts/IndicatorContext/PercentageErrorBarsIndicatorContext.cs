using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class PercentageErrorBarsIndicatorContext: ErrorBarsIndicatorContext
	{
		[Parameter(HelpMessage = "Percentage of error values of series point values.")]
		[PSDefaultValue(Value = 5.0)]
		[DefaultValue(5.0)]
		public double Percent { get; set; } = 5.0;


		public override Indicator CreateIndicator()
		{
			return new PercentageErrorBars();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is PercentageErrorBars percentageErrorBars)
			{
				percentageErrorBars.Percent = Percent;
			}
		}
	}
}
