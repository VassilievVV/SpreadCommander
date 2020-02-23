using DevExpress.XtraMap;
using SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapItem")]
	public class AddMapItemCmdlet: BaseMapWithContextCmdlet, IDynamicParameters
	{
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Map item type - dot, line, path, callout etc.")]
		public MapItemType ItemType { get; set; }


		private BaseMapItemContext _MapItemContext;

		public object GetDynamicParameters()
		{
			switch (ItemType)
			{
				case MapItemType.Dot:
					return CreateMapItemContext(typeof(MapDotContext));
				case MapItemType.Ellipse:
					return CreateMapItemContext(typeof(MapEllipseContext));
				case MapItemType.Line:
					return CreateMapItemContext(typeof(MapLineContext));
				case MapItemType.Polygon:
					return CreateMapItemContext(typeof(MapPolygonContext));
				case MapItemType.Polyline:
					return CreateMapItemContext(typeof(MapPolylineContext));
				case MapItemType.Rectangle:
					return CreateMapItemContext(typeof(MapRectangleContext));
				case MapItemType.Pushpin:
					return CreateMapItemContext(typeof(MapPushpinContext));
				case MapItemType.Custom:
					return CreateMapItemContext(typeof(MapCustomElementContext));
				case MapItemType.Callout:
					return CreateMapItemContext(typeof(MapCalloutContext));
				case MapItemType.Bubble:
					return CreateMapItemContext(typeof(MapBubbleContext));
				default:
					break;
			}

			_MapItemContext = null;
			return null;


			BaseMapItemContext CreateMapItemContext(Type typeContext)
			{
				if (_MapItemContext == null || !(typeContext.IsInstanceOfType(_MapItemContext)))
					_MapItemContext = Activator.CreateInstance(typeContext) as BaseMapItemContext;
				return _MapItemContext;
			}
		}

		protected override void UpdateMapRecord()
		{
			if (_MapItemContext != null)
				_MapItemContext.MapContext = MapContext;

			var item = _MapItemContext?.CreateMapItem() ?? throw new Exception("Cannot configure map item with specified type.");
			_MapItemContext.ConfigureMapItem(item);

			var layer = MapContext.CurrentLayer as VectorItemsLayer ??
				throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");
			var storage = layer.Data as MapItemStorage ??
				throw new Exception("Current vector items layer cannot be used for custom items. Create new layer using cmdlet Add-MapLayerVectorItems.");

			storage.Items.Add(item);
		}
	}
}
