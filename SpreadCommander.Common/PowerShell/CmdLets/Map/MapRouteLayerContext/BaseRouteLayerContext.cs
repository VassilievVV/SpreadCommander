using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapRouteLayerContext
{
	public class BaseRouteLayerContext
	{
		public MapContext MapContext { get; internal set; }

		public virtual InformationDataProviderBase CreateDataProvider(InformationLayer layer, int resultCount)
		{
			return null;
		}
	}
}
