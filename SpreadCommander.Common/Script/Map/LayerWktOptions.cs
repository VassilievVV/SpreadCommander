using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class LayerWktOptions
    {
        [Description("Layer's name.")]
        public string Name { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddLayerWkt(string[] WKT, LayerWktOptions options = null)
        {
            if (WKT == null || WKT.Length <= 0)
                return this;

            options ??= new LayerWktOptions();

            var map = Map;

            var layer = new VectorItemsLayer()
            {
                AllowEditItems     = false,
                EnableHighlighting = false,
                EnableSelection    = false
            };

            if (!string.IsNullOrWhiteSpace(options.Name))
                layer.Name = options.Name;

            var storage = new SqlGeometryItemStorage();
            int counter = 1;
            foreach (var wkt in WKT)
                storage.Items.Add(SqlGeometryItem.FromWkt(wkt, counter++));

            layer.Data = storage;

            map.Layers.Add(layer);
            CurrentLayer = layer;

            return this;
        }
    }
}
