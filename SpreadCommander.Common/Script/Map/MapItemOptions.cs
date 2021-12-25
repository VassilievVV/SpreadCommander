using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class MapItemOptions
    {
        [Description("Layer's name to which add map item.")]
        public string Layer { get; set; }

        protected internal virtual void ConfigureMapItem(SCMap map, MapItem item)
        {
            var layer = map.GetLayer(Layer) as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");
            var storage = layer.Data as MapItemStorage ??
                throw new Exception("Current vector items layer cannot be used for custom items. Create new layer using cmdlet Add-MapLayerVectorItems.");

            storage.Items.Add(item);
        }
    }
}
