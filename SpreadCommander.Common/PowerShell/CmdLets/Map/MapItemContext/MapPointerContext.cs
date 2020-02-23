using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
    public class MapPointerContext: BaseMapItemContext
    {
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Location of a map pointer object.")]
        public double[] Location { get; set; }

        [Parameter(Position = 2, HelpMessage = "Text for a map pointer.")]
        public string Text { get; set; }

        [Parameter(HelpMessage = "Whether to draw map pointer background.")]
        public SwitchParameter DrawBackground { get; set; }

        [Parameter(HelpMessage = "Color that is used to fill a map item.")]
        public string FillColor { get; set; }

        [Parameter(HelpMessage = "Font used to paint a map pointer's text.")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "Image that is assigned to a map pointer.")]
        public Image Image { get; set; }

        [Parameter(HelpMessage = "Image index value of an image assigned to the map pointer.")]
        public int? ImageIndex { get; set; }

        [Parameter(HelpMessage = "Filename that defines the location of an Image file with vector data.")]
        public string ImageFilename { get; set; }

        [Parameter(HelpMessage = "Alignment of the text in a map pointer.")]
        public TextAlignment? TextAlignment { get; set; }

        [Parameter(HelpMessage = "Padding for text shown for map pointer.")]
        public int? TextPadding { get; set; }

        [Parameter(HelpMessage = "Transparency value for the map pointer.")]
        public byte? Transparency { get; set; }


        public override void ConfigureMapItem(MapItem item)
        {
            base.ConfigureMapItem(item);

            var mapItem = item as MapPointer ?? throw new Exception("Map item must be MapPointer.");

            if (Location == null || Location.Length != 2)
                throw new Exception("Location must be double array with 2 elements.");

            mapItem.Location              = MapContext.CreateCoordPoint(Location[0], Location[1]);
            mapItem.Text                  = Text;
            mapItem.BackgroundDrawingMode = DrawBackground ? ElementState.All : ElementState.None;

            var fillColor = Utils.ColorFromString(FillColor);
            if (fillColor != Color.Empty)
                mapItem.Fill = fillColor;

            var font = Utils.StringToFont(Font, out Color textColor);
            if (font != null)
                mapItem.Font = font;
            if (textColor != Color.Empty)
                mapItem.TextColor = textColor;

            if (Image != null)
                mapItem.Image = Image;
            if (ImageIndex.HasValue)
                mapItem.ImageIndex = ImageIndex.Value;

            if (!string.IsNullOrWhiteSpace(ImageFilename))
            {
                var imagePath = Project.Current.MapPath(ImageFilename);
                if (string.IsNullOrWhiteSpace(imagePath) || !System.IO.File.Exists(imagePath))
                    throw new Exception($"Cannot find image: '{imagePath}'.");
                var bmp = new Bitmap(imagePath);
                mapItem.Image = bmp;
            }

            if (TextAlignment.HasValue)
                mapItem.TextAlignment = TextAlignment.Value;

            if (TextPadding.HasValue)
                mapItem.TextPadding = TextPadding.Value;

            if (Transparency.HasValue)
                mapItem.Transparency = Transparency.Value;
        }
    }
}
