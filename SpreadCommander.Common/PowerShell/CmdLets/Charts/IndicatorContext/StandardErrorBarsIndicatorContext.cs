using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class StandardErrorBarsIndicatorContext: ErrorBarsIndicatorContext
	{
		public override Indicator CreateIndicator()
		{
			return new StandardErrorBars();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);
		}
	}
}
