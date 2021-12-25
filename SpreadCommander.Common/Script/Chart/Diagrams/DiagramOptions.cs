using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Diagrams
{
    public class DiagramOptions
    {
        [Description("Name of the data field that contains names for automatically generated series.")]
        public string SeriesField { get; set; }

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values")]
        public bool IgnoreErrors { get; set; }

#pragma warning disable CRRSP06 // A misspelled word has been found
        [Description("Template file - .scchart file created in Chart document")]
#pragma warning restore CRRSP06 // A misspelled word has been found
        public string TemplateFile { get; set; }

        [Description("Palette used to draw the chart's series.")]
        public ChartPaletteName? Palette { get; set; }

        [Description("1-based number of a color within the selected palette, which will be used as a base color to paint series points.")]
        public int? PaletteBaseColorNumber { get; set; }

        [Description("Palette that is used to paint all indicators that exist in a chart.")]
        public ChartPaletteName? IndicatorsPalette { get; set; }

        [Description("Whether the adaptive layout feature is enabled for chart elements in the chart control.")]
        public bool DisableAutoLayout { get; set; }

        [Description("Chart's background color.")]
        public string BackColor { get; set; }

        [Description("Chart's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Border color.")]
        public string BorderColor { get; set; }

        [Description("Border's thickness.")]
        public int? BorderThickness { get; set; }

        [Description("Filling mode for an element's surface.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("String which represents the pattern specifying the text to be displayed within series labels.")]
        public string TextPattern { get; set; }

        [Description("String that formats text for the series or series point legend items.")]
        public string LegendTextPattern { get; set; }

        [Description("Show or hide legend.")]
        public bool? LegendVisibility { get; set; }

        [Description("Minimum indent between adjacent series labels, when an overlapping resolving algorithm is applied to them.")]
        public int? ResolveOverlappingMinIndent { get; set; }

        [Description("Mode to resolve overlapping of series labels.")]
        public ResolveOverlappingMode? ResolveOverlappingMode { get; set; }

        [Description("Show or hide series labels.")]
        public bool? LabelsVisibility { get; set; }

        [Description("Number of lines to which a label's text is allowed to wrap. Value in range 0 (no limit) to 20.")]
        public int? LabelsMaxLineCount { get; set; }

        [Description("Maximum width allowed for series labels.")]
        public int? LabelsMaxWidth { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }



        internal virtual void SetupDiagram(DevExpress.XtraCharts.Diagram diagram)
        {
        }
    }
}
