using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapMiniMap")]
	public class AddMapMiniMapCmdlet: BaseMapWithContextCmdlet
	{
		[Parameter(HelpMessage = "Alignment of a mini map.")]
		public MiniMapAlignment Alignment { get; set; }

		[Parameter(HelpMessage = "Center point of a mini map.")]
		public double[] CenterPoint { get; set; }

		[Parameter(HelpMessage = "Zoom level of a map.")]
		[ValidateRange(0.1, 100)]
		[PSDefaultValue(Value = 1.0)]
		[DefaultValue(1.0)]
		public double ZoomLevel { get; set; } = 1.0;

		[Parameter(HelpMessage = "Width of a mini map.")]
		public int? Width { get; set; }

		[Parameter(HelpMessage = "Height of a mini map.")]
		public int? Height { get; set; }

		[Parameter(HelpMessage = "Color that is used to fill the background of a mini map.")]
		public string BackColor { get; set; }

		[Parameter(HelpMessage = "Stroke color of the border of a mini map.")]
		public string StrokeColor { get; set; }


		protected override void UpdateMap()
		{
			var miniMap = new MiniMap()
			{
				EnableScrolling     = false, 
				EnableZooming       = false, 
				SetMapCenterOnClick = false
			};

			if (Alignment > 0)
				miniMap.Alignment = Alignment;

			if (Width.HasValue)
				miniMap.Width = Width.Value;
			if (Height.HasValue)
				miniMap.Height = Height.Value;

			if (ZoomLevel != 1.0 || CenterPoint != null)
			{
				if (CenterPoint.Length != 2)
					throw new Exception("CenterPoint shall be a double array with 2 elements.");

				var behavior = new FixedMiniMapBehavior()
				{
					CenterPoint = MapContext.CreateCoordPoint(this.CenterPoint[0], this.CenterPoint[1]),
					ZoomLevel   = this.ZoomLevel
				};

				miniMap.Behavior = behavior;
			}

			var backColor = Utils.ColorFromString(BackColor);
			if (backColor != Color.Empty)
				miniMap.ViewportStyle.Fill = backColor;
			var strokeColor = Utils.ColorFromString(StrokeColor);
			if (strokeColor != Color.Empty)
				miniMap.ViewportStyle.Stroke = strokeColor;

			MapContext.Map.MiniMap = miniMap;
			MapContext.Map.PrintOptions.PrintMiniMap = true;
		}
	}
}
