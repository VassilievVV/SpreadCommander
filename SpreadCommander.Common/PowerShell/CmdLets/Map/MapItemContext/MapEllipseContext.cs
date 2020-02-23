using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapEllipseContext: MapShapeContext
	{
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Location of the Ellipse.")]
		public double[] Location { get; set; }

		[Parameter(Mandatory = true, Position = 2, HelpMessage = "Width of the ellipse.")]
		public double Width { get; set; }

		[Parameter(Mandatory = true, Position = 3, HelpMessage = "Height of the ellipse.")]
		public double Height { get; set; }


		public override MapItem CreateMapItem() => new MapEllipse();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapEllipse ?? throw new Exception("Map item must be MapEllipse.");

			if (Location == null || Location.Length != 2)
				throw new Exception("Location must be double array with 2 elements.");

			mapItem.Location = MapContext.CreateCoordPoint(Location[0], Location[1]);

			mapItem.Height = Height;
			mapItem.Width  = Width;
		}
	}
}
