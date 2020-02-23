using DevExpress.XtraMap;
using SpreadCommander.Common.PowerShell.CmdLets.Map.MapSearchLayerContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapLayerSearch")]
	public class AddMapLayerSearchCmdlet: BaseMapWithContextCmdlet, IDynamicParameters
	{
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Search provider type - Bing, OpenStreet.")]
		public SearchProviderType ProviderType { get; set; }

		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Information to search on a map. This string can contain either geographical names or any other information to look for.")]
		public string Search { get; set; }

		[Parameter(HelpMessage = "Number of results that can be obtained by a search request.")]
		[PSDefaultValue(Value = 10)]
		[DefaultValue(10)]
		public int ResultCount { get; set; } = 10;


		private BaseSearchLayerContext _SearchLayerContext;

		public object GetDynamicParameters()
		{
			switch (ProviderType)
			{
				case SearchProviderType.Bing:
					return CreateSearchLayerContext(typeof(BingSearchLayerContext));
				case SearchProviderType.OpenStreet:
					return CreateSearchLayerContext(typeof(OpenStreetSearchLayerContext));
				default:
					break;
			}

			_SearchLayerContext = null;
			return null;


			BaseSearchLayerContext CreateSearchLayerContext(Type typeContext)
			{
				if (_SearchLayerContext == null || !(typeContext.IsInstanceOfType(_SearchLayerContext)))
					_SearchLayerContext = Activator.CreateInstance(typeContext) as BaseSearchLayerContext;
				return _SearchLayerContext;
			}
		}

		protected override void UpdateMapRecord()
		{
			var layer = CreateLayer();

			var map = MapContext.Map;
			map.Layers.Add(layer);

			MapContext.CurrentLayer = layer;
		}

		protected InformationLayer CreateLayer()
		{
			var layer = new InformationLayer()
			{
				EnableHighlighting = false,
				EnableSelection    = false
			};

			if (_SearchLayerContext != null)
				_SearchLayerContext.MapContext = MapContext;

			var provider = _SearchLayerContext?.CreateDataProvider(layer, Search, ResultCount) ?? throw new Exception("Cannot create search provider.");

			if (layer.DataProvider == null)
				layer.DataProvider = provider;

			return layer;
		}
	}
}
