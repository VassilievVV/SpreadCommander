using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapLayerVectorItems")]
	public class AddMapLayerVectorItemsCmdlet: BaseMapWithContextCmdlet
	{
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


		protected override void UpdateMap()
		{
			var map = MapContext.Map;

			var layer = new VectorItemsLayer()
			{
				AllowEditItems     = false,
				EnableHighlighting = false,
				EnableSelection    = false
			};

			if (!string.IsNullOrWhiteSpace(Name))
				layer.Name = Name;

			if (ItemImageIndex.HasValue)
				layer.ItemImageIndex = ItemImageIndex.Value;

			var fillColor = Utils.ColorFromString(FillColor);
			if (fillColor != Color.Empty)
				layer.ItemStyle.Fill = fillColor;

			var font = Utils.StringToFont(Font, out Color textColor);
			if (font != null)
				layer.ItemStyle.Font = font;
			if (textColor != Color.Empty)
				layer.ItemStyle.TextColor = textColor;

			var strokeColor = Utils.ColorFromString(StrokeColor);
			if (strokeColor != Color.Empty)
				layer.ItemStyle.Stroke = strokeColor;
			if (StrokeWidth.HasValue)
				layer.ItemStyle.StrokeWidth = StrokeWidth.Value;

			var textGlowColor = Utils.ColorFromString(TextGlowColor);
			if (textGlowColor != Color.Empty)
				layer.ItemStyle.TextGlowColor = textGlowColor;

			var adapter = new MapItemStorage();
			layer.Data  = adapter;

			map.Layers.Add(layer);

			MapContext.CurrentLayer = layer;
		}
	}
}
