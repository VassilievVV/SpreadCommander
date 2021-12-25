using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Script.Spreadsheet.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class ChartOptions: SpreadsheetWithCopyToBookOptions
    {
        [Description("Name of sheet with data. If not specified - DataTableName is searching in all sheets.")]
        public string DataSheetName { get; set; }

        [Description("Name of new sheet with chart.")]
        public string ChartSheetName { get; set; }

        [Description("Replace existing sheet if it exists")]
        public bool Replace { get; set; }

        [Description("Chart type")]
        public ChartType? ChartType { get; set; }

        [Description("Style to be applied to the chart.")]
        public ChartStyle? Style { get; set; }

        [Description("Whether each data point in the series has a different color.")]
        public bool VaryColors { get; set; }

        [Description("Specifies how empty cells should be plotted on a chart.")]
        public DisplayBlanksAs? DisplayBlanksAs { get; set; }

        //Fill
        //[Description("Fill pattern to fill chart.")]
        //public ShapeFillPatternType? FillPattern { get; set; }

        [Description("Background color.")]
        public string BackColor { get; set; }

        //[Description("Foreground color")]
        //public string ForeColor { get; set; }

        //[Description("Gradient types for the gradient fill.")]
        //public ShapeGradientType? GradientType { get; set; }

        //[Description("Angle by which the linear gradient fill should be rotated within the chart element.")]
        //public double? GradientAngle { get; set; }

        //Title
        [Description("Chart title")]
        public string Title { get; set; }

        [Description("Chart title's font in form 'Tahoma, 12, Bold, Italic, Red'.")]
        public string TitleFont { get; set; }

        //Legend
        [Description("Show or hide legend.")]
        public bool HideLegend { get; set; }

        [Description("Specify the position of the legend.")]
        public LegendPosition? LegendPosition { get; set; }

        [Description("Display category name in data labels.")]
        public bool DataLabelsShowCategoryName { get; set; }

        [Description("Display percentage value in data labels on pie or doughnut chart.")]
        public bool DataLabelsShowPercent { get; set; }

        [Description("Legend's font in form 'Tahoma, 10, Bold, Italic, Blue'.")]
        public string LegendFont { get; set; }

        //Axis
        [Description("Primary axis for chart. Count of axis depends on chart type.")]
        public SpreadChartAxis[] PrimaryAxes { get; set; }

        [Description("Secondary axis for chart. Count of axis depends on chart type.")]
        public SpreadChartAxis[] SecondaryAxes { get; set; }

        [Description("Custom size of the image with chart copied to Book. Must be 2-elements array")]
        public int[] CopyToBookSize { get; set; }

        [Description("Scale of the image with chart copied to Book. Can be used to control image quality similar to setting DPI")]
        public float? CopyToBookScale { get; set; }
    }

    public partial class SCSpreadsheet
    {
        public SCSpreadsheet NewChart(string dataTableName, SpreadChartSeries[] series, ChartOptions options = null)
        {
            ExecuteSynchronized(options, () => DoNewChart(dataTableName, series, options));
            return this;
        }

        protected virtual void DoNewChart(string dataTableName, SpreadChartSeries[] series, ChartOptions options)
        {
            options ??= new ChartOptions();
            var spread = options.Spreadsheet?.Workbook ?? Workbook;

            if (series == null || series.Length <= 0)
                throw new ArgumentException("At least one series must be provided.", nameof(series));

            ChartObject chart;

            using (new UsingProcessor(() => spread.BeginUpdate(), () => spread.EndUpdate()))
            {
                (var dataWorksheet, var table) = FindDataTable();

                int chartSheetIndex = -1;

                if (!string.IsNullOrWhiteSpace(options.ChartSheetName))
                {
                    for (int i = 0; i < spread.ChartSheets.Count; i++)
                    {
                        var sheet = spread.ChartSheets[i];

                        if (string.Compare(sheet.Name, options.ChartSheetName, true) == 0)
                        {
                            if (options.Replace)
                            {
                                chartSheetIndex = sheet.Index;
                                spread.ChartSheets.Remove(sheet);
                            }
                            else
                                throw new Exception($"Cannot create pivot sheet table: sheet '{options.ChartSheetName}' already exists.");

                            break;
                        }
                    }
                }

                var chartSheet = spread.ChartSheets.Add(options.ChartSheetName, (DevExpress.Spreadsheet.Charts.ChartType)(options.ChartType ?? ChartType.ColumnClustered));
                if (chartSheetIndex >= 0 && chartSheetIndex < spread.ChartSheets.Count)
                    chartSheet.Move(chartSheetIndex);

                chart = chartSheet.Chart;

                if (options.Style.HasValue)
                    chart.Style = (DevExpress.Spreadsheet.Charts.ChartStyle)options.Style.Value;

                //Commented code can be used and will produce chart with fill visible in MS Excel but not rendered in SpreadsheetControl
                if (!string.IsNullOrWhiteSpace(options.BackColor))
                {
                    var backColor = Utils.ColorFromString(options.BackColor);
                    //var foreColor = Utils.ColorFromString(ForeColor);

                    /*
                    if (backColor != Color.Empty && foreColor != Color.Empty)
                    {
                        chart.PlotArea.Fill.SetNoFill();

                        if (FillPattern.HasValue)
                            chart.Fill.SetPatternFill(foreColor, backColor, FillPattern.Value);
                        else if (GradientType.HasValue)
                        {
                            chart.Fill.SetGradientFill(GradientType.Value, backColor, foreColor);
                            chart.Fill.GradientFill.Angle = GradientAngle;
                        }
                        else
                            chart.Fill.SetSolidFill(backColor);
                    }
                    else 
                    */
                    if (backColor != Color.Empty)
                    {
                        chart.PlotArea.Fill.SetNoFill();

                        /*
                        if (FillPattern.HasValue)
                            chart.Fill.SetPatternFill(Color.Black, backColor, FillPattern.Value);
                        else 
                        */
                        chart.Fill.SetSolidFill(backColor);
                    }
                }
                /*
                else if (FillPattern.HasValue)
                {
                    chart.PlotArea.Fill.SetNoFill();
                    chart.Fill.SetPatternFill(Color.Beige, Color.White, FillPattern.Value);
                }
                */

                chart.Options.DisplayBlanksAs = (DevExpress.Spreadsheet.Charts.DisplayBlanksAs)(options.DisplayBlanksAs ?? DisplayBlanksAs.Zero);

                if (options.PrimaryAxes != null && options.PrimaryAxes.Length > 0)
                {
                    for (int i = 0; i < Math.Min(chart.PrimaryAxes.Count, options.PrimaryAxes.Length); i++)
                        SetupAxis(options.PrimaryAxes[i], chart.PrimaryAxes[i]);
                }

                if (options.SecondaryAxes != null && options.SecondaryAxes.Length > 0)
                {
                    for (int i = 0; i < Math.Min(chart.SecondaryAxes.Count, options.SecondaryAxes.Length); i++)
                        SetupAxis(options.SecondaryAxes[i], chart.SecondaryAxes[i]);
                }

                for (int i = 0; i < series.Length; i++)
                {
                    var cSeries = series[i];

                    if (cSeries == null)
                        throw new ArgumentException("Series cannot be null.");

                    if (string.IsNullOrWhiteSpace(cSeries.Values))
                        throw new ArgumentException("Series values are not specified.");

                    TableColumn columnArguments = null;
                    if (!string.IsNullOrWhiteSpace(cSeries.Arguments))
                        columnArguments = FindTableColumn(table, cSeries.Arguments);

                    var columnValues = FindTableColumn(table, cSeries.Values);

                    var rangeArguments = columnArguments?.DataRange;
                    var rangeValues = columnValues.DataRange;

                    if (cSeries.FromIndex.HasValue)
                    {
                        if (cSeries.FromIndex.Value >= table.Rows.Count)
                            cSeries.FromIndex = Math.Max(table.Rows.Count - 1, 0);
                        if (cSeries.FromIndex.Value < 0)
                            cSeries.FromIndex = Math.Max(table.Rows.Count - cSeries.FromIndex.Value, 0);

                        if (rangeArguments != null)
                        {
                            var topRowIndex = rangeArguments.TopRowIndex + cSeries.FromIndex.Value;
                            rangeArguments = dataWorksheet.Range.FromLTRB(rangeArguments.LeftColumnIndex, topRowIndex, rangeArguments.RightColumnIndex, rangeArguments.BottomRowIndex);
                        }
                        if (rangeValues != null)
                        {
                            var topRowIndex = rangeValues.TopRowIndex + cSeries.FromIndex.Value;
                            rangeValues = dataWorksheet.Range.FromLTRB(rangeValues.LeftColumnIndex, topRowIndex, rangeValues.RightColumnIndex, rangeValues.BottomRowIndex);
                        }
                    }
                    if (cSeries.ToIndex.HasValue)
                    {
                        if (cSeries.ToIndex.Value >= table.Rows.Count)
                            cSeries.ToIndex = Math.Max(table.Rows.Count - 1, 0);
                        if (cSeries.ToIndex.Value < 0)
                            cSeries.ToIndex = Math.Max(table.Rows.Count - cSeries.ToIndex.Value, 0);

                        if (rangeArguments != null)
                        {
                            var bottomRowIndex = Math.Min(rangeArguments.TopRowIndex + cSeries.ToIndex.Value, rangeArguments.BottomRowIndex);
                            rangeArguments = dataWorksheet.Range.FromLTRB(rangeArguments.LeftColumnIndex, rangeArguments.TopRowIndex, rangeArguments.RightColumnIndex, bottomRowIndex);
                        }
                        if (rangeValues != null)
                        {
                            var bottomRowIndex = Math.Min(rangeValues.TopRowIndex + cSeries.ToIndex.Value, rangeValues.BottomRowIndex);
                            rangeValues = dataWorksheet.Range.FromLTRB(rangeValues.LeftColumnIndex, rangeValues.TopRowIndex, rangeValues.RightColumnIndex, bottomRowIndex);
                        }
                    }

                    var chartSeries = chart.Series.Add(rangeArguments, rangeValues);
                    if (!string.IsNullOrWhiteSpace(cSeries.Name))
                        chartSeries.SeriesName.SetValue(cSeries.Name);

                    chartSeries.ChangeType((DevExpress.Spreadsheet.Charts.ChartType)cSeries.Type);

                    if (chartSeries.Marker != null)
                    {
                        chartSeries.Marker.Symbol = (DevExpress.Spreadsheet.Charts.MarkerStyle)cSeries.Markers;
                        chartSeries.Marker.Size   = Utils.ValueInRange(cSeries.MarkerSize, 2, 72);
                    }

                    chartSeries.Explosion = Utils.ValueInRange(cSeries.Explosion, 0, 100);
                    chartSeries.Shape     = (DevExpress.Spreadsheet.Charts.BarShape)cSeries.Shape;
                    chartSeries.Smooth    = cSeries.Smooth;

                    //Trendlines are not rendered in SpreadsheetControl. Code can be used to display trendlines in MS Excel.
                    /*
                    if (cSeries.Trendlines != null && cSeries.Trendlines.Length > 0)
                    {
                        foreach (var trendline in series.Trendlines)
                        {
                            if (trendline == null)
                                continue;

                            var chartTrendline = chartSeries.Trendlines.Add(trendline.Type);
                            if (trendline.Name != null)
                            {
                                chartTrendline.AutoName   = false;
                                chartTrendline.CustomName = trendline.Name;
                            }

                            chartTrendline.DisplayEquation = trendline.DisplayEquation;
                            chartTrendline.DisplayRSquare  = trendline.DisplayRSquare;
                            chartTrendline.Backward        = trendline.Backward;
                            chartTrendline.Forward         = trendline.Forward;
                            if (trendline.Intercept.HasValue)
                                chartTrendline.Intercept   = trendline.Intercept.Value;
                            if (trendline.Order.HasValue)
                                chartTrendline.Order       = trendline.Order.Value;
                            chartTrendline.Period          = Utils.ValueInRange(trendline.Period, 2, 255);
                            if (!string.IsNullOrWhiteSpace(trendline.Font))
                                Utils.StringToShapeTextFont(trendline.Font, chartTrendline.Label.Font);
                            if (!string.IsNullOrWhiteSpace(trendline.NumberFormat))
                                chartTrendline.Label.NumberFormat.FormatCode = trendline.NumberFormat;
                        }
                    }
                    */

                    //Do as last operation with chartSeries, after that object chartSeries cannot be used.
                    chartSeries.AxisGroup = (DevExpress.Spreadsheet.Charts.AxisGroup)cSeries.AxisGroup;
                }

                chart.Legend.Position = (DevExpress.Spreadsheet.Charts.LegendPosition)(options.LegendPosition ?? LegendPosition.Bottom);

                if (!string.IsNullOrWhiteSpace(options.Title))
                {
                    chart.Title.SetValue(options.Title);

                    if (!string.IsNullOrWhiteSpace(options.TitleFont))
                        Utils.StringToShapeTextFont(options.TitleFont, chart.Title.Font);

                    chart.Title.Visible = true;
                }

                if (options.VaryColors)
                {
                    foreach (var view in chart.Views)
                        view.VaryColors = true;
                }

                chart.Legend.Visible = !options.HideLegend;

                if (!string.IsNullOrWhiteSpace(options.LegendFont))
                    Utils.StringToShapeTextFont(options.LegendFont, chart.Legend.Font);

                if (options.DataLabelsShowCategoryName)
                {
                    foreach (var view in chart.Views)
                        view.DataLabels.ShowCategoryName = true;
                }
                if (options.DataLabelsShowPercent)
                {
                    foreach (var view in chart.Views)
                        view.DataLabels.ShowPercent = true;
                }
            }

            if (options.CopyToBook || options.TargetBook != null)
            {
                if (options.CopyToBookSize != null && options.CopyToBookSize.Length != 2)
                    throw new Exception("Invalid size of the image to copy to Book. Must be 2-elements integer array.");
                if (options.CopyToBookSize != null)
                {
                    var imageSize = new Size(options.CopyToBookSize[0], options.CopyToBookSize[1]);
                    CopyImageToBook(chart.GetImage(imageSize).NativeImage, options.CopyToBookScale, options.CopyToBookScale, options);
                }
                else
                    CopyImageToBook(chart.GetImage().NativeImage, options.CopyToBookScale, options.CopyToBookScale, options);
            }


            static void SetupAxis(SpreadChartAxis axis, Axis chartAxis)
            {
                if (axis.Title != null)
                    chartAxis.Title.SetValue(axis.Title);
                if (!string.IsNullOrWhiteSpace(axis.NumberFormat))
                    chartAxis.NumberFormat.FormatCode = axis.NumberFormat;
                if (!string.IsNullOrWhiteSpace(axis.Font))
                    Utils.StringToShapeTextFont(axis.Font, chartAxis.Font);
                chartAxis.MajorTickMarks = (DevExpress.Spreadsheet.Charts.AxisTickMarks)axis.MajorTickMarks;
                chartAxis.MinorTickMarks = (DevExpress.Spreadsheet.Charts.AxisTickMarks)axis.MinorTickMarks;
                if (axis.ShowMajorGridLines.HasValue)
                    chartAxis.MajorGridlines.Visible = axis.ShowMajorGridLines.Value;
                if (axis.ShowMinorGridLines.HasValue)
                    chartAxis.MinorGridlines.Visible = axis.ShowMinorGridLines.Value;
                if (axis.Position.HasValue)
                    chartAxis.Position = (DevExpress.Spreadsheet.Charts.AxisPosition)axis.Position.Value;
                chartAxis.BaseTimeUnit = (DevExpress.Spreadsheet.Charts.AxisTimeUnits)axis.BaseTimeUnit;
                if (axis.LabelAlignment.HasValue)
                    chartAxis.LabelAlignment = (DevExpress.Spreadsheet.Charts.AxisLabelAlignment)axis.LabelAlignment.Value;
                chartAxis.Scaling.LogBase = axis.LogScaleBase;
                chartAxis.Scaling.LogScale = axis.LogScale;
                if (axis.Minimum.HasValue)
                    chartAxis.Scaling.Min = axis.Minimum.Value;
                if (axis.Maximum.HasValue)
                    chartAxis.Scaling.Max = axis.Minimum.Value;
                chartAxis.Scaling.Orientation = axis.Reversed ? AxisOrientation.MaxMin : AxisOrientation.MinMax;
                chartAxis.TickLabelPosition = axis.HideTickLabels ? AxisTickLabelPosition.None : AxisTickLabelPosition.NextTo;
                chartAxis.Visible = axis.Visible;
            }

            (Worksheet, Table) FindDataTable()
            {
                Worksheet sheet = null;
                Table table     = null;

                if (!string.IsNullOrWhiteSpace(options.DataSheetName))
                {
                    sheet = spread.Worksheets[options.DataSheetName];
                    if (sheet == null)
                        throw new Exception($"Cannot find sheet '{options.DataSheetName}'.");
                }

                if (!string.IsNullOrWhiteSpace(dataTableName))
                {
                    if (sheet != null)
                        table = sheet.Tables.Where(t => string.Compare(t.Name, dataTableName, true) == 0).FirstOrDefault();
                    else
                    {
                        foreach (var worksheet in spread.Worksheets)
                        {
                            table = worksheet.Tables.Where(t => string.Compare(t.Name, dataTableName, true) == 0).FirstOrDefault();
                            if (table != null)
                                break;
                        }
                    }
                }

                if (table == null)
                    throw new Exception($"Cannot find table '{dataTableName}'.");

                return (sheet, table);
            }

            static TableColumn FindTableColumn(Table table, string columnName)
            {
                foreach (var column in table.Columns)
                {
                    if (string.Compare(column.Name, columnName, true) == 0)
                        return column;
                }

                throw new Exception($"Cannot find column '{columnName}' in table '{table.Name}'.");
            }
        }
    }
}
