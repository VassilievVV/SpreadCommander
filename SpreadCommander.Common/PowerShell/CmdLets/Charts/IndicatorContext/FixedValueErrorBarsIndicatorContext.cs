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
	public class FixedValueErrorBarsIndicatorContext: ErrorBarsIndicatorContext
	{
		[Parameter(HelpMessage = "Fixed positive error value.")]
		[PSDefaultValue(Value = 1.0)]
		[DefaultValue(1.0)]
		public double PositiveError { get; set; } = 1.0;

		[Parameter(HelpMessage = "Fixed negative error value.")]
		[PSDefaultValue(Value = 1.0)]
		[DefaultValue(1.0)]
		public double NegativeError { get; set; } = 1.0;


		public override Indicator CreateIndicator()
		{
			return new FixedValueErrorBars();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is FixedValueErrorBars fixedValueErrorBars)
			{
				fixedValueErrorBars.PositiveError = PositiveError;
				fixedValueErrorBars.NegativeError = NegativeError;
			}
		}
	}
}
