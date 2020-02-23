using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapShapeContext: BaseMapItemContext
	{
		[Parameter(HelpMessage = "Color that is used to fill a map item.")]
		public string FillColor { get; set; }

		[Parameter(HelpMessage = "Color that specifies how the map item outline is painted.")]
		public string StrokeColor { get; set; }

		[Parameter(HelpMessage = "Width of the stroke on the map item.")]
		public int? StrokeWidth { get; set; }

		[Parameter(HelpMessage = "Shape's title.")]
		public string Title { get; set; }

		[Parameter(HelpMessage = "Text color of shape titles.")]
		public string TitleColor { get; set; }

		[Parameter(HelpMessage = "Text glowing color of shape titles.")]
		public string TitleGlowColor { get; set; }


		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapShape ?? throw new Exception("Map item must be MapShape.");

			var fillColor = Utils.ColorFromString(FillColor);
			if (fillColor != Color.Empty)
				mapItem.Fill = fillColor;

			var strokeColor = Utils.ColorFromString(StrokeColor);
			if (strokeColor != Color.Empty)
				mapItem.Stroke = strokeColor;

			if (StrokeWidth.HasValue)
				mapItem.StrokeWidth = StrokeWidth.Value;

			if (!string.IsNullOrWhiteSpace(Title))
			{
				mapItem.Attributes.Add(new MapItemAttribute() { Name = "Title", Value = Title });
				mapItem.TitleOptions.Pattern = "{Title}";

				var titleColor = Utils.ColorFromString(TitleColor);
				if (titleColor != Color.Empty)
					mapItem.TitleOptions.TextColor = titleColor;

				var titleGlowColor = Utils.ColorFromString(TitleGlowColor);
				if (titleGlowColor != Color.Empty)
					mapItem.TitleOptions.TextGlowColor = titleGlowColor;
			}
		}
	}
}
