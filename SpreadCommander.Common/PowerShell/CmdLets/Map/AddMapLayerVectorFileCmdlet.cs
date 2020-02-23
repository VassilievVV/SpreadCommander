using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsCommon.Add, "MapLayerVectorFile")]
    public class AddMapLayerVectorFileCmdlet: BaseMapWithContextCmdlet
    {
        public enum VectorFileType { Auto, KML, SHP, SVG }

        [Parameter(HelpMessage = "Layer's name.")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of KML, SHP of SVG file.")]
        [Alias("File")]
        public string VectorFile { get; set; }

        [Parameter(HelpMessage = "Type of vector file - Auto, KML, SHP or SVG.")]
        public VectorFileType? FileType { get; set; }


        protected override void UpdateMap()
        {
            var map = MapContext.Map;

            var layer = new VectorItemsLayer()
            {
                AllowEditItems     = false,
                EnableHighlighting = false,
                EnableSelection    = false
            };

            if (!string.IsNullOrWhiteSpace(Name))
                layer.Name = Name;

            var vectorFile = Project.Current.MapPath(VectorFile);
            if (string.IsNullOrWhiteSpace(vectorFile) || !File.Exists(vectorFile))
                throw new Exception($"Cannot find file: {VectorFile}");

            var fileType = FileType ?? VectorFileType.Auto;
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

            MapContext.CurrentLayer = layer;
        }
    }
}
