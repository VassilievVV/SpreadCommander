using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesColorizerContext
{
	public class BaseSeriesColorizerContext
	{
		public virtual ChartColorizerBase CreateColorizer()
		{
			return null;
		}

		public virtual void SetupXtraChartColorizer(ChartContext chartContext, ChartColorizerBase colorizer)
		{
		}
	}
}
