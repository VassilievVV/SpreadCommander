using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class PointerOptions: MapItemOptions
    {
        [Description("Whether to draw map pointer background.")]
        public bool DrawBackground { get; set; }

        [Description("Color that is used to fill a map item.")]
        public string FillColor { get; set; }

        [Description("Font used to paint a map pointer's text.")]
        public string Font { get; set; }

        [Description("Image that is assigned to a map pointer.")]
        public Image Image { get; set; }

        [Description("Image index value of an image assigned to the map pointer.")]
        public int? ImageIndex { get; set; }

        [Description("Filename that defines the location of an Image file with vector data.")]
        public string ImageFilename { get; set; }

        [Description("Alignment of the text in a map pointer.")]
        public TextAlignment? TextAlignment { get; set; }

        [Description("Padding for text shown for map pointer.")]
        public int? TextPadding { get; set; }

        [Description("Transparency value for the map pointer.")]
        public byte? Transparency { get; set; }


        protected internal virtual void ConfigurePointerItem(SCMap map, MapPointer pointer,
            double[] location, string text)
        {
            if (location == null || location.Length != 2)
                throw new Exception("Location must be double array with 2 elements.");

            pointer.Location = map.CreateCoordPoint(location[0], location[1]);
            pointer.Text = text;

            ConfigureMapItem(map, pointer);
        }

        protected internal override void ConfigureMapItem(SCMap map, MapItem item)
        {
            base.ConfigureMapItem(map, item);

            var mapItem = item as MapPointer ?? throw new Exception("Map item must be MapPointer.");

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
                mapItem.TextAlignment = (DevExpress.XtraMap.TextAlignment)TextAlignment.Value;

            if (TextPadding.HasValue)
                mapItem.TextPadding = TextPadding.Value;

            if (Transparency.HasValue)
                mapItem.Transparency = Transparency.Value;
        }
    }
}
