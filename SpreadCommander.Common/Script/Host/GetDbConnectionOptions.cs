using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public partial class ScriptHost
    {
#pragma warning disable CA1822 // Mark members as static
        public DbConnection GetDbConnection(string connectionName)
#pragma warning restore CA1822 // Mark members as static
        {
            if (string.IsNullOrWhiteSpace(connectionName))
                throw new Exception("ConnectionName cannot be empty.");

            var connections = DBConnections.LoadConnections();
            var connection  = connections.FindConnection(connectionName) ??
                throw new Exception($"Cannot find connection '{connectionName}'.");

            var conn = new Connection(connection.Provider, connection.ConnectionString);
            conn.Open();
            
            return conn.DbConnection;
        }
    }
}
