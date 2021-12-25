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
    public class MapLayerRouteOptions
    {
        [Description("Number of results that can be obtained by a Route request.")]
        [DefaultValue(10)]
        public int ResultCount { get; set; } = 10;

        [Description("Layer's name.")]
        public string Name { get; set; }

        [Description("Index of the image assigned to the map item.")]
        public int? ItemImageIndex { get; set; }

        [Description("Color used to fill a map item.")]
        public string FillColor { get; set; }

        [Description("Map item's font.")]
        public string Font { get; set; }

        [Description("Color that specifies how the map item outline is painted.")]
        public string StrokeColor { get; set; }

        [Description("Width of the stroke on the current map item.")]
        public int? StrokeWidth { get; set; }

        [Description("Map item's text glow color.")]
        public string TextGlowColor { get; set; }

        [Description("Pattern used to generate shape titles.")]
        public string ShapeTitlesPattern { get; set; }


        protected internal virtual void CreateLayer(SCMap map, InformationDataProviderBase provider)
        {
            var layer = new InformationLayer()
            {
                EnableHighlighting = false,
                EnableSelection    = false,
                DataProvider       = provider
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

            if (layer.DataProvider == null)
                layer.DataProvider = provider;

            map.Map.Layers.Add(layer);
            map.CurrentLayer = layer;
        }
    }
}
