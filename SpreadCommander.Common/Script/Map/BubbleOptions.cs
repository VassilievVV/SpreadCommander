using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class BubbleOptions: ShapeOptions
    {
        [Description("Argument for Bubble.")]
        public string Argument { get; set; }

        [Description("Value of the Bubble.")]
        public double? Value { get; set; }

        [Description("Group to which the current MapBubble belongs.")]
        public int? Group { get; set; }

        [Description("Appearance of a bubble marker.")]
        public MarkerType? MarkerType { get; set; }


        protected internal override void ConfigureMapItem(SCMap map, DevExpress.XtraMap.MapItem item)
        {
            base.ConfigureMapItem(map, item);

            var mapItem = item as MapBubble ?? throw new Exception("Map item must be MapBubble.");

            mapItem.Argument = Argument;
            if (Value.HasValue)
                mapItem.Value = Value.Value;
            if (Group.HasValue)
                mapItem.Group = Group.Value;
            if (MarkerType.HasValue)
                mapItem.MarkerType = (DevExpress.XtraMap.MarkerType)MarkerType.Value;
        }
    }

    public partial class SCMap
    {
        public SCMap AddBubble(double[] location, int? size, BubbleOptions options = null)
        {
            if (location == null || location.Length != 2)
                throw new Exception("Location must be double array with 2 elements.");

            options ??= new BubbleOptions();

            var mapItem = new MapBubble()
            {
                Location = CreateCoordPoint(location[0], location[1])
            };

            if (size.HasValue)
                mapItem.Size = size.Value;

            options.ConfigureMapItem(this, mapItem);

            return this;
        }
    }
}
