using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapPushpinContext: MapPointerContext
	{
		[Parameter(HelpMessage = "Glow color of a text for a map custom element.")]
		public string TextGlowColor { get; set; }

		[Parameter(HelpMessage = "Exact XY position where you wish the text to be drawn on the map pushpin.")]
		public int[] TextOrigin { get; set; }


		public override MapItem CreateMapItem() => new MapPushpin();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapPushpin ?? throw new Exception("Map item must be MapPushpin.");

			var textGlowColor = Utils.ColorFromString(TextGlowColor);
			if (textGlowColor != Color.Empty)
				mapItem.TextGlowColor = textGlowColor;

			if (TextOrigin != null && TextOrigin.Length == 2)
				mapItem.TextOrigin = new Point(TextOrigin[0], TextOrigin[1]);
			else if (TextOrigin != null)
				throw new Exception("Invalid text origin. Text origin shall be an array with 2 integer values.");
		}
	}
}
