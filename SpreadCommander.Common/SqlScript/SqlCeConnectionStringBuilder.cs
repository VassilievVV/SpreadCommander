using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Reflection;

namespace SpreadCommander.Common.SqlScript
{
    #region SqlCeConnectionStringBuilder
    public class SqlCeConnectionStringBuilder : DbConnectionStringBuilder
    {
        #region DataSourceEditor
        private class DataSourceEditor : System.Windows.Forms.Design.FileNameEditor
        {
            protected override void InitializeDialog(System.Windows.Forms.OpenFileDialog openFileDialog)
            {
                base.InitializeDialog(openFileDialog);

                openFileDialog.Filter           = "MS SQL Server CE database (*.sdf)|*.sdf|All files (*.*)|*.*";
                openFileDialog.Title            = "Select MS SQL Server CE database";
                openFileDialog.DefaultExt       = "sdf";
                openFileDialog.CheckFileExists  = false;
                openFileDialog.CheckPathExists  = true;
                openFileDialog.RestoreDirectory = true;
            }
        }
        #endregion

        #region FileModeConverter
        private class FileModeConverter : TypeConverter
        {
            private StandardValuesCollection _StandardValues;

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
                if (_StandardValues == null)
                {
                    ArrayList strValues = new ArrayList();
                    foreach (string value in SqlCeConnectionStringBuilder.FileModeValues)
                        strValues.Add(value);
                    _StandardValues = new StandardValuesCollection(strValues);
                }

                return _StandardValues;
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

        public const string keyDataSource            = "Data Source";
        public const string keyPassword              = "Password";
        public const string keyEncryptDatabase       = "Encrypt database";
        public const string keyMaxBufferSize         = "Max buffer size";
        public const string keyMaxDatabaseSize       = "Max database size";
        public const string keyMode                  = "Mode";
        public const string keyDefaultLockTimeout    = "Default lock timeout";
        public const string keyDefaultLockEscalation = "Default lock escalation";
        public const string keyFlushInterval         = "Flush interval";
        public const string keyAutoShrinkThreshold   = "AutoShrink threshold";
        public const string keyTempFileDirectory     = "Temp file directory";
        public const string keyTempFileMaxSize       = "Temp file max size";
        public const string keyPersistSecurityInfo   = "Persist security info";
        public const string keyLocaleIdentifier      = "Locale identifier";

        public static readonly string[] FileModeValues = new string[]
        {
            "Read Write",
            "Read Only",
            "Exclusive",
            "Shared Read"
        };
        public const string DefaultFileModeValue = "Read Write";

        private string _DataSource;
        private string _Password;
        private bool _EncryptDatabase;
        private int _MaxBufferSize;
        private int _MaxDatabaseSize;
        private string _Mode;
        private int _DefaultLockTimeout;
        private int _DefaultLockEscalation;
        private int _FlushInterval;
        private int _AutoShrinkThreshold;
        private string _TempFileDirectory;
        private int _TempFileMaxSize;
        private bool _PersistSecurityInfo;
        private CultureInfo _LocaleIdentifier;

        public SqlCeConnectionStringBuilder(string connectionString)
        {
            Clear();
            if (!string.IsNullOrEmpty(connectionString))
                ConnectionString = connectionString;
        }

        public SqlCeConnectionStringBuilder() : this(null)
        {
        }

        protected virtual string ConvertKeyword(string keyword)
        {
            if (keyword == null)
                return null;

            keyword = keyword.Trim();
            string keywordUpper = keyword.ToUpper();
            string result = keyword;

            switch (keywordUpper)
            {
                case "DATASOURCE":
                case "DATA SOURCE":
                    result = keyDataSource;
                    break;
                case "PASSWORD":
                case "PWD":
                case "DATABASE PASSWORD":
                case "SSCE:DATABASE PASSWORD":
                    result = keyPassword;
                    break;
                case "ENCRYPT":
                case "ENCRYPT DATABASE":
                case "SSCE: ENCRYPT DATABASE":
                    result = keyEncryptDatabase;
                    break;
                case "MAX BUFFER SIZE":
                case "SSCE:MAX BUFFER SIZE":
                    result = keyMaxBufferSize;
                    break;
                case "MAX DATABASE SIZE":
                case "SSCE:MAX DATABASE SIZE":
                    result = keyMaxDatabaseSize;
                    break;
                case "MODE":
                case "FILE MODE":
                case "SSCE:MODE":
                    result = keyMode;
                    break;
                case "DEFAULT LOCK TIMEOUT":
                case "SSCE:DEFAULT LOCK TIMEOUT":
                    result = keyDefaultLockTimeout;
                    break;
                case "DEFAULT LOCK ESCALATION":
                case "SSCE:DEFAULT LOCK ESCALATION":
                    result = keyDefaultLockEscalation;
                    break;
                case "FLUSH INTERVAL":
                case "SSCE:FLUSH INTERVAL":
                    result = keyFlushInterval;
                    break;
                case "AutoShrink THRESHOLD":
                case "SSCE:AutoShrink THRESHOLD":
                    result = keyAutoShrinkThreshold;
                    break;
                case "TEMP PATH":
                case "TEMP FILE DIRECTORY":
                case "SSCE:TEMP FILE DIRECTORY":
                    result = keyTempFileDirectory;
                    break;
                case "TEMP FILE MAX SIZE":
                    result = keyTempFileMaxSize;
                    break;
                case "PERSIST SECURITY INFO":
                    result = keyPersistSecurityInfo;
                    break;
                case "LOCALE IDENTIFIER":
                case "LCID":
                case "INITIAL LCID":
                    result = keyLocaleIdentifier;
                    break;
            }

            return result;
        }

