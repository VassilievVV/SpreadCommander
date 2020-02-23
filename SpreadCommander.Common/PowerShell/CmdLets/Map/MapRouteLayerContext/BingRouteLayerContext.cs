using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapRouteLayerContext
{
    public class BingRouteLayerContext: BaseRouteLayerContext
    {
        [Parameter(Mandatory = true, Position = 1,
            HelpMessage = "Array of way points. One way point calculates road from major roads, multiple way points - calculates road between points.")]
        public BingRouteWayPoint[] WayPoints { get; set; }

        [Parameter(HelpMessage = "Option that allows limitation of the use of the toll or highway roads.")]
        public BingAvoidRoads? AvoidRoads { get; set; }

        [Parameter(HelpMessage = "Value that defines how to measure distances.")]
        public DistanceMeasureUnit? DistanceUnit { get; set; }

        [Parameter(HelpMessage = "Value that defines how a route should be calculated.")]
        public BingTravelMode? Mode { get; set; }

        [Parameter(HelpMessage = "Value that defines how to optimize the route calculation.")]
        public BingRouteOptimization? RouteOptimization { get; set; }

        [Parameter(HelpMessage = "Local culture.")]
        public string Culture { get; set; }


        public override InformationDataProviderBase CreateDataProvider(InformationLayer layer, int resultCount)
        {
            var provider = new BingRouteDataProvider()
            {
                BingKey               = Parameters.BingMapKey,
                ConnectionProtocol    = ConnectionProtocol.Https,
                GenerateLayerItems    = true,
                MaxVisibleResultCount = resultCount,
                ProcessMouseEvents    = false
            };
            if (AvoidRoads.HasValue)
                provider.RouteOptions.AvoidRoads = AvoidRoads.Value;
            if (DistanceUnit.HasValue)
                provider.RouteOptions.DistanceUnit = DistanceUnit.Value;
            if (Mode.HasValue)
                provider.RouteOptions.Mode = Mode.Value;
            if (RouteOptimization.HasValue)
                provider.RouteOptions.RouteOptimization = RouteOptimization.Value;

            var wayPoints = new List<RouteWaypoint>();
            if (WayPoints == null || WayPoints.Length < 1)
                throw new Exception("Route requires at least 1 way point.");

            foreach (var bingWayPoint in WayPoints)
            {
                var wayPoint = ConvertWayPoint(bingWayPoint);
                wayPoints.Add(wayPoint);
            }

            layer.DataProvider = provider;

            provider.CalculateRoute(wayPoints, Culture, null);

            return provider;


            static RouteWaypoint ConvertWayPoint(BingRouteWayPoint point)
            {
                if (!string.IsNullOrWhiteSpace(point.Keyword))
                    return new RouteWaypoint(point.Description, point.Keyword);

                if (point.Location == null || point.Location.Length != 2)
                    throw new Exception("Route way point shall have either Keyword or Location (double array with 2 elements).");

                return new RouteWaypoint(point.Description, new GeoPoint(point.Location[0], point.Location[1]));
            }
        }
    }
}
