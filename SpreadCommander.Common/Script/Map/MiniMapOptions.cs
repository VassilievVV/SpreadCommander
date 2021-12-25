using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class MiniMapOptions
    {
        [Description("Alignment of a mini map.")]
        public MiniMapAlignment Alignment { get; set; }

        [Description("Center point of a mini map.")]
        public double[] CenterPoint { get; set; }

        [Description("Zoom level of a map.")]
        [DefaultValue(1.0)]
        public double ZoomLevel { get; set; } = 1.0;

        [Description("Width of a mini map.")]
        public int? Width { get; set; }

        [Description("Height of a mini map.")]
        public int? Height { get; set; }

        [Description("Color that is used to fill the background of a mini map.")]
        public string BackColor { get; set; }

        [Description("Stroke color of the border of a mini map.")]
        public string StrokeColor { get; set; }


        protected internal virtual void UpdateMiniMap(SCMap map)
        {
            var miniMap = new MiniMap()
            {
                EnableScrolling     = false, 
                EnableZooming       = false, 
                SetMapCenterOnClick = false
            };

            if (Alignment > 0)
                miniMap.Alignment = (DevExpress.XtraMap.MiniMapAlignment)Alignment;

            if (Width.HasValue)
                miniMap.Width = Width.Value;
            if (Height.HasValue)
                miniMap.Height = Height.Value;

            if (ZoomLevel != 1.0 || CenterPoint != null)
            {
                var behavior = new FixedMiniMapBehavior();

                if (CenterPoint != null)
                {
                    if (CenterPoint.Length != 2)
                        throw new Exception("CenterPoint shall be a double array with 2 elements.");
                    behavior.CenterPoint = map.CreateCoordPoint(CenterPoint[0], CenterPoint[1]);
                }

                if (ZoomLevel != 1.0)
                    behavior.ZoomLevel = ZoomLevel;

                miniMap.Behavior = behavior;
            }

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                miniMap.ViewportStyle.Fill = backColor;
            var strokeColor = Utils.ColorFromString(StrokeColor);
            if (strokeColor != Color.Empty)
                miniMap.ViewportStyle.Stroke = strokeColor;

            map.Map.MiniMap = miniMap;
            map.Map.PrintOptions.PrintMiniMap = true;
        }
    }

    public partial class SCMap
    {
        public SCMap AddMiniMap(MiniMapOptions options = null)
        {
            options ??= new MiniMapOptions();
            options.UpdateMiniMap(this);

            return this;
        }
    }
}
