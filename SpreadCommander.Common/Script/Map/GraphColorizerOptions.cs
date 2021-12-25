using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class GraphColorizerOptions: ColorizerOptions
    {
    }

    public partial class SCMap
    {
        public SCMap AddGraphColorizer(string[] colors, GraphColorizerOptions options = null)
        {
            var layer = GetLayer(options?.Layer) as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to vector items layer created with command AddMapLayerVectorItems.");

            if (colors == null || colors.Length < 1)
                throw new Exception("Colors cannot be empty.");

            var colorizer = new GraphColorizer()
            {
                PredefinedColorSchema = PredefinedColorSchema.Palette
            };

            foreach (var col in colors)
            {
                var color = Utils.ColorFromString(col);
                if (color == Color.Empty)
                    throw new Exception($"Color '{col}' is not valid.");

                colorizer.ColorItems.Add(new ColorizerColorItem(color));
            }

            layer.Colorizer = colorizer;

            return this;
        }
    }
}
