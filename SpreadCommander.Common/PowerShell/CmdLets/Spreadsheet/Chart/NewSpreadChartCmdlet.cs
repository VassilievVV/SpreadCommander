using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using DevExpress.Spreadsheet.Drawings;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet.Chart
{
    [Cmdlet(VerbsCommon.New, "SpreadChart")]
    public class NewSpreadChartCmdlet : BaseSpreadsheetWithCopyToBookCmdlet
    {
        [Parameter(HelpMessage = "Name of sheet with data. If not specified - DataTableName is searching in all sheets.")]
        public string DataSheetName { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Name of table with data.")]
        public string DataTableName { get; set; }

        [Parameter(HelpMessage = "Name of new sheet with chart.")]
        public string ChartSheetName { get; set; }

        [Parameter(HelpMessage = "Replace existing sheet if it exists")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Chart type")]
        public ChartType? ChartType { get; set; }

        [Parameter(HelpMessage = "Style to be applied to the chart.")]
        public ChartStyle? Style { get; set; }

        [Parameter(HelpMessage = "Whether each data point in the series has a different color.")]
        public SwitchParameter VaryColors { get; set; }

        [Parameter(HelpMessage = "Specifies how empty cells should be plotted on a chart.")]
        public DisplayBlanksAs? DisplayBlanksAs { get; set; }

        //Fill
        //[Parameter(HelpMessage = "Fill pattern to fill chart.")]
        //public ShapeFillPatternType? FillPattern { get; set; }

        [Parameter(HelpMessage = "Background color.")]
        public string BackColor { get; set; }

        //[Parameter(HelpMessage = "Foreground color")]
        //public string ForeColor { get; set; }

        //[Parameter(HelpMessage = "Gradient types for the gradient fill.")]
        //public ShapeGradientType? GradientType { get; set; }

        //[Parameter(HelpMessage = "Angle by which the linear gradient fill should be rotated within the chart element.")]
        //public double? GradientAngle { get; set; }

        //Title
        [Parameter(HelpMessage = "Chart title")]
        public string Title { get; set; }

        [Parameter(HelpMessage = "Chart title's font in form 'Tahoma, 12, Bold, Italic, Red'.")]
        public string TitleFont { get; set; }

        //Legend
        [Parameter(HelpMessage = "Show or hide legend.")]
        public SwitchParameter HideLegend { get; set; }

        [Parameter(HelpMessage = "Specify the position of the legend.")]
        public LegendPosition? LegendPosition { get; set; }

        [Parameter(HelpMessage = "Display category name in data labels.")]
        public SwitchParameter DataLabelsShowCategoryName { get; set; }

        [Parameter(HelpMessage = "Display percentage value in data labels on pie or doughnut chart.")]
        public SwitchParameter DataLabelsShowPercent { get; set; }

        [Parameter(HelpMessage = "Legend's font in form 'Tahoma, 10, Bold, Italic, Blue'.")]
        public string LegendFont { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Series for the chart.")]
        public SpreadChartSeries[] Series { get; set; }

        //Axis
        [Parameter(HelpMessage = "Primary axis for chart. Count of axis depends on chart type.")]
        public SpreadChartAxis[] PrimaryAxes { get; set; }

        [Parameter(HelpMessage = "Secondary axis for chart. Count of axis depends on chart type.")]
        public SpreadChartAxis[] SecondaryAxes { get; set; }

        [Parameter(HelpMessage = "Custom size of the image with chart copied to Book. Must be 2-elements array")]
        public int[] CopyToBookSize { get; set; }

        [Parameter(HelpMessage = "Scale of the image with chart copied to Book. Can be used to control image quality similar to setting DPI")]
        public float? CopyToBookScale { get; set; }


        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
        }

        protected override void EndProcessing()
        {
            WritePivotTable();
        }

        protected void WritePivotTable()
        {
            WriteTable(GetCmdletSpreadsheet());
        }

        protected void WriteTable(IWorkbook spreadsheet)
        {
            ExecuteSynchronized(() => DoWriteTable(spreadsheet));
        }

        protected virtual void DoWriteTable(IWorkbook spreadsheet)
        {
            if (Series == null || Series.Length <= 0)
                throw new ArgumentException("At least one series must be provided.", nameof(Series));

            ChartObject chart;

            using (new UsingProcessor(() => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
            {
                (var dataWorksheet, var table) = FindDataTable();

                int chartSheetIndex = -1;

                if (!string.IsNullOrWhiteSpace(ChartSheetName))
                {
                    for (int i = 0; i < spreadsheet.ChartSheets.Count; i++)
                    {
                        var sheet = spreadsheet.ChartSheets[i];

                        if (string.Compare(sheet.Name, ChartSheetName, true) == 0)
                        {
                            if (Replace)
                            {
                                chartSheetIndex = sheet.Index;
                                spreadsheet.ChartSheets.Remove(sheet);
                            }
                            else
                                throw new Exception($"Cannot create pivot sheet table: sheet '{ChartSheetName}' already exists.");

                            break;
                        }
                    }
                }

                var chartSheet = spreadsheet.ChartSheets.Add(ChartSheetName, ChartType ?? DevExpress.Spreadsheet.Charts.ChartType.ColumnClustered);
                if (chartSheetIndex >= 0 && chartSheetIndex < spreadsheet.ChartSheets.Count)
                    chartSheet.Move(chartSheetIndex);

                chart = chartSheet.Chart;

                if (Style.HasValue)
                    chart.Style = (ChartStyle)(int)Style.Value;

                //Commented code can be used and will produce chart with fill visible in MS Excel but not rendered in SpreadsheetControl
                if (!string.IsNullOrWhiteSpace(BackColor))
                {
                    var backColor = Utils.ColorFromString(BackColor);
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

                chart.Options.DisplayBlanksAs = DisplayBlanksAs ?? DevExpress.Spreadsheet.Charts.DisplayBlanksAs.Zero;

                if (PrimaryAxes != null && PrimaryAxes.Length > 0)
                {
                    for (int i = 0; i < Math.Min(chart.PrimaryAxes.Count, PrimaryAxes.Length); i++)
                        SetupAxis(PrimaryAxes[i], chart.PrimaryAxes[i]);
                }

                if (SecondaryAxes != null && SecondaryAxes.Length > 0)
                {
                    for (int i = 0; i < Math.Min(chart.SecondaryAxes.Count, SecondaryAxes.Length); i++)
                        SetupAxis(SecondaryAxes[i], chart.SecondaryAxes[i]);
                }

                for (int i = 0; i < Series.Length; i++)
                {
                    var series = Series[i];

                    if (series == null)
                        throw new ArgumentException("Series cannot be null.");

                    if (string.IsNullOrWhiteSpace(series.Values))
                        throw new ArgumentException("Series values are not specified.");

                    TableColumn columnArguments = null;
                    if (!string.IsNullOrWhiteSpace(series.Arguments))
                        columnArguments = FindTableColumn(table, series.Arguments);

                    var columnValues = FindTableColumn(table, series.Values);

                    var rangeArguments = columnArguments?.DataRange;
                    var rangeValues    = columnValues.DataRange;

                    if (series.FromIndex.HasValue)
                    {
                        if (series.FromIndex.Value >= table.Rows.Count)
                            series.FromIndex = Math.Max(table.Rows.Count - 1, 0);
                        if (series.FromIndex.Value < 0)
                            series.FromIndex = Math.Max(table.Rows.Count - series.FromIndex.Value, 0);

                        if (rangeArguments != null)
                        {
                            var topRowIndex = rangeArguments.TopRowIndex + series.FromIndex.Value;
                            rangeArguments = dataWorksheet.Range.FromLTRB(rangeArguments.LeftColumnIndex, topRowIndex, rangeArguments.RightColumnIndex, rangeArguments.BottomRowIndex);
                        }
                        if (rangeValues != null)
                        {
                            var topRowIndex = rangeValues.TopRowIndex + series.FromIndex.Value;
                            rangeValues = dataWorksheet.Range.FromLTRB(rangeValues.LeftColumnIndex, topRowIndex, rangeValues.RightColumnIndex, rangeValues.BottomRowIndex);
                        }
                    }
                    if (series.ToIndex.HasValue)
                    {
                        if (series.ToIndex.Value >= table.Rows.Count)
                            series.ToIndex = Math.Max(table.Rows.Count - 1, 0);
                        if (series.ToIndex.Value < 0)
                            series.ToIndex = Math.Max(table.Rows.Count - series.ToIndex.Value, 0);

                        if (rangeArguments != null)
                        {
                            var bottomRowIndex = Math.Min(rangeArguments.TopRowIndex + series.ToIndex.Value, rangeArguments.BottomRowIndex);
                            rangeArguments = dataWorksheet.Range.FromLTRB(rangeArguments.LeftColumnIndex, rangeArguments.TopRowIndex, rangeArguments.RightColumnIndex, bottomRowIndex);
                        }
                        if (rangeValues != null)
                        {
                            var bottomRowIndex = Math.Min(rangeValues.TopRowIndex + series.ToIndex.Value, rangeValues.BottomRowIndex);
                            rangeValues = dataWorksheet.Range.FromLTRB(rangeValues.LeftColumnIndex, rangeValues.TopRowIndex, rangeValues.RightColumnIndex, bottomRowIndex);
                        }
                    }

                    var chartSeries = chart.Series.Add(rangeArguments, rangeValues);
                    if (!string.IsNullOrWhiteSpace(series.Name))
                        chartSeries.SeriesName.SetValue(series.Name);

                    chartSeries.ChangeType(series.Type);

                    chartSeries.AxisGroup = series.AxisGroup;

                    if (chartSeries.Marker != null)
                    {
                        chartSeries.Marker.Symbol = series.Markers;
                        chartSeries.Marker.Size   = Utils.ValueInRange(series.MarkerSize, 2, 72);
                    }

                    chartSeries.Explosion   = Utils.ValueInRange(series.Explosion, 0, 100);
                    chartSeries.Shape       = series.Shape;
                    chartSeries.Smooth      = series.Smooth;

                    //Trendlines are not rendered in SpreadsheetControl. Code can be used to display trendlines in MS Excel.
                    /*
                    if (series.Trendlines != null && series.Trendlines.Length > 0)
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
                }

                chart.Legend.Position = LegendPosition ?? DevExpress.Spreadsheet.Charts.LegendPosition.Bottom;

                if (!string.IsNullOrWhiteSpace(Title))
                {
                    chart.Title.SetValue(Title);

                    if (!string.IsNullOrWhiteSpace(TitleFont))
                        Utils.StringToShapeTextFont(TitleFont, chart.Title.Font);

                    chart.Title.Visible = true;
                }

                if (VaryColors)
                {
                    foreach (var view in chart.Views)
                        view.VaryColors = true;
                }

                chart.Legend.Visible = !HideLegend;

                if (!string.IsNullOrWhiteSpace(LegendFont))
                    Utils.StringToShapeTextFont(LegendFont, chart.Legend.Font);

                if (DataLabelsShowCategoryName)
                {
                    foreach (var view in chart.Views)
                        view.DataLabels.ShowCategoryName = true;
                }
                if (DataLabelsShowPercent)
                {
                    foreach (var view in chart.Views)
                        view.DataLabels.ShowPercent = true;
                }
            }

            if (CopyToBook || TargetBook != null)
            {
                if (CopyToBookSize != null && CopyToBookSize.Length != 2)
                    throw new Exception("Invalid size of the image to copy to Book. Must be 2-elements integer array.");
                if (CopyToBookSize != null)
                {
                    var imageSize = new Size(CopyToBookSize[0], CopyToBookSize[1]);
                    CopyImageToBook(chart.GetImage(imageSize).NativeImage, CopyToBookScale, CopyToBookScale);
                }
                else
                    CopyImageToBook(chart.GetImage().NativeImage, CopyToBookScale, CopyToBookScale);
            }


            static void SetupAxis(SpreadChartAxis axis, Axis chartAxis)
            {
                if (axis.Title != null)
                    chartAxis.Title.SetValue(axis.Title);
                if (!string.IsNullOrWhiteSpace(axis.NumberFormat))
                    chartAxis.NumberFormat.FormatCode = axis.NumberFormat;
                if (!string.IsNullOrWhiteSpace(axis.Font))
                    Utils.StringToShapeTextFont(axis.Font, chartAxis.Font);
                chartAxis.MajorTickMarks         = axis.MajorTickMarks;
                chartAxis.MinorTickMarks         = axis.MinorTickMarks;
                if (axis.ShowMajorGridLines.HasValue)
                    chartAxis.MajorGridlines.Visible = axis.ShowMajorGridLines.Value;
                if (axis.ShowMinorGridLines.HasValue)
                    chartAxis.MinorGridlines.Visible = axis.ShowMinorGridLines.Value;
                if (axis.Position.HasValue)
                    chartAxis.Position = axis.Position.Value;
                chartAxis.BaseTimeUnit           = axis.BaseTimeUnit;
                if (axis.LabelAlignment.HasValue)
                    chartAxis.LabelAlignment = axis.LabelAlignment.Value;
                chartAxis.Scaling.LogBase        = axis.LogScaleBase;
                chartAxis.Scaling.LogScale       = axis.LogScale;
                if (axis.Minimum.HasValue)
                    chartAxis.Scaling.Min        = axis.Minimum.Value;
                if (axis.Maximum.HasValue)
                    chartAxis.Scaling.Max        = axis.Minimum.Value;
                chartAxis.Scaling.Orientation    = axis.Reversed ? AxisOrientation.MaxMin : AxisOrientation.MinMax;
                chartAxis.TickLabelPosition      = axis.HideTickLabels ? AxisTickLabelPosition.None : AxisTickLabelPosition.NextTo;
                chartAxis.Visible                = axis.Visible;
            }

            (Worksheet, Table) FindDataTable()
            {
                Worksheet sheet = null;
                Table table     = null;

                if (!string.IsNullOrWhiteSpace(DataSheetName))
                {
                    sheet = spreadsheet.Worksheets[DataSheetName];
                    if (sheet == null)
                        throw new Exception($"Cannot find sheet '{DataSheetName}'.");
                }

                if (!string.IsNullOrWhiteSpace(DataTableName))
                {
                    if (sheet != null)
                        table = sheet.Tables.Where(t => string.Compare(t.Name, DataTableName, true) == 0).FirstOrDefault();
                    else
                    {
                        foreach (var worksheet in spreadsheet.Worksheets)
                        {
                            table = worksheet.Tables.Where(t => string.Compare(t.Name, DataTableName, true) == 0).FirstOrDefault();
                            if (table != null)
                                break;
                        }
                    }
                }

                if (table == null)
                    throw new Exception($"Cannot find table '{DataTableName}'.");

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