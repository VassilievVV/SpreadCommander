using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
	public enum MapItemType : byte
	{
		Dot         = 1,
		Ellipse     = 2,
		Line        = 3,
		//Path      = 4,
		Polygon     = 5,
		Polyline    = 6,
		Rectangle   = 7,
		Pushpin     = 8,
		Custom      = 9,
		Callout     = 10,
		Bubble      = 11
		//Pie       = 12
	}
}
