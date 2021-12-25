using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class PolylineOptions: ShapeOptions
    {
        [Description("Indicating whether the Polyline is geodesic.")]
        public bool Geodesic { get; set; }


        protected internal override void ConfigureMapItem(SCMap map, MapItem item)
        {
            base.ConfigureMapItem(map, item);

            var mapItem = item as MapPolyline ?? throw new Exception("Map item must be MapPolyline.");

            mapItem.IsGeodesic = Geodesic;
        }
    }

    public partial class SCMap
    {
        public SCMap AddPolyline(double[][] points, PolylineOptions options = null)
        {
            options ??= new PolylineOptions();

            var mapItem = new MapPolyline();

            if (points == null || points.Length <= 1)
                throw new Exception("Points are not properly defined.");

            foreach (double[] point in points)
            {
                if (point == null || point.Length != 2)
                    throw new Exception("Point is not property defined. Must be double array with 2 elements.");

                mapItem.Points.Add(CreateCoordPoint(point[0], point[1]));
            }

            options.ConfigureMapItem(this, mapItem);

            return this;
        }
    }
}
