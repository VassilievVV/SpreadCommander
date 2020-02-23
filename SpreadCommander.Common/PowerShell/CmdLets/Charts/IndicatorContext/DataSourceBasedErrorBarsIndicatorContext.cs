using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class DataSourceBasedErrorBarsIndicatorContext: ErrorBarsIndicatorContext
	{
		[Parameter(Mandatory = true, HelpMessage = "Name of the data field which contains positive error values of an indicator for generated series points.")]
		public string PositiveErrorDataMember { get; set; }

		[Parameter(Mandatory = true, HelpMessage = "Name of the data field which contains negative error values of an indicator for generated series points.")]
		public string NegativeErrorDataMember { get; set; }


		public override Indicator CreateIndicator()
		{
			return new DataSourceBasedErrorBars();
		}

		public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
			base.SetupXtraChartIndicator(chartContext, indicator);

			if (indicator is DataSourceBasedErrorBars dataSourceBasedErrorBars)
			{
				dataSourceBasedErrorBars.PositiveErrorDataMember = PositiveErrorDataMember;
				dataSourceBasedErrorBars.NegativeErrorDataMember = NegativeErrorDataMember;
			}
		}
	}
}
