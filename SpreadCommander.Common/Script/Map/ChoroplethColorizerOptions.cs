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
    public class ChoroplethColorizerOptions: ColorizerOptions
    {
        [Description("Whether to apply an approximation algorithm to colors in a colorizer.")]
        public bool ApproximateColors { get; set; }

        [Description("Distribution type of range colors in a colorizer.")]
        public ColorizerRangeDistribution? RangeDistribution { get; set; }

        [Description("Factor specifying the percentages of the starting color and the ending color in the colorizer.")]
        public double? RangeDistributionFactor { get; set; }


        protected internal virtual void UpdateMap(SCMap map, string valueField, double[] rangeStops, string[] colors)
        {
            var layer = map.GetLayer(Layer) as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to vector items layer created with command AddMapLayerVectorItems.");

            if (string.IsNullOrWhiteSpace(valueField))
                throw new Exception("ValueField cannot be empty.");

            if (rangeStops == null || rangeStops.Length < 1)
                throw new Exception("Range stops cannot be empty.");
            if (colors == null || colors.Length < 1)
                throw new Exception("Colors cannot be empty.");

            var colorizer = new ChoroplethColorizer
            {
                ValueProvider         = new ShapeAttributeValueProvider() { AttributeName = valueField },
                ApproximateColors     = this.ApproximateColors,
                PredefinedColorSchema = PredefinedColorSchema.Palette
            };

            foreach (var col in colors)
            {
                var color = Utils.ColorFromString(col);
                if (color == Color.Empty)
                    throw new Exception($"Color '{col}' is not valid.");

                colorizer.ColorItems.Add(new ColorizerColorItem(color));
            }

            colorizer.RangeStops.AddRange(rangeStops);

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

    public partial class SCMap
    {
        public SCMap AddChoroplethColorizer(string valueField, double[] rangeStops, string[] colors,
            ChoroplethColorizerOptions options = null)
        {
            options ??= new ChoroplethColorizerOptions();
            options.UpdateMap(this, valueField, rangeStops, colors);

            return this;
        }
    }
}
