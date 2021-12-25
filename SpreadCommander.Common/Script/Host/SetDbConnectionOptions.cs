using DevExpress.Mvvm;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public enum DbProvider { MsSqlServer, MsSqlServerFull, MySql, SQLite, ODBC }

    public partial class ScriptHost
    {
#pragma warning disable CA1822 // Mark members as static
        public void SetDbConnection(string connectionName, DbProvider provider, string connectionString,
#pragma warning restore CA1822 // Mark members as static
            string description = null, bool replace = false)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
                throw new Exception("ConnectionName cannot be empty.");

            if (connectionName.Contains(':'))
                throw new Exception("ConnectionName cannot contain colon (':') character.");

            var connections = DBConnections.LoadConnections();
            var connection  = connections.FindConnection(connectionName);
            if (connection != null)
            {
                if (replace)
                    connections.Connections.Remove(connection);
                else
                    throw new Exception($"Connection '{connectionName}' already exists.");
            }
            var strProvider = provider switch
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
                Name        = connectionName,
                Provider    = strProvider,
                Description = description
            };
            connection.ConnectionString = connectionString;
            connections.Connections.Add(connection);

            DBConnections.SaveConnections(connections);

            Messenger.Default.Send<ConnectionListChangedMessage>(new ConnectionListChangedMessage());
        }
    }
}
