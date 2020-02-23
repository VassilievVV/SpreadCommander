using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsCommon.Add, "MapClusterer")]
    public class AddMapClustererCmdlet: BaseMapWithContextCmdlet
    {
        [Parameter(HelpMessage = "Type of the clusterer.")]
        public ClustererType? Type { get; set; }

        [Parameter(HelpMessage = "Whether or not the map items that can't be clustered should be displayed.")]
        public SwitchParameter DisplayNonClusteredItems { get; set; }

        [Parameter(HelpMessage = "Maximum size of a map item that is the cluster representative.")]
        public int? ItemMaxSize { get; set; }

        [Parameter(HelpMessage = "Minimum size of a map item that is the cluster representative.")]
        public int? ItemMinSize { get; set; }

        [Parameter(HelpMessage = "Maximum distance between MapItem objects inside one cluster.")]
        public int? Size { get; set; }

        [Parameter(HelpMessage = "Group field used to separate an initial list of items into groups.")]
        public string GroupField { get; set; }


        protected override void UpdateMap()
        {
            var layer = MapContext.CurrentLayer as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");

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
}
