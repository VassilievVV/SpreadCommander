using DevExpress.XtraCharts;
using SpreadCommander.Common.PowerShell.CmdLets.Charts.DiagramContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	//Class using to move Chart through PowerShell pipe.
	public class ChartContext
	{
		[Browsable(false)]
		public DevExpress.XtraCharts.Native.Chart Chart { get; set; }

		[Browsable(false)]
		public Series CurrentSeries { get; set; }

		[Browsable(false)]
		public XYDiagramPaneBase CurrentPane { get; set; }

		[Browsable(false)]
		public AxisBase CurrentAxis { get; set; }

		[Browsable(false)]
		public BaseDiagramContext DiagramContext { get; set; }


		public void Clear()
		{
			//No need to dispose other properties, they are part of Chart.
			Chart?.Dispose();

			Chart         = null;
			CurrentSeries = null;
			CurrentPane   = null;
			CurrentAxis   = null;
		}
	}
}
