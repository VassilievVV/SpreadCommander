using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Sankey
{
    public class SankeyOptions
    {
        [Description("Name of a data member that contains link weights.")]
        public string Weight { get; set; }

        [Description("Name of a data member that specifies whether item is selected or no.")]
        public string Selected { get; set; }

        [Description("Name of nodes to display as selected.")]
        public string[] SelectedNodes { get; set; }

        [Description("Pallete using to color nodes.")]
        [DefaultValue(SankeyColorizerPalette.Default)]
        public SankeyColorizerPalette Palette { get; set; } = SankeyColorizerPalette.Default;

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values")]
        public bool IgnoreErrors { get; set; }

        [Description("Deedle frame keys.")]
        public string[] DeedleFrameKeys { get; set; }

        [Description("Width of the image in document units (1/300 of inch). Default value is 2000.")]
        [DefaultValue(2000)]
        public int Width { get; set; } = 2000;

        [Description("Height of the image in document units (1/300 of inch). Default value is 1200.")]
        [DefaultValue(1200)]
        public int Height { get; set; } = 1200;

        [Description("DPI of the image. Default value is 300.")]
        public int? DPI { get; set; }

        [Description("Sankey diagram background color.")]
        public string BackColor { get; set; }

        [Description("Font that is used to display the node label text.")]
        public string NodeFont { get; set; }

        [Description("Maximum width allowed for node labels in pixels.")]
        public int? NodeMaxWidth { get; set; }

        [Description("Number of lines that label text can wrap.")]
        public int? NodeMaxLineCount { get; set; }

        [Description("Node label alignment.")]
        public StringAlignment? NodeTextAlignment { get; set; }

        [Description("Text orientation for node labels.")]
        public TextOrientation? NodeTextOrientation { get; set; }

        [Description("Diagram border's color.")]
        public string BorderColor { get; set; }

        [Description("Diagram border's thickness in pixels.")]
        public int? BorderThickness { get; set; }

        [Description("Node width in pixels.")]
        public int? NodeWidth { get; set; }

        [Description("Indent between nodes in pixels.")]
        public int? VerticalNodeAlignment { get; set; }

        [Description("Link transparency.")]
        public byte? LinkTransparency { get; set; }

        [Description("Diagram's padding")]
        public int[] Padding { get; set; }

        [Description("Text that is displayed at runtime when a diagram has no data to display.")]
        public string EmptyText { get; set; }

        [Description("Font of empty text.")]
        public string EmptyTextFont { get; set; }

        [Description("Alignment of empty text.")]
        public StringAlignment? EmptyTextAlignment { get; set; }

        [Description("Text that is displayed in the diagram when it is too small.")]
        public string SmallText { get; set; }

        [Description("Font of small text.")]
        public string SmallTextFont { get; set; }

        [Description("Alignment of small text.")]
        public StringAlignment? SmallTextAlignment { get; set; }

        [Description("Title's text.")]
        public string TitleText { get; set; }

        [Description("Title's font.")]
        public string TitleFont { get; set; }

        [Description("Title's alignment.")]
        public StringAlignment? TitleAlignment { get; set; }

        [Description("Title's dock style.")]
        public SankeyTitleDockStyle? TitleDock { get; set; }

        [Description("Title's indent.")]
        public int? TitleIndent { get; set; }

        [Description("2nd title's text.")]
        public string Title2Text { get; set; }

        [Description("2nd title's font.")]
        public string Title2Font { get; set; }

        [Description("2nd title's alignment.")]
        public StringAlignment? Title2Alignment { get; set; }

        [Description("2nd title's dock style.")]
        public SankeyTitleDockStyle? Title2Dock { get; set; }

        [Description("2nd title's indent.")]
        public int? Title2Indent { get; set; }

        [Description("3rd title's text.")]
        public string Title3Text { get; set; }

        [Description("3rd title's font.")]
        public string Title3Font { get; set; }

        [Description("3rd title's alignment.")]
        public StringAlignment? Title3Alignment { get; set; }

        [Description("3rd title's dock style.")]
        public SankeyTitleDockStyle? Title3Dock { get; set; }

        [Description("3rd title's indent.")]
        public int? Title3Indent { get; set; }
    }
}
