using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class BaseMapItemContext
	{
		public MapContext MapContext { get; internal set; }

		public virtual MapItem CreateMapItem()
		{
			return null;
		}

		public virtual void ConfigureMapItem(MapItem item)
		{
		}
	}
}
