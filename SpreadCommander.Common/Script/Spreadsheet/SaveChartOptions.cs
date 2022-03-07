using DevExpress.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class SaveChartOptions : SpreadsheetWithCopyToBookOptions
    {
        [Description("Name of sheet containing the chart")]
        public string SheetName { get; set; }

        [Description("0-based index of the chart in sheet")]
        public int? ChartIndex { get; set; }

        [Description("Filename to save chart to")]
        public string FileName { get; set; }

        [Description("If set and file already exists - it will be overwritten")]
        public bool Replace { get; set; }

        [Description("Specifies the format of the image")]
        public ImageFileFormat? Format { get; set; }

        [Description("Custom size of the image with chart copied to Book. Must be 2-elements array")]
        public int[] Size { get; set; }

        [Description("Scale of the image with chart copied to Book. Can be used to control image quality similar to setting DPI")]
        public float? CopyToBookScale { get; set; }
    }

    public partial class SCSpreadsheet
    {
        public void SaveChart(string sheetName, string fileName, SaveChartOptions options = null) =>
            ExecuteSynchronized(options, () => DoSaveChart(sheetName, fileName, options));

        protected virtual void DoSaveChart(string sheetName, string fileName, SaveChartOptions options)
        {
            options ??= new SaveChartOptions();
            var spread = options.Spreadsheet?.Workbook ?? Workbook;

            ChartObject chart = null;
            Image chartImage;
            var chartSheet = spread.ChartSheets[sheetName];
            if (chartSheet != null)
                chart = chartSheet.Chart;
            if (chart == null)
            {
                var worksheet = spread.Worksheets[sheetName];
                if (worksheet != null && worksheet.Charts.Count > Math.Max(0, options.ChartIndex ?? 0))
                    chart = worksheet.Charts[options.ChartIndex ?? 0];
            }

            if (chart == null)
                throw new Exception("Cannot find chart.");

            if (options.Size != null && options.Size.Length != 2)
                throw new Exception("Invalid size of the image. Must be 2-elements integer array.");
            if (options.Size != null)
            {
                var imageSize = new Size(options.Size[0], options.Size[1]);
                chartImage = chart.GetImage(imageSize).NativeImage;
            }
            else
                chartImage = chart.GetImage().NativeImage;

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Filename for image is not specified.");

            fileName = Project.Current.MapPath(fileName);
            var dir  = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                throw new Exception($"Directory '{dir}' does not exist.");

            if (File.Exists(fileName))
            {
                if (options.Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{fileName}' already exists.");
            }

            System.Drawing.Imaging.ImageFormat format = (options.Format ?? GetImageFormatFromFileName(fileName)) switch
            {
                ImageFileFormat.Png  => System.Drawing.Imaging.ImageFormat.Png,
                ImageFileFormat.Tiff => System.Drawing.Imaging.ImageFormat.Tiff,
                ImageFileFormat.Bmp  => System.Drawing.Imaging.ImageFormat.Bmp,
                ImageFileFormat.Gif  => System.Drawing.Imaging.ImageFormat.Gif,
                ImageFileFormat.Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
                _                    => System.Drawing.Imaging.ImageFormat.Png
            };
            chartImage.Save(fileName, format);

            if (options.CopyToBook)
                CopyImageToBook(chartImage, options.CopyToBookScale, options.CopyToBookScale, options);
        }
    }
}
