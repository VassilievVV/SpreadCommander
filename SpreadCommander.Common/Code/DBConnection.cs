#pragma warning disable CRR0047

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DevExpress.Mvvm;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SpreadCommander.Common;
using SpreadCommander.Common.Messages;
using SpreadCommander.Common.SqlScript;

namespace SpreadCommander.Common.Code
{
    #region DBConnections
    public class DBConnections
    {
        public static readonly object LockObject = new object();

        public string SelectedConnection { get; set; }

        public List<DBConnection> Connections = new List<DBConnection>();


        public static DBConnection ParseConnection(string scConnectionString)
        {
            int posColon         = scConnectionString.IndexOf(':');

            var prefix           = scConnectionString.Substring(0, posColon).Trim().ToLowerInvariant();
            var connectionString = scConnectionString.Substring(posColon + 1).Trim();

            switch (prefix)
            {
                case "mssql":
                    var resultMSSQL = new DBConnection() { Provider = ConnectionFactory.SqlClientFactoryName };
                    connectionString = AdjustMSSQLConnectionString(connectionString);
                    resultMSSQL.ConnectionString = connectionString;
                    return resultMSSQL;
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                case "mysql":
                    var resultMySQL = new DBConnection() { Provider = ConnectionFactory.MySqlFactoryName };
                    connectionString = AdjustMySQLConnectionString(connectionString);
                    resultMySQL.ConnectionString = connectionString;
                    return resultMySQL;
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
                case "sqlite":
                    var resultSQLite = new DBConnection() { Provider = ConnectionFactory.SQLiteFactoryName };
                    connectionString = AdjustSQLiteConnectionString(connectionString);
                    resultSQLite.ConnectionString = connectionString;
                    return resultSQLite;
                case "odbc":
                    var resultODBC = new DBConnection() { Provider = ConnectionFactory.OdbcFactoryName };
                    resultODBC.ConnectionString = connectionString;
                    return resultODBC;
            }

            return null;

            
            static string UpdateFirstPart(string value, string firstPartName)
            {
                var result = value;
                
                int posSemicolon = value.IndexOf(';');
                if (posSemicolon < 0)
                    posSemicolon = value.Length;

                var firstPart = value.Substring(0, posSemicolon);
                int posEqual = firstPart.IndexOf('=');
                if (posEqual < 0)
                {
                    result = $"{firstPartName}={firstPart};";
                    if (posSemicolon < value.Length)
                        result += value.Substring(posSemicolon + 1);
                }

                return result;
            }
            
            static string AdjustMSSQLConnectionString(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return string.Empty;

                var result = UpdateFirstPart(value, "Database");
                return result;
            }
            
            static string AdjustMySQLConnectionString(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return string.Empty;

                var result = UpdateFirstPart(value, "Database");
                return result;
            }

            static string AdjustSQLiteConnectionString(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return string.Empty;

                var result = UpdateFirstPart(value, "Data Source");
                return result;
            }
        }
        
        public DBConnection FindConnection(string connectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
                return null;

            int posColon = connectionName.IndexOf(':');
            if (posColon < 0)
            {
                var result = Connections.Where(c => string.Compare(c.Name, connectionName, true) == 0).FirstOrDefault();
                return result;
            }
            else
            {
                var parseResult = ParseConnection(connectionName);
                return parseResult;
            }
        }

        public static DBConnections LoadConnections()
        {
            var fileName = Path.Combine(Parameters.ApplicationDataFolder, "Connections.xml");
            if (!File.Exists(fileName))
                return new DBConnections();

            lock (LockObject)
            {
                DBConnections connections;
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    connections = Utils.DeserializeObject<DBConnections>(stream);
                }

                connections.Connections.Sort((x, y) => StringLogicalComparer.Default.Compare(x.Name, y.Name));

                return connections;
            }
        }

        public static void SaveConnections(DBConnections connections)
        {
            lock (LockObject)
            {
                var fileName = Path.Combine(Parameters.ApplicationDataFolder, "Connections.xml");
                var fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists && fileInfo.LastWriteTime < DateTime.Today.AddDays(-1))
                    Utils.BackupFile(fileName);

                using var stream = new FileStream(fileName, FileMode.Create);
                Utils.SerializeObjectToStream<DBConnections>(connections, stream);
            }

            Messenger.Default.Send(new ConnectionListChangedMessage());
        }
    }
    #endregion

    [DataContract(Namespace = Parameters.ApplicationNamespace)]
    public class DBConnection
    {
        [DataMember()]
        public string Name { get; set; }

        [DataMember()]
        public string Description { get; set; }

        [DataMember()]
        public string Provider { get; set; }

        [PasswordPropertyText(true)]
        [XmlIgnore()]
        [JsonIgnore()]
        public string ConnectionString
        {
            get
            {
                var encConnectionString = EncConnectionString;
                if (string.IsNullOrWhiteSpace(encConnectionString))
                    return null;
                return Utils.UnProtectUserString(encConnectionString);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    EncConnectionString = null;
                else
                    EncConnectionString = Utils.ProtectUserString(value);
            }
        }

        [DataMember()]
        [Browsable(false)]
        [PasswordPropertyText(true)]
        public string EncConnectionString { get; set; }

        public bool ShouldSerializeConnectionString() => false;

        [XmlIgnore()]
        [JsonIgnore()]
        public int ImageIndex => 0;



        //DBConnection is saved to XML, so do not use properties for following methods
        public ConnectionFactory GetConnectionFactory()
        {
            var result = ConnectionFactory.CreateConnectionFactory(Provider);
            return result;
        }
    }
}
