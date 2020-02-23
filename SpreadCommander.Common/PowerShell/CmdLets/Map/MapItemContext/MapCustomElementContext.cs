using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraMap;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapCustomElementContext: MapPointerContext
	{
		[Parameter(HelpMessage = "Whether HTML formatting is allowed")]
		public SwitchParameter HtmlText { get; set; }

		[Parameter(HelpMessage = "Internal space between the map custom element's content and its edge, in pixels.")]
		public int[] Padding { get; set; }

		[Parameter(HelpMessage = "Center point of any possible render, relative to the bounds of the map custom element.")]
		public double[] RenderOrigin { get; set; }

		[Parameter(HelpMessage = "Glow color of a text for a map custom element.")]
		public string TextGlowColor { get; set; }


		public override MapItem CreateMapItem() => new MapCustomElement();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapCustomElement ?? throw new Exception("Map item must be MapCustomElement.");
			mapItem.AllowHtmlText = HtmlText;

			if (Padding != null)
			{
				if (Padding.Length == 1)
					mapItem.Padding = new Padding(Padding[0]);
				else if (Padding.Length == 4)
					mapItem.Padding = new Padding(Padding[0], Padding[1], Padding[2], Padding[3]);
				else
					throw new Exception("Invalid padding. Padding shall be an array with 1 or 4 integer values.");
			}

			if (RenderOrigin != null && RenderOrigin.Length == 2)
				mapItem.RenderOrigin = new MapPoint(RenderOrigin[0], RenderOrigin[1]);
			else if (RenderOrigin != null)
				throw new Exception("Invalid render origin. Render origin shall be an array with 2 double values.");

			var textGlowColor = Utils.ColorFromString(TextGlowColor);
			if (textGlowColor != Color.Empty)
				mapItem.TextGlowColor = textGlowColor;
		}
	}
}
