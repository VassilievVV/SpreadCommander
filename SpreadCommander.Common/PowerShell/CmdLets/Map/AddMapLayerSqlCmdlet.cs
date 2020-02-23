using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
	[Cmdlet(VerbsCommon.Add, "MapLayerSql")]
	public class AddMapLayerSqlCmdlet: BaseMapWithContextCmdlet
	{
		[Parameter(HelpMessage = "Layer's name.")]
		public string Name { get; set; }

		[Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of MS SQL Server connection containing geo data.")]
		[Alias("Connection", "conn")]
		public string ConnectionName { get; set; }

		[Parameter(Mandatory = true, Position = 1, HelpMessage = "Query that returns geo data.")]
		public string Query { get; set; }

		[Parameter(HelpMessage = "Column name containing geo data.")]
		[PSDefaultValue(Value = "Spatial")]
		[DefaultValue("Spatial")]
		public string SpatialColumn { get; set; } = "Spatial";

		[Parameter(HelpMessage = "Pattern used to generate shape titles")]
		public string ShapeTitlesPattern { get; set; }


		protected override void UpdateMap()
		{
			var map = MapContext.Map;

			var layer = new VectorItemsLayer()
			{
				AllowEditItems     = false,
				EnableHighlighting = false,
				EnableSelection    = false
			};

			if (!string.IsNullOrWhiteSpace(Name))
				layer.Name = Name;
			if (!string.IsNullOrWhiteSpace(ShapeTitlesPattern))
			{
				layer.ShapeTitlesPattern    = ShapeTitlesPattern;
				layer.ShapeTitlesVisibility = VisibilityMode.Visible;
			}
			else
				layer.ShapeTitlesVisibility = VisibilityMode.Hidden;

			var connections = DBConnections.LoadConnections();
			var connection  = connections.FindConnection(ConnectionName);
			if (connection == null)
				throw new Exception($"Cannot find connection '{ConnectionName}'.");
			if (!ConnectionFactory.IsMSSQLServer(connection.Provider))
				throw new Exception("DBMS must be Microsoft SQL Server.");

			var adapter = new SqlGeometryDataAdapter
			{
				ConnectionString  = connection.ConnectionString,
				SqlText           = Query,
				SpatialDataMember = SpatialColumn
			};

			layer.Data = adapter;

			map.Layers.Add(layer);

			MapContext.CurrentLayer = layer;
		}
	}
}
