using DevExpress.Utils;
using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class PaneOptions
    {
        [Description("Pane's background color.")]
        public string BackColor { get; set; }

        [Description("Pane's border color.")]
        public string BorderColor { get; set; }

        [Description("Whether the pane's border is visible.")]
        public bool? BorderVisible { get; set; }

        [Description("Pane's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Pane's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Index of the grid layout column the current pane occupies.")]
        public int? Column { get; set; }

        [Description("Number of grid layout columns that the pane occupies.")]
        public int? ColumnSpan { get; set; }

        [Description("Index of the grid layout row the current pane occupies.")]
        public int? Row { get; set; }

        [Description("Number of grid layout rows that the pane occupies.")]
        public int? RowSpan { get; set; }

        [Description("Shadow's color.")]
        public string ShadowColor { get; set; }

        [Description("Shadow's thickness, in pixels.")]
        public int? ShadowSize { get; set; }

        [Description("Title's text, with HTML formatting.")]
        public string Title { get; set; }

        [Description("Value that defines how to align the pane title.")]
        public StringAlignment? TitleAlignment { get; set; }

        [Description("Font used to display the title's text.")]
        public string TitleFont { get; set; }

        [Description("Indent between the pane and its title, in pixels.")]
        public int[] TitleMargins { get; set; }


        protected internal virtual void SetupXtraChartsPane(XYDiagramPaneBase pane)
        {
            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                pane.BackColor = backColor;
            var borderColor = Utils.ColorFromString(BorderColor);
            if (borderColor != Color.Empty)
            {
                pane.BorderColor   = borderColor;
                pane.BorderVisible = true;
            }
            if (BorderVisible.HasValue)
                pane.BorderVisible = true;

            if (FillMode.HasValue)
            {
                pane.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)FillMode.Value;
                switch (FillMode.Value)
                {
                    case Chart.FillMode.Empty:
                        break;
                    case Chart.FillMode.Solid:
                        break;
                    case Chart.FillMode.Gradient:
                        if (pane.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                        }
                        break;
                    case Chart.FillMode.Hatch:
                        if (pane.FillStyle.Options is HatchFillOptions hatchOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                hatchOptions.Color2 = backColor2;
                            if (FillHatchStyle.HasValue)
                                hatchOptions.HatchStyle = FillHatchStyle.Value;
                        }
                        break;
                }
            }

            if (Column.HasValue)
                pane.LayoutOptions.Column = Column.Value;
            if (ColumnSpan.HasValue)
                pane.LayoutOptions.ColumnSpan = ColumnSpan.Value;
            if (Row.HasValue)
                pane.LayoutOptions.Row = Row.Value;
            if (RowSpan.HasValue)
                pane.LayoutOptions.RowSpan = RowSpan.Value;

            var shadowColor = Utils.ColorFromString(ShadowColor);
            if (shadowColor != Color.Empty)
            {
                pane.Shadow.Color   = shadowColor;
                pane.Shadow.Visible = true;
            }
            if (ShadowSize.HasValue)
            {
                pane.Shadow.Size    = ShadowSize.Value;
                pane.Shadow.Visible = true;
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                pane.Title.Text       = Title;
                pane.Title.Visibility = DefaultBoolean.True;
            }
            if (TitleAlignment.HasValue)
                pane.Title.Alignment = TitleAlignment.Value;
            var font = Utils.StringToFont(TitleFont, out Color titleColor);
            if (font != null)
            {
                pane.Title.Font = font;
                if (titleColor != Color.Empty)
                    pane.Title.TextColor = titleColor;
            }

            if (TitleMargins != null && TitleMargins.Length == 1)
                pane.Title.Margins.All = TitleMargins[0];
            else if (TitleMargins != null && TitleMargins.Length == 4)
            {
                pane.Title.Margins.Left   = TitleMargins[0];
                pane.Title.Margins.Top    = TitleMargins[1];
                pane.Title.Margins.Right  = TitleMargins[2];
                pane.Title.Margins.Bottom = TitleMargins[3];
            }
        }
    }

    public partial class SCChart
    {
        public SCChart AddPane(string name, PaneOptions options = null)
        {
            if (Chart.Diagram is not XYDiagram diagram)
                throw new Exception("Cannot determine chart's pane to apply settings. Ensure that chart is 2D XY chart.");

            options ??= new PaneOptions();

            var pane = new XYDiagramPane(name);
            diagram.Panes.Add(pane);

            options.SetupXtraChartsPane(pane);

            return this;
        }

        public SCChart SetDefaultPane(PaneOptions options)
        {
            if (Chart.Diagram is not XYDiagram diagram)
                throw new Exception("Cannot determine chart's pane to apply settings. Ensure that chart is 2D XY chart.");

            options ??= new PaneOptions();

            var pane = diagram.DefaultPane;

            options.SetupXtraChartsPane(pane);

            return this;
        }
    }
}
