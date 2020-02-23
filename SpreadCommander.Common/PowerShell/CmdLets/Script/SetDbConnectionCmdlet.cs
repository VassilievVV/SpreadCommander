using DevExpress.Mvvm;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsCommon.Set, "DbConnection")]
    public class SetDbConnectionCmdlet: SCCmdlet
    {
        public enum DbProvider { MsSqlServer, MsSqlServerFull, MySql, SQLite, ODBC }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of connection")]
        public string ConnectionName { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Database provider")]
        public DbProvider Provider { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Connection string")]
        public string ConnectionString { get; set; }

        [Parameter(HelpMessage = "Description of the connection")]
        public string Description { get; set; }

        [Parameter(HelpMessage = "If set and connection with specified ConnectionName already exists, it will be replaced")]
        public SwitchParameter Replace { get; set; }


        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(ConnectionName))
                throw new Exception("ConnectionName cannot be empty.");

            if (ConnectionName.Contains(":"))
                throw new Exception("ConnectionName cannot contain colon (':') character.");

            var connections = DBConnections.LoadConnections();
            var connection  = connections.FindConnection(ConnectionName);
            if (connection != null)
            {
                if (Replace)
                    connections.Connections.Remove(connection);
                else
                    throw new Exception($"Connection '{ConnectionName}' already exists.");
            }
            var strProvider = Provider switch
            {
                DbProvider.MsSqlServer     => ConnectionFactory.SqlClientFactoryLightName,
                DbProvider.MsSqlServerFull => ConnectionFactory.SqlClientFactoryName,
                DbProvider.MySql           => ConnectionFactory.MySqlFactoryName,
                DbProvider.SQLite          => ConnectionFactory.SQLiteFactoryName,
                DbProvider.ODBC            => ConnectionFactory.OdbcFactoryName,
                _                          => ConnectionFactory.SqlClientFactoryLightName
            };
            connection = new DBConnection()
            {
                Name             = ConnectionName,
                Provider         = strProvider,
                Description      = this.Description
            };
            connection.ConnectionString = ConnectionString;
            connections.Connections.Add(connection);

            DBConnections.SaveConnections(connections);

            Messenger.Default.Send<ConnectionListChangedMessage>(new ConnectionListChangedMessage());
        }
    }
}
