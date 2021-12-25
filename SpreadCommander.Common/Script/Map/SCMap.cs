using DevExpress.Map;
using DevExpress.XtraMap;
using DevExpress.XtraMap.Native;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Utils;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace SpreadCommander.Common.Script.Map
{
    public partial class SCMap : ScriptHostObject, IDisposable
    {
        protected internal InnerMap Map { get; set; }

        protected internal object CurrentLayer { get; set; }

        public SCMap(MapOptions options = null)
        {
            options ??= new MapOptions();
            InitializeMap(options);
        }

        public void Dispose()
        {
        }

        public void Clear()
        {
            if (Map?.ImageList is IDisposable images)
            {
                Map.ImageList = null;
                images.Dispose();
            }

            Map?.Dispose();
            Map = null;
        }

        protected internal CoordPoint CreateCoordPoint(double latitude, double longitude)
        {
            if (Map?.CoordinateSystem == null)
                throw new Exception("Map is not initialized.");

            return Map.CoordinateSystem switch
            {
                GeoMapCoordinateSystem _       => (CoordPoint)new GeoPoint(latitude, longitude),
                CartesianMapCoordinateSystem _ => (CoordPoint)new CartesianPoint(latitude, longitude),
                _                              => throw new Exception("Invalid coordinate system.")
            };
        }

        protected virtual void InitializeMap(MapOptions options)
        {
            Utils.StartProfile("MapScript");

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

            Map = map;

            switch (options.CoordinateSystem)
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
                switch (options.Projection ?? MapProjection.Default)
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
            else if ((options.Projection ?? MapProjection.Default) != MapProjection.Default)
                throw new Exception("Projection can be set only in Geo coordinate system.");

            if (options.ImageList != null && options.ImageList.Length > 0)
            {
                ExecuteLocked(() =>
                {
                    var images = new ImageCollection();
                    if (options.ImageSize.HasValue)
                        images.ImageSize = options.ImageSize.Value;
                    foreach (var imageFile in options.ImageList)
                    {
                        var imagePath = Project.Current.MapPath(imageFile);
                        if (string.IsNullOrWhiteSpace(imagePath) || !System.IO.File.Exists(imagePath))
                            throw new Exception($"Cannot find image: '{imagePath}'.");
                        var bmp = new Bitmap(imagePath);
                        images.AddImage(bmp);
                    }
                    map.ImageList = images;
                }, options.LockFiles ? LockObject : null);
            }

            var backColor = Utils.ColorFromString(options.BackColor);
            if (backColor != Color.Empty)
                map.BackColor = backColor;
        }

        protected internal virtual Bitmap PaintMap(InnerMap map)
        {
            using var stream = new MemoryStream();
            map.ExportToImage(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);

            var image = new Bitmap(stream);
            return image;
        }

        protected internal void ScrollToCaret()
        {
            Host?.Engine?.ScrollToCaret();
        }

        protected internal object GetLayer(string layerName)
        {
            if (string.IsNullOrWhiteSpace(layerName))
                return CurrentLayer;
            return Map.Layers[layerName];
        }
    }
}
