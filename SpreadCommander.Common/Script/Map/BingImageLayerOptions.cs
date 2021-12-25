using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class BingImageLayerOptions: MapLayerImageOptions
    {
        [Description("Culture name used to obtain data from Bing GIS services.")]
        public string CultureName { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddBingImageLayer(BingMapKind kind, BingImageLayerOptions options = null)
        {
            options ??= new BingImageLayerOptions();

            var provider = new BingMapDataProvider()
            {
                BingKey            = Parameters.BingMapKey,
                ConnectionProtocol = ConnectionProtocol.Https,
                Kind               = (DevExpress.XtraMap.BingMapKind)kind,
                CultureName        = Utils.NonNullString(options.CultureName)
            };

            options.UpdateLayerImage(this, provider);

            return this;
        }
    }
}
