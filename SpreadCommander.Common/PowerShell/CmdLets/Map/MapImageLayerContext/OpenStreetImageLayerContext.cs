using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapImageLayerContext
{
    public class OpenStreetImageLayerContext: BaseImageLayerContext
    {
        [Parameter(Position = 1, HelpMessage = "Type of images to be displayed on a map.")]
        public OpenStreetMapKind? Kind { get; set; }


        public override MapImageDataProviderBase CreateMapDataProvider()
        {
            var provider = new OpenStreetMapDataProvider();

            if (Kind.HasValue)
                provider.Kind = Kind.Value;

            return provider;
        }
    }
}
