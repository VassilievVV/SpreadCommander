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
    public class XY2DSeriesBaseExSeriesOptions: XY2DSeriesBaseSeriesOptions
    {
        [Description("Series shadow's color.")]
        public string ShadowColor { get; set; }

        [Description("Series shadow's thickness.")]
        public int? ShadowSize { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is XYDiagramSeriesViewBase view)
            {
                var shadowColor = Utils.ColorFromString(ShadowColor);
                if (shadowColor != System.Drawing.Color.Empty)
                {
                    view.Shadow.Color   = shadowColor;
                    view.Shadow.Visible = true;
                }
                if (ShadowSize.HasValue)
                {
                    view.Shadow.Size    = ShadowSize.Value;
                    view.Shadow.Visible = true;
                }
            }
        }
    }
}
