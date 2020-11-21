using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SpreadCommander.Common.Code
{
    public enum RibbonCommandLayout { Classic, Compact }
    public enum SpreadsheetCulture  { Default, Invariant, OS }

    [DataContract(Namespace = Parameters.ApplicationNamespace)]
    public class ApplicationSettings
    {
        #region MSSQLSettings
        [DataContract(Namespace = Parameters.ApplicationNamespace)]
        [DefaultProperty("DefaultServer")]
        public class MSSQLSettings
        {
            [DataMember()]
            [Category("SQL Server")]
            [DisplayName("Default Server")]
            [Description("Default server name. This value will be used when server is not specified in SpreadCommander cmdlets.")]
            public string DefaultServer { get; set; }

            [DataMember()]
            [Category("SQL Server")]
            [DisplayName("Default User ID")]
            [Description("Default User ID. This value will be used when server is not specified in SpreadCommander cmdlets and when DefaultServer is used.")]
            public string DefaultUserID { get; set; }

            [Category("SQL Server")]
            [DisplayName("Default Password")]
            [Description("Default Password. This value will be used when server is not specified in SpreadCommander cmdlets and when DefaultServer is used.")]
            [PasswordPropertyText(true)]
            [XmlIgnore()]
            [JsonIgnore()]
            public string DefaultPassword
            {
                get
                {
                    var encPassword = EncPassword;
                    if (string.IsNullOrWhiteSpace(encPassword))
                        return null;
                    return Utils.UnProtectUserString(encPassword);
                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        EncPassword = null;
                    else
                        EncPassword = Utils.ProtectUserString(value);
                }
            }

            [DataMember()]
            [Browsable(false)]
            [PasswordPropertyText(true)]
            public string EncPassword { get; set; }

#pragma warning disable CA1822 // Mark members as static
            public bool ShouldSerializePassword() => false;
#pragma warning restore CA1822 // Mark members as static

            public override string ToString()
            {
                return $"Server={DefaultServer};User ID = {DefaultUserID};";
            }
        }
        #endregion

        #region MySQLSettings
        [DataContract(Namespace = Parameters.ApplicationNamespace)]
        [DefaultProperty("DefaultServer")]
        public class MySQLSettings
        {
            [DataMember()]
            [Category("MySQL Server")]
            [DisplayName("Default Server")]
            [Description("Default server name. This value will be used when server is not specified in SpreadCommander cmdlets.")]
            public string DefaultServer { get; set; }

            [DataMember()]
            [Category("MySQL Server")]
            [DisplayName("Default port")]
            [Description("Default server port. This value will be used when server is not specified in SpreadCommander cmdlets.")]
            public uint? DefaultPort { get; set; }

            [DataMember()]
            [Category("MySQL Server")]
            [DisplayName("Default SSL mode")]
            [Description("Default SSL mode. This value will be used when server is not specified in SpreadCommander cmdlets and when DefaultServer is used.")]
            public MySqlConnector.MySqlSslMode? DefaultSslMode { get; set; }

            [DataMember()]
            [Category("SQL Server")]
            [DisplayName("Default User ID")]
            [Description("Default User ID. This value will be used when server is not specified in SpreadCommander cmdlets and when DefaultServer is used.")]
            public string DefaultUserID { get; set; }

            [Category("SQL Server")]
            [DisplayName("Default Password")]
            [Description("Default Password. This value will be used when server is not specified in SpreadCommander cmdlets and when DefaultServer is used.")]
            [PasswordPropertyText(true)]
            [XmlIgnore()]
            [JsonIgnore()]
            public string DefaultPassword
            {
                get
                {
                    var encPassword = EncPassword;
                    if (string.IsNullOrWhiteSpace(encPassword))
                        return null;
                    return Utils.UnProtectUserString(encPassword);
                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        EncPassword = null;
                    else
                        EncPassword = Utils.ProtectUserString(value);
                }
            }

            [DataMember()]
            [Browsable(false)]
            [PasswordPropertyText(true)]
            public string EncPassword { get; set; }

#pragma warning disable CA1822 // Mark members as static
            public bool ShouldSerializePassword() => false;
#pragma warning restore CA1822 // Mark members as static

            public override string ToString()
            {
                return $"Server={DefaultServer};User ID = {DefaultUserID};";
            }
        }
        #endregion

        public static ApplicationSettings Default { get; }

        static ApplicationSettings()
        {
            Default = LoadSettings();
        }

        public static string SettingsFileName => Path.Combine(Parameters.ApplicationDataFolder, "AppSettings.xml");

        private static ApplicationSettings LoadSettings()
        {
            try
            {
                var fileName = SettingsFileName;

                if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
                {
                    var settings = Utils.DeserializeObjectFromFile<ApplicationSettings>(fileName);
                    return settings;
                }
                else
                    return new ApplicationSettings();
            }
            catch (Exception)
            {
                return new ApplicationSettings();
            }
        }

        public static void SaveSettings(ApplicationSettings settings)
        {
            var fileName = SettingsFileName;
            Utils.SerializeObjectToFile<ApplicationSettings>(settings, fileName);
        }

        public void SaveSettings() => SaveSettings(this);


        [DataMember()]
        [Category("Layout")]
        [DisplayName("Command layout")]
        [Description("Ribbon's command layout")]
        [DefaultValue(RibbonCommandLayout.Compact)]
        public RibbonCommandLayout RibbonCommandLayout { get; set; } = RibbonCommandLayout.Compact;

        [DataMember()]
        [Category("Spreadsheet")]
        [DisplayName("Culture")]
        [Description("Culture used in spreadsheet")]
        [DefaultValue(SpreadsheetCulture.Default)]
        public SpreadsheetCulture SpreadsheetCulture { get; set; } = SpreadsheetCulture.Default;

        [DataMember()]
        [Category("Appearance")]
        [DisplayName("Dialog opacity")]
        [Description("Opacity of dialogs")]
        [DefaultValue(100)]
        public int DialogOpacity { get; set; } = 100;

        private MSSQLSettings _MSSqlServerSettings;
        [DataMember()]
        [Category("Databases")]
        [DisplayName("MS SQL Server")]
        [Description("Settings for Microsoft SQL Server connections")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MSSQLSettings MSSQLServer
        {
            get
            {
                if (_MSSqlServerSettings == null)
                    _MSSqlServerSettings = new MSSQLSettings();
                return _MSSqlServerSettings;
            }
            set
            {
                _MSSqlServerSettings = value;
            }
        }

        private MySQLSettings _MySqlServerSettings;
        [DataMember()]
        [Category("Databases")]
        [DisplayName("MySQL Server")]
        [Description("Settings for Microsoft SQL Server connections")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MySQLSettings MySQLServer
        {
            get
            {
                if (_MySqlServerSettings == null)
                    _MySqlServerSettings = new MySQLSettings();
                return _MySqlServerSettings;
            }
            set
            {
                _MySqlServerSettings = value;
            }
        }

        [DataMember()]
        [Category("Optimization")]
        [DisplayName("Optimize AMD math")]
        [Description("Whether to optimize MKL for AMD CPUs")]
        public bool OptimizeAmdMath { get; set; }

        [DataMember()]
        [Category("Projects")]
        [DisplayName("Default location")]
        [Description("Location of the default project.")]
        public string DefaultProjectLocation { get; set; }
    }
}
