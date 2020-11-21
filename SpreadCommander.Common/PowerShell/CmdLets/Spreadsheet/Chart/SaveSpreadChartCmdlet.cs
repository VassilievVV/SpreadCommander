using DevExpress.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet.Chart
{
    [Cmdlet(VerbsData.Save, "SpreadChart")]
    public class SaveSpreadChartCmdlet: BaseSpreadsheetWithCopyToBookCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of sheet containing the chart")]
        public string SheetName { get; set; }

        [Parameter(Position = 1, HelpMessage = "0-based index of the chart in sheet")]
        [Alias("ci")]
        public int? ChartIndex { get; set; }

        [Parameter(HelpMessage = "Filename to save chart to")]
        [Alias("fn")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "If set and file already exists - it will be overwritten")]
        [Alias("r")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Specifies the format of the image")]
        [Alias("f")]
        public ImageFormat? Format { get; set; }

        [Parameter(HelpMessage = "Custom size of the image with chart copied to Book. Must be 2-elements array")]
        public int[] Size { get; set; }

        [Parameter(HelpMessage = "Scale of the image with chart copied to Book. Can be used to control image quality similar to setting DPI")]
        public float? CopyToBookScale { get; set; }


        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(SheetName))
                throw new Exception("Sheet name is not specified.");

            ExecuteSynchronized(() => DoSaveImage());
        }

        protected void DoSaveImage()
        {
            var workbook = GetCmdletSpreadsheet();

            ChartObject chart = null;
            Image chartImage;
            var chartSheet = workbook.ChartSheets[SheetName];
            if (chartSheet != null)
                chart = chartSheet.Chart;
            if (chart == null)
            {
                var worksheet = workbook.Worksheets[SheetName];
                if (worksheet != null && worksheet.Charts.Count > Math.Max(0, ChartIndex ?? 0))
                    chart = worksheet.Charts[ChartIndex ?? 0];
            }

            if (chart == null)
                throw new Exception("Cannot find chart.");

            if (Size != null && Size.Length != 2)
                throw new Exception("Invalid size of the image. Must be 2-elements integer array.");
            if (Size != null)
            {
                var imageSize = new Size(Size[0], Size[1]);
                chartImage = chart.GetImage(imageSize).NativeImage;
            }
            else
                chartImage = chart.GetImage().NativeImage;

            if (string.IsNullOrWhiteSpace(FileName))
                throw new Exception("Filename for image is not specified.");

            var fileName = Project.Current.MapPath(FileName);
            var dir      = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                throw new Exception($"Directory '{dir}' does not exist.");

            if (File.Exists(fileName))
            {
                if (Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{FileName}' already exists.");
            }

            System.Drawing.Imaging.ImageFormat format = (Format ?? GetImageFormatFromFileName(fileName)) switch
            {
                ImageFormat.Png  => System.Drawing.Imaging.ImageFormat.Png,
                ImageFormat.Tiff => System.Drawing.Imaging.ImageFormat.Tiff,
                ImageFormat.Bmp  => System.Drawing.Imaging.ImageFormat.Bmp,
                ImageFormat.Gif  => System.Drawing.Imaging.ImageFormat.Gif,
                ImageFormat.Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
                _                => System.Drawing.Imaging.ImageFormat.Png
            };
            chartImage.Save(FileName, format);

            if (CopyToBook)
                CopyImageToBook(chartImage, CopyToBookScale, CopyToBookScale);
        }
    }
}
