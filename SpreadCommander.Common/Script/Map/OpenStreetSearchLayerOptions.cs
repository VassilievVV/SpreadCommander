using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class OpenStreetSearchLayerOptions: MapLayerSearchOptions
    {
        [Description("Local culture.")]
        public string Culture { get; set; }

        [Description("Country codes.")]
        public string[] CountryCodes { get; set; }

        [Description("Region to search for.")]
        public double[] BoundingBox { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddOpenStreetSearchLayer(string search, OpenStreetSearchLayerOptions options = null)
        {
            options ??= new OpenStreetSearchLayerOptions();

            SearchBoundingBox boundingBox = null;

            if (options.BoundingBox != null && options.BoundingBox.Length > 0 && options.BoundingBox.Length != 4)
                throw new Exception("Invalid bounding box. Shall be a double array with 4 elements.");

            if (options.BoundingBox != null && options.BoundingBox.Length == 4)
                boundingBox = new SearchBoundingBox(options.BoundingBox[0], options.BoundingBox[1], options.BoundingBox[2], options.BoundingBox[3]);

            var provider = new OsmSearchDataProvider()
            {
                GenerateLayerItems    = true,
                MaxVisibleResultCount = options.ResultCount,
                ResultsCount          = options.ResultCount,
                ProcessMouseEvents    = false
            };

            options.CreateLayer(this, provider);

            provider.Search(search, options.Culture, options.CountryCodes, boundingBox, options.ResultCount);

            return this;
        }
    }
}
