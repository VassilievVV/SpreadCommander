using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapSearchLayerContext
{
	public class OpenStreetSearchLayerContext: BaseSearchLayerContext
	{
		[Parameter(HelpMessage = "Local culture.")]
		public string Culture { get; set; }

		[Parameter(HelpMessage = "Country codes.")]
		public string[] CountryCodes { get; set; }

		[Parameter(HelpMessage = "Region to search for.")]
		public double[] BoundingBox { get; set; }

		public override InformationDataProviderBase CreateDataProvider(InformationLayer layer, string search, int resultCount)
		{
			SearchBoundingBox boundingBox = null;

			if (BoundingBox != null && BoundingBox.Length > 0 && BoundingBox.Length != 4)
				throw new Exception("Invalid bounding box. Shall be a double array with 4 elements.");

			if (BoundingBox != null && BoundingBox.Length == 4)
				boundingBox = new SearchBoundingBox(BoundingBox[0], BoundingBox[1], BoundingBox[2], BoundingBox[3]);

			var provider = new OsmSearchDataProvider()
			{
				GenerateLayerItems    = true,
				MaxVisibleResultCount = resultCount,
				ResultsCount          = resultCount,
				ProcessMouseEvents    = false
			};

			layer.DataProvider = provider;

			/*
			var completion = new TaskCompletionSource<InformationDataProviderBase>();

			provider.SearchCompleted += (s, e) =>
			{
				completion.SetResult(provider);
			};
			*/

			provider.Search(search, Culture, CountryCodes, boundingBox, resultCount);

			//return completion.Task.Result;

			return provider;
		}
	}
}
