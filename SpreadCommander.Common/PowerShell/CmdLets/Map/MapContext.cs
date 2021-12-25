using DevExpress.Map;
using DevExpress.XtraMap;
using DevExpress.XtraMap.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    //Class using to move Map through PowerShell pipe.
    public class MapContext: IDisposable
    {
        [Browsable(false)]
        public InnerMap Map { get; set; }

        [Browsable(false)]
        public object CurrentLayer { get; set; }


        public void Dispose()
        {
            Clear();
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

        public CoordPoint CreateCoordPoint(double latitude, double longitude)
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
    }
}
