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
    public class Bar3DSeriesOptions: XY3DSeriesBaseOptions
    {
        [Description("Depth of bars (the extent of the bar along the Z-axis) in 3D Bar series.")]
        public double? BarDepth { get; set; }

        [Description("Width of bars in 3D Bar series.")]
        public double? BarWidth { get; set; }

        [Description("Whether each data point of a series is shown in a different color.")]
        public bool ColorEach { get; set; }

        [Description("Filling mode for an element's surface.")]
        public FillMode3D? FillMode { get; set; }

        [Description("Series's 2nd background color, if FillMode is gradient.")]
        public string Color2 { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("3D model used to draw series points of a Bar 3D series.")]
        public Bar3DModel? Model { get; set; }

        [Description("Whether the top facet should be visible for flat-top models of Bar 3D series.")]
        public bool HideFacet { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is Bar3DSeriesView view)
            {
                if (BarDepth.HasValue)
                    view.BarDepth = BarDepth.Value;
                if (BarWidth.HasValue)
                    view.BarWidth = BarWidth.Value;
                view.ColorEach = ColorEach;

                if (FillMode.HasValue)
                {
                    view.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode3D)FillMode.Value;
                    switch (FillMode.Value)
                    {
                        case FillMode3D.Empty:
                            break;
                        case FillMode3D.Solid:
                            break;
                        case FillMode3D.Gradient:
                            if (view.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var color2 = Utils.ColorFromString(Color2);
                                if (color2 != System.Drawing.Color.Empty)
                                    gradientOptions.Color2 = color2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                            }
                            break;
                    }
                }

                if (Model.HasValue)
                    view.Model = (DevExpress.XtraCharts.Bar3DModel)Model.Value;
                if (HideFacet)
                    view.ShowFacet = false;
            }
        }
    }
}
