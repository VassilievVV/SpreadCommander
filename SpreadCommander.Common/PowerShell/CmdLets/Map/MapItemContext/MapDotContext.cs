using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapDotContext: MapShapeContext
	{
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Location of the Dot.")]
		public double[] Location { get; set; }

		[Parameter(HelpMessage = "Dot size in pixels.")]
		public int? Size { get; set; }

		[Parameter(HelpMessage = "Shape that is used to draw a dot on a map.")]
		public MapDotShapeKind? ShapeKind { get; set; }


		public override MapItem CreateMapItem() => new MapDot();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapDot ?? throw new Exception("Map item must be MapDot.");

			if (Location == null || Location.Length != 2)
				throw new Exception("Location must be double array with 2 elements.");

			mapItem.Location = MapContext.CreateCoordPoint(Location[0], Location[1]);

			if (Size.HasValue)
				mapItem.Size = Size.Value;
			if (ShapeKind.HasValue)
				mapItem.ShapeKind = ShapeKind.Value;
		}
	}
}
