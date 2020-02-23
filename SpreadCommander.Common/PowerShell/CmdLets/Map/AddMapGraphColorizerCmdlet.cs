using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapGraphColorizer")]
	public class AddMapGraphColorizerCmdlet : BaseMapWithContextCmdlet
	{
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Colors for this colorizer.")]
		public string[] Colors { get; set; }


		protected override void UpdateMap()
		{
			var layer = MapContext.CurrentLayer as VectorItemsLayer ??
				throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");

			if (Colors == null || Colors.Length < 1)
				throw new Exception("Colors cannot be empty.");

			var colorizer = new GraphColorizer()
			{
				PredefinedColorSchema = PredefinedColorSchema.Palette
			};

			foreach (var col in Colors)
			{
				var color = Utils.ColorFromString(col);
				if (color == Color.Empty)
					throw new Exception($"Color '{col}' is not valid.");

				colorizer.ColorItems.Add(new ColorizerColorItem(color));
			}

			layer.Colorizer = colorizer;
		}
	}
}