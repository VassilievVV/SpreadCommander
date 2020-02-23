using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class DbSchemaViewer : BaseForm
    {
        private readonly DbConnection			        	_Connection;
        private readonly string					        	_DatabaseName;
        private readonly string					        	_DefaultSchemaName;
        private readonly List<string>			        	_CollectionNames;
        private Dictionary<string, string>              	_Restrictions;
        private readonly Dictionary<string, List<DataRow>>	_SchemaRestrictions = new Dictionary<string, List<DataRow>>();
    
        public DbSchemaViewer(DbConnection connection, string databaseName)
        {
            InitializeComponent();
            
            _Connection		= connection;
            _DatabaseName	= databaseName;
            
            if (_Connection == null)
                throw new ArgumentException("Connection for schema viewer is not specified");
        }

        public DbSchemaViewer(DbConnection connection, string databaseName, string defaultSchemaName, 
            Dictionary<string, string> restrictions):
            this(connection, databaseName)
        {
            _DefaultSchemaName = defaultSchemaName;
            _Restrictions      = restrictions;
        }

        public DbSchemaViewer(DbConnection connection, string databaseName, string defaultSchemaName,
            List<string> collectionNames, Dictionary<string, string> restrictions):
            this(connection, databaseName)
        {
            _DefaultSchemaName = defaultSchemaName;
            _CollectionNames   = collectionNames;
            _Restrictions      = restrictions;
        }
        
        private bool OpenConnection()
        {
            if (_Connection.State == ConnectionState.Open)
                return true;
        
            try
            {
                _Connection.Open();
                if (!string.IsNullOrEmpty(_DatabaseName))
                    _Connection.ChangeDatabase(_DatabaseName);
                    
                return true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, "Cannot connect to database.", ex.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            
                return false;
            }
        }

        private bool HasRestriction(string collectionName, string restrictionName)
        {
            if (_SchemaRestrictions == null || !_SchemaRestrictions.ContainsKey(collectionName))
                return false;

            try
            {
                List<DataRow> rows = _SchemaRestrictions[collectionName];
                foreach (DataRow row in rows)
                {
                    string collectionRestriction = Convert.ToString(row["RestrictionName"]);
                    if (string.Compare(collectionRestriction, restrictionName, true) == 0)
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        private bool ForceCollection(string collectionName)
        {
            if (_CollectionNames == null)
                return false;

            foreach (string colName in _CollectionNames)
            {
                if (string.Compare(colName, collectionName, true) == 0)
                    return true;
            }

            return false;
        }

        private bool CanShowCollection(string collectionName)
        {
            if (_CollectionNames != null)
                return ForceCollection(collectionName);

            if (_Restrictions == null)
                return (_CollectionNames == null);

            foreach (string key in _Restrictions.Keys)
            {
                if (!HasRestriction(collectionName, key))
                    return false;
            }

            return true;
        }

        private void SchemaViewer_Load(object sender, EventArgs e)
        {
            Initialize(false);
        }

        private void Initialize(bool clearRestrictions)
        {
            if (clearRestrictions)
            {
                _SchemaRestrictions.Clear();
                _Restrictions = null;
            }

            if (!OpenConnection())
                return;
            try
            {
                if (!string.IsNullOrEmpty(_DatabaseName))
                    _Connection.ChangeDatabase(_DatabaseName);
                    
                DataTable tblSchemas  = _Connection.GetSchema();

                try
                {
                    DataTable tblRestrictions = _Connection.GetSchema("Restrictions");
                    foreach (DataRow rowRestriction in tblRestrictions.Rows)
                    {
                        string collectionName = Convert.ToString(rowRestriction["CollectionName"]);

                        List<DataRow> rowsCollection;
                        if (_SchemaRestrictions.ContainsKey(collectionName))
                            rowsCollection = _SchemaRestrictions[collectionName];
                        else
                        {
                            rowsCollection = new List<DataRow>();
                            _SchemaRestrictions[collectionName] = rowsCollection;
                        }
                        rowsCollection.Add(rowRestriction);
                    }
                }
                catch (Exception)
                {					
                }

                int startIndex = -1;

                using (new UsingProcessor(() => listSchemas.BeginUpdate(), () => listSchemas.EndUpdate()))
                {
                    foreach (DataRow row in tblSchemas.Rows)
                    {
                        string collectionName = Convert.ToString(row["CollectionName"]);
                        if (!string.IsNullOrEmpty(collectionName) && CanShowCollection(collectionName))
                        {
                            listSchemas.Items.Add(collectionName);
                            if (startIndex < 0 && !string.IsNullOrEmpty(_DefaultSchemaName) &&
                                string.Compare(collectionName, _DefaultSchemaName, true) == 0)
                                startIndex = listSchemas.Items.Count - 1;
                        }
                    }
                }
                
                if (listSchemas.Items.Count > 0)
                    listSchemas.SelectedIndex = Math.Max(startIndex, 0);
            }
            finally
            {
                _Connection.Close();
            }
        }

        private void ListSchemas_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridSchema.ClearDataSource();
            if (listSchemas.SelectedIndex < 0)
                return;
        
            string schemaName = listSchemas.SelectedItem as string;
            if (string.IsNullOrEmpty(schemaName))
                return;
            
            if (!OpenConnection())
                return;
            try
            {
                if (!string.IsNullOrEmpty(_DatabaseName))
                    _Connection.ChangeDatabase(_DatabaseName);

                List<string> restrictions = new List<string>();
                try
                {
                    if (_Restrictions != null && _SchemaRestrictions != null)
                    {
                        List<DataRow> rowsRestrictions = _SchemaRestrictions.ContainsKey(schemaName) ? _SchemaRestrictions[schemaName] : null;
                        if (rowsRestrictions != null)
                        {
                            foreach (DataRow rowRestriction in rowsRestrictions)
                            {
                                string restrictionName = Convert.ToString(rowRestriction["RestrictionName"]);
                                if (string.IsNullOrWhiteSpace(restrictionName))
                                    break;
                                if (_Restrictions.ContainsKey(restrictionName))
                                    restrictions.Add(_Restrictions[restrictionName]);
                                else
                                    restrictions.Add(null);
                            }
                        }
                    }
                }
                catch (Exception)
                {					
                }

                DataTable tblSchema   = restrictions.Count > 0 ? _Connection.GetSchema(schemaName, restrictions.ToArray()) : _Connection.GetSchema(schemaName);
                gridSchema.AttachDataSource(tblSchema);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, "Cannot show schema", ex.Message, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _Connection.Close();
            }
        }
    }
}