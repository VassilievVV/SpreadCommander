using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
    public class MapPolygonContext: MapShapeContext
    {
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Collection of points defining the map polygon. 2D double array in form @(@(10, -170), @(10, 170)).")]
        public double[][] Points { get; set; }

        [Parameter(HelpMessage = "Image that will be used as background of map polygon.")]
        public Image Image { get; set; }

        [Parameter(HelpMessage = "Filename of the image that will be used as background of map polygon.")]
        public string ImageFile { get; set; }

        [Parameter(HelpMessage = "Image transparency")]
        public byte? ImageTransparency { get; set; }


        public override MapItem CreateMapItem() => new MapPolygon();

        public override void ConfigureMapItem(MapItem item)
        {
            base.ConfigureMapItem(item);

            var mapItem = item as MapPolygon ?? throw new Exception("Map item must be MapPolygon.");

            if (Points == null || Points.Length <= 1)
                throw new Exception("Points are not properly defined.");

            foreach (double[] point in Points)
            {
                if (point == null || point.Length != 2)
                    throw new Exception("Point is not property defined. Must be double array with 2 elements.");

                mapItem.Points.Add(MapContext.CreateCoordPoint(point[0], point[1]));
            }

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
}
