using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Sankey
{
    //No options for Save

    public partial class SCSankey
    {
        public SCSankey Save(string fileName, ImageFileFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename is not specified.", nameof(fileName));

            var bmp = PaintSankey(_HostControl, _Options.DPI);

            if (format.HasValue)
            {
                System.Drawing.Imaging.ImageFormat fmt = (format ?? GetImageFormatFromFileName(fileName)) switch
                {
                    ImageFileFormat.Png  => System.Drawing.Imaging.ImageFormat.Png,
                    ImageFileFormat.Tiff => System.Drawing.Imaging.ImageFormat.Tiff,
                    ImageFileFormat.Bmp  => System.Drawing.Imaging.ImageFormat.Bmp,
                    ImageFileFormat.Gif  => System.Drawing.Imaging.ImageFormat.Gif,
                    ImageFileFormat.Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
                    _                    => System.Drawing.Imaging.ImageFormat.Png
                };

                bmp.Save(fileName, fmt);
            }
            else
                bmp.Save(fileName);

            return this;
        }
    }
}
