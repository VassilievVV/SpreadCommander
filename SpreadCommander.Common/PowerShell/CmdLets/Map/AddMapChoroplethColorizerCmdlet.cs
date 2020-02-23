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
    [Cmdlet(VerbsCommon.Add, "MapChoroplethColorizer")]
    public class AddMapChoroplethColorizerCmdlet: BaseMapWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Field in datasource (collection, shapefile, KML etc.) to color map according to data.")]
        public string ValueField { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Range stops for Choropleth colorizer.")]
        public double[] RangeStops { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Colors for this colorizer.")]
        public string[] Colors { get; set; }

        [Parameter(HelpMessage = "Whether to apply an approximation algorithm to colors in a colorizer.")]
        public SwitchParameter ApproximateColors { get; set; }

        [Parameter(HelpMessage = "Distribution type of range colors in a colorizer.")]
        [Alias("Distribution", "d")]
        public ColorizerRangeDistribution? RangeDistribution { get; set; }

        [Parameter(HelpMessage = "Factor specifying the percentages of the starting color and the ending color in the colorizer.")]
        [Alias("Factor", "f")]
        public double? RangeDistributionFactor { get; set; }


        protected override void UpdateMap()
        {
            var layer = MapContext.CurrentLayer as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");

            if (string.IsNullOrWhiteSpace(ValueField))
                throw new Exception("ValueField cannot be empty.");

            if (RangeStops == null || RangeStops.Length < 1)
                throw new Exception("Range stops cannot be empty.");
            if (Colors == null || Colors.Length < 1)
                throw new Exception("Colors cannot be empty.");

            var colorizer = new ChoroplethColorizer
            {
                ValueProvider         = new ShapeAttributeValueProvider() { AttributeName = ValueField },
                ApproximateColors     = this.ApproximateColors,
                PredefinedColorSchema = PredefinedColorSchema.Palette
            };

            foreach (var col in Colors)
            {
                var color = Utils.ColorFromString(col);
                if (color == Color.Empty)
                    throw new Exception($"Color '{col}' is not valid.");

                colorizer.ColorItems.Add(new ColorizerColorItem(color));
            }

            colorizer.RangeStops.AddRange(RangeStops);

            switch (RangeDistribution ?? ColorizerRangeDistribution.Linear)
            {
                case ColorizerRangeDistribution.Linear:
                    colorizer.RangeDistribution = new LinearRangeDistribution();
                    break;
                case ColorizerRangeDistribution.Logarithmic:
                    var logDistribution = new LogarithmicRangeDistribution();
                    if (RangeDistributionFactor.HasValue)
                        logDistribution.Factor = RangeDistributionFactor.Value;

                    colorizer.RangeDistribution = logDistribution;
                    break;
                case ColorizerRangeDistribution.Exponential:
                    var expDistribution = new ExponentialRangeDistribution();
                    if (RangeDistributionFactor.HasValue)
                        expDistribution.Factor = RangeDistributionFactor.Value;

                    colorizer.RangeDistribution = expDistribution;
                    break;
            }

            layer.Colorizer = colorizer;
        }
    }
}