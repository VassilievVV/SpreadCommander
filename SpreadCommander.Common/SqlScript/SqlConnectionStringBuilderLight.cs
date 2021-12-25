using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.SqlScript
{
    #region SqlConnectionStringBuilderLight
    public class SqlConnectionStringBuilderLight: DbConnectionStringBuilder, ICustomTypeDescriptor
    {
        #region ServerConverter
        private class ServerConverter : TypeConverter
        {
            private StandardValuesCollection _LocalServers;

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;
                else
                    return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                    return true;
                else
                    return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                    return value;
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value is string)
                    return value;
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                /*
                if (_LocalServers == null)
                {
                    //Return only local instances. Full list of instances in network is available in full SqlConnectionStringBuilder but it is slow.
                    var serverNames = new List<string>();
                    var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");
                    if (key != null)
                    {
                        try
                        {
                            var names = key.GetValueNames();
                            foreach (var name in names)
                            {
                                //Default instance - MSSQLSERVER
                                if (string.Compare(name, "MSSQLSERVER", true) == 0)
                                    serverNames.Add(".");
                                else
                                    serverNames.Add($@".\{name}");
                            }
                        }
                        finally
                        {
                            key.Close();
                        }
                    }

                    _LocalServers = new StandardValuesCollection(serverNames);
                }
                */

                if (_LocalServers == null)
                {
                    var servers = SqlServerEnumerator.ListSqlServers();
                    _LocalServers = new StandardValuesCollection(servers);
                }

                return _LocalServers;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool IsValid(ITypeDescriptorContext context, object value)
            {
                return true;
            }
        }
        #endregion

        #region DatabaseConverter
        private class DatabaseConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;
                else
                    return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                    return true;
                else
                    return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                    return value;
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value is string)
                    return value;
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                var result = new List<string>();

                if (context.Instance is not SqlConnectionStringBuilderLight instance)
                    return new StandardValuesCollection(result);

                var server             = instance.Server;
                var integratedSecurity = instance.IntegratedSecurity;
                var userID             = instance.UserID;

                if (string.IsNullOrWhiteSpace(server) || (!integratedSecurity && string.IsNullOrWhiteSpace(userID)))
                    return new StandardValuesCollection(result);

                var connStr = $"Server={server};Database=master;" +
                    (integratedSecurity ? "Integrated Security=True;" : string.Empty) +
                    (!string.IsNullOrWhiteSpace(userID) ? $"User ID={userID}" : string.Empty);

                try
                {
                    using var conn = new SqlConnection(connStr);
                    conn.Open();

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "select name from sys.databases order by name";

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                        result.Add(Convert.ToString(reader[0]));
                }
                catch (Exception)
                {
                    //Do nothing, could not read list of databases, so return empty list
                }

                return new StandardValuesCollection(result);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool IsValid(ITypeDescriptorContext context, object value)
            {
                return true;
            }
        }
        #endregion

        #region AttachDBFilenameEditor
        private class AttachDBFilenameEditor : System.Windows.Forms.Design.FileNameEditor
        {
            protected override void InitializeDialog(System.Windows.Forms.OpenFileDialog openFileDialog)
            {
                base.InitializeDialog(openFileDialog);

                openFileDialog.Filter           = "MS SQL Server database (*.mdf)|*.mdf|All files (*.*)|*.*";
                openFileDialog.Title            = "Select MS SQL Server database";
                openFileDialog.DefaultExt       = "mdf";
                openFileDialog.CheckFileExists  = false;
                openFileDialog.CheckPathExists  = true;
                openFileDialog.RestoreDirectory = true;
            }
        }
        #endregion

        #region SqlServerEnumerator
        public static class SqlServerEnumerator
        {
            public static string SqlDataSourcesFileName => Path.Combine(Parameters.ApplicationDataFolder, "SqlDataSources.xml");

            public static void EnumerateSqlServers()
            {
                var fileInfo = new FileInfo(SqlDataSourcesFileName);
                if (fileInfo.Exists && fileInfo.CreationTime >= DateTime.Today)	//do not update connections if they were enumerated today
                    return;

                //Do nothing - .Net Core does not support SqlDataSourceEnumerator
                //var dataSources = SqlDataSourceEnumerator.Instance.GetDataSources();
                //dataSources.WriteXml(SqlDataSourcesFileName, XmlWriteMode.WriteSchema);
            }

            public static List<string> ListSqlServers()
            {
                var result = new List<string>();

                var sqlServersFileName = SqlDataSourcesFileName;

                //Return empty list now and enumerate SQL Server for the next time
                Task.Run(() => EnumerateSqlServers());

                if (!File.Exists(sqlServersFileName))
                    return result;

                try
                {
                    using var tableSources = new DataTable();
                    tableSources.ReadXml(sqlServersFileName);
                    foreach (DataRow rowSource in tableSources.Rows)
                    {
                        var server   = Convert.ToString(rowSource["ServerName"]);
                        var instance = Convert.ToString(rowSource["InstanceName"]);

                        var fullName = !string.IsNullOrWhiteSpace(instance) ? $@"{server}\{instance}" : server;
                        result.Add(fullName);
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        File.Delete(sqlServersFileName);
                    }
                    catch (Exception)
                    {
                        //Do nothing
                    }
                }

                return result;
            }
        }
        #endregion

        public const string keyServer                 = "Server";
        public const string keyDatabase               = "Database";
        public const string keyIntegratedSecurity     = "Integrated Security";
        public const string keyUserID                 = "User ID";
        public const string keyPassword               = "Password";
        public const string keyPersistSecurityInfo    = "Persist security info";
        public const string keyAttachDBFilename       = "AttachDBFilename";
        public const string keyEncrypt                = "Encrypt";
        public const string keyTrustServerCertificate = "Trust Server Certificate";

        private string _Server;
        private string _Database;
        private bool _IntegratedSecurity;
        private string _UserID;
        private string _Password;
        private bool _PersistSecurityInfo;
        private string _AttachDBFilename;
        private bool _Encrypt;
        private bool _TrustServerCertificate;

        public SqlConnectionStringBuilderLight(string connectionString)
        {
            Clear();
            if (!string.IsNullOrEmpty(connectionString))
                ConnectionString = connectionString;
        }

        public SqlConnectionStringBuilderLight() : this(null)
        {
        }

        protected virtual string ConvertKeyword(string keyword)
        {
            if (keyword == null)
                return null;

            keyword             = keyword.Trim();
            string keywordUpper = keyword.ToUpper();
            string result       = keyword;

            switch (keywordUpper)
            {
                case "SERVER":
                case "DATASOURCE":
                case "DATA SOURCE":
                    result = keyServer;
                    break;
                case "DATABASE":
                case "INITIAL CATALOG":
                    result = keyDatabase;
                    break;
                case "INTEGRATED SECURITY":
                case "INTEGRATEDSECURITY":
                case "WINDOWS AUTHENTICATION":
                case "WINDOWSAUTHENTICATION":
                    result = keyIntegratedSecurity;
                    break;
                case "USER ID":
                case "USERID":
                    result = keyUserID;
                    break;
                case "PASSWORD":
                case "PWD":
                case "DATABASE PASSWORD":
                    result = keyPassword;
                    break;
                case "PERSIST SECURITY INFO":
                    result = keyPersistSecurityInfo;
                    break;
                case "ATTACHDBFILENAME":
                case "EXTENDED PROPERTIES":
                case "INITIAL FILE NAME":
                    result = keyAttachDBFilename;
                    break;
                case "ENCRYPT":
                    result = keyEncrypt;
                    break;
                case "TRUSTSERVERCERTIFICATE":
                case "TRUST SERVER CERTIFICATE":
                    result = keyTrustServerCertificate;
                    break;
            }

            return result;
        }

        private static string ValueToString(object value, string defaultValue)
        {
            if (value == null || value.Equals(string.Empty))
                return defaultValue;
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private static int ValueToInt(object value, int defaultValue)
        {
            if (value == null)
                return defaultValue;

            try
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
#pragma warning restore IDE0051 // Remove unused private members

        private static bool ValueToBool(object value, bool defaultValue)
        {
            if (value == null)
                return defaultValue;

            try
            {
                return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        private void UpdateProperty(string keyword, object value)
        {
            switch (keyword)
            {
                case keyServer:
                    Server = ValueToString(value, null);
                    break;
                case keyDatabase:
                    Database = ValueToString(value, null);
                    break;
                case keyIntegratedSecurity:
                    if (string.Compare(Convert.ToString(value), "SSPI", true) == 0)
                        value = true;
                    IntegratedSecurity = ValueToBool(value, false);
                    break;
                case keyUserID:
                    UserID = ValueToString(value, null);
                    break;
                case keyPassword:
                    Password = ValueToString(value, null);
                    break;
                case keyPersistSecurityInfo:
                    PersistSecurityInfo = ValueToBool(value, false);
                    break;
                case keyAttachDBFilename:
                    AttachDBFilename = ValueToString(value, null);
                    break;
                case keyEncrypt:
                    Encrypt = ValueToBool(value, true);
                    break;
                case keyTrustServerCertificate:
                    TrustServerCertificate = ValueToBool(value, false);
                    break;
            }
        }

        private void SetValue(string keyword, object value)
        {
            string strValue = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(strValue))
                base.Remove(keyword);
            else
                base[keyword] = value;
        }

        public override bool Remove(string keyword)
        {
            keyword = ConvertKeyword(keyword);
            if (base.Remove(keyword))
            {
                UpdateProperty(keyword, null);
                return true;
            }
            return false;
        }

        public override bool IsFixedSize
        {
            get { return true; }
        }

        public override ICollection Keys
        {
            get
            {
                var result = new ArrayList()
                {
                    keyServer,
                    keyDatabase,
                    keyIntegratedSecurity,
                    keyUserID,
                    keyPassword,
                    keyPersistSecurityInfo,
                    keyAttachDBFilename,
                    keyEncrypt,
                    keyTrustServerCertificate
                };
                return result;
            }
        }

        public override object this[string keyword]
        {
            get
            {
                keyword = ConvertKeyword(keyword);
                return base[keyword];
            }
            set
            {
                keyword = ConvertKeyword(keyword);
                UpdateProperty(keyword, value);
            }
        }

        public override bool ContainsKey(string keyword)
        {
            if (keyword == null)
                return false;

            keyword = keyword.Trim();
            string keywordUpper = keyword.ToUpper();

            return keywordUpper switch
            {
                "SERVER" or "DATASOURCE" or "DATA SOURCE" or 
                "DATABASE" or "INITIAL CATALOG" or "INTEGRATED SECURITY" or 
                "INTEGRATEDSECURITY" or "WINDOWS AUTHENTICATION" or 
                "WINDOWSAUTHENTICATION" or "USER ID" or "USERID" or 
                "PASSWORD" or "PWD" or "DATABASE PASSWORD" or 
                "PERSIST SECURITY INFO" or "ATTACHDBFILENAME" or 
                "EXTENDED PROPERTIES" or "INITIAL FILE NAME" or 
                "ENCRYPT" or "TRUSTSERVERCERTIFICATE" or "TRUST SERVER CERTIFICATE" => true,
                _ => false,
            };
        }

        public override bool TryGetValue(string keyword, out object value)
        {
            keyword = ConvertKeyword(keyword);

            switch (keyword)
            {
                case keyServer:
                    value = Server;
                    return true;
                case keyDatabase:
                    value = Database;
                    return true;
                case keyIntegratedSecurity:
                    value = IntegratedSecurity;
                    return true;
                case keyUserID:
                    value = IntegratedSecurity ? null : UserID;
                    return true;
                case keyPassword:
                    value = IntegratedSecurity ? null : Password;
                    return true;
                case keyPersistSecurityInfo:
                    value = PersistSecurityInfo;
                    return true;
                case keyAttachDBFilename:
                    value = AttachDBFilename;
                    return true;
                case keyEncrypt:
                    value = Encrypt;
                    return true;
                case keyTrustServerCertificate:
                    value = TrustServerCertificate;
                    return true;
            }

            value = null;
            return false;
        }

        public override void Clear()
        {
            base.Clear();

            Server                 = null;
            Database               = null;
            IntegratedSecurity     = true;
            UserID                 = null;
            Password               = null;
            PersistSecurityInfo    = false;
            AttachDBFilename       = null;
            Encrypt                = true;
            TrustServerCertificate = false;
        }

        [Description("Server name of Microsoft SQL Server")]
        [DisplayName("Server")]
        [Category("Source")]
        [TypeConverter(typeof(ServerConverter))]
        public virtual string Server
        {
            get { return _Server; }
            set { SetValue(keyServer, value); _Server = value; }
        }

        [Description("Database name")]
        [DisplayName("Database")]
        [Category("Source")]
        [TypeConverter(typeof(DatabaseConverter))]
        public virtual string Database
        {
            get { return _Database; }
            set { SetValue(keyDatabase, value); _Database = value; }
        }

        [Description("Use Windows Authentication (true) or SQL authentication (false)")]
        [DisplayName("Integrated Security")]
        [Category("Security")]
        public virtual bool IntegratedSecurity
        {
            get { return _IntegratedSecurity; }
            set { SetValue(keyIntegratedSecurity, value); _IntegratedSecurity = value; }
        }

        [Description("SQL user name. Leave empty if Integrated Security is using")]
        [DisplayName("User ID")]
        [Category("Security")]
        public virtual string UserID
        {
            get { return _UserID; }
            set { SetValue(keyUserID, value); _UserID = value; }
        }

        [Description("SQL password. Leave empty if Integrated Security is using")]
        [DisplayName("Password")]
        [Category("Security")]
        public virtual string Password
        {
            get { return _Password; }
            set { SetValue(keyPassword, value); _Password = value; }
        }

        [Description("When set to false (which is strongly recommended), security-sensitive " +
            "information, such as the password, is not returned as part of the connection " +
            "if the connection is open or has ever been in an open state. " +
            "Resetting the connection string resets all connection string values, " +
            "including the password. The default value is false.")]
        [DisplayName("Persist Security Info")]
        [Category("Security")]
        [DefaultValue(false)]
        public virtual bool PersistSecurityInfo
        {
            get { return _PersistSecurityInfo; }
            set { SetValue(keyPersistSecurityInfo, value); _PersistSecurityInfo = value; }
        }

        [Description("The name of the primary database file, including the full path name of an attachable database. AttachDBFilename is only supported for primary data files with an .mdf extension.\r\n " +
            "If the value of the AttachDBFileName key is specified in the connection string, the database is attached and becomes the default database for the connection.\r\n " +
            "If this key is not specified and if the database was previously attached, the database will not be re-attached.The previously attached database will be used as the default database for the connection.\r\n " +
            "If this key is specified together with the AttachDBFileName key, the value of this key will be used as the alias.However, if the name is already used in another attached database, the connection will fail.\r\n " +
            "The path may be absolute or relative by using the DataDirectory substitution string. If DataDirectory is used, the database file must exist within a subdirectory of the directory pointed to by the substitution string.\r\n " +
            "Note: Remote server, HTTP, and UNC path names are not supported.\r\n " +
            "The database name must be specified with the keyword 'database' (or one of its aliases) as in the following:\r\n " +
            "\"AttachDbFileName=|DataDirectory|\\data\\YourDB.mdf;integrated security=true;database=YourDatabase\"\r\n " +
            "An error will be generated if a log file exists in the same directory as the data file and the 'database' keyword is used when attaching the primary data file.In this case, remove the log file. Once the database is attached, a new log file will be automatically generated based on the physical path.")]
        [DisplayName("Attach DB Filename")]
        [Category("Source")]
        [EditorAttribute(typeof(AttachDBFilenameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual string AttachDBFilename
        {
            get { return _AttachDBFilename; }
            set { SetValue(keyAttachDBFilename, value); _AttachDBFilename = value; }
        }

        [Description("Value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.")]
        [DisplayName("Encrypt")]
        [Category("Security")]
        public virtual bool Encrypt
        {
            get { return _Encrypt; }
            set { SetValue(keyEncrypt, value); _Encrypt = value; }
        }

        [Description("Value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust.")]
        [DisplayName("Trust Server Certificate")]
        [Category("Security")]
        public virtual bool TrustServerCertificate
        {
            get { return _TrustServerCertificate; }
            set { SetValue(keyTrustServerCertificate, value); _TrustServerCertificate = value; }
        }
    }
    #endregion

    #region SqlClientFactoryLight
    public class SqlClientFactoryLight: DbProviderFactory
    {
        public override bool CanCreateDataSourceEnumerator                           => SqlClientFactory.Instance.CanCreateDataSourceEnumerator;
        public override DbCommand CreateCommand()                                    => SqlClientFactory.Instance.CreateCommand();
        public override DbCommandBuilder CreateCommandBuilder()                      => SqlClientFactory.Instance.CreateCommandBuilder();
        public override DbConnection CreateConnection()                              => SqlClientFactory.Instance.CreateConnection();
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()    => new SqlConnectionStringBuilderLight();
        public override DbDataAdapter CreateDataAdapter()                            => SqlClientFactory.Instance.CreateDataAdapter();
        public override DbDataSourceEnumerator CreateDataSourceEnumerator()          => SqlClientFactory.Instance.CreateDataSourceEnumerator();
        public override DbParameter CreateParameter()                                => SqlClientFactory.Instance.CreateParameter();
        //public override CodeAccessPermission CreatePermission(PermissionState state) => SqlClientFactory.Instance.CreatePermission(state);
    }
    #endregion
}
