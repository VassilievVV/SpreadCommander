#pragma warning disable CRR0050

#define PropertyGridEx

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using SpreadCommander.Common;
using DevExpress.XtraLayout.Utils;
using System.IO;
using SpreadCommander.Documents.Code;
using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class DbConnectionEditor : BaseForm
    {
        private readonly string _InitProvider;
        private readonly string _InitConnectionString;
        private readonly List<ConnectionFactory> _InitFactories;

        private DBConnection _CustomConnection;
        private DBConnections _DBConnections;
        private DBConnection _ModifyingConnection;
        //private bool _Modified;

        public DbConnectionEditor()
        {
            using (new WaitCursor())
            {
                InitializeComponent();

#if PropertyGridEx
                PropertyGrid.AutoGenerateRows = true;
#endif

                ApplicationName = Parameters.ApplicationName;
            }
        }

        public DbConnectionEditor(string InitialProvider) : this()
        {
            _InitProvider = InitialProvider;
        }

        public DbConnectionEditor(string InitialProvider, string InitialConnectionString) : this()
        {
            _InitProvider         = InitialProvider;
            _InitConnectionString = InitialConnectionString;
        }

        public DbConnectionEditor(string InitialProvider, string InitialConnectionString,
            List<ConnectionFactory> InitialFactories) : this()
        {
            _InitProvider         = InitialProvider;
            _InitConnectionString = InitialConnectionString;
            _InitFactories        = InitialFactories;
        }

        public string ApplicationName { get; set; }

        public ConnectionFactory SelectedFactory { get; private set; }

        public void LoadConnections()
        {
            bindingConnections.Clear();
            
            _CustomConnection = new DBConnection() { Name = "(Custom connection)", Description = "Custom connection that is not saved" };
            bindingConnections.Add(_CustomConnection);

            _DBConnections = DBConnections.LoadConnections();

            int selectedIndex = 0;
            for (int i = 0; i < _DBConnections.Connections.Count; i++)
            {
                var connection = _DBConnections.Connections[i];

                int index = bindingConnections.Add(connection);

                if (string.Compare(connection.Name, _DBConnections.SelectedConnection, true) == 0)
                    selectedIndex = index;
            }

            if (bindingConnections.Count > 0)
            {
                if (selectedIndex >= 0 && selectedIndex < bindingConnections.Count)
                    bindingConnections.Position = selectedIndex;
                else
                    bindingConnections.Position = 0;
            }
        }

        public void SaveConnections()
        {
            var prevSelectedConnection = _DBConnections?.SelectedConnection;
            
            _DBConnections = new DBConnections();

            foreach (DBConnection connection in bindingConnections)
                if (connection != _CustomConnection)
                    _DBConnections.Connections.Add(connection);

            if (bindingConnections.Current is DBConnection selectedConnection &&
                selectedConnection != _CustomConnection)
                _DBConnections.SelectedConnection = selectedConnection.Name;
            else
                _DBConnections.SelectedConnection = prevSelectedConnection;

            DBConnections.SaveConnections(_DBConnections);
        }

        public string SelectedConnectionName
        {
            get => (bindingConnections.Current as DBConnection)?.Name;
        }
        
        public Connection SelectedConnection
        {
            get
            {
                object sel = PropertyGrid.SelectedObject;
                
                if (sel == null || SelectedFactory == null)
                    return null;
                
                if (sel is DbConnectionStringBuilder selBuilder)
                {
                    var provFactory = SelectedFactory;
                    if (provFactory == null)
                        return null;

                    if (selBuilder.ContainsKey("Application Name"))
                        selBuilder["Application Name"] = ApplicationName;

                    Connection conn = provFactory.CreateConnection(selBuilder.ConnectionString);
                    return conn;
                }
                
                return null;
            }
        }
        
        public string SelectedConnectionString
        {
            get
            {
                object sel = PropertyGrid.SelectedObject;
                
                if (sel == null || SelectedFactory == null)
                    return null;

                if (sel is DbConnectionStringBuilder selBuilder)
                {
                    if (selBuilder.ContainsKey("Application Name"))
                        selBuilder["Application Name"] = ApplicationName;

                    return selBuilder.ConnectionString;
                }
                
                return null;
            }
        }
        
        public string SelectedFactoryName
        {
            get {return SelectedFactory?.Name;}
        }
        
        public string SelectedFactoryInvariantName
        {
            get {return SelectedFactory?.InvariantName;}
        }
        
        public string SelectedFactoryDescription
        {
            get {return SelectedFactory?.Description;}
        }
        
        public bool FixedProvider
        {
            get {return !comboDataSource.Enabled;}
            set {comboDataSource.Enabled = !value;}
        }
        
        public string ServerName
        {
            get {return edName.Text;}
            set {edName.Text = value;}
        }
        
        public string ServerDescription
        {
            get {return edServerDescription.Text;}
            set {edServerDescription.Text = value;}
        }

        private void ConnectionEditor_Load(object sender, EventArgs e)
        {
            List<ConnectionFactory> connFactories = _InitFactories;
            if (connFactories == null || connFactories.Count <= 0)
                connFactories = ConnectionFactory.ListConnectionFactories();

            connFactories.Sort(new ConnectionFactoryComparison());

            foreach (ConnectionFactory factory in connFactories)
                comboDataSource.Properties.Items.Add(factory);
            
            SelectProvider(_InitProvider, _InitConnectionString);

            LoadConnections();

            Invalidate();
        }

        private void ConnectionEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing && e.CloseReason != CloseReason.None)
                return;

            if (_ModifyingConnection != null)
            {
                if (XtraMessageBox.Show(this, "Do you want to update connection?", "Update connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    if (DialogResult != DialogResult.Cancel)
                        e.Cancel = true;
                    return;
                }
                else 
                    UpdateEditingItem();
            }
            //if (_Modified)
                SaveConnections();

            if (DialogResult != DialogResult.OK)
                return;

            Connection conn = SelectedConnection;
            if (conn == null)
            {
                XtraMessageBox.Show(this, "Connection is not configured.",
                    "Please configure connection.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            try
            {
                using (new WaitCursor())
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                if (XtraMessageBox.Show(this, "Cannot connect. Do you want to select another connection?",
                    ex.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        
        private void SelectProvider(string provider, string connStr)
        {
            if (string.IsNullOrEmpty(provider))
                provider = ConnectionFactory.SqlClientFactoryLightName;

            ComboDataSource_EditValueChanged(this, EventArgs.Empty);

            for (int i = 0; i < comboDataSource.Properties.Items.Count; i++)
            {
                ConnectionFactory factory = (ConnectionFactory)comboDataSource.Properties.Items[i];
                if (factory != null && string.Compare(factory.InvariantName, provider, true) == 0)
                {
                    comboDataSource.EditValue = factory;
                    
                    if (!string.IsNullOrEmpty(connStr) &&
                        PropertyGrid.SelectedObject is DbConnectionStringBuilder builder)
                    {
                        builder.ConnectionString			= connStr;
                        PropertyGrid.SelectedObject			= builder;
                        PropertyGrid.Invalidate();
                    }
                    
                    return;
                }
            }

            for (int i = 0; i < comboDataSource.Properties.Items.Count; i++)
            {
                ConnectionFactory factory = (ConnectionFactory)comboDataSource.Properties.Items[i];
                if (factory != null && (string.Compare(factory.Name, provider, true) == 0 || 
                    string.Compare(factory.Description, provider, true) == 0))
                {
                    comboDataSource.EditValue = factory;

                    if (!string.IsNullOrEmpty(connStr) &&
                        PropertyGrid.SelectedObject is DbConnectionStringBuilder builder)
                    {
                        builder.ConnectionString			= connStr;
                        PropertyGrid.SelectedObject			= builder;
                        PropertyGrid.Invalidate();
                    }

                    return;
                }
            }

            XtraMessageBox.Show(this, "Cannot connect", "Cannot open selected connection",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ComboDataSource_EditValueChanged(object sender, EventArgs e)
        {
            PropertyGrid.SelectedObject = null;
            SelectedFactory = null;

            ConnectionFactory connFactory = (ConnectionFactory)comboDataSource.EditValue;
            if (connFactory == null)
                return;
            
            try
            {
                SelectedFactory = connFactory;
                
                DbConnectionStringBuilder builder = null;
                try
                {
                    builder = SelectedFactory.CreateConnectionStringBuilder();
                    builder.BrowsableConnectionString = ConnectionFactory.ShowConnectionString(SelectedFactory.InvariantName);
                    if (!string.IsNullOrEmpty(ApplicationName) && builder.ContainsKey("Application Name"))
                        builder["Application Name"] = ApplicationName;
                }
                catch (Exception)
                {
                    throw;
                }
                
                if (builder == null)
                    builder = new DbConnectionStringBuilder();
                
                PropertyGrid.SelectedObject = builder;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, "Can not connect to specified provider.", ex.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            Connection connection = SelectedConnection;
            if (connection == null)
            {
                XtraMessageBox.Show(this, "Connection is not configured.",
                    "Please select and configure connection to database.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                using (new WaitCursor())
                {
                    connection.Open();
                    connection.Close();
                }

                XtraMessageBox.Show(this, "Connection successful.", 
                    "Connection to selected provider and database is successful.",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, "Connection failed.", ex.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateToolButtons()
        {
            barAdd.Enabled               = true;
            barDelete.Enabled            = bindingConnections.Current != null;
            barUpdate.Enabled            = bindingConnections.Current != null;
            barUp.Enabled                = bindingConnections.Count > 0 && bindingConnections.Position > 0;
            barDockControlBottom.Enabled = bindingConnections.Count > 0 && bindingConnections.Position < bindingConnections.Count - 1;
        }

        private void CheckEditingItem()
        {
            if (_ModifyingConnection != null)
            {
                if (XtraMessageBox.Show(this, "Do you want to save changes?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    UpdateEditingItem();
                _ModifyingConnection = null;
            }
        }

        private string GetUniqueConnectionName(DBConnection connection, string initialName = null)
        {
            if (initialName == null)
                initialName = connection.Name;

            //':' is invalid in names
            initialName = Utils.NonNullString(initialName).Replace(':', ' ');

            if (string.IsNullOrWhiteSpace(initialName))
                initialName = "NewConnection";

            var names = new List<string>();
            foreach (DBConnection conn in bindingConnections)
                if (conn != connection)
                    names.Add(conn.Name);

            var result = Utils.AddUniqueString(names, initialName, StringComparison.CurrentCulture, false);
            return result;
        }

        private void UpdateEditingItem()
        {
            var connection = _ModifyingConnection;
            if (connection == null)
                return;

            connection.Name        = GetUniqueConnectionName(connection, edName.EditValue as string);
            connection.Description = edServerDescription.EditValue as string;
            connection.Provider    = SelectedFactoryInvariantName;

            var connStr                 = SelectedConnectionString;
            connection.ConnectionString = Utils.Encrypt(connStr);

            _ModifyingConnection = null;
            //_Modified            = true;
        }

        private void DisplayItem()
        {
            //Clear
            edName.EditValue              = string.Empty;
            edServerDescription.EditValue = string.Empty;
            comboDataSource.SelectedIndex = 0;
            PropertyGrid.SelectedObject   = null;

            if (!(bindingConnections.Current is DBConnection connection))
                return;

            //Fill with new connection
            edName.EditValue              = connection.Name;
            edServerDescription.EditValue = connection.Description;

            var connStr = connection.ConnectionString;
            SelectProvider(connection.Provider, connStr);

        }

        private void BindingConnections_CurrentChanged(object sender, EventArgs e)
        {
            CheckEditingItem();
            UpdateToolButtons();
            DisplayItem();
        }

        private void BarAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            CheckEditingItem();

            var connection              = new DBConnection();
            connection.Name             = GetUniqueConnectionName(connection);
            int index                   = bindingConnections.Add(connection);
            bindingConnections.Position = index;
        }

        private void BarDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            var index = bindingConnections.Position;
            if (index < 0)
                return;

            var connection = (DBConnection)bindingConnections[index];
            if (connection == _CustomConnection)
            {
                XtraMessageBox.Show(this, "Custom connection cannot be deleted.", "Delete connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (XtraMessageBox.Show(this, "Do you want to delete connection?", "Delete connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            bindingConnections.RemoveAt(index);
            //_Modified = true;
        }

        private void BarUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (bindingConnections.Position < 0)
                return;

            if (XtraMessageBox.Show(this, "Do you want to update connection?", "Update connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _ModifyingConnection = bindingConnections.Current as DBConnection;
            UpdateEditingItem();
        }

        private void BarUp_ItemClick(object sender, ItemClickEventArgs e)
        {
            var index = bindingConnections.Position;
            if (index < 1 || index >= bindingConnections.Count)
                return;

            var connection = bindingConnections.Current;
            if (connection == _CustomConnection)
                return;
            
            bindingConnections.Remove(connection);
            bindingConnections.Insert(index - 1, connection);
            bindingConnections.Position = index - 1;

            //_Modified = true;
        }

        private void BarDown_ItemClick(object sender, ItemClickEventArgs e)
        {
            var index = bindingConnections.Position;
            if (index < 0 || index > bindingConnections.Count - 2)
                return;

            var connection = bindingConnections.Current;
            if (connection == _CustomConnection)
                return;

            bindingConnections.Remove(connection);
            bindingConnections.Insert(index + 1, connection);
            bindingConnections.Position = index + 1;

            //_Modified = true;
        }

        private void PropertyGrid_PropertyValueChanged(object sender, Controls.PropertyGridEx.PropertyValueChangedEventArgs e)
        {
            _ModifyingConnection = bindingConnections.Current as DBConnection;
        }

        private void EdName_EditValueChanged(object sender, EventArgs e)
        {
            dxErrorProvider.ClearErrors();

            if (!(bindingConnections.Current is DBConnection connection))
                return;

            var connectionName = edName.EditValue as string;
            if (string.IsNullOrWhiteSpace(connectionName))
                return;

            if (connectionName.IndexOf(':') >= 0)
            {
                dxErrorProvider.SetError(edName, "Name cannot contain color (':') character", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning);
            }
            else
            {
                var checkName = GetUniqueConnectionName(connection, connectionName);
                if (string.Compare(checkName, connectionName, true) != 0)
                    dxErrorProvider.SetError(edName, "Entered name already exists", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning);
            }
        }

        private void BtnCustomConnectionString_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Tag == null)
                return;
            
            if (Convert.ToString(e.Button.Tag) == "Parse")
            {
                var search = Utils.NonNullString(Convert.ToString(btnCustomConnectionString.EditValue));
                var p = search.IndexOf(':');
                if (p < 0)
                {
                    XtraMessageBox.Show(this, "String to parse shall contain color (':') character, such as 'sqlite:~\\Data\\myDb.sqlite'.",
                        "Invalid parse string", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dbConnection = DBConnections.ParseConnection(search);
                if (dbConnection == null)
                {
                    XtraMessageBox.Show(this, "Cannot parse provided string.",
                        "Invalid parse string", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bindingConnections.Position = bindingConnections.IndexOf(_CustomConnection);
                SelectProvider(dbConnection.Provider, dbConnection.ConnectionString);
            }
        }
    }
}