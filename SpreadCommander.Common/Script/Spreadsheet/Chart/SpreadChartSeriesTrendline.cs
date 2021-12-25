using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet.Chart
{
    public class SpreadChartSeriesTrendline
    {
        [Description("Type of the trendline.")]
        public ChartTrendlineType Type { get; set; }

        [Description("Name of the trendline.")]
        public string Name { get; set; }

        [Description("Whether the trendline equation is displayed on a chart.")]
        public bool DisplayEquation { get; set; }

        [Description("Whether to display the R-squared value of the trendline on the chart.")]
        public bool DisplayRSquare { get; set; }

        [Description("Number of categories (or units on a scatter chart) that the trendline extends before the data.")]
        public double Backward { get; set; }

        [Description("Number of categories (or units on a scatter chart) that the trendline extends after the data.")]
        public double Forward { get; set; }

        [Description("Point where the trendline crosses the value axis.")]
        public double? Intercept { get; set; }

        [Description("Value that specifies the trendline order for polynomial trendline.")]
        public int? Order { get; set; }

        [Description("Period for the moving-average trendline. Values from 2 to 255 are allowed.")]
        public int Period { get; set; } = 2;

        [Description("Font for text displayed in a labels in form 'Tahoma, 8, Bold, Italic, Red'.")]
        public string Font { get; set; }

        [Description("Format string used to display numeric data.")]
        public string NumberFormat { get; set; }
    }
}
