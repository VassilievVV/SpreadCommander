using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class HeatmapImageLayerOptions: MapLayerImageOptions
    {
        [Description("Particular heatmap point's radius in pixels.")]
        public int? PointRadius { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddHeatmapImageLayer(HeatmapImageLayerOptions options = null)
        {
            options ??= new HeatmapImageLayerOptions();

            var provider = new HeatmapProvider();
            
            var algorithm = new HeatmapDensityBasedAlgorithm();
            if (options.PointRadius.HasValue)
                algorithm.PointRadius = options.PointRadius.Value;
            provider.Algorithm = algorithm;

            options.UpdateLayerImage(this, provider);

            return this;
        }
    }
}
