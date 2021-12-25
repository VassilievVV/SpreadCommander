using DevExpress.Utils;
using DevExpress.XtraCharts;
using Native = DevExpress.XtraCharts.Native;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Script.Chart.Diagrams;

namespace SpreadCommander.Common.Script.Chart
{
    public partial class SCChart : ScriptHostObject, IDisposable
    {
        private readonly ChartViewType _ViewType;
        private readonly Native.Chart _Chart;
        private readonly DiagramOptions _ChartOptions;
        private readonly object _DataSource;
        private readonly string[] _Arguments;
        private readonly string[] _Values;

        public SCChart(object dataSource, ChartViewType viewType, 
            string[] arguments, string[] values,
            DiagramOptions options = null) : base()
        {
            _ViewType     = viewType;
            _ChartOptions = options ?? new DiagramOptions();
            _Chart        = CreateChart(options);
            
            _DataSource   = dataSource;
            _Arguments    = arguments;
            _Values       = values;

            InitializeChart();
        }

        public SCChart(object dataSource, ChartViewType viewType,
            DiagramOptions options = null) : this(dataSource, viewType, (string[])null, (string[])null, options)
        {
        }

        public SCChart(object dataSource, ChartViewType viewType,
            string arguments, string[] values,
            DiagramOptions options = null) : this(dataSource, viewType, new string[] { arguments }, values, options)
        {
        }

        public SCChart(object dataSource, ChartViewType viewType,
            string arguments, string values,
            DiagramOptions options = null) : this(dataSource, viewType, new string[] { arguments }, new string[] { values }, options)
        {
        }

        public void Dispose()
        {
        }

        internal Native.Chart Chart     => _Chart;
        internal ChartViewType ViewType => _ViewType;

        protected virtual void SetupDiagram(Diagram diagram)
        {
            _ChartOptions?.SetupDiagram(diagram);
        }

