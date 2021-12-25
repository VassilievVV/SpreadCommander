using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class ClustererOptions
    {
        [Description("Layer's name to which add map item.")]
        public string Layer { get; set; }

        [Description("Type of the clusterer.")]
        public ClustererType? Type { get; set; }

        [Description("Whether or not the map items that can't be clustered should be displayed.")]
        public bool DisplayNonClusteredItems { get; set; }

        [Description("Maximum size of a map item that is the cluster representative.")]
        public int? ItemMaxSize { get; set; }

        [Description("Minimum size of a map item that is the cluster representative.")]
        public int? ItemMinSize { get; set; }

        [Description("Maximum distance between MapItem objects inside one cluster.")]
        public int? Size { get; set; }

        [Description("Group field used to separate an initial list of items into groups.")]
        public string GroupField { get; set; }


        protected internal virtual void UpdateMap(SCMap map)
        {
            var layer = map.GetLayer(Layer) as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to vector items layer created with command AddMapLayerVectorItems.");

            var dataAdapter = layer.Data as MapDataAdapterBase ??
                throw new Exception("Cannot apply clusterer to current layer.");

            var clusterer = (Type ?? ClustererType.Distance) switch
            {
                ClustererType.Distance => (MapClustererBase)new DistanceBasedClusterer(),
                ClustererType.Marker   => (MapClustererBase)new MarkerClusterer(),
                _                      => throw new Exception("Unrecognized clusterer type.")
            };
            clusterer.DisplayNonClusteredItems = DisplayNonClusteredItems;

            if (ItemMaxSize.HasValue)
                clusterer.ItemMaxSize = ItemMaxSize.Value;
            if (ItemMinSize.HasValue)
                clusterer.ItemMinSize = ItemMinSize.Value;
            if (Size.HasValue)
                clusterer.StepInPixels = Size.Value;

            if (!string.IsNullOrWhiteSpace(GroupField))
                clusterer.GroupProvider = new AttributeGroupProvider() { AttributeName = GroupField };

            dataAdapter.Clusterer = clusterer;
        }
    }

    public partial class SCMap
    {
        public SCMap AddClusterer(ClustererOptions options = null)
        {
            options ??= new ClustererOptions();
            options.UpdateMap(this);

            return this;
        }
    }
}