        private string ValueToString(object value, string defaultValue)
        {
            if (value == null || value.Equals(string.Empty))
                return defaultValue;
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private int ValueToInt(object value, int defaultValue)
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

        private bool ValueToBool(object value, bool defaultValue)
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
                case keyDataSource:
                    DataSource = ValueToString(value, null);
                    break;
                case keyPassword:
                    Password = ValueToString(value, null);
                    break;
                case keyEncryptDatabase:
                    EncryptDatabase = ValueToBool(value, false);
                    break;
                case keyMaxBufferSize:
                    MaxBufferSize = ValueToInt(value, 640);
                    break;
                case keyMaxDatabaseSize:
                    MaxDatabaseSize = ValueToInt(value, 128);
                    break;
                case keyMode:
                    Mode = ValueToString(value, null);
                    break;
                case keyDefaultLockTimeout:
                    DefaultLockTimeout = ValueToInt(value, 2000);
                    break;
                case keyDefaultLockEscalation:
                    DefaultLockEscalation = ValueToInt(value, 100);
                    break;
                case keyFlushInterval:
                    FlushInterval = ValueToInt(value, 10);
                    break;
                case keyAutoShrinkThreshold:
                    AutoShrinkThreshold = ValueToInt(value, 60);
                    break;
                case keyTempFileDirectory:
                    TempFileDirectory = ValueToString(value, null);
                    break;
                case keyTempFileMaxSize:
                    TempFileMaxSize = ValueToInt(value, 128);
                    break;
                case keyPersistSecurityInfo:
                    PersistSecurityInfo = ValueToBool(value, false);
                    break;
                case keyLocaleIdentifier:
                    if (value is CultureInfo)
                        LocaleIdentifier = (CultureInfo)value;
                    else
                        LocaleIdentifierString = ValueToString(value, null);
                    break;
            }
        }

