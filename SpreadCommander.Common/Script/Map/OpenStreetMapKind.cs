using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public enum OpenStreetMapKind
    {
#pragma warning disable CA1069 // Enums values should not be duplicated
        Mapquest          = -1,
        MapquestSatellite = -1,
        SkiMap            = -1,
#pragma warning restore CA1069 // Enums values should not be duplicated
        Basic = 0,
        CycleMap          = 1,
        Hot               = 2,
        GrayScale         = 3,
        Transport         = 11,
        SeaMarks          = 14,
        HikingRoutes      = 15,
        CyclingRoutes     = 16,
        PublicTransport   = 17
    }
}
