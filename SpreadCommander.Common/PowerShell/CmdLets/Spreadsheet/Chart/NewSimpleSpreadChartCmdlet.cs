using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using DevExpress.Spreadsheet.Drawings;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet.Chart
{
    [Cmdlet(VerbsCommon.New, "SimpleSpreadChart")]
    public class NewSimpleSpreadChartCmdlet : BaseSpreadsheetWithCopyToBookCmdlet
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
///
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

        //Series

        [Parameter(HelpMessage = "Data (column names) to plot as series arguments.")]
        public string[] Arguments { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Data (column names) to plot as series values. Multiple values will be using for individual series. " + 
            "Bubble chart requires pairs of adjoined columns. Stock chart requires set of adjoined columns.")]
        public string[] Values { get; set; }

        [Parameter(HelpMessage = "Name of chart series. If skipped - column names will be using as series names.")]
        public string[] SeriesNames { get; set; }

        [Parameter(HelpMessage = "Chart types for individual series, when chart contain series with different types. " +
            "ChartType has to be set to the type compatible with types of all series.")]
        public ChartType[] SeriesTypes { get; set; }

        [Parameter(HelpMessage = "Shape of markers which can be painted at each data point in the series on the line, scatter or radar chart and within the chart legend.")]
        public MarkerStyle[] SeriesMarkers { get; set; }

        [Parameter(HelpMessage = "Axis types for series. Default is primary axis. First axis group must be Primary.")]
        public AxisGroup[] AxisGroups { get; set; }

        //Axis
        [Parameter(HelpMessage = "Titles for axis. Count of titles depends on chart type.")]
        public string[] AxisTitles { get; set; }

        [Parameter(HelpMessage = "Titles for secondary axis. Count of titles depends on chart type.")]
        public string[] SecondaryAxisTitles { get; set; }

        [Parameter(HelpMessage = "Format of axis labels. Count of formats depends on chart type.")]
        public string[] AxisNumberFormats { get; set; }

        [Parameter(HelpMessage = "Axis font in form 'Tahoma, 8, Bold, Italic, Green'.")]
        public string AxisFont { get; set; }

        [Parameter(HelpMessage = "Position of major tick marks on the axis.")]
        public AxisTickMarks[] MajorTickMarks { get; set; }

        [Parameter(HelpMessage = "Position of major tick marks on the axis.")]
        public AxisTickMarks[] MinorTickMarks { get; set; }

        [Parameter(HelpMessage = "Whether to draw major gridlines on the chart.")]
        public SwitchParameter ShowMajorGridLines { get; set; }

        [Parameter(HelpMessage = "Whether to draw minor gridlines on the chart.")]
        public SwitchParameter ShowMinorGridLines { get; set; }

        [Parameter(HelpMessage = "Position of axis on the chart.")]
        public AxisPosition[] AxisPositions { get; set; }

        [Parameter(HelpMessage = "Position of secondary axis on the chart.")]
        public AxisPosition[] SecondaryAxisPositions { get; set; }

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
            WriteChartTable();
        }

        protected void WriteChartTable()
        {
            WriteTable(GetCmdletSpreadsheet());
        }

        protected void WriteTable(IWorkbook spreadsheet)
        {
            ExecuteSynchronized(() => DoWriteTable(spreadsheet));
        }

        protected virtual void DoWriteTable(IWorkbook spreadsheet)
        {
            ChartObject chart;

            using (new UsingProcessor(() => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
            {
                var table = FindDataTable();

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
                                throw new Exception($"Cannot create chart sheet table: sheet '{ChartSheetName}' already exists.");

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

                if (Values != null && Values.Length > 0)
                {
                    TableColumn columnArguments = null;

                    for (int i = 0; i < Values.Length; i++)
                    {
                        var values = Values[i];

                        if (Arguments != null && i < Arguments.Length)
                            columnArguments = FindTableColumn(table, Arguments[i]);
                        //If there are less Arguments than Values - propagate last Arguments to remaining Values.

                        var columnValues = FindTableColumn(table, values);
                        var seriesName   = SeriesNames != null && i < SeriesNames.Length ? SeriesNames[i] : values;

                        var series = chart.Series.Add(columnArguments?.DataRange, columnValues.DataRange);
                        if (!string.IsNullOrWhiteSpace(seriesName))
                            series.SeriesName.SetValue(seriesName);

                        if (SeriesTypes != null && i < SeriesTypes.Length)
                            series.ChangeType( SeriesTypes[i]);

                        if (SeriesMarkers != null && i < SeriesMarkers.Length)
                            series.Marker.Symbol = SeriesMarkers[i];

                        //Do as last operation with series, after that object series cannot be used.
                        if (AxisGroups != null && i < AxisGroups.Length)
                            series.AxisGroup = AxisGroups[i];
                    }
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

                if (HideLegend)
                    chart.Legend.Visible = false;

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

                if (AxisTitles != null && AxisTitles.Length > 0)
                {
                    for (int i = 0; i < Math.Min(AxisTitles.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].Title.SetValue(AxisTitles[i]);
                }
                if (SecondaryAxisTitles != null && SecondaryAxisTitles.Length > 0)
                {
                    for (int i = 0; i < Math.Min(SecondaryAxisTitles.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].Title.SetValue(SecondaryAxisTitles[i]);
                }

                if (AxisNumberFormats != null && AxisNumberFormats.Length > 0)
                {
                    for (int i = 0; i < Math.Min(AxisNumberFormats.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].NumberFormat.FormatCode = AxisNumberFormats[i];

                    for (int i = 0; i < Math.Min(AxisNumberFormats.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].NumberFormat.FormatCode = AxisNumberFormats[i];
                }

                if (!string.IsNullOrWhiteSpace(AxisFont))
                {
                    for (int i = 0; i < chart.PrimaryAxes.Count; i++)
                        Utils.StringToShapeTextFont(AxisFont, chart.PrimaryAxes[i].Font);

                    for (int i = 0; i < chart.SecondaryAxes.Count; i++)
                        Utils.StringToShapeTextFont(AxisFont, chart.SecondaryAxes[i].Font);
                }

                if (MajorTickMarks != null && MajorTickMarks.Length > 0)
                {
                    for (int i = 0; i < Math.Min(MajorTickMarks.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].MajorTickMarks = MajorTickMarks[i];

                    for (int i = 0; i < Math.Min(MajorTickMarks.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].MajorTickMarks = MajorTickMarks[i];
                }

                if (MinorTickMarks != null && MinorTickMarks.Length > 0)
                {
                    for (int i = 0; i < Math.Min(MinorTickMarks.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].MinorTickMarks = MinorTickMarks[i];

                    for (int i = 0; i < Math.Min(MinorTickMarks.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].MinorTickMarks = MinorTickMarks[i];
                }

                if (AxisPositions != null && AxisPositions.Length > 0)
                {
                    for (int i = 0; i < Math.Min(AxisPositions.Length, chart.PrimaryAxes.Count); i++)
                        chart.PrimaryAxes[i].Position = AxisPositions[i];
                }
                if (SecondaryAxisPositions != null && SecondaryAxisPositions.Length > 0)
                {
                    for (int i = 0; i < Math.Min(SecondaryAxisPositions.Length, chart.SecondaryAxes.Count); i++)
                        chart.SecondaryAxes[i].Position = SecondaryAxisPositions[i];
                }

                if (ShowMajorGridLines)
                {
                    for (int i = 0; i < chart.PrimaryAxes.Count; i++)
                        chart.PrimaryAxes[i].MajorGridlines.Visible = true;

                    for (int i = 0; i < chart.SecondaryAxes.Count; i++)
                        chart.SecondaryAxes[i].MajorGridlines.Visible = true;
                }

                if (ShowMinorGridLines)
                {
                    for (int i = 0; i < chart.PrimaryAxes.Count; i++)
                        chart.PrimaryAxes[i].MinorGridlines.Visible = true;

                    for (int i = 0; i < chart.SecondaryAxes.Count; i++)
                        chart.SecondaryAxes[i].MinorGridlines.Visible = true;
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


            Table FindDataTable()
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