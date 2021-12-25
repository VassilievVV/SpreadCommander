using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class LineOptions: ShapeOptions
    {
    }

    public partial class SCMap
    {
        public SCMap AddLine(double[] point1, double[] point2, bool geodesic, LineOptions options = null)
        {
            options ??= new LineOptions();

            var mapItem = new MapLine();

            if (point1 == null || point1.Length != 2)
                throw new Exception("Point1 must be double array with 2 elements.");
            if (point2 == null || point2.Length != 2)
                throw new Exception("Point2 must be double array with 2 elements.");

            mapItem.Point1     = CreateCoordPoint(point1[0], point1[1]);
            mapItem.Point2     = CreateCoordPoint(point2[0], point2[1]);
            mapItem.IsGeodesic = geodesic;

            options.ConfigureMapItem(this, mapItem);

            return this;
        }
    }
}
