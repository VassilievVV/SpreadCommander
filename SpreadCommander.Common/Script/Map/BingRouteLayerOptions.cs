using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class BingRouteLayerOptions: MapLayerRouteOptions
    {
        [Description("Option that allows limitation of the use of the toll or highway roads.")]
        public BingAvoidRoads? AvoidRoads { get; set; }

        [Description("Value that defines how to measure distances.")]
        public DistanceMeasureUnit? DistanceUnit { get; set; }

        [Description("Value that defines how a route should be calculated.")]
        public BingTravelMode? Mode { get; set; }

        [Description("Value that defines how to optimize the route calculation.")]
        public BingRouteOptimization? RouteOptimization { get; set; }

        [Description("Local culture.")]
        public string Culture { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddBingRouteLayer(BingRouteWayPoint[] wayPoints, BingRouteLayerOptions options = null)
        {
            if (wayPoints == null || wayPoints.Length < 1)
                throw new Exception("Route requires at least 1 way point.");

            options ??= new BingRouteLayerOptions();

            var provider = new BingRouteDataProvider()
            {
                BingKey               = Parameters.BingMapKey,
                ConnectionProtocol    = ConnectionProtocol.Https,
                GenerateLayerItems    = true,
                MaxVisibleResultCount = options.ResultCount,
                ProcessMouseEvents    = false
            };
            if (options.AvoidRoads.HasValue)
                provider.RouteOptions.AvoidRoads = (DevExpress.XtraMap.BingAvoidRoads)options.AvoidRoads.Value;
            if (options.DistanceUnit.HasValue)
                provider.RouteOptions.DistanceUnit = (DevExpress.XtraMap.DistanceMeasureUnit)options.DistanceUnit.Value;
            if (options.Mode.HasValue)
                provider.RouteOptions.Mode = (DevExpress.XtraMap.BingTravelMode)options.Mode.Value;
            if (options.RouteOptimization.HasValue)
                provider.RouteOptions.RouteOptimization = (DevExpress.XtraMap.BingRouteOptimization)options.RouteOptimization.Value;

            var routePoints = new List<RouteWaypoint>();

            foreach (var bingWayPoint in wayPoints)
            {
                var routePoint = ConvertWayPoint(bingWayPoint);
                routePoints.Add(routePoint);
            }

            options.CreateLayer(this, provider);

            provider.CalculateRoute(routePoints, options.Culture, null);

            return this;


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
