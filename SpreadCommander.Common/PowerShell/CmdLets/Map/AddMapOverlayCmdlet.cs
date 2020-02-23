using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadCommander.Common.Extensions;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsCommon.Add, "MapOverlay")]
    public class AddMapOverlayCmdlet: BaseMapWithContextCmdlet
    {
        [Parameter(HelpMessage = "Alignment of the map overlay.")]
        public ContentAlignment? Alignment { get; set; }

        [Parameter(HelpMessage = "Background color.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Position of the map overlay - integer array with 2 elements (X and Y coordinates).")]
        public int[] Position { get; set; }

        [Parameter(HelpMessage = "Margin of the map overlay.")]
        public int[] Margin { get; set; }

        [Parameter(HelpMessage = "Padding of the map overlay.")]
        public int[] Padding { get; set; }

        [Parameter(HelpMessage = "Size of the map overlay - integer array with 2 elements.")]
        public int[] Size { get; set; }

        [Parameter(HelpMessage = "Image alignment.")]
        public ContentAlignment? ImageAlignment { get; set; }

        [Parameter(HelpMessage = "Text alignment.")]
        public ContentAlignment? TextAlignment { get; set; }

        [Parameter(HelpMessage = "Margin of the image.")]
        public int[] ImageMargin { get; set; }

        [Parameter(HelpMessage = "Padding of the image.")]
        public int[] ImagePadding { get; set; }

        [Parameter(HelpMessage = "Margin of the text.")]
        public int[] TextMargin { get; set; }

        [Parameter(HelpMessage = "Padding of the text.")]
        public int[] TextPadding { get; set; }

        [Parameter(HelpMessage = "Image that the overlay displays.")]
        public Image Image { get; set; }

        [Parameter(HelpMessage = "Filename of the image that overlay displays.")]
        public string ImageFile { get; set; }

        [Parameter(HelpMessage = "Index of an image in map's image list.")]
        public int? ImageIndex { get; set; }

        [Parameter(HelpMessage = "Text that overlay displays.")]
        public string Text { get; set; }

        [Parameter(HelpMessage = "Font of text that overlay displays.")]
        public string Font { get; set; }


        protected override void UpdateMapRecord()
        {
            if (Position != null && Position.Length != 2)
                throw new Exception("Position shall be an integer array with 2 elements.");
            if (Margin != null && Margin.Length != 4 && Margin.Length != 1)
                throw new Exception("Margin shall be an integer array with 1 or 4 elements.");
            if (Padding != null && Padding.Length != 4 && Padding.Length != 1)
                throw new Exception("Padding shall be an integer array with 1 or 4 elements.");
            if (Size != null && Size.Length != 2)
                throw new Exception("Size shall be an integer array with 2 elements.");

            if (ImageMargin != null && ImageMargin.Length != 4 && ImageMargin.Length != 1)
                throw new Exception("ImageMargin shall be an integer array with 1 or 4 elements.");
            if (ImagePadding != null && ImagePadding.Length != 4 && ImagePadding.Length != 1)
                throw new Exception("ImagePadding shall be an integer array with 1 or 4 elements.");

            if (TextMargin != null && TextMargin.Length != 4 && TextMargin.Length != 1)
                throw new Exception("TextMargin shall be an integer array with 1 or 4 elements.");
            if (TextPadding != null && TextPadding.Length != 4 && TextMargin.Length != 1)
                throw new Exception("TextPadding shall be an integer array with 1 or 4 elements.");

            var overlay = new MapOverlay();
            if (Alignment.HasValue)
                overlay.Alignment = Alignment.Value;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                overlay.BackgroundStyle.Fill = backColor;

            if (Position != null)
                overlay.Location = new Point(Position[0], Position[1]);
            if (Margin != null)
                overlay.Margin = GetPadding(Margin);
            if (Padding != null)
                overlay.Padding = GetPadding(Padding);
            if (Size != null)
                overlay.Size = new Size(Size[0], Size[1]);

            if (Image != null || !string.IsNullOrWhiteSpace(ImageFile) || ImageIndex.HasValue)
            {
                var imageItem = new MapOverlayImageItem();
                if (ImageAlignment.HasValue)
                    imageItem.Alignment = ImageAlignment.Value;

                if (ImageMargin != null)
                    imageItem.Margin = GetPadding(ImageMargin);
                if (ImagePadding != null)
                    imageItem.Padding = GetPadding(ImagePadding);

                if (Image != null)
                    imageItem.Image = Image;
                else if (!string.IsNullOrWhiteSpace(ImageFile))
                {
                    var imageFile = Project.Current.MapPath(ImageFile);
                    if (string.IsNullOrWhiteSpace(imageFile) || !File.Exists(ImageFile))
                        throw new Exception($"Cannot find file: {ImageFile}");

                    imageItem.ImageUri = new Uri($"file://{imageFile}");
                }
                else if (ImageIndex.HasValue)
                    imageItem.ImageIndex = ImageIndex.Value;

                if (backColor != Color.Empty)
                    imageItem.BackgroundStyle.Fill = backColor;

                overlay.Items.Add(imageItem);
            }

            if (!string.IsNullOrWhiteSpace(Text))
            {
                if (TextAlignment == 0)
                    TextAlignment = (Image != null || !string.IsNullOrWhiteSpace(ImageFile) || ImageIndex.HasValue) ? ContentAlignment.MiddleRight : ContentAlignment.MiddleCenter;

                var textItem = new MapOverlayTextItem();
                if (TextAlignment.HasValue)
                    textItem.Alignment = TextAlignment.Value;

                if (TextMargin != null)
                    textItem.Margin = GetPadding(TextMargin);
                if (TextPadding != null)
                    textItem.Padding = GetPadding(TextPadding);
                textItem.Text = Text;

                var font = Utils.StringToFont(Font, out Color foreColor);
                if (font != null)
                    textItem.TextStyle.Font = font;
                if (foreColor != Color.Empty)
                    textItem.TextStyle.TextColor = foreColor;
                else if (backColor == Color.Empty)
                    textItem.TextStyle.TextColor = Color.White;

                if (backColor != Color.Empty)
                    textItem.BackgroundStyle.Fill = backColor;

                overlay.Items.Add(textItem);
            }

            MapContext.Map.Overlays.Add(overlay);
            MapContext.Map.PrintOptions.PrintOverlays = true;


            static Padding GetPadding(int[] values)
            {
                if (values.Length == 1)
                    return new Padding(values[0]);
                else if (values.Length == 4)
                    return new Padding(values[0], values[1], values[2], values[3]);

                throw new Exception("Invalid margin or padding. Shall be an integer array with 1 or 4 elements.");
            }
        }
    }
}
