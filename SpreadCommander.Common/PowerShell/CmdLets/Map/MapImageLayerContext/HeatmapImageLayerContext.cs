using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapImageLayerContext
{
    public class HeatmapImageLayerContext: BaseImageLayerContext
    {
        [Parameter(HelpMessage = "Particular heatmap point's radius in pixels.")]
        public int? PointRadius { get; set; }


        public override MapImageDataProviderBase CreateMapDataProvider()
        {
            var provider = new HeatmapProvider();
            
            var algorithm = new HeatmapDensityBasedAlgorithm();
            if (PointRadius.HasValue)
                algorithm.PointRadius = PointRadius.Value;
            provider.Algorithm = algorithm;

            return provider;
        }
    }
}
