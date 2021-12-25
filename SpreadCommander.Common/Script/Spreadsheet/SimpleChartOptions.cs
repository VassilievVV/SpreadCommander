using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using DevExpress.Spreadsheet.Drawings;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class SimpleChartOptions: SpreadsheetWithCopyToBookOptions
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

        //Series
        [Description("Name of chart series. If skipped - column names will be using as series names.")]
        public string[] SeriesNames { get; set; }

        [Description("Chart types for individual series, when chart contain series with different types. " +
            "ChartType has to be set to the type compatible with types of all series.")]
        public ChartType[] SeriesTypes { get; set; }

        [Description("Shape of markers which can be painted at each data point in the series on the line, scatter or radar chart and within the chart legend.")]
        public MarkerStyle[] SeriesMarkers { get; set; }

        [Description("Axis types for series. Default is primary axis. First axis group must be Primary.")]
        public AxisGroup[] AxisGroups { get; set; }

        //Axis
        [Description("Titles for axis. Count of titles depends on chart type.")]
        public string[] AxisTitles { get; set; }

        [Description("Titles for secondary axis. Count of titles depends on chart type.")]
        public string[] SecondaryAxisTitles { get; set; }

        [Description("Format of axis labels. Count of formats depends on chart type.")]
        public string[] AxisNumberFormats { get; set; }

        [Description("Axis font in form 'Tahoma, 8, Bold, Italic, Green'.")]
        public string AxisFont { get; set; }

        [Description("Position of major tick marks on the axis.")]
        public AxisTickMarks[] MajorTickMarks { get; set; }

        [Description("Position of major tick marks on the axis.")]
        public AxisTickMarks[] MinorTickMarks { get; set; }

        [Description("Whether to draw major gridlines on the chart.")]
        public bool ShowMajorGridLines { get; set; }

        [Description("Whether to draw minor gridlines on the chart.")]
        public bool ShowMinorGridLines { get; set; }

        [Description("Position of axis on the chart.")]
        public AxisPosition[] AxisPositions { get; set; }

        [Description("Position of secondary axis on the chart.")]
        public AxisPosition[] SecondaryAxisPositions { get; set; }

        [Description("Custom size of the image with chart copied to Book. Must be 2-elements array")]
        public int[] CopyToBookSize { get; set; }

        [Description("Scale of the image with chart copied to Book. Can be used to control image quality similar to setting DPI")]
        public float? CopyToBookScale { get; set; }
    }

    public partial class SCSpreadsheet
    {
        public SCSpreadsheet NewSimpleChart(string dataTableName, string[] arguments, string[] values, SimpleChartOptions options = null)
        {
            ExecuteSynchronized(options, () => DoNewSimpleChart(dataTableName, arguments, values, options));
            return this;
        }

        protected virtual void DoNewSimpleChart(string dataTableName, string[] arguments, string[] values, SimpleChartOptions options)
        {
            options ??= new SimpleChartOptions();
            var spread = options.Spreadsheet?.Workbook ?? Workbook;

            ChartObject chart;

            using (new UsingProcessor(() => spread.BeginUpdate(), () => spread.EndUpdate()))
            {
                var table = FindDataTable();

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
                                throw new Exception($"Cannot create chart sheet table: sheet '{options.ChartSheetName}' already exists.");

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

                if (values != null && values.Length > 0)
                {
                    TableColumn columnArguments = null;

                    for (int i = 0; i < values.Length; i++)
                    {
                        var value = values[i];

                        if (arguments != null && i < arguments.Length)
                            columnArguments = FindTableColumn(table, arguments[i]);
                        //If there are less Arguments than Values - propagate last Arguments to remaining Values.

                        var columnValues = FindTableColumn(table, value);
                        var seriesName   = options.SeriesNames != null && i < options.SeriesNames.Length ? options.SeriesNames[i] : value;

                        var series = chart.Series.Add(columnArguments?.DataRange, columnValues.DataRange);
                        if (!string.IsNullOrWhiteSpace(seriesName))
                            series.SeriesName.SetValue(seriesName);

                        if (options.SeriesTypes != null && i < options.SeriesTypes.Length)
                            series.ChangeType((DevExpress.Spreadsheet.Charts.ChartType)options.SeriesTypes[i]);

                        if (options.SeriesMarkers != null && i < options.SeriesMarkers.Length)
                            series.Marker.Symbol = (DevExpress.Spreadsheet.Charts.MarkerStyle)options.SeriesMarkers[i];

                        //Do as last operation with series, after that object series cannot be used.
                        if (options.AxisGroups != null && i < options.AxisGroups.Length)
                            series.AxisGroup = (DevExpress.Spreadsheet.Charts.AxisGroup)options.AxisGroups[i];
                    }
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

                if (options.HideLegend)
                    chart.Legend.Visible = false;

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

                if (options.AxisTitles != null && options.AxisTitles.Length > 0)
                {
                    for (int i = 0; i < Math.Min(options.AxisTitles.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].Title.SetValue(options.AxisTitles[i]);
                }
                if (options.SecondaryAxisTitles != null && options.SecondaryAxisTitles.Length > 0)
                {
                    for (int i = 0; i < Math.Min(options.SecondaryAxisTitles.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].Title.SetValue(options.SecondaryAxisTitles[i]);
                }

                if (options.AxisNumberFormats != null && options.AxisNumberFormats.Length > 0)
                {
                    for (int i = 0; i < Math.Min(options.AxisNumberFormats.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].NumberFormat.FormatCode = options.AxisNumberFormats[i];

                    for (int i = 0; i < Math.Min(options.AxisNumberFormats.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].NumberFormat.FormatCode = options.AxisNumberFormats[i];
                }

                if (!string.IsNullOrWhiteSpace(options.AxisFont))
                {
                    for (int i = 0; i < chart.PrimaryAxes.Count; i++)
                        Utils.StringToShapeTextFont(options.AxisFont, chart.PrimaryAxes[i].Font);

                    for (int i = 0; i < chart.SecondaryAxes.Count; i++)
                        Utils.StringToShapeTextFont(options.AxisFont, chart.SecondaryAxes[i].Font);
                }

                if (options.MajorTickMarks != null && options.MajorTickMarks.Length > 0)
                {
                    for (int i = 0; i < Math.Min(options.MajorTickMarks.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].MajorTickMarks = (DevExpress.Spreadsheet.Charts.AxisTickMarks)options.MajorTickMarks[i];

                    for (int i = 0; i < Math.Min(options.MajorTickMarks.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].MajorTickMarks = (DevExpress.Spreadsheet.Charts.AxisTickMarks)options.MajorTickMarks[i];
                }

                if (options.MinorTickMarks != null && options.MinorTickMarks.Length > 0)
                {
                    for (int i = 0; i < Math.Min(options.MinorTickMarks.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].MinorTickMarks = (DevExpress.Spreadsheet.Charts.AxisTickMarks)options.MinorTickMarks[i];

                    for (int i = 0; i < Math.Min(options.MinorTickMarks.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].MinorTickMarks = (DevExpress.Spreadsheet.Charts.AxisTickMarks)options.MinorTickMarks[i];
                }

                if (options.AxisPositions != null && options.AxisPositions.Length > 0)
                {
                    for (int i = 0; i < Math.Min(options.AxisPositions.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].Position = (DevExpress.Spreadsheet.Charts.AxisPosition)options.AxisPositions[i];
                }
                if (options.SecondaryAxisPositions != null && options.SecondaryAxisPositions.Length > 0)
                {
                    for (int i = 0; i < Math.Min(options.SecondaryAxisPositions.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].Position = (DevExpress.Spreadsheet.Charts.AxisPosition)options.SecondaryAxisPositions[i];
                }

                if (options.ShowMajorGridLines)
                {
                    for (int i = 0; i < chart.PrimaryAxes.Count; i++)
                        chart.PrimaryAxes[i].MajorGridlines.Visible = true;

                    for (int i = 0; i < chart.SecondaryAxes.Count; i++)
                        chart.SecondaryAxes[i].MajorGridlines.Visible = true;
                }

                if (options.ShowMinorGridLines)
                {
                    for (int i = 0; i < chart.PrimaryAxes.Count; i++)
                        chart.PrimaryAxes[i].MinorGridlines.Visible = true;

                    for (int i = 0; i < chart.SecondaryAxes.Count; i++)
                        chart.SecondaryAxes[i].MinorGridlines.Visible = true;
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


            Table FindDataTable()
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

                return table;
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
