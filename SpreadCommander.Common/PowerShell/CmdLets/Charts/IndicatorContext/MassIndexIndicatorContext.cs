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
	public class MassIndexIndicatorContext: SeparatePanelIndicatorContext
	{
		[Parameter(HelpMessage = "Count of points used to calculate the exponential moving average (EMA).")]
		[PSDefaultValue(Value = 9)]
		[DefaultValue(9)]
		public int MovingAveragePointsCount { get; set; } = 9;

		[Parameter(HelpMessage = "Count of summable values.")]
		[PSDefaultValue(Value = 25)]
		[DefaultValue(25)]
		public int SumPointsCount { get; set; } = 25;


		public override Indicator CreateIndicator()
		{
			return new MassIndex();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is MassIndex massIndex)
			{
				massIndex.MovingAveragePointsCount = MovingAveragePointsCount;
				massIndex.SumPointsCount           = SumPointsCount;
			}
		}
	}
}
