﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
	public class TriangularMovingAverageIndicatorContext: MovingAverageIndicatorContext
	{
		public override Indicator CreateIndicator()
		{
			return new TriangularMovingAverage();
		}
	}
}
