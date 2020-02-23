using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsCommon.Get, "DbConnection")]
    [OutputType(typeof(DbConnection))]
    public class GetDbConnectionCmdlet: SCCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of connection")]
        public string ConnectionName { get; set; }


        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(ConnectionName))
                throw new Exception("ConnectionName cannot be empty.");
            
            var connections = DBConnections.LoadConnections();
            var connection  = connections.FindConnection(ConnectionName) ??
                throw new Exception($"Cannot find connection '{ConnectionName}'.");

            var conn = new Connection(connection.Provider, connection.ConnectionString);
            conn.Open();
            var result = conn.DbConnection;

            WriteObject(result);
        }
    }
}