        private void SetValue(string keyword, object value)
        {
            base[keyword] = Convert.ToString(value, CultureInfo.InvariantCulture);
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
                ArrayList result = new ArrayList() 
                {
                    keyDataSource,
                    keyPassword,
                    keyEncryptDatabase,
                    keyMaxBufferSize,
                    keyMaxDatabaseSize,
                    keyMode,
                    keyDefaultLockTimeout,
                    keyDefaultLockEscalation,
                    keyFlushInterval,
                    keyAutoShrinkThreshold,
                    keyTempFileDirectory,
                    keyTempFileMaxSize,
                    keyPersistSecurityInfo,
                    keyLocaleIdentifier
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

            switch (keywordUpper)
            {
                case "DATASOURCE":
                case "DATA SOURCE":
                case "PASSWORD":
                case "PWD":
                case "DATABASE PASSWORD":
                case "SSCE:DATABASE PASSWORD":
                case "ENCRYPT":
                case "ENCRYPT DATABASE":
                case "SSCE: ENCRYPT DATABASE":
                case "MAX BUFFER SIZE":
                case "SSCE:MAX BUFFER SIZE":
                case "MAX DATABASE SIZE":
                case "SSCE:MAX DATABASE SIZE":
                case "MODE":
                case "FILE MODE":
                case "SSCE:MODE":
                case "DEFAULT LOCK TIMEOUT":
                case "SSCE:DEFAULT LOCK TIMEOUT":
                case "DEFAULT LOCK ESCALATION":
                case "SSCE:DEFAULT LOCK ESCALATION":
                case "FLUSH INTERVAL":
                case "SSCE:FLUSH INTERVAL":
                case "AutoShrink THRESHOLD":
                case "SSCE:AutoShrink THRESHOLD":
                case "TEMP PATH":
                case "TEMP FILE DIRECTORY":
                case "SSCE:TEMP FILE DIRECTORY":
                case "TEMP FILE MAX SIZE":
                case "PERSIST SECURITY INFO":
                case "LOCALE IDENTIFIER":
                case "LCID":
                case "INITIAL LCID":
                    return true;
            }

            return false;
        }

        public override bool TryGetValue(string keyword, out object value)
        {
            keyword = ConvertKeyword(keyword);

            switch (keyword)
            {
                case keyDataSource:
                    value = DataSource;
                    return true;
                case keyPassword:
                    value = Password;
                    return true;
                case keyEncryptDatabase:
                    value = EncryptDatabase;
                    return true;
                case keyMaxBufferSize:
                    value = MaxBufferSize;
                    return true;
                case keyMaxDatabaseSize:
                    value = MaxDatabaseSize;
                    return true;
                case keyMode:
                    value = Mode;
                    return true;
                case keyDefaultLockTimeout:
                    value = DefaultLockTimeout;
                    return true;
                case keyDefaultLockEscalation:
                    value = DefaultLockEscalation;
                    return true;
                case keyFlushInterval:
                    value = FlushInterval;
                    return true;
                case keyAutoShrinkThreshold:
                    value = AutoShrinkThreshold;
                    return true;
                case keyTempFileDirectory:
                    value = TempFileDirectory;
                    return true;
                case keyTempFileMaxSize:
                    value = TempFileMaxSize;
                    return true;
                case keyPersistSecurityInfo:
                    value = PersistSecurityInfo;
                    return true;
                case keyLocaleIdentifier:
                    value = LocaleIdentifier;
                    return true;
            }

            value = null;
            return false;
        }

        public override void Clear()
        {
            base.Clear();

            _DataSource = string.Empty;
            _Password = string.Empty;
            _EncryptDatabase = false;
            _MaxBufferSize = 640;
            _MaxDatabaseSize = 128;
            _Mode = DefaultFileModeValue;
            _DefaultLockTimeout = 2000;
            _DefaultLockEscalation = 100;
            _FlushInterval = 10;
            _AutoShrinkThreshold = 60;
            _TempFileDirectory = string.Empty;
            _TempFileMaxSize = 128;
            _PersistSecurityInfo = false;
            _LocaleIdentifier = CultureInfo.InvariantCulture;
        }

        [Description("The file path and name of the SQL Server Compact Edition database. " +
            "To indicate a relative path to the database from the application directory, " +
            "use the Data Source = |DataDirectory| (enclosed in pipe symbols) " +
            "substitution string. Use the SetData method on the AppDomain object to set " +
            "the application's data directory. DataDirectory is not supported for devices. " +
            "For more information, see \"Installing SQL Server Compact Edition on a Desktop and " +
            "a Tablet PC\" in SQL Server Compact Edition Books Online.")]
        [DisplayName("Data Source")]
        [Category("Source")]
        [EditorAttribute(typeof(SqlCeConnectionStringBuilder.DataSourceEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual string DataSource
        {
            get { return _DataSource; }
            set { SetValue(keyDataSource, value); _DataSource = value; }
        }

        [Description("The database password, which can be up to 40 characters in length. " +
            "If not specified, the default value is no password. This property is required " +
            "if you enable encryption on the database. If you specify a password, " +
            "encryption is automatically enabled on the database.")]
        [DisplayName("Password")]
        [Category("Security")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [PasswordPropertyText(true)]
        public virtual string Password
        {
            get { return _Password; }
            set
            {
                if (value != null && value.Length > 40)
                    value = value.Substring(0, 40);
                SetValue(keyPassword, value);
                _Password = value;
            }
        }

        [Description("A Boolean value that determines whether or not the database is encrypted. " +
            "Must be set to true to enable encryption or false for no encryption. " +
            "If not specified, the default value is false. If you enable encryption, " +
            "you must also specify a password with the password property. " +
            "If you specify a password, encryption is enabled regardless of how you set this property.")]
        [DisplayName("Encrypt database")]
        [Category("Security")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual bool EncryptDatabase
        {
            get { return _EncryptDatabase; }
            set { SetValue(keyEncryptDatabase, value); _EncryptDatabase = value; }
        }

        [Description("The largest amount of memory, in kilobytes, that SQL Server Compact Edition " +
            "can use before it starts flushing changes to disk. If not specified, " +
            "the default value is 640.")]
        [DisplayName("Max buffer size")]
        [DefaultValue(640)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int MaxBufferSize
        {
            get { return _MaxBufferSize; }
            set { SetValue(keyMaxBufferSize, value); _MaxBufferSize = value; }
        }

        [Description("The maximum size of the database, in Megabytes. If not specified, the default value is 128.")]
        [DisplayName("Max database size")]
        [DefaultValue(128)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int MaxDatabaseSize
        {
            get { return _MaxDatabaseSize; }
            set { SetValue(keyMaxDatabaseSize, value); _MaxDatabaseSize = value; }
        }

        [Description("The mode to use when opening the database file. " +
            "If not specified, the default value is 'Read Write'.")]
        [DisplayName("Mode")]
        [Category("Source")]
        [DefaultValue(DefaultFileModeValue)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof(FileModeConverter))]
        public virtual string Mode
        {
            get { return _Mode; }
            set { SetValue(keyMode, value); _Mode = value; }
        }

        [Description("The default number of milliseconds that a transaction will wait for a lock. " +
            "If not specified, the default value is 2000.")]
        [DisplayName("Default lock timeout")]
        [DefaultValue(2000)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int DefaultLockTimeout
        {
            get { return _DefaultLockTimeout; }
            set { SetValue(keyDefaultLockTimeout, value); _DefaultLockTimeout = value; }
        }

        [Description("The number of locks a transaction will acquire before attempting " +
            "escalation from row to page, or from page to table. " +
            "If not specified, the default value is 100.")]
        [DisplayName("Default lock escalation")]
        [DefaultValue(100)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int DefaultLockEscalation
        {
            get { return _DefaultLockEscalation; }
            set { SetValue(keyDefaultLockEscalation, value); _DefaultLockEscalation = value; }
        }

        [Description("Specified the interval time (in seconds) before " +
            "all committed transactions are flushed to disk. " +
            "If not specified, the default value is 10.")]
        [DisplayName("Flush interval")]
        [DefaultValue(10)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int FlushInterval
        {
            get { return _FlushInterval; }
            set { SetValue(keyFlushInterval, value); _FlushInterval = value; }
        }

        [Description("The percent of free space in the database file that is allowed before " +
            "auto-shrink begins. A value of 100 disables auto-shrink. " +
            "If not specified, the default value is 60.")]
        [DisplayName("AutoShrink threshold")]
        [DefaultValue(60)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int AutoShrinkThreshold
        {
            get { return _AutoShrinkThreshold; }
            set { SetValue(keyAutoShrinkThreshold, value); _AutoShrinkThreshold = value; }
        }

        [Description("The location of the temporary database. If not specified, " +
            "the default is to use the database specified in the data source property " +
            "for temporary storage.")]
        [DisplayName("Temp file directory")]
        [Category("Temporary database")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual string TempFileDirectory
        {
            get { return _TempFileDirectory; }
            set { SetValue(keyTempFileDirectory, value); _TempFileDirectory = value; }
        }

        [Description("The maximum size of the temporary database file, in Megabytes. " +
            "If not specified, the default value is 128.")]
        [DisplayName("Temp file max size")]
        [Category("Temporary database")]
        [DefaultValue(128)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int TempFileMaxSize
        {
            get { return _TempFileMaxSize; }
            set { SetValue(keyTempFileMaxSize, value); _TempFileMaxSize = value; }
        }

        [Description("When set to false (which is strongly recommended), security-sensitive " +
            "information, such as the password, is not returned as part of the connection " +
            "if the connection is open or has ever been in an open state. " +
            "Resetting the connection string resets all connection string values, " +
            "including the password. The default value is false.")]
        [DisplayName("Persist security info")]
        [Category("Security")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual bool PersistSecurityInfo
        {
            get { return _PersistSecurityInfo; }
            set { SetValue(keyPersistSecurityInfo, value); _PersistSecurityInfo = value; }
        }

        [Description("The locale ID (LCID) to use with the database.")]
        [DisplayName("Locale identifier")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual CultureInfo LocaleIdentifier
        {
            get { return _LocaleIdentifier; }
            set
            {
                _LocaleIdentifier = value ?? CultureInfo.InvariantCulture;
                SetValue(keyLocaleIdentifier, _LocaleIdentifier.ToString());
            }
        }

#pragma warning disable IDE0052 // Remove unread private members
        private string LocaleIdentifierString
        {
            get { return _LocaleIdentifier.ToString(); }
            set { _LocaleIdentifier = new CultureInfo(value); SetValue(keyLocaleIdentifier, value); }
        }
#pragma warning restore IDE0052 // Remove unread private members
    }
    #endregion
}