using DevExpress.XtraMap;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace SpreadCommander.Common.Script.Map
{
    public class LayerVectorFileOptions
    {
        [Description("Layer's name.")]
        public string Name { get; set; }

        [Description("Type of vector file - Auto, KML, SHP or SVG.")]
        public VectorFileType? FileType { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddLayerVectorFile(string vectorFile, LayerVectorFileOptions options = null)
        {
            options ??= new LayerVectorFileOptions();

            var map = Map;

            var layer = new VectorItemsLayer()
            {
                AllowEditItems     = false,
                EnableHighlighting = false,
                EnableSelection    = false
            };

            if (!string.IsNullOrWhiteSpace(options.Name))
                layer.Name = options.Name;

            vectorFile = Project.Current.MapPath(vectorFile);
            if (string.IsNullOrWhiteSpace(vectorFile) || !File.Exists(vectorFile))
                throw new Exception($"Cannot find file: {vectorFile}");

            var fileType = options.FileType ?? VectorFileType.Auto;
            if (fileType == VectorFileType.Auto)
            {
                var ext = Path.GetExtension(vectorFile);
                fileType = ext?.ToLower() switch
                {
                    ".kml" => VectorFileType.KML,
                    ".shp" => VectorFileType.SHP,
                    ".svg" => VectorFileType.SVG,
                    _      => throw new Exception("Cannot determine file type. Set property FileType to KML, SHP or SVG.")
                };
            }
            var data = fileType switch
            {
                VectorFileType.KML => (FileDataAdapterBase)new KmlFileDataAdapter(),
                VectorFileType.SHP => (FileDataAdapterBase)new ShapefileDataAdapter(),
                VectorFileType.SVG => (FileDataAdapterBase)new SvgFileDataAdapter(),
                _ => throw new Exception("Cannot determine file type. Set property FileType to KML, SHP or SVG."),
            };
            data.FileUri = new Uri($"file://{vectorFile}");

            if (map.CoordinateSystem != null && map.CoordinateSystem is CartesianMapCoordinateSystem)
                data.SourceCoordinateSystem = new CartesianSourceCoordinateSystem();

            layer.Data   = data;

            map.Layers.Add(layer);
            CurrentLayer = layer;

            return this;
        }
    }
}
