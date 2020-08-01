#pragma warning disable CRR0047

using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using SpreadCommander.Common.DBMS.SQLite;

namespace SpreadCommander.Common.Code
{
    #region ConnectionFactory
    public class ConnectionFactory : IComparable<ConnectionFactory>
    {
        public const string SqlClientFactoryLightName = "SpreadCommander.Common.SqlClient";
        public const string SqlCeInvariantNamePrefix  = "System.Data.SqlServerCe";
        public const string SqlClientFactoryName      = "Microsoft.Data.SqlClient";
        public const string OldSqlClientFactoryName   = "System.Data.SqlClient";
        public const string SqlCeFactoryName          = "System.Data.SqlServerCe";
        public const string SqlCe35FactoryName        = "System.Data.SqlServerCe.3.5";
        public const string OracleFactoryName         = "System.Data.OracleClient";
        public const string OleDbFactoryName          = "System.Data.OleDb";
        public const string OdbcFactoryName           = "System.Data.Odbc";
        public const string MySqlFactoryName          = "MySqlConnector";
        public const string MySqlFactoryName_Old      = "MySql.Data.MySqlClient";
        public const string SQLiteFactoryName         = "System.Data.SQLite";


        public ConnectionFactory(string name, string invariantName, string description, 
            Type connectionType, Type connectionStringBuilderType)
        {
            InitialName                 = name;
            Name                        = name;
            Description                 = description;
            InvariantName               = invariantName;
            ConnectionType              = connectionType;
            ConnectionStringBuilderType = connectionStringBuilderType;

            UpdateName();
        }

        public static ConnectionFactory CreateConnectionFactory(string InvariantName)
        {
            if (string.Compare(InvariantName, SqlClientFactoryLightName, true) == 0)
                return new ConnectionFactory("Microsoft SQL Server (light)", SqlClientFactoryLightName,
                    "Microsoft SQL Server (simplified connection string)",
                    typeof(Microsoft.Data.SqlClient.SqlConnection), typeof(SqlScript.SqlConnectionStringBuilderLight));
            else if (string.Compare(InvariantName, SqlClientFactoryName, true) == 0 || string.Compare(InvariantName, OldSqlClientFactoryName, true) == 0)
                return new ConnectionFactory("Microsoft SQL Server", SqlClientFactoryName,
                    "Microsoft SQL Server", typeof(Microsoft.Data.SqlClient.SqlConnection), typeof(Microsoft.Data.SqlClient.SqlConnectionStringBuilder));
            else if (string.Compare(InvariantName, OdbcFactoryName, true) == 0)
                return new ConnectionFactory("ODBC", OdbcFactoryName, "ODBC connection",
                    typeof(System.Data.Odbc.OdbcConnection), typeof(System.Data.Odbc.OdbcConnectionStringBuilder));
            else if (string.Compare(InvariantName, MySqlFactoryName, true) == 0 ||
                string.Compare(InvariantName, MySqlFactoryName_Old, true) == 0)
                return new ConnectionFactory("MySQL", MySqlFactoryName, "MySQL database",
                    typeof(MySqlConnector.MySqlConnection), typeof(MySqlConnector.MySqlConnectionStringBuilder));
            else if (string.Compare(InvariantName, SQLiteFactoryName, true) == 0)
                return new ConnectionFactory("SQLite", SQLiteFactoryName, "SQLite database",
                    typeof(SQLiteConnection), typeof(SQLiteConnectionStringBuilder));
            else if (string.Compare(InvariantName, OleDbFactoryName, true) == 0)
                return new ConnectionFactory("OLEDB", OleDbFactoryName, "OLEDB connection",
                    typeof(System.Data.OleDb.OleDbConnection), typeof(System.Data.OleDb.OleDbConnectionStringBuilder));

            return null;
        }

        public static bool IsMSSQLServer(string provider)
        {
            return string.Compare(provider, SqlClientFactoryName, true) == 0 ||
                string.Compare(provider, SqlClientFactoryLightName, true) == 0;
        }

