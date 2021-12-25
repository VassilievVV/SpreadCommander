using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Diagrams
{
    public class XYDiagramOptions: DiagramOptions
    {
        [Description("Name of diagram's pane at which series will be drawn by default.")]
        public string DefaultPane { get; set; }

        [Description("Indent between the diagram's edge and other chart elements (e.g. legend and chart titles), in pixels.")]
        public int[] Margins { get; set; }

        [Description("Distance between a diagram's panes.")]
        public int? PaneDistance { get; set; }

        [Description("Whether the diagram is rotated.")]
        public bool Rotated { get; set; }

        [Description("Whether to use grid layout for panes.")]
        public bool GridLayout { get; set; }

        [Description("Whether to layout panes horizontally rather than vertically.")]
        public bool HorizontalLayout { get; set; }

        [Description("Grid pane layout's columns.")]
        public string ColumnDefinitions { get; set; }

        [Description("Grid pane layout's rows.")]
        public string RowDefinitions { get; set; }


        internal override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (diagram is not XYDiagram2D diagramXY)
                return;

            if (Margins != null && Margins.Length == 1)
                diagramXY.Margins.All = Margins[0];
            else if (Margins != null && Margins.Length == 4)
            {
                diagramXY.Margins.Left   = Margins[0];
                diagramXY.Margins.Top    = Margins[1];
                diagramXY.Margins.Right  = Margins[2];
                diagramXY.Margins.Bottom = Margins[3];
            }

            if (PaneDistance.HasValue)
                diagramXY.PaneDistance = PaneDistance.Value;

            if (Rotated && (diagramXY is XYDiagram xyDiagram) && (diagramXY is not GanttDiagram))
                xyDiagram.Rotated = true;

            if (GridLayout)
                diagramXY.PaneLayout.AutoLayoutMode = PaneAutoLayoutMode.Grid;

            if (HorizontalLayout)
                diagramXY.PaneLayout.Direction = PaneLayoutDirection.Horizontal;

            SetLayoutDefinitions(ColumnDefinitions, diagramXY.PaneLayout.ColumnDefinitions);
            SetLayoutDefinitions(RowDefinitions, diagramXY.PaneLayout.RowDefinitions);


            static void SetLayoutDefinitions(string value, LayoutDefinitionCollection collection)
            {
                collection.Clear();

                if (string.IsNullOrWhiteSpace(value))
                    return;

                var parts = Utils.SplitString(value, ',');
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i]?.Trim();
                    if (string.IsNullOrWhiteSpace(part))
                        part = "1";

                    bool isPixels = part.EndsWith("px", StringComparison.InvariantCultureIgnoreCase);
                    if (isPixels)
                        part = part[^2..].Trim();

                    if (!double.TryParse(part, out double dPart))
                        throw new Exception($"Cannot parse layout definition part: {parts[i]}");

                    var layoutDefinition = new LayoutDefinition()
                    {
                        SizeMode     = isPixels ? PaneSizeMode.UseSizeInPixels : PaneSizeMode.UseWeight,
                        SizeInPixels = isPixels ? Convert.ToInt32(dPart) : 0,
                        Weight       = isPixels ? 0.0 : dPart
                    };
                    collection.Add(layoutDefinition);
                }
            }
        }
    }
}
