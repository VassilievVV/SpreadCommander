using DevExpress.XtraMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapKeyColorizer")]
	public class AddMapKeyColorizerCmdlet : BaseMapWithContextCmdlet
	{
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Field in datasource (collection, shapefile, KML etc.) to color map according to data.")]
		public string KeyField { get; set; }

		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Keys for this colorizer in form Name=Value pairs.")]
		public Hashtable Keys { get; set; }

		[Parameter(Mandatory = true, Position = 2, HelpMessage = "Colors for this colorizer.")]
		public string[] Colors { get; set; }


		protected override void UpdateMap()
		{
			var layer = MapContext.CurrentLayer as VectorItemsLayer ??
				throw new Exception("Cannot determine layer. Map items can be added only to recent vector items layer created with cmdlet Add-MapLayerVectorItems.");

			if (string.IsNullOrWhiteSpace(KeyField))
				throw new Exception("KeyField cannot be empty.");

			if (Keys == null || Keys.Count < 1)
				throw new Exception("Keys cannot be empty.");

			if (Colors == null || Colors.Length < 1)
				throw new Exception("Colors cannot be empty.");

			var colorizer = new KeyColorColorizer()
			{
				PredefinedColorSchema = PredefinedColorSchema.Palette
			};

			colorizer.ItemKeyProvider = new AttributeItemKeyProvider() { AttributeName = KeyField };

			foreach (DictionaryEntry keyPair in Keys)
				colorizer.Keys.Add(new ColorizerKeyItem() { Name = Convert.ToString(keyPair.Value), Key = keyPair.Key });

			foreach (var col in Colors)
			{
				var color = Utils.ColorFromString(col);
				if (color == Color.Empty)
					throw new Exception($"Color '{col}' is not valid.");

				colorizer.Colors.Add(color);
			}

			layer.Colorizer = colorizer;
		}
	}
}