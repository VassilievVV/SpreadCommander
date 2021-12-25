using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapRectangleContext: MapShapeContext
	{
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Location of the Rectangle.")]
		public double[] Location { get; set; }

		[Parameter(Mandatory = true, Position = 2, HelpMessage = "Width of the Rectangle.")]
		public double Width { get; set; }

		[Parameter(Mandatory = true, Position = 3, HelpMessage = "Height of the Rectangle.")]
		public double Height { get; set; }


		public override MapItem CreateMapItem() => new MapRectangle();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapRectangle ?? throw new Exception("Map item must be MapRectangle.");

			if (Location == null || Location.Length != 2)
				throw new Exception("Location must be double array with 2 elements.");

			mapItem.Location = MapContext.CreateCoordPoint(Location[0], Location[1]);

			mapItem.Height = Height;
			mapItem.Width  = Width;
		}
	}
}
