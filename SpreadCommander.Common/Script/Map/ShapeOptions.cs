using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class ShapeOptions: MapItemOptions
    {
        [Description("Color that is used to fill a map item.")]
        public string FillColor { get; set; }

        [Description("Color that specifies how the map item outline is painted.")]
        public string StrokeColor { get; set; }

        [Description("Width of the stroke on the map item.")]
        public int? StrokeWidth { get; set; }

        [Description("Shape's title.")]
        public string Title { get; set; }

        [Description("Text color of shape titles.")]
        public string TitleColor { get; set; }

        [Description("Text glowing color of shape titles.")]
        public string TitleGlowColor { get; set; }


        protected internal override void ConfigureMapItem(SCMap map, MapItem item)
        {
            base.ConfigureMapItem(map, item);

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
