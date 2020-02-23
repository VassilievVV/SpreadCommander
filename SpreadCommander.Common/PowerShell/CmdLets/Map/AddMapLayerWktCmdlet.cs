using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapLayerWkt")]
	public class AddMapLayerWktCmdlet: BaseMapWithContextCmdlet
	{
		[Parameter(HelpMessage = "Layer's name.")]
		public string Name { get; set; }

		[Parameter(Mandatory = true, Position = 0, HelpMessage = "WKT (well-known text) items.")]
		public string[] WKT { get; set; }

		protected override void UpdateMap()
		{
			if (WKT == null || WKT.Length <= 0)
				return;

			var map = MapContext.Map;

			var layer = new VectorItemsLayer()
			{
				AllowEditItems     = false,
				EnableHighlighting = false,
				EnableSelection    = false
			};

			if (!string.IsNullOrWhiteSpace(Name))
				layer.Name = Name;

			var storage = new SqlGeometryItemStorage();
			int counter = 1;
			foreach (var wkt in WKT)
				storage.Items.Add(SqlGeometryItem.FromWkt(wkt, counter++));

			layer.Data = storage;

			map.Layers.Add(layer);

			MapContext.CurrentLayer = layer;
		}

		/*
Unknown = 0, 
Dot = 1, 
Ellipse = 2, 
Line = 3, 
Path = 4, 
Polygon = 5, 
Polyline = 6, 
Rectangle = 7, 
Pushpin = 8, 
Custom = 9, 
Callout = 10
		 */
	}
}