        protected virtual void InitializeChart()
        {
            var dataSource = GetDataSource(_DataSource,
                new DataSourceParameters() { IgnoreErrors = _ChartOptions.IgnoreErrors, Columns = _ChartOptions.SelectColumns, SkipColumns = _ChartOptions.SkipColumns });

            var chart = CreateChart(_ChartOptions);
            chart.Tag = this;
            chart.DataContainer.DataSource = dataSource;
            chart.DataContainer.DataMember = null;

            if (_ChartOptions.LegendVisibility.HasValue)
                chart.Legend.Visibility = _ChartOptions.LegendVisibility.Value ? DefaultBoolean.True : DefaultBoolean.False;

            if (!string.IsNullOrWhiteSpace(_ChartOptions.SeriesField))
            {
                //Series template
                if (_Arguments == null || _Arguments.Length != 1)
                    throw new Exception("Series template allows only one Arguments");
                if (_Values == null || _Values.Length <= 0)
                    throw new Exception("Series template requires at least one Values");

                var seriesTemplate = chart.DataContainer.SeriesTemplate;
                seriesTemplate.ChangeView((ViewType)_ViewType);
                seriesTemplate.SeriesDataMember   = _ChartOptions.SeriesField;
                seriesTemplate.ArgumentDataMember = _Arguments[0];
                seriesTemplate.ValueDataMembers.AddRange(_Values);

                if (seriesTemplate.Label != null)
                {
                    if (_ChartOptions.TextPattern != null)    //allow empty string pattern
                        seriesTemplate.Label.TextPattern = _ChartOptions.TextPattern;
                    if (_ChartOptions.ResolveOverlappingMinIndent.HasValue)
                        seriesTemplate.Label.ResolveOverlappingMinIndent = _ChartOptions.ResolveOverlappingMinIndent.Value;
                    if (_ChartOptions.ResolveOverlappingMode.HasValue)
                        seriesTemplate.Label.ResolveOverlappingMode = (DevExpress.XtraCharts.ResolveOverlappingMode)_ChartOptions.ResolveOverlappingMode.Value;

                    if (_ChartOptions.LabelsMaxLineCount.HasValue)
                        seriesTemplate.Label.MaxLineCount = _ChartOptions.LabelsMaxLineCount.Value;
                    if (_ChartOptions.LabelsMaxWidth.HasValue)
                        seriesTemplate.Label.MaxWidth = _ChartOptions.LabelsMaxWidth.Value;
                }

                if (_ChartOptions.LabelsVisibility.HasValue)
                    seriesTemplate.LabelsVisibility = _ChartOptions.LabelsVisibility.Value ? DefaultBoolean.True : DefaultBoolean.False;

                if (_ChartOptions.LegendTextPattern != null)
                    seriesTemplate.LegendTextPattern = _ChartOptions.LegendTextPattern;
            }
            else
            {
                //Quick series
                if (_Values != null && _Values.Length > 0)
                {
                    string argumentField = null;
                    int seriesCounter = 0;
                    while (seriesCounter < _Values.Length)
                    {
                        if (_Arguments != null && seriesCounter < _Arguments.Length)
                            argumentField = _Arguments[seriesCounter];   //Propagate argument field to next series. I.e. if there is 1 Argument and 3 Values - use this Argument for all series.

                        var series = new DevExpress.XtraCharts.Series($"Series{seriesCounter}", (ViewType)_ViewType)
                        {
                            Name = _Values[seriesCounter],
                            ArgumentDataMember = argumentField
                        };

                        if (series.Label != null)
                        {
                            series.Label.EnableAntialiasing = DefaultBoolean.True;

                            if (_ChartOptions.TextPattern != null)    //allow empty string pattern
                                series.Label.TextPattern = _ChartOptions.TextPattern;
                            if (_ChartOptions.ResolveOverlappingMinIndent.HasValue)
                                series.Label.ResolveOverlappingMinIndent = _ChartOptions.ResolveOverlappingMinIndent.Value;
                            if (_ChartOptions.ResolveOverlappingMode.HasValue)
                                series.Label.ResolveOverlappingMode = (DevExpress.XtraCharts.ResolveOverlappingMode)_ChartOptions.ResolveOverlappingMode.Value;

                            if (_ChartOptions.LabelsMaxLineCount.HasValue)
                                series.Label.MaxLineCount = _ChartOptions.LabelsMaxLineCount.Value;
                            if (_ChartOptions.LabelsMaxWidth.HasValue)
                                series.Label.MaxWidth = _ChartOptions.LabelsMaxWidth.Value;
                        }

                        if (_ChartOptions.LabelsVisibility.HasValue)
                            series.LabelsVisibility = _ChartOptions.LabelsVisibility.Value ? DefaultBoolean.True : DefaultBoolean.False;

                        if (_ChartOptions.LegendTextPattern != null)
                            series.LegendTextPattern = _ChartOptions.LegendTextPattern;

                        int valueCount = Math.Max(GetValuePointCount(_ViewType), 1);
                        for (int i = 0; i < valueCount && seriesCounter < _Values.Length; i++)
                            series.ValueDataMembers.AddRange(_Values[seriesCounter++]);

                        chart.Series.Add(series);
                    }
                }
            }
        }

        protected Bitmap PaintChart(DevExpress.XtraCharts.Native.Chart chart, 
            int width = 2000, int height = 1200, int? dpi = null)
        {
            if (_Chart.Diagram != null)
                SetupDiagram(_Chart.Diagram);

            if (dpi == null || dpi == 0)
                dpi = DefaultDPI;

            float scaleBitmap = dpi.Value / 300f;
            float scaleSVG    = scaleBitmap * 96f / 300f;

            var svg = chart.CreateSvg(new Size((int)Math.Ceiling(width * scaleSVG), (int)Math.Ceiling(height * scaleSVG)));

            var doc            = SvgDocument.Open(svg);
            doc.ShapeRendering = SvgShapeRendering.GeometricPrecision;

            var bitmap = new Bitmap((int)Math.Ceiling(width * scaleBitmap), (int)Math.Ceiling(height * scaleBitmap));
            bitmap.SetResolution(dpi.Value, dpi.Value);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode   = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode     = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                graphics.Clear(Color.White);

                var renderer = SvgRenderer.FromGraphics(graphics);
                renderer.SmoothingMode = SmoothingMode.HighQuality;
                if (scaleSVG != 1f && scaleSVG > 0f)
                    renderer.ScaleTransform(1 / scaleSVG, 1 / scaleSVG);

                doc.Draw(renderer);
            }

