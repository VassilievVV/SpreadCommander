using DevExpress.Utils;
using DevExpress.XtraMap;
using DevExpress.XtraMap.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsCommon.New, "Map")]
    public class NewMapCmdlet: BaseMapCmdlet
    {
        [Parameter(HelpMessage = "Coordinate system of a map.")]
        public MapCoordinateSystem CoordinateSystem { get; set; }

        [Parameter(HelpMessage = "Projection used by the map data provider.")]
        public MapProjection? Projection { get; set; }

        [Parameter(HelpMessage = "List of images to use in map item. All images shall have same size.")]
        public string[] ImageList { get; set; }

        [Parameter(HelpMessage = "Size of images in ImageList.")]
        public Size? ImageSize { get; set; }

        [Parameter(HelpMessage = "Background color of the map.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        protected override void EndProcessing()
        {
            Utils.StartProfile("MapCmdlet");

            var map = new InnerMap()
            {
                EnableAnimation        = false,
                EnableScrolling        = false,
                EnableZooming          = false, 
                EnableDelayedScrolling = false
            };

            map.NavigationPanelOptions.ShowCoordinates     = false;
            map.NavigationPanelOptions.ShowKilometersScale = false;
            map.NavigationPanelOptions.ShowMilesScale      = false;

            switch (CoordinateSystem)
            {
                case MapCoordinateSystem.Geo:
                    map.CoordinateSystem  = new GeoMapCoordinateSystem();
                    break;
                case MapCoordinateSystem.Cartesian:
                    map.CoordinateSystem = new CartesianMapCoordinateSystem();
                    break;
            }

            if (map.CoordinateSystem is GeoMapCoordinateSystem geoCoordSystem)
            {
                switch (Projection ?? MapProjection.Default)
                {
                    case MapProjection.Default:
                        //Leave projection as is
                        break;
                    case MapProjection.BraunStereographic:
                        geoCoordSystem.Projection = new BraunStereographicProjection();
                        break;
                    case MapProjection.EllipticalMercator:
                        geoCoordSystem.Projection = new EllipticalMercatorProjection();
                        break;
                    case MapProjection.EqualArea:
                        geoCoordSystem.Projection = new EqualAreaProjection();
                        break;
                    case MapProjection.Equidistant:
                        geoCoordSystem.Projection = new EquidistantProjection();
                        break;
                    case MapProjection.Equirectangular:
                        geoCoordSystem.Projection = new EquirectangularProjection();
                        break;
                    case MapProjection.Kavrayskiy:
                        geoCoordSystem.Projection = new KavrayskiyProjection();
                        break;
                    case MapProjection.LambertCylindricalEqualArea:
                        geoCoordSystem.Projection = new LambertCylindricalEqualAreaProjection();
                        break;
                    case MapProjection.Miller:
                        geoCoordSystem.Projection = new MillerProjection();
                        break;
                    case MapProjection.Sinusoidal:
                        geoCoordSystem.Projection = new SinusoidalProjection();
                        break;
                    case MapProjection.SphericalMercator:
                        geoCoordSystem.Projection = new SphericalMercatorProjection();
                        break;
                }
            }
            else if ((Projection ?? MapProjection.Default) != MapProjection.Default)
                throw new Exception("Projection can be set only in Geo coordinate system.");

            if (ImageList != null && ImageList.Length > 0)
            {
                ExecuteLocked(() =>
                {
                    var images = new ImageCollection();
                    if (ImageSize != null)
                        images.ImageSize = ImageSize.Value;
                    foreach (var imageFile in ImageList)
                    {
                        var imagePath = Project.Current.MapPath(imageFile);
                        if (string.IsNullOrWhiteSpace(imagePath) || !System.IO.File.Exists(imagePath))
                            throw new Exception($"Cannot find image: '{imagePath}'.");
                        var bmp = new Bitmap(imagePath);
                        images.AddImage(bmp);
                    }
                    map.ImageList = images;
                }, LockFiles ? LockObject : null);
            }

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                map.BackColor = backColor;

            var context = new MapContext()
            {
                Map = map
            };

            WriteObject(context);
        }
    }
}
