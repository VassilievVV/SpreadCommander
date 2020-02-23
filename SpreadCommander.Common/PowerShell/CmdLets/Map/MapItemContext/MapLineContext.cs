using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapLineContext: MapShapeContext
	{
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "First point that defines this line.")]
		public double[] Point1 { get; set; }

		[Parameter(Mandatory = true, Position = 2, HelpMessage = "Second point that defines this line.")]
		public double[] Point2 { get; set; }

		[Parameter(HelpMessage = "Indicating whether the line is geodesic.")]
		public SwitchParameter Geodesic { get; set; }


		public override MapItem CreateMapItem() => new MapLine();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapLine ?? throw new Exception("Map item must be MapLine.");

			if (Point1 == null || Point1.Length != 2)
				throw new Exception("Point1 must be double array with 2 elements.");
			if (Point2 == null || Point2.Length != 2)
				throw new Exception("Point2 must be double array with 2 elements.");

			mapItem.Point1     = MapContext.CreateCoordPoint(Point1[0], Point1[1]);
			mapItem.Point2     = MapContext.CreateCoordPoint(Point2[0], Point2[1]);
			mapItem.IsGeodesic = Geodesic;
		}
	}
}