            return bitmap;
        }

        protected virtual void DoWriteImage(Document book, Image chartBitmap)
        {
            if (chartBitmap == null)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { book.EndUpdate(); }))
            {
                book.Images.Append(chartBitmap);
                var paragraph = book.Paragraphs.Append();

                book.CaretPosition = paragraph.Range.End;
                Book.SCBook.ResetBookFormatting(book);
            }
        }

        protected static int GetValuePointCount(ChartViewType viewType)
        {
            return viewType switch
            {
                ChartViewType.Bar                        => 1,
                ChartViewType.StackedBar                 => 1,
                ChartViewType.FullStackedBar             => 1,
                ChartViewType.SideBySideStackedBar       => 1,
                ChartViewType.SideBySideFullStackedBar   => 1,
                ChartViewType.Pie                        => 1,
                ChartViewType.Doughnut                   => 1,
                ChartViewType.NestedDoughnut             => 1,
                ChartViewType.Funnel                     => 1,
                ChartViewType.Point                      => 1,
                ChartViewType.Bubble                     => 2,	//Value and Weight
                ChartViewType.Line                       => 1,
                ChartViewType.StackedLine                => 1,
                ChartViewType.FullStackedLine            => 1,
                ChartViewType.StepLine                   => 1,
                ChartViewType.Spline                     => 1,
                ChartViewType.ScatterLine                => 1,
                ChartViewType.SwiftPlot                  => 1,
                ChartViewType.Area                       => 1,
                ChartViewType.StepArea                   => 1,
                ChartViewType.SplineArea                 => 1,
                ChartViewType.StackedArea                => 1,
                ChartViewType.StackedStepArea            => 1,
                ChartViewType.StackedSplineArea          => 1,
                ChartViewType.FullStackedArea            => 1,
                ChartViewType.FullStackedSplineArea      => 1,
                ChartViewType.FullStackedStepArea        => 1,
                ChartViewType.RangeArea                  => 2,
                ChartViewType.Stock                      => 4,   //Low, High, Open, Close
                ChartViewType.CandleStick                => 4,   //Low, High, Open, Close
                ChartViewType.SideBySideRangeBar         => 2,
                ChartViewType.RangeBar                   => 2,
                ChartViewType.SideBySideGantt            => 2,   //Date-time values (Start and End)
                ChartViewType.Gantt                      => 2,   //Date-time values (Start and End)
                ChartViewType.PolarPoint                 => 1,
                ChartViewType.PolarLine                  => 1,
                ChartViewType.ScatterPolarLine           => 1,
                ChartViewType.PolarArea                  => 1,
                ChartViewType.PolarRangeArea             => 2,
                ChartViewType.RadarPoint                 => 1,
                ChartViewType.RadarLine                  => 1,
                ChartViewType.ScatterRadarLine           => 1,
                ChartViewType.RadarArea                  => 1,
                ChartViewType.RadarRangeArea             => 2,
                ChartViewType.Bar3D                      => 1,
                ChartViewType.StackedBar3D               => 1,
                ChartViewType.FullStackedBar3D           => 1,
                ChartViewType.ManhattanBar               => 1,
                ChartViewType.SideBySideStackedBar3D     => 1,
                ChartViewType.SideBySideFullStackedBar3D => 1,
                ChartViewType.Pie3D                      => 1,
                ChartViewType.Doughnut3D                 => 1,
                ChartViewType.Funnel3D                   => 1,
                ChartViewType.Line3D                     => 1,
                ChartViewType.StackedLine3D              => 1,
                ChartViewType.FullStackedLine3D          => 1,
                ChartViewType.StepLine3D                 => 1,
                ChartViewType.Area3D                     => 1,
                ChartViewType.StackedArea3D              => 1,
                ChartViewType.FullStackedArea3D          => 1,
                ChartViewType.StepArea3D                 => 1,
                ChartViewType.Spline3D                   => 1,
                ChartViewType.SplineArea3D               => 1,
                ChartViewType.StackedSplineArea3D        => 1,
                ChartViewType.FullStackedSplineArea3D    => 1,
                ChartViewType.RangeArea3D                => 2,
                _                                        => 1
            };
        }

        protected virtual DevExpress.XtraCharts.Native.Chart CreateChart(DiagramOptions options)
        {
            Utils.StartProfile("ChartScriptObject");

            options ??= new DiagramOptions();

            var container = new ChartContainer();
            var chart     = new DevExpress.XtraCharts.Native.Chart(container)
            {
                CacheToMemory      = true,
                AnimationStartMode = ChartAnimationMode.None
            };
            container.Assign(chart);

            using (new UsingProcessor(() => container.BeginLoad(), () => container.EndLoad()))
            {
                if (!string.IsNullOrWhiteSpace(options.TemplateFile))
                {
                    var templateFile = Project.Current.MapPath(options.TemplateFile);
                    if (!File.Exists(templateFile))
                        throw new Exception($"File '{options.TemplateFile}' does not exist.");

                    ExecuteLocked(() =>
                    {
                        using var stream = new FileStream(templateFile, FileMode.Open);
                        chart.LoadLayout(stream);
                    }, options.LockFiles ? LockObject : null);
                }

                if (options.Palette.HasValue && options.Palette.Value != ChartPaletteName.None)
                {
                    string paletteName = Regex.Replace(Enum.GetName(typeof(ChartPaletteName), options.Palette.Value), "([A-Z])", " $1").Trim();
                    chart.PaletteName  = paletteName;
                }
                if (options.PaletteBaseColorNumber.HasValue)
                    chart.PaletteBaseColorNumber = options.PaletteBaseColorNumber.Value;
                if (options.IndicatorsPalette.HasValue && options.IndicatorsPalette != ChartPaletteName.None)
                {
                    string paletteName = Regex.Replace(Enum.GetName(typeof(ChartPaletteName), options.Palette), "(?-i)([A-Z])", " $1").Trim();
                    chart.IndicatorsPaletteName = paletteName;
                }

                if (options.DisableAutoLayout)
                    chart.AutoLayout = false;

                if (!string.IsNullOrWhiteSpace(options.BackColor))
                {
                    var backColor = Utils.ColorFromString(options.BackColor);
                    if (backColor != Color.Empty)
                        chart.BackColor = backColor;
                }

                if (!string.IsNullOrWhiteSpace(options.BorderColor))
                {
                    var borderColor = Utils.ColorFromString(options.BorderColor);
                    if (borderColor != Color.Empty)
                    {
                        chart.Border.Color      = borderColor;
                        chart.Border.Visibility = DefaultBoolean.True;
                    }
                }

                chart.Border.Visibility = options.BorderThickness > 0 ? DefaultBoolean.True : DefaultBoolean.False;
                if (options.BorderThickness.HasValue)
                    chart.Border.Thickness = options.BorderThickness.Value;

                if (options.FillMode.HasValue)
                {
                    chart.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)options.FillMode.Value;
                    switch (options.FillMode.Value)
                    {
                        case FillMode.Empty:
                            break;
                        case FillMode.Solid:
                            var backColor = Utils.ColorFromString(options.BackColor);
                            if (backColor != Color.Empty)
                                chart.BackColor = backColor;
                            break;
                        case FillMode.Gradient:
                            if (chart.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var backColor2 = Utils.ColorFromString(options.BackColor2);
                                if (backColor2 != Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (options.FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)options.FillGradientMode.Value;
                            }
                            break;
                        case FillMode.Hatch:
                            if (chart.FillStyle.Options is HatchFillOptions hatchOptions)
                            {
                                var backColor2 = Utils.ColorFromString(options.BackColor2);
                                if (backColor2 != Color.Empty)
                                    hatchOptions.Color2 = backColor2;
                                if (options.FillHatchStyle.HasValue)
                                    hatchOptions.HatchStyle = options.FillHatchStyle.Value;
                            }
                            break;
                    }
                }
            }

            return chart;
        }

        protected void ScrollToCaret()
        {
            Host?.Engine?.ScrollToCaret();
        }
    }
}
