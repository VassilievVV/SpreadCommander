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
    public class LegendOptions
    {
        [Description("Name of layer to which add legend.")]
        public string LayerName { get; set; }

        [Description("How a map legend is aligned.")]
        public LegendAlignment? Alignment { get; set; }

        [Description("Color that is used to fill the legend's background")]
        public string BackColor { get; set; }

        [Description("Stroke color of the bordered legend.")]
        public string BackStrokeColor { get; set; }

        [Description("Legend's stroke width.")]
        public int? StrokeWidth { get; set; }

        [Description("Description of the map legend.")]
        public string Description { get; set; }

        [Description("Font used to display map legend's description.")]
        public string DescriptionFont { get; set; }

        [Description("Header of the map legend.")]
        public string Header { get; set; }

        [Description("Font used to display map legend's header.")]
        public string HeaderFont { get; set; }

        [Description("Font used to display map legend's items.")]
        public string ItemFont { get; set; }

        [Description("Display format of range stops on a color legend.")]
        public string RangeStopsFormat { get; set; }

        [Description("Sort order of the color list legend items.")]
        public LegendItemsSortOrder? SortOrder { get; set; }

        [Description("Whether or not it is necessary to hide tick marks for the size legend.")]
        public bool HideTickMarks { get; set; }


        protected internal virtual void UpdateMapLegend(SCMap map, ItemsLayerLegend legend)
        {
            var layer = map.GetLayer(LayerName) as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");

            legend.Layer = layer;

            if (Alignment.HasValue)
                legend.Alignment = (DevExpress.XtraMap.LegendAlignment)Alignment.Value;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                legend.BackgroundStyle.Fill = backColor;
            var backStrokeColor = Utils.ColorFromString(BackStrokeColor);
            if (backStrokeColor != Color.Empty)
                legend.BackgroundStyle.Stroke = backStrokeColor;
            if (StrokeWidth.HasValue)
                legend.BackgroundStyle.StrokeWidth = StrokeWidth.Value;

            legend.Description = Description;
            var fontDescription = Utils.StringToFont(DescriptionFont, out Color descriptionColor);
            if (fontDescription != null)
                legend.DescriptionStyle.Font = fontDescription;
            if (descriptionColor != Color.Empty)
                legend.DescriptionStyle.TextColor = descriptionColor;

            legend.Header = Header;
            var fontHeader = Utils.StringToFont(HeaderFont, out Color headerColor);
            if (fontHeader != null)
                legend.HeaderStyle.Font = fontHeader;
            if (headerColor != Color.Empty)
                legend.HeaderStyle.TextColor = headerColor;

            var fontItem = Utils.StringToFont(ItemFont, out Color itemColor);
            if (fontItem != null)
                legend.ItemStyle.Font = fontItem;
            if (itemColor != Color.Empty)
                legend.ItemStyle.TextColor = itemColor;

            if (!string.IsNullOrWhiteSpace(RangeStopsFormat))
                legend.RangeStopsFormat = RangeStopsFormat;

            if (legend is ColorListLegend listLegend)
            {
                if (SortOrder.HasValue)
                    listLegend.SortOrder = (DevExpress.XtraMap.LegendItemsSortOrder)SortOrder.Value;
            }

            if (legend is SizeLegend sizeLegend)
                sizeLegend.ShowTickMarks = !HideTickMarks;

            map.Map.Legends.Add(legend);
        }
    }

    public partial class SCMap
    {
        public SCMap AddLegend(MapLegendType legendType, LegendOptions options = null)
        {
            options ??= new LegendOptions();

            var legend = legendType switch
            {
                MapLegendType.ColorScale => (ItemsLayerLegend)new ColorScaleLegend(),
                MapLegendType.ColorList  => (ItemsLayerLegend)new ColorListLegend(),
                MapLegendType.SizeInline => (ItemsLayerLegend)new SizeLegend() { Type = SizeLegendType.Inline },
                MapLegendType.SizeNested => (ItemsLayerLegend)new SizeLegend() { Type = SizeLegendType.Nested },
                _                        => throw new Exception("Invalid legend type.")
            };

            options.UpdateMapLegend(this, legend);

            return this;
        }
    }
}
