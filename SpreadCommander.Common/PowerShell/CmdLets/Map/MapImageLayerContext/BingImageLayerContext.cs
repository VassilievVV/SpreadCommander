using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapImageLayerContext
{
    public class BingImageLayerContext: BaseImageLayerContext
    {
        [Parameter(Position = 1, HelpMessage = "Type of images to be displayed on a map.")]
        public BingMapKind? Kind { get; set; }

        [Parameter(HelpMessage = "Culture name used to obtain data from Bing GIS services.")]
        public string CultureName { get; set; }

        public override MapImageDataProviderBase CreateMapDataProvider()
        {
            var provider = new BingMapDataProvider()
            {
                BingKey            = Parameters.BingMapKey,
                ConnectionProtocol = ConnectionProtocol.Https,
                CultureName        = Utils.NonNullString(this.CultureName)
            };

            if (Kind.HasValue)
                provider.Kind = Kind.Value;

            return provider;
        }
    }
}
