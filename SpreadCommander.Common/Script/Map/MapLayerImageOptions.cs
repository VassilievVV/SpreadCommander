using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class MapLayerImageOptions
    {
        [Description("Layer's name.")]
        public string Name { get; set; }

        [Description("When set - layer is added to mini map.")]
        public bool MiniMap { get; set; }

        [Description("Layer's transparency.")]
        public byte? Transparency { get; set; }


        protected internal virtual void UpdateLayerImage(SCMap map, MapImageDataProviderBase provider)
        {
            if (!MiniMap)
            {
                var layer = new ImageLayer()
                {
                    DataProvider       = provider,
                    EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True,
                };
                if (Transparency.HasValue)
                    layer.Transparency = Transparency.Value;

                if (!string.IsNullOrWhiteSpace(Name))
                    layer.Name = Name;

                map.Map.Layers.Add(layer);
                map.CurrentLayer = layer;
            }
            else
            {
                var dataProvider = provider as MapDataProviderBase ?? throw new Exception("MiniMap does not support selected data provider.");

                var layerMini = new MiniMapImageTilesLayer()
                {
                    DataProvider = dataProvider
                };

                if (!string.IsNullOrWhiteSpace(Name))
                    layerMini.Name = Name;

                if (map.Map.MiniMap == null)
                    throw new Exception("Mini map is not created.");

                map.Map.MiniMap.Layers.Add(layerMini);
                map.CurrentLayer = layerMini;
            }
        }
    }
}
