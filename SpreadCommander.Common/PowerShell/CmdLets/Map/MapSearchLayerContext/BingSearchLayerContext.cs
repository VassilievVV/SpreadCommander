using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapSearchLayerContext
{
	public class BingSearchLayerContext: BaseSearchLayerContext
	{
		[Parameter(HelpMessage = "Local culture.")]
		public string Culture { get; set; }

		[Parameter(HelpMessage = "Geographical point, around which it is necessary to search.")]
		public double[] AnchorPoint { get; set; }

		[Parameter(HelpMessage = "Region to search for.")]
		public double[] BoundingBox { get; set; }


		public override InformationDataProviderBase CreateDataProvider(InformationLayer layer, string search, int resultCount)
		{
			GeoPoint anchorPoint = null;

			if (AnchorPoint != null && AnchorPoint.Length > 0 && AnchorPoint.Length != 2)
				throw new Exception("Invalid anchor point. Shall be a double array with 2 elements.");

			if (AnchorPoint != null && AnchorPoint.Length == 2)
				anchorPoint = new GeoPoint(AnchorPoint[0], AnchorPoint[1]);

			SearchBoundingBox boundingBox = null;

			if (BoundingBox != null && BoundingBox.Length > 0 && BoundingBox.Length != 4)
				throw new Exception("Invalid bounding box. Shall be a double array with 4 elements.");

			if (BoundingBox != null && BoundingBox.Length == 4)
				boundingBox = new SearchBoundingBox(BoundingBox[0], BoundingBox[1], BoundingBox[2], BoundingBox[3]);

			var provider = new BingSearchDataProvider()
			{
				BingKey               = Parameters.BingMapKey,
				ConnectionProtocol    = ConnectionProtocol.Https,
				GenerateLayerItems    = true,
				MaxVisibleResultCount = resultCount,
				ProcessMouseEvents    = false
			};
			provider.SearchOptions.ResultsCount = resultCount;

			layer.DataProvider = provider;

			/*
			var completion = new TaskCompletionSource<InformationDataProviderBase>();

			provider.SearchCompleted += (s, e) =>
			{
				completion.SetResult(provider);
			};
			*/

			provider.Search(search, Culture, anchorPoint, boundingBox);

			//return completion.Task.Result;
			return provider;
		}
	}
}
