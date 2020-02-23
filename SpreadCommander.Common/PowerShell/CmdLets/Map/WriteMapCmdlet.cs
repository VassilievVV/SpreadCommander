using DevExpress.XtraMap;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsCommunications.Write, "Map")]
    public class WriteMapCmdlet: BaseMapWithContextCmdlet
    {
        [Parameter(HelpMessage = "Center point of a map.")]
        public double[] CenterPoint { get; set; }

        [Parameter(HelpMessage = "Zoom level of a map.")]
        [ValidateRange(0.1, 100)]
        [PSDefaultValue(Value = 1.0)]
        [DefaultValue(1.0)]
        public double ZoomLevel { get; set; } = 1.0;

        [Parameter(HelpMessage = "Width of the image in document units (1/300 of inch). Default value is 2000.")]
        [ValidateRange(300, 20000)]
        [PSDefaultValue(Value = 2000)]
        [DefaultValue(2000)]
        public int Width { get; set; } = 2000;

        [Parameter(HelpMessage = "Height of the image in document units (1/300 of inch). Default value is 1200.")]
        [ValidateRange(200, 20000)]
        [PSDefaultValue(Value = 1200)]
        [DefaultValue(1200)]
        public int Height { get; set; } = 1200;

        [Parameter(HelpMessage = "Scaling factor of the image.")]
        [ValidateRange(0.01, 100)]
        [PSDefaultValue(Value = 1)]
        [DefaultValue(1.0f)]
        public float Scale { get; set; } = 1;

        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Parameter(HelpMessage = "Paragraph's text alignment")]
        public ParagraphAlignment? Alignment { get; set; }

        [Parameter(HelpMessage = "Indent of the first line of a paragraph")]
        public float? FirstLineIdent { get; set; }

        [Parameter(HelpMessage = "Whether and how a paragraph's first line is indented")]
        public ParagraphFirstLineIndent? FirstLineIndentType { get; set; }

        [Parameter(HelpMessage = "Paragraph's left indent")]
        public float? LeftIndent { get; set; }

        [Parameter(HelpMessage = "Paragraph's right indent")]
        public float? RightIndent { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }


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

            if (CenterPoint != null && CenterPoint.Length != 2)
                throw new Exception("CanterPoint shall be array of exactly 2 double values.");

            if (CenterPoint != null)
                map.PublicCenterPoint = MapContext.CreateCoordPoint(CenterPoint[0], CenterPoint[1]);

            int w = Convert.ToInt32(Width  * 96.0 / 300.0);
            int h = Convert.ToInt32(Height * 96.0 / 300.0);

            map.ZoomLevel         = ZoomLevel;
            map.SetClientRectangle(new Rectangle(0, 0, w, h));

            var mapBitmap = PaintMap(map);

            ExecuteSynchronized(() => DoWriteImage(book, mapBitmap));
        }

        protected override void DoWriteImage(Document book, Image mapBitmap)
        {
            if (mapBitmap == null)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { ResetBookFormatting(book); book.EndUpdate(); }))
            {
                var image = book.Images.Append(mapBitmap);

                if (Scale != 1.0f)
                    image.ScaleX = image.ScaleY = Scale;

                var range = image.Range;

                if (!string.IsNullOrWhiteSpace(ParagraphStyle) || FirstLineIdent.HasValue || FirstLineIndentType.HasValue ||
                    LeftIndent.HasValue || RightIndent.HasValue)
                {
                    var pp = book.BeginUpdateParagraphs(range);
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(ParagraphStyle))
                        {
                            var style = book.ParagraphStyles[ParagraphStyle] ?? throw new Exception($"Paragraph style '{ParagraphStyle}' does not exist.");
                            pp.Style = style;
                        }

                        if (FirstLineIdent.HasValue)
                            pp.FirstLineIndent = FirstLineIdent;
                        if (FirstLineIndentType.HasValue)
                            pp.FirstLineIndentType = FirstLineIndentType;

                        if (LeftIndent.HasValue)
                            pp.LeftIndent = LeftIndent;
                        if (RightIndent.HasValue)
                            pp.RightIndent = RightIndent;
                    }
                    finally
                    {
                        book.EndUpdateParagraphs(pp);
                    }
                }

                var rangeNewLine = book.AppendText(Environment.NewLine);

                AddComments(book, range);

                book.CaretPosition = rangeNewLine.End;
                ScrollToCaret();
            }
        }
    }
}
