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
	public class SubsetBasedIndicatorContext: BaseIndicatorContext
	{
		[Parameter(HelpMessage = "Number of data points used to calculate of the subset-based indicator.")]
		[PSDefaultValue(Value = 10)]
		[DefaultValue(10)]
		public int PointsCount { get; set; } = 10;


		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is SubsetBasedIndicator subsetIndicator)
				subsetIndicator.PointsCount = PointsCount;
		}
	}
}
