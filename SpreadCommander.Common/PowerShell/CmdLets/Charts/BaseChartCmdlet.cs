using DevExpress.Office.Utils;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.CmdLets.Book;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    public class BaseChartCmdlet: BaseBookCmdlet
    {
        protected virtual Bitmap PaintChart(Chart chart, int width = 2000, int height = 1200, int? dpi = null)
        {
            if (dpi == null || dpi == 0)
                dpi = ExternalHost?.DefaultDPI ?? DefaultDPI;

            float scaleBitmap = dpi.Value / 300f;
            float scaleSVG    = scaleBitmap * 96f / 300f;

            var svg = chart.CreateSvg(new Size((int)Math.Ceiling(width * scaleSVG), (int)Math.Ceiling(height * scaleSVG)));

            var doc = SvgDocument.Open(svg);
            doc.ShapeRendering = SvgShapeRendering.GeometricPrecision;

            var bitmap = new Bitmap((int)Math.Ceiling(width * scaleBitmap), (int)Math.Ceiling(height * scaleBitmap));
            bitmap.SetResolution(dpi.Value, dpi.Value);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode   = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode     = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                graphics.Clear(Color.White);

                var renderer = SvgRenderer.FromGraphics(graphics);
                renderer.SmoothingMode = SmoothingMode.HighQuality;
                if (scaleSVG != 1f && scaleSVG > 0f)
                    renderer.ScaleTransform(1 / scaleSVG, 1 / scaleSVG);

                doc.Draw(renderer);
            }

            return bitmap;
        }

        protected void FlushChart(Chart chart)
        {
            FlushChartImage(GetCmdletBook(), chart);
        }

        protected void FlushChartImage(Document book, Chart chart)
        {
            if (chart == null)
                throw new Exception("Chart is not provided. Please use one of New-Chart cmdlets to create a chart.");

            var chartBitmap = PaintChart(chart);

            ExecuteSynchronized(() => DoWriteImage(book, chartBitmap));
        }

        protected virtual void DoWriteImage(Document book, Image chartBitmap)
        {
            if (chartBitmap == null)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { ResetBookFormatting(book); book.EndUpdate(); }))
            {
                book.Images.Append(chartBitmap);
                var paragraph = book.Paragraphs.Append();

                book.CaretPosition = paragraph.Range.End;
                ScrollToCaret();
            }
        }

        protected static int GetValuePointCount(ViewType viewType)
        {
            return viewType switch
            {
                ViewType.Bar                        => 1,
                ViewType.StackedBar                 => 1,
                ViewType.FullStackedBar             => 1,
                ViewType.SideBySideStackedBar       => 1,
                ViewType.SideBySideFullStackedBar   => 1,
                ViewType.Pie                        => 1,
                ViewType.Doughnut                   => 1,
                ViewType.NestedDoughnut             => 1,
                ViewType.Funnel                     => 1,
                ViewType.Point                      => 1,
                ViewType.Bubble                     => 2,	//Value and Weight
                ViewType.Line                       => 1,
                ViewType.StackedLine                => 1,
                ViewType.FullStackedLine            => 1,
                ViewType.StepLine                   => 1,
                ViewType.Spline                     => 1,
                ViewType.ScatterLine                => 1,
                ViewType.SwiftPlot                  => 1,
                ViewType.Area                       => 1,
                ViewType.StepArea                   => 1,
                ViewType.SplineArea                 => 1,
                ViewType.StackedArea                => 1,
                ViewType.StackedStepArea            => 1,
                ViewType.StackedSplineArea          => 1,
                ViewType.FullStackedArea            => 1,
                ViewType.FullStackedSplineArea      => 1,
                ViewType.FullStackedStepArea        => 1,
                ViewType.RangeArea                  => 2,
                ViewType.Stock                      => 4,   //Low, High, Open, Close
                ViewType.CandleStick                => 4,   //Low, High, Open, Close
                ViewType.SideBySideRangeBar         => 2,
                ViewType.RangeBar                   => 2,
                ViewType.SideBySideGantt            => 2,   //Date-time values (Start and End)
                ViewType.Gantt                      => 2,   //Date-time values (Start and End)
                ViewType.PolarPoint                 => 1,
                ViewType.PolarLine                  => 1,
                ViewType.ScatterPolarLine           => 1,
                ViewType.PolarArea                  => 1,
                ViewType.PolarRangeArea             => 2,
                ViewType.RadarPoint                 => 1,
                ViewType.RadarLine                  => 1,
                ViewType.ScatterRadarLine           => 1,
                ViewType.RadarArea                  => 1,
                ViewType.RadarRangeArea             => 2,
                ViewType.Bar3D                      => 1,
                ViewType.StackedBar3D               => 1,
                ViewType.FullStackedBar3D           => 1,
                ViewType.ManhattanBar               => 1,
                ViewType.SideBySideStackedBar3D     => 1,
                ViewType.SideBySideFullStackedBar3D => 1,
                ViewType.Pie3D                      => 1,
                ViewType.Doughnut3D                 => 1,
                ViewType.Funnel3D                   => 1,
                ViewType.Line3D                     => 1,
                ViewType.StackedLine3D              => 1,
                ViewType.FullStackedLine3D          => 1,
                ViewType.StepLine3D                 => 1,
                ViewType.Area3D                     => 1,
                ViewType.StackedArea3D              => 1,
                ViewType.FullStackedArea3D          => 1,
                ViewType.StepArea3D                 => 1,
                ViewType.Spline3D                   => 1,
                ViewType.SplineArea3D               => 1,
                ViewType.StackedSplineArea3D        => 1,
                ViewType.FullStackedSplineArea3D    => 1,
                ViewType.RangeArea3D                => 2,
                _                                   => 1,
            };
        }
    }
}
