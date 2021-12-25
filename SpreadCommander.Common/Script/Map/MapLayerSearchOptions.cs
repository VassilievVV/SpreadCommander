using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class MapLayerSearchOptions
    {
        [Description("Number of results that can be obtained by a search request.")]
        [DefaultValue(10)]
        public int ResultCount { get; set; } = 10;


        protected internal virtual void CreateLayer(SCMap map, InformationDataProviderBase provider)
        {
            var layer = new InformationLayer()
            {
                EnableHighlighting = false,
                EnableSelection    = false
            };

            if (layer.DataProvider == null)
                layer.DataProvider = provider;

            map.Map.Layers.Add(layer);
            map.CurrentLayer = layer;
        }
    }
}
