﻿using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class RateOfChangeIndicatorContext: SeparatePanelIndicatorContext
	{
		[Parameter(HelpMessage = "Number of data points used to calculate the indicator.")]
		[PSDefaultValue(Value = 14)]
		[DefaultValue(14)]
		public int PointsCount { get; set; } = 14;

		[Parameter(HelpMessage = "Value specifying which series point value should be used to calculate the indicator.")]
		public ValueLevel? ValueLevel { get; set; }


		public override Indicator CreateIndicator()
		{
			return new RateOfChange();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is RateOfChange rateOfChange)
			{
				rateOfChange.PointsCount = PointsCount;
				if (ValueLevel.HasValue)
					rateOfChange.ValueLevel = ValueLevel.Value;
			}
		}
	}
}
