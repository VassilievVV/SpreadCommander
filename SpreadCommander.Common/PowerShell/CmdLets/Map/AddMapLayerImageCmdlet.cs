using DevExpress.XtraMap;
using SpreadCommander.Common.PowerShell.CmdLets.Map.MapImageLayerContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsCommon.Add, "MapLayerImage")]
    public class AddMapLayerImageCmdlet: BaseMapWithContextCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Image layer type - Bing, OpenStreet")]
        public ImageLayerType LayerType { get; set; }

        [Parameter(HelpMessage = "Layer's name.")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "When set - layer is added to mini map.")]
        public SwitchParameter MiniMap { get; set; }

        [Parameter(HelpMessage = "Layer's transparency.")]
        public byte? Transparency { get; set; }


        private BaseImageLayerContext _ImageLayerContext;

        public object GetDynamicParameters()
        {
            switch (LayerType)
            {
                case ImageLayerType.Bing:
                    return CreateImageLayerContext(typeof(BingImageLayerContext));
                case ImageLayerType.OpenStreet:
                    return CreateImageLayerContext(typeof(OpenStreetImageLayerContext));
                case ImageLayerType.WMS:
                    return CreateImageLayerContext(typeof(WmsImageLayerContext));
                case ImageLayerType.Heatmap:
                    return CreateImageLayerContext(typeof(HeatmapImageLayerContext));
                default:
                    break;
            }

            _ImageLayerContext = null;
            return null;


            BaseImageLayerContext CreateImageLayerContext(Type typeContext)
            {
                if (_ImageLayerContext == null || !(typeContext.IsInstanceOfType(_ImageLayerContext)))
                    _ImageLayerContext = Activator.CreateInstance(typeContext) as BaseImageLayerContext;
                return _ImageLayerContext;
            }
        }

        protected override void UpdateMapRecord()
        {
            var provider = _ImageLayerContext?.CreateMapDataProvider() ?? throw new Exception("Cannot create image layer.");
            var map      = MapContext.Map;

            if (!MiniMap)
            {
                var layer = new ImageLayer()
                {
                    DataProvider       = provider,
                    EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True,
                };
                if (Transparency.HasValue)
                    layer.Transparency = Transparency.Value;

                if (!string.IsNullOrWhiteSpace(Name))
                    layer.Name = Name;

                map.Layers.Add(layer);
                MapContext.CurrentLayer = layer;
            }
            else
            {
                var dataProvider = provider as MapDataProviderBase ?? throw new Exception("MiniMap does not support selected data provider.");

                var layerMini = new MiniMapImageTilesLayer()
                {
                    DataProvider = dataProvider
                };

                if (!string.IsNullOrWhiteSpace(Name))
                    layerMini.Name = Name;

                if (map.MiniMap == null)
                    throw new Exception("Mini map is not created.");

                map.MiniMap.Layers.Add(layerMini);
                MapContext.CurrentLayer = layerMini;
            }
        }
    }
}
