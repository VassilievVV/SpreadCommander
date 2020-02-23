#pragma warning disable CRR0047

using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	#region SCSeriesAggregateFunction
	public enum SCSeriesAggregateFunction
	{
		Default   = -1,
		None      = 0,
		Average   = 1,
		Minimum   = 2,
		Maximum   = 3,
		Sum       = 4,
		Count     = 5,
		Financial = 6
	}
	#endregion
	public class BaseSeriesContext
	{
		public virtual void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
		}

		public virtual void BoundDataChanged(ChartContext chartContext, Series series)
		{
		}
	}
}
