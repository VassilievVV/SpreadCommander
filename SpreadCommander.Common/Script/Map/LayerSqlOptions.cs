using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class LayerSqlOptions
    {
        [Description("Layer's name.")]
        public string Name { get; set; }

        [Description("Column name containing geo data.")]
        [DefaultValue("Spatial")]
        public string SpatialColumn { get; set; } = "Spatial";

        [Description("Pattern used to generate shape titles")]
        public string ShapeTitlesPattern { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddLayerSql(string connectionName, string query, LayerSqlOptions options = null)
        {
            options ??= new LayerSqlOptions();

            var map = this.Map;

            var layer = new VectorItemsLayer()
            {
                AllowEditItems     = false,
                EnableHighlighting = false,
                EnableSelection    = false
            };

            if (!string.IsNullOrWhiteSpace(options.Name))
                layer.Name = options.Name;
            if (!string.IsNullOrWhiteSpace(options.ShapeTitlesPattern))
            {
                layer.ShapeTitlesPattern    = options.ShapeTitlesPattern;
                layer.ShapeTitlesVisibility = VisibilityMode.Visible;
            }
            else
                layer.ShapeTitlesVisibility = VisibilityMode.Hidden;

            var connections = DBConnections.LoadConnections();
            var connection  = connections.FindConnection(connectionName);
            if (connection == null)
                throw new Exception($"Cannot find connection '{connectionName}'.");
            if (!ConnectionFactory.IsMSSQLServer(connection.Provider))
                throw new Exception("DBMS must be Microsoft SQL Server.");

            var adapter = new SqlGeometryDataAdapter
            {
                ConnectionString  = connection.ConnectionString,
                SqlText           = query,
                SpatialDataMember = options.SpatialColumn
            };

            layer.Data = adapter;

            map.Layers.Add(layer);
            CurrentLayer = layer;

            return this;
        }
    }
}
