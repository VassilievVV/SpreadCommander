using DevExpress.XtraMap;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsData.Save, "Map")]
    public class SaveMapCmdlet : BaseMapWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to save image to.")]
        [Alias("fn")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "If set and file already exists - it will be overwritten")]
        [Alias("r")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Center point of a map.")]
        [Alias("cp")]
        public double[] CenterPoint { get; set; }

        [Parameter(HelpMessage = "Zoom level of a map.")]
        [ValidateRange(0.1, 100)]
        [PSDefaultValue(Value = 1.0)]
        [DefaultValue(1.0)]
        [Alias("zoom", "z")]
        public double ZoomLevel { get; set; } = 1.0;

        [Parameter(HelpMessage = "Width of the image in document units (1/300 of inch). Default value is 2000.")]
        [ValidateRange(300, 20000)]
        [PSDefaultValue(Value = 2000)]
        [DefaultValue(2000)]
        [Alias("w")]
        public int Width { get; set; } = 2000;

        [Parameter(HelpMessage = "Height of the image in document units (1/300 of inch). Default value is 1200.")]
        [ValidateRange(200, 20000)]
        [PSDefaultValue(Value = 1200)]
        [DefaultValue(1200)]
        [Alias("h")]
        public int Height { get; set; } = 1200;

        [Parameter(HelpMessage = "Scaling factor of the image.")]
        [ValidateRange(0.01, 100)]
        [PSDefaultValue(Value = 1)]
        [DefaultValue(1.0f)]
        [Alias("s")]
        public float Scale { get; set; } = 1;

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Specifies the format of the image")]
        [Alias("f")]
        public ImageFileFormat? Format { get; set; }

        [Parameter(HelpMessage = "If set - sends file to FileViewer")]
        public SwitchParameter Preview { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        protected override bool PassThruMapContext => PassThru;

        protected override void UpdateMap()
        {
            WriteMap();
        }

        protected void WriteMap()
        {
            WriteImage(GetCmdletBook());
        }

        protected void WriteImage(Document book)
        {
            var map = MapContext?.Map;
            if (map == null)
                throw new Exception("Map is not provided. Please use one of New-Map cmdlets to create a map.");

            var fileName = Project.Current.MapPath(FileName);
            var dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                throw new Exception($"Directory '{dir}' does not exist.");

            if (File.Exists(fileName))
            {
                if (Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{FileName}' already exists.");
            }

            if (CenterPoint != null && CenterPoint.Length != 2)
                throw new Exception("CanterPoint shall be array of exactly 2 double values.");

            if (CenterPoint != null)
                map.PublicCenterPoint = MapContext.CreateCoordPoint(CenterPoint[0], CenterPoint[1]);

            int w = Convert.ToInt32(Width  * 96.0 / 300.0);
            int h = Convert.ToInt32(Height * 96.0 / 300.0);

            map.ZoomLevel = ZoomLevel;
            map.SetClientRectangle(new Rectangle(0, 0, w, h));

            var mapBitmap = PaintMap(map);

            DoWriteImage(book, mapBitmap);

            if (Preview)
                PreviewFile(fileName);
        }

        protected override void DoWriteImage(Document book, Image mapBitmap)
        {
            var fileName = Project.Current.MapPath(FileName);

            if (mapBitmap == null)
                throw new Exception("No image to save.");

            if (string.IsNullOrWhiteSpace(FileName))
                throw new Exception("Filename for image is not specified.");
            System.Drawing.Imaging.ImageFormat format = (Format ?? GetImageFormatFromFileName(fileName)) switch
            {
                ImageFileFormat.Png  => System.Drawing.Imaging.ImageFormat.Png,
                ImageFileFormat.Tiff => System.Drawing.Imaging.ImageFormat.Tiff,
                ImageFileFormat.Bmp  => System.Drawing.Imaging.ImageFormat.Bmp,
                ImageFileFormat.Gif  => System.Drawing.Imaging.ImageFormat.Gif,
                ImageFileFormat.Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
                _                => System.Drawing.Imaging.ImageFormat.Png
            };
            ExecuteLocked(() => mapBitmap.Save(fileName, format), LockFiles ? LockObject : null);
        }
    }
}