        public static ConnectionFactory CreateFactoryForConnection(DbConnection connection)
        {
            if (connection == null)
                return null;
            else if (connection is SqlConnection)
                return new ConnectionFactory("Microsoft SQL Server", SqlClientFactoryName,
                    "Microsoft SQL Server", typeof(Microsoft.Data.SqlClient.SqlConnection), typeof(Microsoft.Data.SqlClient.SqlConnectionStringBuilder));
            else if (connection is System.Data.Odbc.OdbcConnection)
                return new ConnectionFactory("ODBC", OdbcFactoryName, "ODBC connection",
                    typeof(System.Data.Odbc.OdbcConnection), typeof(System.Data.Odbc.OdbcConnectionStringBuilder));
            else if (connection is MySqlConnector.MySqlConnection)
                return new ConnectionFactory("MySQL", MySqlFactoryName, "MySQL database",
                    typeof(MySqlConnector.MySqlConnection), typeof(MySqlConnector.MySqlConnectionStringBuilder));
            else if (connection is SQLiteConnection)
                return new ConnectionFactory("SQLite", SQLiteFactoryName, "SQLite database",
                    typeof(SQLiteConnection), typeof(SQLiteConnectionStringBuilder));
            else if (connection is System.Data.OleDb.OleDbConnection)
                return new ConnectionFactory("OLEDB", OleDbFactoryName, "OLEDB connection",
                    typeof(System.Data.OleDb.OleDbConnection), typeof(System.Data.OleDb.OleDbConnectionStringBuilder));

            return null;
        }

        public static bool ShowConnectionString(string provider)
        {
            return string.Compare(provider, OdbcFactoryName, true) == 0;
        }

        public static List<ConnectionFactory> ListConnectionFactories()
        {
            List<ConnectionFactory> connFactories = new List<ConnectionFactory>
            {
                CreateConnectionFactory(SqlClientFactoryLightName),
                CreateConnectionFactory(SqlClientFactoryName),
                CreateConnectionFactory(OdbcFactoryName),
                CreateConnectionFactory(MySqlFactoryName),
                CreateConnectionFactory(SQLiteFactoryName),
                CreateConnectionFactory(OleDbFactoryName)
            };

            connFactories.Sort();

            return connFactories;
        }

        public static ConnectionFactory GetConnectionFactory(string name)
        {
            List<ConnectionFactory> factories = ListConnectionFactories();
            foreach (ConnectionFactory factory in factories)
            {
                if ((string.Compare(name, factory.InitialName, true) == 0) ||
                    (string.Compare(name, factory.Name, true) == 0) ||
                    (string.Compare(name, factory.InvariantName, true) == 0))
                    return factory;
            }
            return null;
        }

        public static string GetConnectionFactoryInvariantName(string name)
        {
            ConnectionFactory factory = GetConnectionFactory(name);
            return factory?.InvariantName;
        }

        public static string GetConnectionFactoryName(string invariantName)
        {
            ConnectionFactory factory = GetConnectionFactory(invariantName);
            return factory?.InitialName;
        }

        public static Connection CreateFromDbConnection(DbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            
            var factory = CreateFactoryForConnection(connection);
            return new Connection(factory, connection);
        }

        public static Connection CreateFromString(string value)
        {
            var connections = DBConnections.LoadConnections();
            var connection  = connections.FindConnection(value) ??
                throw new Exception($"Cannot find connection '{value}'.");

            var result = new Connection(connection.Provider, connection.ConnectionString);
            return result;
        }

        private void UpdateName()
        {
            switch (InvariantName)
            {
                case "System.Data.Odbc":
                    Name = "ODBC data sources";
                    break;
                case "System.Data.OleDb":
                    Name = "OLEDB data sources";
                    break;
                case "System.Data.OracleClient":
                    Name = "Oracle";
                    break;
                case "Microsoft.Data.SqlClient":
                case "System.Data.SqlClient":
                    Name = "Microsoft SQL Server";
                    break;
                case SqlClientFactoryLightName:
                    Name = "Microsoft SQL Server (light)";
                    break;
                case MySqlFactoryName:
                    Name = "MySQL";
                    break;
                case SQLiteFactoryName:
                    Name = "SQLite";
                    break;
            }

        }

