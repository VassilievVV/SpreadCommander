using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraMap;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map.MapItemContext
{
	public class MapCalloutContext: MapPointerContext
	{
		[Parameter(HelpMessage = "Whether HTML formatting is allowed")]
		public SwitchParameter HtmlText { get; set; }


		public override MapItem CreateMapItem() => new MapCallout();

		public override void ConfigureMapItem(MapItem item)
		{
			base.ConfigureMapItem(item);

			var mapItem = item as MapCallout ?? throw new Exception("Map item must be MapCallout.");
			mapItem.AllowHtmlText = HtmlText;
		}
	}
}
