using DevExpress.XtraMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapImageLayerContext
{
    public class WmsImageLayerContext: BaseImageLayerContext
    {
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "URL of server that supports Web Map Service.")]
        public string ServerUrl { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Active layer name retrieved from a Web Map Service.")]
        public string ActiveLayer { get; set; }

        [Parameter(HelpMessage = "Parameters for request to Web Map Service.")]
        public Hashtable Parameters { get; set; }

        [Parameter(HelpMessage = "Maximum image width that the provider can request from a server.")]
        public int? MaxRequestedImageWidth { get; set; }

        [Parameter(HelpMessage = "Maximum image height that the provider can request from a server.")]
        public int? MaxRequestedImageHeight { get; set; }

        public override MapImageDataProviderBase CreateMapDataProvider()
        {
            var provider = new WmsDataProvider()
            {
                ServerUri               = ServerUrl,
                ActiveLayerName         = this.ActiveLayer,
            };

            if (MaxRequestedImageWidth.HasValue)
                provider.MaxRequestedImageWidth = MaxRequestedImageWidth.Value;
            if (MaxRequestedImageHeight.HasValue)
            provider.MaxRequestedImageHeight = MaxRequestedImageHeight.Value;


            if (Parameters != null)
            {
                foreach (DictionaryEntry parameter in Parameters)
                    provider.CustomParameters.Add(Convert.ToString(parameter.Key), Convert.ToString(parameter.Value));
            }

            return provider;
        }
    }
}