        public string Name                          { get; private set; }

        public string InitialName                   { get; private set; }

        public string Description                   { get; private set; }

        public string InvariantName                 { get; private set; }

        public Type ConnectionType                  { get; private set; }

        public Type ConnectionStringBuilderType     { get; private set; }

        public override string ToString() => $"{Name} ({Description})";

        public Connection CreateConnection(string connectionString)
        {
            return new Connection(InvariantName, connectionString);
        }

        public DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            try
            {
                if (ConnectionStringBuilderType == null)
                    throw new Exception("Connection StringBuilder type is not specified.");

                DbConnectionStringBuilder builder = Activator.CreateInstance(ConnectionStringBuilderType) as DbConnectionStringBuilder;
                builder.BrowsableConnectionString = false;
                return builder;
            }
            catch (Exception)
            {
                DbConnectionStringBuilder result = new DbConnectionStringBuilder(false)
                {
                    BrowsableConnectionString = false
                };
                return result;
            }
        }

        public Image GetIcon()
        {
            try
            {
                Type connType = ConnectionType;
                if (connType == null)
                    return null;

                Image img = ToolboxBitmapAttribute.GetImageFromResource(ConnectionType, null, true);
                return img;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int CompareTo(ConnectionFactory other)
        {
            if (other == null)
                return 1;

            return string.Compare(Name, other.Name, true);
        }
    }
    #endregion

    #region Connection
    public class Connection
    {

        public Connection(string factoryInvariantName) :
            this(factoryInvariantName, null)
        {
        }

        public Connection(string factoryInvariantName, string connectionString)
        {
            var factory = ConnectionFactory.GetConnectionFactory(factoryInvariantName);

            FactoryInvariantName        = factoryInvariantName;
            ConnectionType              = factory?.ConnectionType;
            ConnectionStringBuilderType = factory?.ConnectionStringBuilderType;
            ConnectionString            = connectionString;

            SetApplicationName(Parameters.ApplicationName);
        }

        public Connection(string factoryInvariantName, Type connectionType, Type connectionStringBuilderType,
            DbConnection conn)
        {
            FactoryInvariantName        = factoryInvariantName;
            ConnectionType              = connectionType;
            ConnectionStringBuilderType = connectionStringBuilderType;
            DbConnection                = conn;
            ConnectionString            = DbConnection?.ConnectionString;

            SetApplicationName(Parameters.ApplicationName);
        }
        
        internal Connection(ConnectionFactory factory, DbConnection connection)
        {
            FactoryInvariantName        = factory?.InvariantName;
            ConnectionType              = connection.GetType();
            ConnectionStringBuilderType = factory?.ConnectionStringBuilderType;
            ConnectionString            = connection.ConnectionString;
            DbConnection                = connection;

            SetApplicationName(Parameters.ApplicationName);
        }

        public string FactoryInvariantName { get; private set; }

        public Type ConnectionType { get; private set; }
        
        public Type ConnectionStringBuilderType { get; private set; }

        public string ConnectionString { get; private set; }

        public string Database
        {
            get { return DbConnection?.Database; }
            set { ChangeDatabase(value); }
        }

        public DbConnection DbConnection { get; private set; }

        public ConnectionState State
        {
            get { return DbConnection != null ? DbConnection.State : ConnectionState.Closed; }
        }

        protected void SetConnectionString(DbConnection connection, string connectionString)
        {
            if (DbConnection is SQLiteConnection sqliteConnection)
            {
                var sqliteBuilder = new SQLiteConnectionStringBuilder(connectionString);

                if (!string.IsNullOrWhiteSpace(sqliteBuilder.DataSource) && !sqliteBuilder.DataSource.Trim().StartsWith(":"))
                {
                    sqliteBuilder.DataSource = Project.Current.MapPath(sqliteBuilder.DataSource);

                    //If SQLite database file is provided (database is not in-memory) and file does not exist - create it.
                    if (!string.IsNullOrWhiteSpace(sqliteBuilder.DataSource) &&
                        !System.IO.File.Exists(sqliteBuilder.DataSource))
                        SQLiteConnection.CreateFile(sqliteBuilder.DataSource);
                }
                
                sqliteConnection.ConnectionString = sqliteBuilder.ConnectionString;
            }
            else if (DbConnection is SqlConnection sqlConnection)
            {
                var sqlBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    PersistSecurityInfo = true
                };

                if (string.IsNullOrWhiteSpace(sqlBuilder.DataSource) &&
                    !string.IsNullOrWhiteSpace(ApplicationSettings.Default.MSSQLServer?.DefaultServer))
                {
                    sqlBuilder.DataSource = ApplicationSettings.Default.MSSQLServer.DefaultServer;
                    if (string.IsNullOrWhiteSpace(sqlBuilder.UserID))
                    {
                        if (!string.IsNullOrWhiteSpace(ApplicationSettings.Default.MSSQLServer.DefaultUserID))
                        {
                            sqlBuilder.UserID = ApplicationSettings.Default.MSSQLServer.DefaultUserID;
                            if (!string.IsNullOrWhiteSpace(ApplicationSettings.Default.MSSQLServer.DefaultPassword))
                                sqlBuilder.Password = ApplicationSettings.Default.MSSQLServer.DefaultPassword;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(sqlBuilder.AttachDBFilename))
                    sqlBuilder.AttachDBFilename = Project.Current.MapPath(sqlBuilder.AttachDBFilename);
                if (!sqlBuilder.IntegratedSecurity && string.IsNullOrWhiteSpace(sqlBuilder.UserID))
                    sqlBuilder.IntegratedSecurity = true;

                sqlBuilder.ApplicationName = Parameters.ApplicationName;

                sqlConnection.ConnectionString = sqlBuilder.ConnectionString;
            }
            else if (DbConnection is MySqlConnection mySqlConnection)
            {
                var mySqlBuilder = new MySqlConnectionStringBuilder(connectionString)
                {
                    PersistSecurityInfo = true
                };

                if (string.IsNullOrWhiteSpace(mySqlBuilder.Server) &&
                    !string.IsNullOrWhiteSpace(ApplicationSettings.Default.MySQLServer?.DefaultServer))
                {
                    mySqlBuilder.Server = ApplicationSettings.Default.MySQLServer.DefaultServer;
                    if ((ApplicationSettings.Default.MySQLServer.DefaultPort ?? 0) > 0)
                        mySqlBuilder.Port = ApplicationSettings.Default.MySQLServer.DefaultPort.Value;

                    if (string.IsNullOrWhiteSpace(mySqlBuilder.UserID))
                    {
                        if (!string.IsNullOrWhiteSpace(ApplicationSettings.Default.MySQLServer.DefaultUserID))
                        {
                            mySqlBuilder.UserID = ApplicationSettings.Default.MySQLServer.DefaultUserID;
                            if (!string.IsNullOrWhiteSpace(ApplicationSettings.Default.MySQLServer.DefaultPassword))
                                mySqlBuilder.Password = ApplicationSettings.Default.MySQLServer.DefaultPassword;
                        }
                    }

                    if (ApplicationSettings.Default.MySQLServer.DefaultSslMode.HasValue)
                        mySqlBuilder.SslMode = ApplicationSettings.Default.MySQLServer.DefaultSslMode.Value;
                }

                mySqlBuilder.ApplicationName = Parameters.ApplicationName;

                mySqlConnection.ConnectionString = mySqlBuilder.ConnectionString;
            }
            else
                connection.ConnectionString = connectionString;
        }
        
        public void Open()
        {
            if (DbConnection != null)
                throw new InvalidOperationException("Connection is already open.");

            if (ConnectionType == null)
                throw new Exception("Connection type is not specified.");

            DbConnection = Activator.CreateInstance(ConnectionType) as DbConnection;
            if (DbConnection == null)
                throw new Exception("Invalid connection type.");

            SetConnectionString(DbConnection, ConnectionString);

            DbConnection.Open();

            if (DbConnection is SQLiteConnection sqliteConnection)
                sqliteConnection.BindSCFunctions();
        }

        public Task OpenAsync(CancellationToken cancellationToken) =>
            Task.Run(() => Open(), cancellationToken);

        public void Close()
        {
            if (DbConnection != null)
            {
                DbConnection.Close();
                DbConnection.Dispose();
                DbConnection = null;
            }
        }

        public void ChangeDatabase(string databaseName)
        {
            if (DbConnection == null)
                throw new InvalidOperationException("Connection is not assigned.");

            DbConnection.ChangeDatabase(databaseName);
        }

        public DbCommand CreateDbCommand()
        {
            return DbConnection.CreateCommand();
        }

        public DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            try
            {
                if (ConnectionStringBuilderType == null)
                    throw new Exception("Connection StringBuilder type is not specified.");
                
                DbConnectionStringBuilder builder = Activator.CreateInstance(ConnectionStringBuilderType) as DbConnectionStringBuilder;
                builder.ConnectionString = ConnectionString;
                builder.BrowsableConnectionString = false;
                return builder;
            }
            catch (Exception)
            {
                DbConnectionStringBuilder result = new DbConnectionStringBuilder(false)
                {
                    ConnectionString = Utils.NonNullString(ConnectionString),
                    BrowsableConnectionString = false
            };
                return result;
            }
        }

        public void UpdateConnectionString(object ConnectionStringBuilder)
        {
            ConnectionString = ConnectionStringBuilder.ToString();
        }

        public void SetApplicationName(string AppName)
        {
            try
            {
                if (ConnectionStringBuilderType == null)
                    throw new Exception("Connection StringBuilder type is not specified.");

                DbConnectionStringBuilder builder = Activator.CreateInstance(ConnectionStringBuilderType) as DbConnectionStringBuilder;
                builder.ConnectionString          = ConnectionString;
                builder.BrowsableConnectionString = false;

                if (builder.ContainsKey("Application Name"))
                    builder["Application Name"] = AppName;

                ConnectionString = builder.ToString();
            }
            catch (Exception)
            {
                //Do nothing, it is optional method
            }
        }

        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            if (DbConnection == null)
                return null;

            if (collectionName == null)
                return DbConnection.GetSchema();
            if (restrictionValues == null)
                return DbConnection.GetSchema(collectionName);
            return DbConnection.GetSchema(collectionName, restrictionValues);
        }

        public DbConnection CreateClosedConnection()
        {
            if (ConnectionType == null)
                throw new Exception("Connection type is not specified.");

            if (!(Activator.CreateInstance(ConnectionType) is DbConnection connection))
                throw new Exception("Invalid connection type.");

            connection.ConnectionString = ConnectionString;
            return connection;
        }
    }
    #endregion

    #region ConnectionFactoryComparison
    public class ConnectionFactoryComparison : IComparer<ConnectionFactory>
    {
        private static readonly string[] Providers = new string[]
        {
            ConnectionFactory.SqlClientFactoryLightName,
            ConnectionFactory.SqlClientFactoryName,
            ConnectionFactory.OdbcFactoryName,
            ConnectionFactory.MySqlFactoryName,
            ConnectionFactory.SQLiteFactoryName
        };

        public int Compare(ConnectionFactory x, ConnectionFactory y)
        {
            if (x == null && y == null)
                return 0;
            else if (x == null)
                return -1;
            else if (y == null)
                return 1;

            int indX = Array.IndexOf<string>(Providers, x.InvariantName);
            int indY = Array.IndexOf<string>(Providers, y.InvariantName);

            if ((indX >= 0 && indY >= 0) || (indX < 0 && indY < 0))
                return indX - indY;
            else if (indX >= 0)
                return -1;
            else
                return 1;
        }
    }
    #endregion
}