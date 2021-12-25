using DevExpress.XtraCharts;
using SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using DevExpress.Utils;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartSeries")]
    public class AddChartSeriesCmdlet: BaseChartWithContextCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Series type.")]
        public ViewType SeriesType { get; set; }

        [Parameter(HelpMessage = "Name of the series.")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "Series data source.")]
        public object DataSource { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Name of the data field that contains series point arguments.")]
        public string Argument { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Names of data fields that contain series point values.")]
        public string[] Values { get; set; }

        [Parameter(HelpMessage = "Name of the data field that is used to specify series point colors.")]
        public string ColorField { get; set; }

        [Parameter(HelpMessage = "Scale type for the argument data of the series' data points.")]
        [PSDefaultValue(Value = ScaleType.Auto)]
        [DefaultValue(ScaleType.Auto)]
        public ScaleType ArgumentScaleType { get; set; } = ScaleType.Auto;

        [Parameter(HelpMessage = "Scale type for the value data of the series' data points.")]
        [PSDefaultValue(Value = ScaleType.Auto)]
        [DefaultValue(ScaleType.Auto)]
        public ScaleType ValueScaleType { get; set; } = ScaleType.Auto;

        [Parameter(HelpMessage = "Detail level for date-time values.")]
        public DateTimeMeasureUnit? DateTimeSummaryMeasureUnit { get; set; }

        [Parameter(HelpMessage = "Factor on which the series multiplies the measurement unit to form a custom measurement unit.")]
        public int? DateTimeSummaryMeasureUnitMultiplier { get; set; }

        [Parameter(HelpMessage = "Summary function that calculates the series points' total value.")]
        public string DateTimeSummaryFunction { get; set; }

        [Parameter(HelpMessage = "Detail level for numeric values.")]
        public double? NumericSummaryMeasureUnit { get; set; }

        [Parameter(HelpMessage = "Summary function that calculates the series points' total value.")]
        public string NumericSummaryFunction { get; set; }

        [Parameter(HelpMessage = "Summary function that calculates the series points' total value.")]
        public string QualitativeSummaryFunction { get; set; }

        [Parameter(HelpMessage = "Filter expression.")]
        public string Filter { get; set; }

        [Parameter(HelpMessage = "Name of legend (added with cmdlet Add-ChartLegend) to use for series.")]
        public string LegendName { get; set; }

        [Parameter(HelpMessage = "String which represents the pattern specifying the text to be displayed within legend items")]
        public string LegendTextPattern { get; set; }

        [Parameter(HelpMessage = "Sort order of the series' points.")]
        public SortingMode? PointsSorting { get; set; }

        [Parameter(HelpMessage = "Whether the data series is represented in the chart control's legend.")]
        public SwitchParameter HideInLegend { get; set; }

        [Parameter(HelpMessage = "Threshold value for TopN feature. Its meaning depends on TopNMode.")]
        public double? TopNThreshold { get; set; }

        [Parameter(HelpMessage = "Specifies how to determine the total number of top N series points. TopN is ignored for series with more than one Values, such as Bubble and Financial series.")]
        public TopNMode? TopNMode { get; set; }

        [Parameter(HelpMessage = "Whether it is necessary to show the 'Others' series point.")]
        [PSDefaultValue(Value = true)]
        [DefaultValue(true)]
        public bool? TopNShowOthers { get; set; } = true;

        [Parameter(HelpMessage = "Text which should be displayed as the argument of the 'Others' series point.")]
        public string TopNOthersArgument { get; set; }

        [Parameter(HelpMessage = "Hide series labels.")]
        [Alias("HideLabels")]
        public SwitchParameter NoLabels { get; set; }

        [Parameter(HelpMessage = "Show series labels.")]
        public SwitchParameter ShowLabels { get; set; }


        private BaseSeriesContext _SeriesContext;

        public object GetDynamicParameters()
        {
            switch (SeriesType)
            {
                //Simple
                case ViewType.Pie:
                    return CreateSeriesContext(typeof(PieSeriesContext));
                case ViewType.Doughnut:
                    return CreateSeriesContext(typeof(DoughnutSeriesContext));
                case ViewType.NestedDoughnut:
                    return CreateSeriesContext(typeof(NestedDoughnutSeriesContext));
                case ViewType.Funnel:
                    return CreateSeriesContext(typeof(FunnelSeriesContext));

                //Simple3D
                case ViewType.Pie3D:
                    return CreateSeriesContext(typeof(PieSeriesContext));
                case ViewType.Doughnut3D:
                    return CreateSeriesContext(typeof(DoughnutSeriesContext));

                //XY
                case ViewType.Bar:
                    return CreateSeriesContext(typeof(SideBySideBarSeriesContext));
                case ViewType.StackedBar:
                    return CreateSeriesContext(typeof(BarSeriesContext));
                case ViewType.FullStackedBar:
                    return CreateSeriesContext(typeof(BarSeriesContext));
                case ViewType.SideBySideStackedBar:
                    return CreateSeriesContext(typeof(SideBySideStackedBarSeriesContext));
                case ViewType.SideBySideFullStackedBar:
                    return CreateSeriesContext(typeof(SideBySideFullStackedBarSeriesContext));
                case ViewType.Point:
                    return CreateSeriesContext(typeof(PointSeriesContext));
                case ViewType.Bubble:
                    return CreateSeriesContext(typeof(BubbleSeriesContext));
                case ViewType.Line:
                    return CreateSeriesContext(typeof(LineSeriesContext));
                case ViewType.StackedLine:
                    return CreateSeriesContext(typeof(LineSeriesContext));
                case ViewType.FullStackedLine:
                    return CreateSeriesContext(typeof(LineSeriesContext));
                case ViewType.StepLine:
                    return CreateSeriesContext(typeof(LineSeriesContext));
                case ViewType.Spline:
                    return CreateSeriesContext(typeof(SplineSeriesContext));
                case ViewType.ScatterLine:
                    return CreateSeriesContext(typeof(LineSeriesContext));
                case ViewType.Area:
                    return CreateSeriesContext(typeof(AreaSeriesContext));
                case ViewType.StepArea:
                    return CreateSeriesContext(typeof(StepAreaSeriesContext));
                case ViewType.SplineArea:
                    return CreateSeriesContext(typeof(SplineAreaSeriesContext));
                case ViewType.StackedArea:
                    return CreateSeriesContext(typeof(StackedAreaSeriesContext));
                case ViewType.StackedStepArea:
                    return CreateSeriesContext(typeof(StackedAreaSeriesContext));
                case ViewType.StackedSplineArea:
                    return CreateSeriesContext(typeof(StackedSplineAreaSeriesContext));
                case ViewType.FullStackedArea:
                    return CreateSeriesContext(typeof(StackedAreaSeriesContext));
                case ViewType.FullStackedSplineArea:
                    return CreateSeriesContext(typeof(FullStackedSplineAreaSeriesContext));
                case ViewType.FullStackedStepArea:
                    return CreateSeriesContext(typeof(StackedAreaSeriesContext));
                case ViewType.RangeArea:
                    return CreateSeriesContext(typeof(RangeAreaSeriesContext));
                case ViewType.Stock:
                    return CreateSeriesContext(typeof(FinancialSeriesContext));
                case ViewType.CandleStick:
                    return CreateSeriesContext(typeof(CandleStickSeriesContext));
                case ViewType.SideBySideRangeBar:
                    return CreateSeriesContext(typeof(SideBySideRangeBarSeriesContext));
                case ViewType.RangeBar:
                    return CreateSeriesContext(typeof(RangeBarSeriesContext));
                case ViewType.BoxPlot:
                    return CreateSeriesContext(typeof(BoxPlotSeriesContext));
                case ViewType.Waterfall:
                    return CreateSeriesContext(typeof(WaterfallSeriesContext));

                //XY3D
                case ViewType.Bar3D:
                    return CreateSeriesContext(typeof(Bar3DSeriesContext));
                case ViewType.StackedBar3D:
                    return CreateSeriesContext(typeof(Bar3DSeriesContext));
                case ViewType.FullStackedBar3D:
                    return CreateSeriesContext(typeof(Bar3DSeriesContext));
                case ViewType.ManhattanBar:
                    return CreateSeriesContext(typeof(Bar3DSeriesContext));
                case ViewType.SideBySideStackedBar3D:
                    return CreateSeriesContext(typeof(SideBySideStackedBar3DSeriesContext));
                case ViewType.SideBySideFullStackedBar3D:
                    return CreateSeriesContext(typeof(SideBySideStackedBar3DSeriesContext));
                case ViewType.Line3D:
                    return CreateSeriesContext(typeof(Line3DSeriesContext));
                case ViewType.StackedLine3D:
                    return CreateSeriesContext(typeof(Line3DSeriesContext));
                case ViewType.FullStackedLine3D:
                    return CreateSeriesContext(typeof(Line3DSeriesContext));
                case ViewType.StepLine3D:
                    return CreateSeriesContext(typeof(StepLine3DSeriesContext));
                case ViewType.Area3D:
                    return CreateSeriesContext(typeof(Area3DSeriesContext));
                case ViewType.StackedArea3D:
                    return CreateSeriesContext(typeof(Area3DSeriesContext));
                case ViewType.FullStackedArea3D:
                    return CreateSeriesContext(typeof(Area3DSeriesContext));
                case ViewType.StepArea3D:
                    return CreateSeriesContext(typeof(StepArea3DSeriesContext));
                case ViewType.Spline3D:
                    return CreateSeriesContext(typeof(Spline3DSeriesContext));
                case ViewType.SplineArea3D:
                    return CreateSeriesContext(typeof(SplineAreaSeriesContext));
                case ViewType.StackedSplineArea3D:
                    return CreateSeriesContext(typeof(Area3DSeriesContext));
                case ViewType.FullStackedSplineArea3D:
                    return CreateSeriesContext(typeof(Area3DSeriesContext));
                case ViewType.RangeArea3D:
                    return CreateSeriesContext(typeof(Area3DSeriesContext));

                //Gannt
                case ViewType.SideBySideGantt:
                    return CreateSeriesContext(typeof(SideBySideGanttSeriesContext));
                case ViewType.Gantt:
                    return CreateSeriesContext(typeof(GanttSeriesContext));

                //Funnel3D
                case ViewType.Funnel3D:
                    return CreateSeriesContext(typeof(Funnel3DSeriesContext));

                //Radar and Polar
                case ViewType.PolarPoint:
                    return CreateSeriesContext(typeof(RadarPointSeriesContext));
                case ViewType.PolarLine:
                    return CreateSeriesContext(typeof(RadarLineSeriesContext));
                case ViewType.ScatterPolarLine:
                    return CreateSeriesContext(typeof(RadarLineSeriesContext));
                case ViewType.PolarArea:
                    return CreateSeriesContext(typeof(RadarAreaSeriesContext));
                case ViewType.PolarRangeArea:
                    return CreateSeriesContext(typeof(RadarRangeAreaSeriesContext));
                case ViewType.RadarPoint:
                    return CreateSeriesContext(typeof(RadarPointSeriesContext));
                case ViewType.RadarLine:
                    return CreateSeriesContext(typeof(RadarLineSeriesContext));
                case ViewType.ScatterRadarLine:
                    return CreateSeriesContext(typeof(RadarLineSeriesContext));
                case ViewType.RadarArea:
                    return CreateSeriesContext(typeof(RadarAreaSeriesContext));
                case ViewType.RadarRangeArea:
                    return CreateSeriesContext(typeof(RadarRangeAreaSeriesContext));

                //Swift
                case ViewType.SwiftPlot:
                    return CreateSeriesContext(typeof(SwiftPlotSeriesContext));
            }

            _SeriesContext = null;
            return null;


            BaseSeriesContext CreateSeriesContext(Type typeContext)
            {
                if (_SeriesContext == null || !(typeContext.IsInstanceOfType(_SeriesContext)))
                    _SeriesContext = Activator.CreateInstance(typeContext) as BaseSeriesContext;
                return _SeriesContext;
            }
        }

        protected override void UpdateChart()
        {
            var series = new Series(Name, SeriesType);

            ChartContext.Chart.Series.Add(series);
            ChartContext.CurrentSeries = series;

            series.Tag = _SeriesContext;
            SetupXtraChartSeries(series);
            if (_SeriesContext != null)
                _SeriesContext.SetupXtraChartSeries(ChartContext, series);
        }

        protected virtual void SetupXtraChartSeries(Series series)
        {
            if (!string.IsNullOrWhiteSpace(Name))
                series.Name = Name;
            else if (!string.IsNullOrWhiteSpace(Argument))
                series.Name = Argument;

            if (DataSource != null)
                series.DataSource = GetDataSource(null, DataSource, new DataSourceParameters());

            series.ArgumentDataMember = Argument;
            if (Values != null)
                series.ValueDataMembers.AddRange(Values);
            series.ColorDataMember = ColorField;

            if (ArgumentScaleType != ScaleType.Auto)
                series.ArgumentScaleType = ArgumentScaleType;
            if (ValueScaleType != ScaleType.Auto)
                series.ValueScaleType = ValueScaleType;

            if (DateTimeSummaryMeasureUnit.HasValue)
            {
                series.DateTimeSummaryOptions.MeasureUnit = DateTimeSummaryMeasureUnit.Value;
                series.DateTimeSummaryOptions.UseAxisMeasureUnit = true;
            }
            if (DateTimeSummaryMeasureUnitMultiplier.HasValue)
                series.DateTimeSummaryOptions.MeasureUnitMultiplier = DateTimeSummaryMeasureUnitMultiplier.Value;
            if (!string.IsNullOrWhiteSpace(DateTimeSummaryFunction))
                series.DateTimeSummaryOptions.SummaryFunction = DateTimeSummaryFunction;

            if (NumericSummaryMeasureUnit.HasValue)
            {
                series.NumericSummaryOptions.MeasureUnit = NumericSummaryMeasureUnit.Value;
                series.NumericSummaryOptions.UseAxisMeasureUnit = true;
            }
            if (!string.IsNullOrWhiteSpace(NumericSummaryFunction))
                series.NumericSummaryOptions.SummaryFunction = NumericSummaryFunction;

            if (!string.IsNullOrWhiteSpace(QualitativeSummaryFunction))
                series.QualitativeSummaryOptions.SummaryFunction = QualitativeSummaryFunction;

            if (!string.IsNullOrWhiteSpace(Filter))
                series.FilterString = Filter;

            if (!string.IsNullOrWhiteSpace(LegendName))
            {
                var legend = ChartContext.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'.");
                series.Legend = legend;
            }

            if (!string.IsNullOrWhiteSpace(LegendTextPattern))
                series.LegendTextPattern = LegendTextPattern;
            if (PointsSorting.HasValue)
                series.SeriesPointsSorting = PointsSorting.Value;
            series.ShowInLegend = !HideInLegend;
            if (TopNMode.HasValue)
            {
                series.TopNOptions.Mode = TopNMode.Value;
                switch (TopNMode.Value)
                {
                    case DevExpress.XtraCharts.TopNMode.Count:
                        series.TopNOptions.Count = Convert.ToInt32(TopNThreshold);
                        break;
                    case DevExpress.XtraCharts.TopNMode.ThresholdValue:
                        if (TopNThreshold.HasValue)
                            series.TopNOptions.ThresholdValue = TopNThreshold.Value;
                        break;
                    case DevExpress.XtraCharts.TopNMode.ThresholdPercent:
                        if (TopNThreshold.HasValue)
                            series.TopNOptions.ThresholdPercent = TopNThreshold.Value;
                        break;
                }
                if (TopNShowOthers.HasValue)
                    series.TopNOptions.ShowOthers = TopNShowOthers.Value;
                if (!string.IsNullOrWhiteSpace(TopNOthersArgument))
                    series.TopNOptions.OthersArgument = TopNOthersArgument;
            }

            if (NoLabels)
                series.LabelsVisibility = DefaultBoolean.False;
            if (ShowLabels)
                series.LabelsVisibility = DefaultBoolean.True;
        }
    }
}
