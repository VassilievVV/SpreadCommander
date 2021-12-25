using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class DotOptions: ShapeOptions
    {
        [Description("Dot size in pixels.")]
        public int? Size { get; set; }

        [Description("Shape that is used to draw a dot on a map.")]
        public MapDotShapeKind? ShapeKind { get; set; }


        protected internal override void ConfigureMapItem(SCMap map, DevExpress.XtraMap.MapItem item)
        {
            base.ConfigureMapItem(map, item);

            var mapItem = item as MapDot ?? throw new Exception("Map item must be MapDot.");

            if (Size.HasValue)
                mapItem.Size = Size.Value;
            if (ShapeKind.HasValue)
                mapItem.ShapeKind = (DevExpress.XtraMap.MapDotShapeKind)ShapeKind.Value;
        }
    }

    public partial class SCMap
    {
        public SCMap AddDot(double[] location, DotOptions options = null)
        {
            options ??= new DotOptions();

            var mapItem = new MapDot();

            if (location == null || location.Length != 2)
                throw new Exception("Location must be double array with 2 elements.");

            mapItem.Location = CreateCoordPoint(location[0], location[1]);

            options.ConfigureMapItem(this, mapItem);

            return this;
        }
    }
}
