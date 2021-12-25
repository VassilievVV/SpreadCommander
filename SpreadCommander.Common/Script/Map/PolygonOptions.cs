using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class PolygonOptions: ShapeOptions
    {
        [Description("Image that will be used as background of map polygon.")]
        public Image Image { get; set; }

        [Description("Filename of the image that will be used as background of map polygon.")]
        public string ImageFile { get; set; }

        [Description("Image transparency")]
        public byte? ImageTransparency { get; set; }


        protected internal override void ConfigureMapItem(SCMap map, MapItem item)
        {
            base.ConfigureMapItem(map, item);

            var mapItem = item as MapPolygon ?? throw new Exception("Map item must be MapPolygon.");

            if (Image != null || !string.IsNullOrWhiteSpace(ImageFile))
            {
                if (Image != null)
                    mapItem.Image.Source = Image;
                else if (!string.IsNullOrWhiteSpace(ImageFile))
                {
                    var imageFile = Project.Current.MapPath(ImageFile);
                    if (string.IsNullOrWhiteSpace(imageFile) || !File.Exists(ImageFile))
                        throw new Exception($"Cannot find file: {ImageFile}");

                    mapItem.Image.Source = Image.FromFile(ImageFile);
                }

                if (ImageTransparency.HasValue)
                    mapItem.Image.Transparency = ImageTransparency.Value;
            }
        }
    }

    public partial class SCMap
    {
        public SCMap AddPolygon(double[][] points, PolygonOptions options = null)
        {
            options ??= new PolygonOptions();

            var mapItem = new MapPolygon();

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
