using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class BaseIndicatorContext
	{
		public virtual Indicator CreateIndicator()
		{
			return null;
		}

		public virtual void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
		{
		}
	}
}
