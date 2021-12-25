using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class RectangleOptions: ShapeOptions
    {
    }

    public partial class SCMap
    {
        public SCMap AddRectangle(double[] location, double width, double height, RectangleOptions options = null)
        {
            options ??= new RectangleOptions();

            var mapItem = new MapRectangle();

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
