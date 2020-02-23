using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
    public class MapBubbleContext: MapShapeContext
    {
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Location of the Bubble.")]
        public double[] Location { get; set; }

        [Parameter(HelpMessage = "Argument for Bubble.")]
        public string Argument { get; set; }

        [Parameter(HelpMessage = "Value of the Bubble.")]
        public double? Value { get; set; }

        [Parameter(Position = 1, HelpMessage = "Bubble size in pixels.")]
        public int? Size { get; set; }

        [Parameter(HelpMessage = "Group to which the current MapBubble belongs.")]
        public int? Group { get; set; }

        [Parameter(HelpMessage = "Appearance of a bubble marker.")]
        public MarkerType? MarkerType { get; set; }


        public override MapItem CreateMapItem() => new MapBubble();

        public override void ConfigureMapItem(MapItem item)
        {
            base.ConfigureMapItem(item);

            var mapItem = item as MapBubble ?? throw new Exception("Map item must be MapBubble.");

            if (Location == null || Location.Length != 2)
                throw new Exception("Location must be double array with 2 elements.");

            mapItem.Location = MapContext.CreateCoordPoint(Location[0], Location[1]);

            mapItem.Argument = Argument;
            if (Value.HasValue)
                mapItem.Value = Value.Value;
            if (Size.HasValue)
                mapItem.Size = Size.Value;
            if (Group.HasValue)
                mapItem.Group = Group.Value;
            if (MarkerType.HasValue)
                mapItem.MarkerType = MarkerType.Value;
        }
    }
}
