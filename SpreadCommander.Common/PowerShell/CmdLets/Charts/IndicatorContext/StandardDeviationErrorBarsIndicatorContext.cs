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
	public class StandardDeviationErrorBarsIndicatorContext: ErrorBarsIndicatorContext
	{
		[Parameter(HelpMessage = "Multiplier on which the standard deviation value is multiplied before display.")]
		[PSDefaultValue(Value = 1.0)]
		[DefaultValue(1.0)]
		public double Multiplier { get; set; } = 1.0;


		public override Indicator CreateIndicator()
		{
			return new StandardDeviationErrorBars();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is StandardDeviationErrorBars standardDeviationErrorBars)
			{
				standardDeviationErrorBars.Multiplier = Multiplier;
			}
		}
	}
}
