using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsData.Save, "Chart")]
    public class SaveChartCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(Position = 0, HelpMessage = "Filename to save image to")]
        [Alias("fn")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "If set - cmdlet will output System.Drawing.Bitmap into pipeline.")]
        public SwitchParameter InMemory { get; set; }

        [Parameter(HelpMessage = "If set and file already exists - it will be overwritten")]
        [Alias("r")]
        public SwitchParameter Replace { get; set; }

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

        [Parameter(HelpMessage = "DPI of the image.")]
        [ValidateRange(48, 4800)]
        public int? DPI { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Specifies the format of the image")]
        [Alias("f")]
        public ImageFormat? Format { get; set; }
        
        [Parameter(HelpMessage = "If set - sends file to FileViewer")]
        public SwitchParameter Preview { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        protected override bool PassThruChartContext => PassThru;

        protected override void UpdateChart()
        {
            WriteChart();
        }

        protected void WriteChart()
        {
            WriteImage();
        }

        protected void WriteImage()
        {
            var chart = ChartContext?.Chart;
            if (chart == null)
                throw new Exception("Chart is not provided. Please use one of New-Chart cmdlets to create a chart.");

            string fileName = null;
            if (!InMemory)
            {
                fileName = Project.Current.MapPath(FileName);
                var dir  = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir))
                    throw new Exception($"Directory '{dir}' does not exist.");

                if (File.Exists(fileName))
                {
                    if (Replace)
                        File.Delete(fileName);
                    else
                        throw new Exception($"File '{FileName}' already exists.");
                }
            }

            var chartBitmap = PaintChart(chart, Width, Height, DPI);

            if (!InMemory)
            {
                //Do not synchronize, saving image to file does not need to be executed in UI thread.
                DoWriteImage(null, chartBitmap);

                if (Preview)
                    PreviewFile(fileName);
            }
            else
            {
                PassThru = false;
                WriteObject(chartBitmap);
            }
        }

        protected override void DoWriteImage(Document book, Image chartBitmap)
        {
            var fileName = Project.Current.MapPath(FileName);

            if (chartBitmap == null)
                throw new Exception("No image to save.");

            if (string.IsNullOrWhiteSpace(FileName))
                throw new Exception("Filename for image is not specified.");
            System.Drawing.Imaging.ImageFormat format = (Format ?? GetImageFormatFromFileName(fileName)) switch
            {
                ImageFormat.Png  => System.Drawing.Imaging.ImageFormat.Png,
                ImageFormat.Tiff => System.Drawing.Imaging.ImageFormat.Tiff,
                ImageFormat.Bmp  => System.Drawing.Imaging.ImageFormat.Bmp,
                ImageFormat.Gif  => System.Drawing.Imaging.ImageFormat.Gif,
                ImageFormat.Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
                _                => System.Drawing.Imaging.ImageFormat.Png
            };
            ExecuteLocked(() => chartBitmap.Save(fileName, format), LockFiles ? LockObject : null);
        }
    }
}
