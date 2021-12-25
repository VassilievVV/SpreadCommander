using DevExpress.Charts.Model;
using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class SeriesOptions
    {
        [Description("Series data source.")]
        public object DataSource { get; set; }

        [Description("Name of the data field that is used to specify series point colors.")]
        public string ColorField { get; set; }

        [Description("Scale type for the argument data of the series' data points.")]
        [DefaultValue(ScaleType.Auto)]
        public ScaleType ArgumentScaleType { get; set; } = ScaleType.Auto;

        [Description("Scale type for the value data of the series' data points.")]
        [DefaultValue(ScaleType.Auto)]
        public ScaleType ValueScaleType { get; set; } = ScaleType.Auto;

        [Description("Detail level for date-time values.")]
        public DateTimeMeasureUnit? DateTimeSummaryMeasureUnit { get; set; }

        [Description("Factor on which the series multiplies the measurement unit to form a custom measurement unit.")]
        public int? DateTimeSummaryMeasureUnitMultiplier { get; set; }

        [Description("Summary function that calculates the series points' total value.")]
        public string DateTimeSummaryFunction { get; set; }

        [Description("Detail level for numeric values.")]
        public double? NumericSummaryMeasureUnit { get; set; }

        [Description("Summary function that calculates the series points' total value.")]
        public string NumericSummaryFunction { get; set; }

        [Description("Summary function that calculates the series points' total value.")]
        public string QualitativeSummaryFunction { get; set; }

        [Description("Filter expression.")]
        public string Filter { get; set; }

        [Description("Name of legend (added with cmdlet Add-ChartLegend) to use for series.")]
        public string LegendName { get; set; }

        [Description("String which represents the pattern specifying the text to be displayed within legend items")]
        public string LegendTextPattern { get; set; }

        [Description("Sort order of the series' points.")]
        public SortingMode? PointsSorting { get; set; }

        [Description("Whether the data series is represented in the chart control's legend.")]
        public bool HideInLegend { get; set; }

        [Description("Threshold value for TopN feature. Its meaning depends on TopNMode.")]
        public double? TopNThreshold { get; set; }

        [Description("Specifies how to determine the total number of top N series points. TopN is ignored for series with more than one Values, such as Bubble and Financial series.")]
        public TopNMode? TopNMode { get; set; }

        [Description("Whether it is necessary to show the 'Others' series point.")]
        [DefaultValue(true)]
        public bool? TopNShowOthers { get; set; } = true;

        [Description("Text which should be displayed as the argument of the 'Others' series point.")]
        public string TopNOthersArgument { get; set; }

        [Description("Show or hide series labels.")]
        public bool? ShowLabels { get; set; }


        protected internal virtual void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            if (!string.IsNullOrWhiteSpace(name))
                series.Name = name;
            else if (!string.IsNullOrWhiteSpace(argument))
                series.Name = argument;

            if (DataSource != null)
                series.DataSource = Utils.ConvertListToDataTable(DataSource, true);

            series.ArgumentDataMember = argument;
            if (values != null)
                series.ValueDataMembers.AddRange(values);
            series.ColorDataMember = ColorField;

            if (ArgumentScaleType != ScaleType.Auto)
                series.ArgumentScaleType = (DevExpress.XtraCharts.ScaleType)ArgumentScaleType;
            if (ValueScaleType != ScaleType.Auto)
                series.ValueScaleType = (DevExpress.XtraCharts.ScaleType)ValueScaleType;

            if (DateTimeSummaryMeasureUnit.HasValue)
            {
                series.DateTimeSummaryOptions.MeasureUnit = (DevExpress.XtraCharts.DateTimeMeasureUnit)DateTimeSummaryMeasureUnit.Value;
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
                var legend = chart.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'.");
                series.Legend = legend;
            }

            if (!string.IsNullOrWhiteSpace(LegendTextPattern))
                series.LegendTextPattern = LegendTextPattern;
            if (PointsSorting.HasValue)
                series.SeriesPointsSorting = (DevExpress.XtraCharts.SortingMode)PointsSorting.Value;
            series.ShowInLegend = !HideInLegend;
            if (TopNMode.HasValue)
            {
                series.TopNOptions.Mode = (DevExpress.XtraCharts.TopNMode)TopNMode.Value;
                switch (TopNMode.Value)
                {
                    case Chart.TopNMode.Count:
                        series.TopNOptions.Count = Convert.ToInt32(TopNThreshold);
                        break;
                    case Chart.TopNMode.ThresholdValue:
                        if (TopNThreshold.HasValue)
                            series.TopNOptions.ThresholdValue = TopNThreshold.Value;
                        break;
                    case Chart.TopNMode.ThresholdPercent:
                        if (TopNThreshold.HasValue)
                            series.TopNOptions.ThresholdPercent = TopNThreshold.Value;
                        break;
                }
                if (TopNShowOthers.HasValue)
                    series.TopNOptions.ShowOthers = TopNShowOthers.Value;
                if (!string.IsNullOrWhiteSpace(TopNOthersArgument))
                    series.TopNOptions.OthersArgument = TopNOthersArgument;
            }

            if (ShowLabels.HasValue)
                series.LabelsVisibility = ShowLabels.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
        }

        protected internal virtual void BoundDataChanged(SCChart chart, DevExpress.XtraCharts.Series series)
        {
        }
    }
}

namespace SpreadCommander.Common.Script.Chart
{ 
    public partial class SCChart
    {
        protected internal DevExpress.XtraCharts.Series CurrentSeries { get; set; }

        public SCChart AddSeries(ChartViewType viewType, string argument, string values, Series.SeriesOptions options = null) =>
            AddSeries(viewType, null, argument, new string[] { values }, options);

        public SCChart AddSeries(ChartViewType viewType, string argument, string[] values, Series.SeriesOptions options = null) =>
            AddSeries(viewType, null, argument, values, options);

        public SCChart AddSeries(ChartViewType viewType, string name, string argument, string values, Series.SeriesOptions options = null) =>
            AddSeries(viewType, name, argument, new string[] { values }, options);

        public SCChart AddSeries(ChartViewType viewType, string name, string argument, string[] values, Series.SeriesOptions options = null)
        {
            options ??= new Series.SeriesOptions();

            var series = new DevExpress.XtraCharts.Series(name, (ViewType)viewType)
            {
                Tag = options
            };

            options.SetupXtraChartSeries(this, series, name, argument, values);

            CurrentSeries = series;

            return this;
        }
    }
}
