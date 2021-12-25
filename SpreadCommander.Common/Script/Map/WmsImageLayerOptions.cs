using DevExpress.XtraMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class WmsImageLayerOptions: MapLayerImageOptions
    {
        [Description("Parameters for request to Web Map Service.")]
        public Hashtable Parameters { get; set; }

        [Description("Maximum image width that the provider can request from a server.")]
        public int? MaxRequestedImageWidth { get; set; }

        [Description("Maximum image height that the provider can request from a server.")]
        public int? MaxRequestedImageHeight { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddWmsImageLayer(string serverUrl, string activeLayer, WmsImageLayerOptions options = null)
        {
            options ??= new WmsImageLayerOptions();

            var provider = new WmsDataProvider()
            {
                ServerUri       = serverUrl,
                ActiveLayerName = activeLayer,
            };

            if (options.MaxRequestedImageWidth.HasValue)
                provider.MaxRequestedImageWidth = options.MaxRequestedImageWidth.Value;
            if (options.MaxRequestedImageHeight.HasValue)
            provider.MaxRequestedImageHeight = options.MaxRequestedImageHeight.Value;


            if (options.Parameters != null)
            {
                foreach (DictionaryEntry parameter in options.Parameters)
                    provider.CustomParameters.Add(Convert.ToString(parameter.Key), Convert.ToString(parameter.Value));
            }

            options.UpdateLayerImage(this, provider);

            return this;
        }
    }
}
