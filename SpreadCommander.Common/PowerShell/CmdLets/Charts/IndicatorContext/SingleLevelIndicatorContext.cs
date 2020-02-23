using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class SingleLevelIndicatorContext: BaseIndicatorContext
	{
		[Parameter(HelpMessage = "Value level of the indicator - Close, High, Low, Open etc.")]
		public ValueLevel? ValueLevel { get; set; }


		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is SingleLevelIndicator singleLevelIndicator)
			{
				if (ValueLevel.HasValue)
					singleLevelIndicator.ValueLevel = ValueLevel.Value;
			}
		}
	}
}
