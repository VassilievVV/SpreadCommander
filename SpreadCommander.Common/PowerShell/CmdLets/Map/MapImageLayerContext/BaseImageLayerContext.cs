using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapImageLayerContext
{
	public class BaseImageLayerContext
	{
		public virtual MapImageDataProviderBase CreateMapDataProvider()
		{
			return null;
		}
	}
}
