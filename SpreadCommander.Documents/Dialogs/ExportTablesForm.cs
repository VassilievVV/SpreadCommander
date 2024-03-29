﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.Code;
using System.Data.Common;
using System.Threading;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Code.Exporters;
using DevExpress.Mvvm;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class ExportTablesForm : BaseForm
    {
        #region ExportTable
        public class ExportTable: BindableBase
        {
            public bool Selected
            {
                get => GetProperty(() => Selected);
                set => SetProperty(() => Selected, value);
            }
            
            public string TableName
            {
                get => GetProperty(() => TableName);
                set => SetProperty(() => TableName, value);
            }

            public string NewTableSchema
            {
                get => GetProperty(() => NewTableSchema);
                set => SetProperty(() => NewTableSchema, value);
            }

            public string NewTableName
            {
                get => GetProperty(() => NewTableName);
                set => SetProperty(() => NewTableName, value);
            }
        }
        #endregion

        #region ExportProcessor
        public class ExportProcessor
        {
            public DbExporter Exporter              { get; set; }

            public object ConnectionStringBuilder   { get; set; }

            public DbConnection Connection          { get; set; }

            public string Text => Exporter?.Name;

            public override string ToString() => Exporter?.Name;
        }
        #endregion

        private CancellationTokenSource _CancelScriptTokenSource;
        private IExportSource _ExportSource;
        private DBConnection _CustomConnection;

        public ExportTablesForm()
        {
            InitializeComponent();

            LoadConnections();

            this.Disposed += ExportTablesForm_Disposed;
        }

        private void ExportTablesForm_Disposed(object sender, EventArgs e)
        {
            _CancelScriptTokenSource?.Dispose();
        }

        public void LoadConnections()
        {
            bindingConnections.Clear();

            _CustomConnection = new DBConnection() { Name = "(Custom connection)", Description = "Use connection string in top edit box." };
            bindingConnections.Add(_CustomConnection);

            var connections = DBConnections.LoadConnections();

            int selectedIndex = 0;
            foreach (var connection in connections.Connections)
            {
                var exporter = DbExporter.GetDbExporter(connection.Provider);
                if (exporter == null)
                    continue;

                int index = bindingConnections.Add(connection);

                if (string.Compare(connection.Name, connections.SelectedConnection, true) == 0)
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

        public void SetExportSource(IExportSource source)
        {
            bindingTables.Clear();

            _ExportSource = source;

            var tableNames    = source.GetTableNames();
            var newTableNames = GenerateNewTableNames();

            for (int i = 0; i < tableNames.Length; i++)
            {
                var exportTable = new ExportTable() { TableName = tableNames[i], NewTableName = newTableNames[i], Selected = false };
                bindingTables.Add(exportTable);
            }


            string[] GenerateNewTableNames()
            {
                var newNames = new List<string>();
                int counter = 1;

                foreach (var tableName in tableNames)
                {
                    if (string.IsNullOrWhiteSpace(tableName))
                    { 
                        newNames.Add($"Table{counter++}");
                        continue;
                    }

                    int p = tableName.IndexOf('!');
                    if (p >= 0 && p < tableName.Length - 1)
                    {
                        var newName = tableName[(p + 1)..];
                        if (string.IsNullOrWhiteSpace(newName))
                            newNames.Add($"Table{counter++}");
                        else
                            newNames.Add(newName);
                    }
                    else if (p >= 0)
                        newNames.Add(tableName[0..p]);
                    else
                        newNames.Add(tableName);
                }

                var result = new List<string>();

                foreach (var tableName in newNames)
                {
                    if (!Utils.ContainsString(result, tableName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        //result.Add(Utils.QuoteStringIfNeeded(tableName, "\"", true));
                        result.Add(tableName);
                        continue;
                    }

                    var newTableName = Utils.AddUniqueString(result, $"{tableName}_1", StringComparison.CurrentCultureIgnoreCase, false);
                    //newTableName     = Utils.QuoteStringIfNeeded(newTableName, "\"", true);
                    result.Add(newTableName);
                }

                return result.ToArray();
            }
        }

        private void EnableButtons(bool enable)
        {
            btnTestConnection.Enabled = enable;
            btnExport.Enabled         = enable;
        }

        private async void BtnTestConnection_Click(object sender, EventArgs e)
        {
            if (bindingConnections.Current is not DBConnection connection)
            {
                XtraMessageBox.Show(this, "Please select connection", "Connection is not selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var exporter = DbExporter.GetDbExporter(connection.Provider);
            if (exporter == null)
            {
                XtraMessageBox.Show(this, "Select and configure connection", "Connection is not selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var connectionStringBuilder = connection.GetConnectionFactory()?.CreateConnectionStringBuilder();
            if (connectionStringBuilder == null)
            {
                XtraMessageBox.Show(this, "Cannot create connection string builder", "Cannot create connection string builder",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_CancelScriptTokenSource != null)
            {
                XtraMessageBox.Show(this, "Cancel previous operation before starting new one", "Previous operation is active",

                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableButtons(false);
            _CancelScriptTokenSource = new CancellationTokenSource();
            try
            {
                var conn = exporter.CreateConnection(connectionStringBuilder);
                try
                {
                    Connection.SetConnectionString(conn, connection.ConnectionString);
                    await conn.OpenAsync(_CancelScriptTokenSource.Token).ConfigureAwait(true);
                }
                finally
                {
                    conn.Close();
                }

                XtraMessageBox.Show(this, "Connection is successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, ex.Message, "Cannot connect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                _CancelScriptTokenSource.Dispose();
                _CancelScriptTokenSource = null;

                EnableButtons(true);
            }
        }

        private async void BtnExport_Click(object sender, EventArgs e)
        {
            if (bindingConnections.Current is not DBConnection connection)
            {
                XtraMessageBox.Show(this, "Please select connection", "Connection is not selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var exporter = DbExporter.GetDbExporter(connection.Provider);
            if (exporter == null)
            {
                XtraMessageBox.Show(this, "Select and configure connection", "Connection is not selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var connectionStringBuilder = connection.GetConnectionFactory()?.CreateConnectionStringBuilder();
            if (connectionStringBuilder == null)
            {
                XtraMessageBox.Show(this, "Cannot create connection string builder", "Cannot create connection string builder",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_CancelScriptTokenSource != null)
            {
                XtraMessageBox.Show(this, "Cancel previous operation before starting new one", "Previous operation is active", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            memoLog.Text = string.Empty;

            var prefix = textTableNamePrefix.Text;

            var exportTables = new List<ExportTable>();
            foreach (ExportTable exportTable in bindingTables)
                exportTables.Add(exportTable);

            EnableButtons(false);
            var conn = exporter.CreateConnection(connectionStringBuilder);
            _CancelScriptTokenSource = new CancellationTokenSource();
            try
            {
                Connection.SetConnectionString(conn, connection.ConnectionString);
                await conn.OpenAsync(_CancelScriptTokenSource.Token).ConfigureAwait(true);

                bool hasErrors = false;

                gridTableNames.Enabled = false;
                Cursor                 = Cursors.WaitCursor;
                exporter.Progress     += Exporter_Progress;
                try
                {
                    await Task.Run(() =>
                    {
                        foreach (var table in exportTables)
                        {
                            if (!table.Selected)
                                continue;

                            try
                            {
                                var dataTable = _ExportSource.GetDataTable(table.TableName);
                                if (dataTable != null)
                                    exporter.ExportDataTable(conn, dataTable, table.NewTableSchema, prefix + table.NewTableName, true, _CancelScriptTokenSource.Token);
                                else
                                    throw new Exception("Cannot load source table");

                                ReportMessage($"Table '{table.NewTableName}' is exported successfully.");
                            }
                            catch (Exception ex)
                            {
                                ReportMessage($"Table '{table.NewTableName}' failed: {ex.Message}");
                                hasErrors = true;
                            }
                        }
                    }, _CancelScriptTokenSource.Token).ConfigureAwait(true);
                }
                finally
                {
                    exporter.Progress     -= Exporter_Progress;
                    gridTableNames.Enabled = true;
                    Cursor                 = Cursors.Default;

                    conn.Close();
                }

                if (hasErrors)
                    XtraMessageBox.Show(this, "Export finished with errors. Please review log", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    XtraMessageBox.Show(this, "Tables are exported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, ex.Message, "Cannot export tables", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _CancelScriptTokenSource.Dispose();
                _CancelScriptTokenSource = null;

                conn.Close();
                conn.Dispose();

                EnableButtons(true);
            }
        }

        private void Exporter_Progress(object sender, DbExporter.ExportProgressEventArgs e)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                progressBar.Properties.Maximum = (int)e.Max;
                progressBar.Position           = (int)e.Progress;
            }));
        }

        private void ReportMessage(string message)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                memoLog.Text += message + Environment.NewLine;
            }));
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var cancel = _CancelScriptTokenSource;
            if (cancel != null)
                cancel.Cancel();
            else
                Close();
        }

        private void TextDefaultSchema_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var newSchema = textDefaultSchema.Text;

            using (new UsingProcessor(() => viewTableNames.BeginUpdate(), () => viewTableNames.EndUpdate()))
            {
                foreach (ExportTable tbl in bindingTables)
                    tbl.NewTableSchema = newSchema;
            }
        }

        private void BtnCustomConnectionString_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Tag == null)
                return;

            var tag = Convert.ToString(e.Button.Tag);
            if (tag == "Parse")
            {
                var search = Utils.NonNullString(Convert.ToString(btnCustomConnectionString.EditValue));
                var p = search.IndexOf(':');
                if (p < 0)
                {
                    XtraMessageBox.Show(this, "String to parse shall contain color (':') character, such as 'sqlite:~#\\Data\\myDb.sqlite'.",
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

                _CustomConnection.Provider            = dbConnection.Provider;
                _CustomConnection.EncConnectionString = dbConnection.EncConnectionString;

                bindingConnections.Position = bindingConnections.IndexOf(_CustomConnection);
            }
            else if (tag == "Edit")
            {
                SelectConnection();
            }
        }

        private void SelectConnection()
        {
            string selectedConnectionName = null;

            using (var frm = new DbConnectionEditor())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    selectedConnectionName = frm.SelectedConnectionName;
            }

            LoadConnections();

            if (!string.IsNullOrWhiteSpace(selectedConnectionName))
            {
                for (int i = 0; i < bindingConnections.Count; i++)
                {
                    var conn = bindingConnections[i] as DBConnection;
                    if (string.Compare(conn?.Name, selectedConnectionName, true) == 0)
                    {
                        bindingConnections.Position = i;
                        break;
                    }
                }
            }
        }
    }
}