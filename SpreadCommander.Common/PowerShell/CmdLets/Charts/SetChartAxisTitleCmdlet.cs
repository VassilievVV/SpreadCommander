using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	[Cmdlet(VerbsCommon.Set, "ChartAxisTitle")]
	public class SetChartAxisTitleCmdlet: BaseChartWithContextCmdlet
	{
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Axis type - X or Y.")]
		public ChartAxisType AxisType { get; set; }

		[Parameter(Position = 1, HelpMessage = "Name of the axis.")]
		public string AxisName { get; set; }

		[Parameter(HelpMessage = "Alignment of the axis title.")]
		public StringAlignment? Alignment { get; set; }

		[Parameter(HelpMessage = "Font used to display the title's text.")]
		public string Font { get; set; }

		[Parameter(HelpMessage = "Number of lines to which a title text is allowed to wrap.")]
		public int? MaxLineCount { get; set; }

		[Parameter(HelpMessage = "Title's text. HTML formatting is supported.")]
		public string Text { get; set; }

		[Parameter(HelpMessage = "Value which specifies whether to show an axis title on a diagram.")]
		public bool? Visible { get; set; }

		[Parameter(HelpMessage = "Whether a title's text should wrap when it's too lengthy.")]
		public SwitchParameter WordWrap { get; set; }


		protected override void UpdateChart()
		{
			AxisBase axis;

			if (!string.IsNullOrWhiteSpace(AxisName))
			{
				axis = BaseAxisCmdlet.GetSecondaryAxis(ChartContext.Chart.Diagram, AxisType, AxisName);
				if (axis == null)
					throw new Exception($"Cannot find axis '{AxisName}'.");
			}
			else
			{
				axis = BaseAxisCmdlet.GetPrimaryAxis(ChartContext.Chart.Diagram, AxisType);
				if (axis == null)
					throw new Exception("Cannot find primary axis.");
			}

			if (axis is not Axis2D axis2D)
				throw new Exception("Only 2D axis support title.");

			var title = axis2D.Title;

			title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

			if (Alignment.HasValue)
				title.Alignment = Alignment.Value;

			var font = Utils.StringToFont(Font, out Color textColor);
			if (font != null)
				title.Font = font;
			if (textColor != Color.Empty)
				title.TextColor = textColor;

			if (MaxLineCount.HasValue)
				title.MaxLineCount = MaxLineCount.Value;

			if (!string.IsNullOrWhiteSpace(Text))
			{
				title.Text = Text;
				title.Visibility = DevExpress.Utils.DefaultBoolean.True;
			}

			if (Visible.HasValue)
				title.Visibility = Visible.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

			title.WordWrap = WordWrap;
		}
	}
}
