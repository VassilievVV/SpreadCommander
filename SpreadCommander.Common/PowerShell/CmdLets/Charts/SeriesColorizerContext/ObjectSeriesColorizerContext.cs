using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesColorizerContext
{
	public class ObjectSeriesColorizerContext: BaseSeriesColorizerContext
	{
		public override ChartColorizerBase CreateColorizer()
		{
			return new ColorObjectColorizer();
		}

		public override void SetupXtraChartColorizer(ChartContext chartContext, ChartColorizerBase colorizer)
		{
			//Do nothing
		}
	}
}
