using DevExpress.XtraMap;
using SpreadCommander.Common.PowerShell.CmdLets.Map.MapRouteLayerContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapLayerRoute")]
	public class AddMapLayerRouteCmdlet: BaseMapWithContextCmdlet, IDynamicParameters
	{
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Route provider type - Bing, OpenStreet.")]
		public RouteProviderType ProviderType { get; set; }

		[Parameter(HelpMessage = "Number of results that can be obtained by a Route request.")]
		[PSDefaultValue(Value = 10)]
		[DefaultValue(10)]
		public int ResultCount { get; set; } = 10;

		[Parameter(HelpMessage = "Layer's name.")]
		public string Name { get; set; }

		[Parameter(HelpMessage = "Index of the image assigned to the map item.")]
		public int? ItemImageIndex { get; set; }

		[Parameter(HelpMessage = "Color used to fill a map item.")]
		public string FillColor { get; set; }

		[Parameter(HelpMessage = "Map item's font.")]
		public string Font { get; set; }

		[Parameter(HelpMessage = "Color that specifies how the map item outline is painted.")]
		public string StrokeColor { get; set; }

		[Parameter(HelpMessage = "Width of the stroke on the current map item.")]
		public int? StrokeWidth { get; set; }

		[Parameter(HelpMessage = "Map item's text glow color.")]
		public string TextGlowColor { get; set; }

		[Parameter(HelpMessage = "Pattern used to generate shape titles.")]
		public string ShapeTitlesPattern { get; set; }


		private BaseRouteLayerContext _RouteLayerContext;

		public object GetDynamicParameters()
		{
			switch (ProviderType)
			{
				case RouteProviderType.Bing:
					return CreateRouteLayerContext(typeof(BingRouteLayerContext));
				default:
					break;
			}

			_RouteLayerContext = null;
			return null;


			BaseRouteLayerContext CreateRouteLayerContext(Type typeContext)
			{
				if (_RouteLayerContext == null || !(typeContext.IsInstanceOfType(_RouteLayerContext)))
					_RouteLayerContext = Activator.CreateInstance(typeContext) as BaseRouteLayerContext;
				return _RouteLayerContext;
			}
		}

		protected override void UpdateMapRecord()
		{
			var layer = CreateLayer();

			var map = MapContext.Map;
			map.Layers.Add(layer);

			MapContext.CurrentLayer = layer;
		}

		protected InformationLayer CreateLayer()
		{
			var layer = new InformationLayer()
			{
				EnableHighlighting = false,
				EnableSelection = false
			};

			if (!string.IsNullOrWhiteSpace(Name))
				layer.Name = Name;

			if (!string.IsNullOrWhiteSpace(ShapeTitlesPattern))
			{
				layer.ShapeTitlesPattern    = ShapeTitlesPattern;
				layer.ShapeTitlesVisibility = VisibilityMode.Visible;
			}

			if (ItemImageIndex.HasValue)
				layer.ItemImageIndex = ItemImageIndex.Value;

			var itemStyle = layer.ItemStyle;

			var fillColor = Utils.ColorFromString(FillColor);
			if (fillColor != Color.Empty)
				itemStyle.Fill = fillColor;

			var font = Utils.StringToFont(Font, out Color textColor);
			if (font != null)
				itemStyle.Font = font;
			if (textColor != Color.Empty)
				itemStyle.TextColor = textColor;

			var strokeColor = Utils.ColorFromString(StrokeColor);
			if (strokeColor != Color.Empty)
				itemStyle.Stroke = strokeColor;
			if (StrokeWidth.HasValue)
				itemStyle.StrokeWidth = StrokeWidth.Value;

			var textGlowColor = Utils.ColorFromString(TextGlowColor);
			if (textGlowColor != Color.Empty)
				itemStyle.TextGlowColor = textGlowColor;

			if (_RouteLayerContext != null)
				_RouteLayerContext.MapContext = MapContext;

			var provider = _RouteLayerContext?.CreateDataProvider(layer, ResultCount) ?? throw new Exception("Cannot create Route provider.");

			if (layer.DataProvider == null)
				layer.DataProvider = provider;

			return layer;
		}
	}
}
