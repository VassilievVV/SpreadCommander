using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class EllipseOptions: ShapeOptions
    {
    }

    public partial class SCMap
    {
        public SCMap AddEllipse(double[] location, double width, double height, EllipseOptions options = null)
        {
            options ??= new EllipseOptions();

            var mapItem = new MapEllipse();

            if (location == null || location.Length != 2)
                throw new Exception("Location must be double array with 2 elements.");

            mapItem.Location = CreateCoordPoint(location[0], location[1]);

            mapItem.Height = height;
            mapItem.Width  = width;

            options.ConfigureMapItem(this, mapItem);

            return this;
        }
    }
}
