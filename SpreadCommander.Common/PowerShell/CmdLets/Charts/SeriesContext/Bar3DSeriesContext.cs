using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class Bar3DSeriesContext: XY3DSeriesBaseContext
    {
        [Parameter(HelpMessage = "Depth of bars (the extent of the bar along the Z-axis) in 3D Bar series.")]
        public double? BarDepth { get; set; }

        [Parameter(HelpMessage = "Width of bars in 3D Bar series.")]
        public double? BarWidth { get; set; }

        [Parameter(HelpMessage = "Whether each data point of a series is shown in a different color.")]
        public SwitchParameter ColorEach { get; set; }

        [Parameter(HelpMessage = "Filling mode for an element's surface.")]
        public FillMode3D? FillMode { get; set; }

        [Parameter(HelpMessage = "Series's 2nd background color, if FillMode is gradient.")]
        public string Color2 { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "3D model used to draw series points of a Bar 3D series.")]
        public Bar3DModel? Model { get; set; }

        [Parameter(HelpMessage = "Whether the top facet should be visible for flat-top models of Bar 3D series.")]
        public SwitchParameter HideFacet { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is Bar3DSeriesView view)
            {
                if (BarDepth.HasValue)
                    view.BarDepth = BarDepth.Value;
                if (BarWidth.HasValue)
                    view.BarWidth = BarWidth.Value;
                view.ColorEach = ColorEach;

                if (FillMode.HasValue)
                {
                    view.FillStyle.FillMode = FillMode.Value;
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
                                    gradientOptions.GradientMode = FillGradientMode.Value;
                            }
                            break;
                    }
                }

                if (Model.HasValue)
                    view.Model = Model.Value;
                if (HideFacet)
                    view.ShowFacet = false;
            }
        }
    }
}
