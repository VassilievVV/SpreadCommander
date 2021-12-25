using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
	public class BingRouteWayPoint
	{
		[Description("Keyword indicating the required route waypoint to search for on a map.")]
		public string Keyword { get; set; }

		[Description("Description of a waypoint.")]
		public string Description { get; set; }

		[Description("Location of the route waypoint.")]
		public double[] Location { get; set; }
	}
}
