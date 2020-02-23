using DevExpress.XtraMap;
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
    [Cmdlet(VerbsCommon.Add, "MapLegend")]
    public class AddMapLegendCmdlet: BaseMapWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Type of map legend.")]
        public MapLegendType LegendType { get; set; }

        [Parameter(HelpMessage = "How a map legend is aligned.")]
        public LegendAlignment? Alignment { get; set; }

        [Parameter(HelpMessage = "Color that is used to fill the legend's background")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Stroke color of the bordered legend.")]
        public string BackStrokeColor { get; set; }

        [Parameter(HelpMessage = "Legend's stroke width.")]
        public int? StrokeWidth { get; set; }

        [Parameter(HelpMessage = "Description of the map legend.")]
        public string Description { get; set; }

        [Parameter(HelpMessage = "Font used to display map legend's description.")]
        public string DescriptionFont { get; set; }

        [Parameter(HelpMessage = "Header of the map legend.")]
        public string Header { get; set; }

        [Parameter(HelpMessage = "Font used to display map legend's header.")]
        public string HeaderFont { get; set; }

        [Parameter(HelpMessage = "Font used to display map legend's items.")]
        public string ItemFont { get; set; }

        [Parameter(HelpMessage = "Display format of range stops on a color legend.")]
        public string RangeStopsFormat { get; set; }

        [Parameter(HelpMessage = "Sort order of the color list legend items.")]
        public LegendItemsSortOrder? SortOrder { get; set; }

        [Parameter(HelpMessage = "Whether or not it is necessary to hide tick marks for the size legend.")]
        public SwitchParameter HideTickMarks { get; set; }


        protected override void UpdateMap()
        {
            var layer = MapContext.CurrentLayer as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");

            var legend = LegendType switch
            {
                MapLegendType.ColorScale => (ItemsLayerLegend)new ColorScaleLegend(),
                MapLegendType.ColorList  => (ItemsLayerLegend)new ColorListLegend(),
                MapLegendType.SizeInline => (ItemsLayerLegend)new SizeLegend() { Type = SizeLegendType.Inline },
                MapLegendType.SizeNested => (ItemsLayerLegend)new SizeLegend() { Type = SizeLegendType.Nested },
                _                        => throw new Exception("Invalid legend type.")
            };
            legend.Layer = layer;

            if (Alignment.HasValue)
                legend.Alignment = Alignment.Value;

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
                    listLegend.SortOrder = SortOrder.Value;
            }

            if (legend is SizeLegend sizeLegend)
                sizeLegend.ShowTickMarks = !HideTickMarks;

            MapContext.Map.Legends.Add(legend);
        }
    }
}
