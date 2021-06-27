using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Sankey
{
    [Cmdlet(VerbsData.Save, "SankeyDiagram")]
    public class SaveSankeyDiagramCmdlet: BaseSankeyDiagramCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "File name to save Sankey diagram.")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "Image format for Sankey diagram.")]
        public ImageFileFormat? Format { get; set; }


        protected override void WriteSankey(Bitmap bmp)
        {
            if (string.IsNullOrWhiteSpace(FileName))
                throw new ArgumentException("Filename is not specified.", nameof(FileName));

            if (Format.HasValue)
            {
                System.Drawing.Imaging.ImageFormat format = (Format ?? GetImageFormatFromFileName(FileName)) switch
                {
                    ImageFileFormat.Png  => System.Drawing.Imaging.ImageFormat.Png,
                    ImageFileFormat.Tiff => System.Drawing.Imaging.ImageFormat.Tiff,
                    ImageFileFormat.Bmp  => System.Drawing.Imaging.ImageFormat.Bmp,
                    ImageFileFormat.Gif  => System.Drawing.Imaging.ImageFormat.Gif,
                    ImageFileFormat.Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
                    _                    => System.Drawing.Imaging.ImageFormat.Png
                };

                bmp.Save(FileName, format);
            }
            else
                bmp.Save(FileName);
        }
    }
}
