using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class FinancialIndicatorContext: BaseIndicatorContext
	{
		[Parameter(Mandatory = true, HelpMessage = "Individual point for the indicators, that are drawn through two data points (such as Fibonacci Indicator or a Trend Line).")]
		public object Point1Argument { get; set; }

		[Parameter(HelpMessage = "Value indicating how to obtain the value of a financial indicator's point. Close, High, Lot, Open etc.")]
		public ValueLevel? Point1ValueLevel { get; set; }

		[Parameter(Mandatory = true, HelpMessage = "Individual point for the indicators, that are drawn through two data points (such as Fibonacci Indicator or a Trend Line).")]
		public object Point2Argument { get; set; }

		[Parameter(HelpMessage = "Value indicating how to obtain the value of a financial indicator's point. Close, High, Lot, Open etc.")]
		public ValueLevel? Point2ValueLevel { get; set; }


		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is FinancialIndicator financialIndicator)
			{
				financialIndicator.Point1.Argument = Point1Argument;
				if (Point1ValueLevel.HasValue)
					financialIndicator.Point1.ValueLevel = Point1ValueLevel.Value;
				financialIndicator.Point2.Argument = Point2Argument;
				if (Point2ValueLevel.HasValue)
					financialIndicator.Point2.ValueLevel = Point2ValueLevel.Value;
			}
		}
	}
}
