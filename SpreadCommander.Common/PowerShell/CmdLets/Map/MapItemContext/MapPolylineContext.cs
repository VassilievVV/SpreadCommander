using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapPolylineContext: MapShapeContext
	{
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Collection of points defining the map Polyline. 2D double array in form @(@(10, -170), @(10, 170)).")]
		public double[][] Points { get; set; }


		[Parameter(HelpMessage = "Indicating whether the Polyline is geodesic.")]
		public SwitchParameter Geodesic { get; set; }


		public override MapItem CreateMapItem() => new MapPolyline();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapPolyline ?? throw new Exception("Map item must be MapPolyline.");

			if (Points == null || Points.Length <= 1)
				throw new Exception("Points are not properly defined.");

			foreach (double[] point in Points)
			{
				if (point == null || point.Length != 2)
					throw new Exception("Point is not property defined. Must be double array with 2 elements.");

				mapItem.Points.Add(MapContext.CreateCoordPoint(point[0], point[1]));
			}

			mapItem.IsGeodesic = Geodesic;
		}
	}
}
