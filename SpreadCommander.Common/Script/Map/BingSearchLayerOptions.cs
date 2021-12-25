using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class BingSearchLayerOptions: MapLayerSearchOptions
    {
        [Description("Local culture.")]
        public string Culture { get; set; }

        [Description("Geographical point, around which it is necessary to search.")]
        public double[] AnchorPoint { get; set; }

        [Description("Region to search for.")]
        public double[] BoundingBox { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddBingSearchLayer(string search, BingSearchLayerOptions options = null)
        {
            options ??= new BingSearchLayerOptions();

            GeoPoint anchorPoint = null;

            if (options.AnchorPoint != null && options.AnchorPoint.Length > 0 && options.AnchorPoint.Length != 2)
                throw new Exception("Invalid anchor point. Shall be a double array with 2 elements.");

            if (options.AnchorPoint != null && options.AnchorPoint.Length == 2)
                anchorPoint = new GeoPoint(options.AnchorPoint[0], options.AnchorPoint[1]);

            SearchBoundingBox boundingBox = null;

            if (options.BoundingBox != null && options.BoundingBox.Length > 0 && options.BoundingBox.Length != 4)
                throw new Exception("Invalid bounding box. Shall be a double array with 4 elements.");

            if (options.BoundingBox != null && options.BoundingBox.Length == 4)
                boundingBox = new SearchBoundingBox(options.BoundingBox[0], options.BoundingBox[1], options.BoundingBox[2], options.BoundingBox[3]);

            var provider = new BingSearchDataProvider()
            {
                BingKey               = Parameters.BingMapKey,
                ConnectionProtocol    = ConnectionProtocol.Https,
                GenerateLayerItems    = true,
                MaxVisibleResultCount = options.ResultCount,
                ProcessMouseEvents    = false
            };
            provider.SearchOptions.ResultsCount = options.ResultCount;

            options.CreateLayer(this, provider);

            provider.Search(search, options.Culture, anchorPoint, boundingBox);

            return this;
        }
    }
}
