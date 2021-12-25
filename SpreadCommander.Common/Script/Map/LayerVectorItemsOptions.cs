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
    public class LayerVectorItemsOptions
    {
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
    }

    public partial class SCMap
    {
        public SCMap AddLayerVectorItems(LayerVectorItemsOptions options = null)
        {
            options ??= new LayerVectorItemsOptions();

            var map = Map;

            var layer = new VectorItemsLayer()
            {
                AllowEditItems     = false,
                EnableHighlighting = false,
                EnableSelection    = false
            };

            if (!string.IsNullOrWhiteSpace(options.Name))
                layer.Name = options.Name;

            if (options.ItemImageIndex.HasValue)
                layer.ItemImageIndex = options.ItemImageIndex.Value;

            var fillColor = Utils.ColorFromString(options.FillColor);
            if (fillColor != Color.Empty)
                layer.ItemStyle.Fill = fillColor;

            var font = Utils.StringToFont(options.Font, out Color textColor);
            if (font != null)
                layer.ItemStyle.Font = font;
            if (textColor != Color.Empty)
                layer.ItemStyle.TextColor = textColor;

            var strokeColor = Utils.ColorFromString(options.StrokeColor);
            if (strokeColor != Color.Empty)
                layer.ItemStyle.Stroke = strokeColor;
            if (options.StrokeWidth.HasValue)
                layer.ItemStyle.StrokeWidth = options.StrokeWidth.Value;

            var textGlowColor = Utils.ColorFromString(options.TextGlowColor);
            if (textGlowColor != Color.Empty)
                layer.ItemStyle.TextGlowColor = textGlowColor;

            var adapter = new MapItemStorage();
            layer.Data  = adapter;

            map.Layers.Add(layer);
            CurrentLayer = layer;

            return this;
        }
    }
}
