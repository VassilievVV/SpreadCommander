using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.CmdLets.Charts.DiagramContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    public enum ChartBorderType { Inside, Outside }

    [Cmdlet(VerbsCommon.New, "Chart")]
    public class NewChartCmdlet : BaseChartCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Chart type for quick series specified in Arguments and Values.")]
        [Alias("type", "t")]
        public ViewType ChartType { get; set; }

        [Parameter(Position = 1, HelpMessage = "Argument fields for quick series.")]
        [Alias("arg", "a")]
        public string[] Arguments { get; set; }

        [Parameter(Position = 2, HelpMessage = "Value fields for quick series.")]
        [Alias("val", "v")]
        public string[] Values { get; set; }

        [Parameter(HelpMessage = "Name of the data field that contains names for automatically generated series.")]
        public string SeriesField { get; set; }

        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables to pass source records through pipe.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Template file - .scchart file created in Chart document")]
        public string TemplateFile { get; set; }

        [Parameter(HelpMessage = "Palette used to draw the chart's series.")]
        public ChartPaletteName? Palette { get; set; }

        [Parameter(HelpMessage = "1-based number of a color within the selected palette, which will be used as a base color to paint series points.")]
        [ValidateRange(0, 100)]
        public int? PaletteBaseColorNumber { get; set; }

        [Parameter(HelpMessage = "Palette that is used to paint all indicators that exist in a chart.")]
        public ChartPaletteName? IndicatorsPalette { get; set; }

        [Parameter(HelpMessage = "Whether the adaptive layout feature is enabled for chart elements in the chart control.")]
        public SwitchParameter DisableAutoLayout { get; set; }

        [Parameter(HelpMessage = "Chart's background color.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Chart's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Parameter(HelpMessage = "Border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Border's thickness.")]
        public int? BorderThickness { get; set; } 

        [Parameter(HelpMessage = "Filling mode for an element's surface.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "String which represents the pattern specifying the text to be displayed within series labels.")]
        public string TextPattern { get; set; }

        [Parameter(HelpMessage = "String that formats text for the series or series point legend items.")]
        public string LegendTextPattern { get; set; }

        [Parameter(HelpMessage = "Hide legend.")]
        [Alias("HideLegend")]
        public SwitchParameter NoLegend { get; set; }

        [Parameter(HelpMessage = "Minimum indent between adjacent series labels, when an overlapping resolving algorithm is applied to them.")]
        public int? ResolveOverlappingMinIndent { get; set; }

        [Parameter(HelpMessage = "Mode to resolve overlapping of series labels.")]
        public ResolveOverlappingMode? ResolveOverlappingMode { get; set; }

        [Parameter(HelpMessage = "Hide series labels.")]
        [Alias("HideLabels")]
        public SwitchParameter NoLabels { get; set; }

        [Parameter(HelpMessage = "Show series labels.")]
        public SwitchParameter ShowLabels { get; set; }

        [Parameter(HelpMessage = "Number of lines to which a label's text is allowed to wrap. Value in range 0 (no limit) to 20.")]
        public int? LabelsMaxLineCount { get; set; }

        [Parameter(HelpMessage = "Maximum width allowed for series labels.")]
        public int? LabelsMaxWidth { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        private BaseDiagramContext _DiagramContext;

        public object GetDynamicParameters()
        {
            switch (ChartType)
            {
                //Simple
                case ViewType.Pie:
                case ViewType.Doughnut:
                case ViewType.NestedDoughnut:
                case ViewType.Funnel:
                    return CreateDiagramContext(typeof(PieDiagramContext));

                //Simple3D
                case ViewType.Pie3D:
                case ViewType.Doughnut3D:
                    return CreateDiagramContext(typeof(PieDiagram3DContext));

                //XY
                case ViewType.Bar:
                case ViewType.StackedBar:
                case ViewType.FullStackedBar:
                case ViewType.SideBySideStackedBar:
                case ViewType.SideBySideFullStackedBar:
                case ViewType.Point:
                case ViewType.Bubble:
                case ViewType.Line:
                case ViewType.StackedLine:
                case ViewType.FullStackedLine:
                case ViewType.StepLine:
                case ViewType.Spline:
                case ViewType.ScatterLine:
                case ViewType.Area:
                case ViewType.StepArea:
                case ViewType.SplineArea:
                case ViewType.StackedArea:
                case ViewType.StackedStepArea:
                case ViewType.StackedSplineArea:
                case ViewType.FullStackedArea:
                case ViewType.FullStackedSplineArea:
                case ViewType.FullStackedStepArea:
                case ViewType.RangeArea:
                case ViewType.Stock:
                case ViewType.CandleStick:
                case ViewType.SideBySideRangeBar:
                case ViewType.RangeBar:
                case ViewType.BoxPlot:
                case ViewType.Waterfall:
                    return CreateDiagramContext(typeof(XYDiagramContext));

                //XY3D
                case ViewType.Bar3D:
                case ViewType.StackedBar3D:
                case ViewType.FullStackedBar3D:
                case ViewType.ManhattanBar:
                case ViewType.SideBySideStackedBar3D:
                case ViewType.SideBySideFullStackedBar3D:
                case ViewType.Line3D:
                case ViewType.StackedLine3D:
                case ViewType.FullStackedLine3D:
                case ViewType.StepLine3D:
                case ViewType.Area3D:
                case ViewType.StackedArea3D:
                case ViewType.FullStackedArea3D:
                case ViewType.StepArea3D:
                case ViewType.Spline3D:
                case ViewType.SplineArea3D:
                case ViewType.StackedSplineArea3D:
                case ViewType.FullStackedSplineArea3D:
                case ViewType.RangeArea3D:
                    return CreateDiagramContext(typeof(XYDiagram3DContext));

                //Gannt
                case ViewType.SideBySideGantt:
                case ViewType.Gantt:
                    return CreateDiagramContext(typeof(XYDiagramContext));

                //Funnel3D
                case ViewType.Funnel3D:
                    return CreateDiagramContext(typeof(PieDiagram3DContext));

                //Radar and Polar
                case ViewType.PolarPoint:
                case ViewType.PolarLine:
                case ViewType.ScatterPolarLine:
                case ViewType.PolarArea:
                case ViewType.PolarRangeArea:
                case ViewType.RadarPoint:
                case ViewType.RadarLine:
                case ViewType.ScatterRadarLine:
                case ViewType.RadarArea:
                case ViewType.RadarRangeArea:
                    return CreateDiagramContext(typeof(RadarDiagramContext));

                //Swift
                case ViewType.SwiftPlot:
                    return CreateDiagramContext(typeof(XYDiagramContext));
            }

            _DiagramContext = null;
            return null;


            BaseDiagramContext CreateDiagramContext(Type typeContext)
            {
                if (_DiagramContext == null || !(typeContext.IsInstanceOfType(_DiagramContext)))
                    _DiagramContext = Activator.CreateInstance(typeContext) as BaseDiagramContext;
                return _DiagramContext;
            }
        }


        private readonly List<PSObject> _Output = new List<PSObject>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Output.Add(obj);
        }

        protected override void EndProcessing()
        {
            var dataSource = GetDataSource(_Output, DataSource, 
                new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns });

            var chart = CreateChart();
            chart.DataContainer.DataSource = dataSource;
            chart.DataContainer.DataMember = null;

            if (NoLegend)
                chart.Legend.Visibility = DefaultBoolean.False;

            if (!string.IsNullOrWhiteSpace(SeriesField))
            {
                //Series template
                if (Arguments == null || Arguments.Length != 1)
                    throw new Exception("Series template allows only one Arguments");
                if (Values == null || Values.Length <= 0)
                    throw new Exception("Series template requires at least one Values");

                var seriesTemplate = chart.DataContainer.SeriesTemplate;
                seriesTemplate.ChangeView(ChartType);
                seriesTemplate.SeriesDataMember   = SeriesField;
                seriesTemplate.ArgumentDataMember = Arguments[0];
                seriesTemplate.ValueDataMembers.AddRange(Values);

                if (seriesTemplate.Label != null)
                {
                    if (TextPattern != null)    //allow empty string pattern
                        seriesTemplate.Label.TextPattern = TextPattern;
                    if (ResolveOverlappingMinIndent.HasValue)
                        seriesTemplate.Label.ResolveOverlappingMinIndent = ResolveOverlappingMinIndent.Value;
                    if (ResolveOverlappingMode.HasValue)
                        seriesTemplate.Label.ResolveOverlappingMode = ResolveOverlappingMode.Value;

                    if (LabelsMaxLineCount.HasValue)
                        seriesTemplate.Label.MaxLineCount = LabelsMaxLineCount.Value;
                    if (LabelsMaxWidth.HasValue)
                        seriesTemplate.Label.MaxWidth = LabelsMaxWidth.Value;
                }

                if (NoLabels)
                    seriesTemplate.LabelsVisibility = DefaultBoolean.False;
                if (ShowLabels)
                    seriesTemplate.LabelsVisibility = DefaultBoolean.True;

                if (LegendTextPattern != null) 
                    seriesTemplate.LegendTextPattern = LegendTextPattern;
            }
            else
            {
                //Quick series
                if (Values != null && Values.Length > 0)
                {
                    string argumentField = null;
                    int seriesCounter = 0;
                    while (seriesCounter < Values.Length)
                    {
                        if (Arguments != null && seriesCounter < Arguments.Length)
                            argumentField = Arguments[seriesCounter];   //Propagate argument field to next series. I.e. if there is 1 Argument and 3 Values - use this Argument for all series.

                        var series = new Series($"Series{seriesCounter}", ChartType)
                        {
                            ArgumentDataMember = argumentField
                        };

                        if (series.Label != null)
                        {
                            series.Label.EnableAntialiasing = DefaultBoolean.True;

                            if (TextPattern != null)    //allow empty string pattern
                                series.Label.TextPattern = TextPattern;
                            if (ResolveOverlappingMinIndent.HasValue)
                                series.Label.ResolveOverlappingMinIndent = ResolveOverlappingMinIndent.Value;
                            if (ResolveOverlappingMode.HasValue)
                                series.Label.ResolveOverlappingMode = ResolveOverlappingMode.Value;

                            if (LabelsMaxLineCount.HasValue)
                                series.Label.MaxLineCount = LabelsMaxLineCount.Value;
                            if (LabelsMaxWidth.HasValue)
                                series.Label.MaxWidth = LabelsMaxWidth.Value;
                        }

                        if (NoLabels)
                            series.LabelsVisibility = DefaultBoolean.False;
                        if (ShowLabels)
                            series.LabelsVisibility = DefaultBoolean.True;

                        if (LegendTextPattern != null)
                            series.LegendTextPattern = LegendTextPattern;

                        int valueCount = Math.Max(GetValuePointCount(ChartType), 1);
                        for (int i = 0; i < valueCount && seriesCounter < Values.Length; i++)
                            series.ValueDataMembers.AddRange(Values[seriesCounter++]);

                        chart.Series.Add(series);
                    }
                }
            }

            var chartContext = new ChartContext()
            {
                Chart          = chart,
                DiagramContext = _DiagramContext
            };
            chart.Tag = chartContext;
            WriteObject(chartContext);
        }

        protected override Bitmap PaintChart(Chart chart, int width = 2000, int height = 1200, int? dpi = null)
        {
            if (_DiagramContext != null && chart.Diagram != null)
                _DiagramContext.SetupDiagram(chart.Diagram);

            return base.PaintChart(chart, width, height, dpi);
        }

        protected virtual DevExpress.XtraCharts.Native.Chart CreateChart()
        {
            Utils.StartProfile("ChartCmdlet");

            var container = new ChartContainer();
            var chart = new DevExpress.XtraCharts.Native.Chart(container)
            {
                CacheToMemory = true,
                AnimationStartMode = ChartAnimationMode.None
            };
            container.Assign(chart);

            using (new UsingProcessor(() => container.BeginLoad(), () => container.EndLoad()))
            {
                if (!string.IsNullOrWhiteSpace(TemplateFile))
                {
                    var templateFile = Project.Current.MapPath(TemplateFile);
                    if (!File.Exists(templateFile))
                        throw new Exception($"File '{TemplateFile}' does not exist.");

                    ExecuteLocked(() =>
                    {
                        using var stream = new FileStream(templateFile, FileMode.Open);
                        chart.LoadLayout(stream);
                    }, LockFiles ? LockObject : null);
                }

                if (Palette.HasValue && Palette.Value != ChartPaletteName.None)
                {
                    string paletteName = Regex.Replace(Enum.GetName(typeof(ChartPaletteName), Palette.Value), "([A-Z])", " $1").Trim();
                    chart.PaletteName = paletteName;
                }
                if (PaletteBaseColorNumber.HasValue)
                    chart.PaletteBaseColorNumber = PaletteBaseColorNumber.Value;
                if (IndicatorsPalette.HasValue && IndicatorsPalette != ChartPaletteName.None)
                {
                    string paletteName = Regex.Replace(Enum.GetName(typeof(ChartPaletteName), Palette), "(?-i)([A-Z])", " $1").Trim();
                    chart.IndicatorsPaletteName = paletteName;
                }

                if (DisableAutoLayout)
                    chart.AutoLayout = false;

                if (!string.IsNullOrWhiteSpace(BackColor))
                {
                    var backColor = Utils.ColorFromString(BackColor);
                    if (backColor != Color.Empty)
                        chart.BackColor = backColor;
                }

                if (!string.IsNullOrWhiteSpace(BorderColor))
                {
                    var borderColor = Utils.ColorFromString(BorderColor);
                    if (borderColor != Color.Empty)
                    {
                        chart.Border.Color = borderColor;
                        chart.Border.Visibility = DefaultBoolean.True;
                    }
                }

                chart.Border.Visibility = BorderThickness > 0 ? DefaultBoolean.True : DefaultBoolean.False;
                if (BorderThickness.HasValue)
                    chart.Border.Thickness = BorderThickness.Value;

                if (FillMode.HasValue)
                {
                    chart.FillStyle.FillMode = FillMode.Value;
                    switch (FillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            var backColor = Utils.ColorFromString(BackColor);
                            if (backColor != Color.Empty)
                                chart.BackColor = backColor;
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (chart.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = FillGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (chart.FillStyle.Options is HatchFillOptions hatchOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != Color.Empty)
                                    hatchOptions.Color2 = backColor2;
                                if (FillHatchStyle.HasValue)
                                    hatchOptions.HatchStyle = FillHatchStyle.Value;
                            }
                            break;
                    }
                }
            }

            return chart;
        }
    }
}
