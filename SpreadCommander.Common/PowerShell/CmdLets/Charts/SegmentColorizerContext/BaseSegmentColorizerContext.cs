using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SegmentColorizerContext
{
	public class BaseSegmentColorizerContext
	{
		public virtual SegmentColorizerBase CreateColorizer()
		{
			return null;
		}

		public virtual void SetupXtraChartColorizer(ChartContext chartContext, SegmentColorizerBase colorizer)
		{
		}
	}
}
