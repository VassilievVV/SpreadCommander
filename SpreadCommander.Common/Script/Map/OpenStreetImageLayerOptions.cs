using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class OpenStreetImageLayerOptions: MapLayerImageOptions
    {
    }

    public partial class SCMap
    {
        public SCMap AddOpenStreetImageLayer(OpenStreetMapKind kind, OpenStreetImageLayerOptions options = null)
        {
            options ??= new OpenStreetImageLayerOptions();

            var provider = new OpenStreetMapDataProvider()
            {
                Kind = (DevExpress.XtraMap.OpenStreetMapKind)kind
            };

            provider.WebRequest += (s, e) =>
            {
                e.UserAgent = "SpreadCommander";
            };

            options.UpdateLayerImage(this, provider);

            return this;
        }
    }
}
