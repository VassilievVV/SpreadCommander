using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using SpreadCommander.Common.Code;
using DevExpress.Mvvm;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class UnboundExpressionEditor : DevExpress.XtraEditors.XtraForm
    {
        public enum ColumnUnboundType { String, Integer, Decimal, DateTime, Boolean }
        
        #region ComboItem
        public class ComboItem: BindableBase
        {
            public GridColumn Column
            {
                get => GetProperty(() => Column);
                set => SetProperty(() => Column, value);
            }

            public string Name
            {
                get => GetProperty(() => Name);
                set => SetProperty(() => Name, value);
            }

            public UnboundColumnType ColumnType
            {
                get => GetProperty(() => ColumnType);
                set => SetProperty(() => ColumnType, value);
            }

            public ColumnUnboundType UnboundType
            {
                get
                {
                    return ColumnType switch
                    {
                        UnboundColumnType.Bound    => ColumnUnboundType.String,
                        UnboundColumnType.Integer  => ColumnUnboundType.Integer,
                        UnboundColumnType.Decimal  => ColumnUnboundType.Decimal,
                        UnboundColumnType.DateTime => ColumnUnboundType.DateTime,
                        UnboundColumnType.String   => ColumnUnboundType.String,
                        UnboundColumnType.Boolean  => ColumnUnboundType.Boolean,
                        UnboundColumnType.Object   => ColumnUnboundType.String,
                        _                          => ColumnUnboundType.String
                    };
                }
                set
                {
                    switch (value)
                    {
                        case ColumnUnboundType.String:
                            ColumnType = UnboundColumnType.String;
                            break;
                        case ColumnUnboundType.Integer:
                            ColumnType = UnboundColumnType.Integer;
                            break;
                        case ColumnUnboundType.Decimal:
                            ColumnType = UnboundColumnType.Decimal;
                            break;
                        case ColumnUnboundType.DateTime:
                            ColumnType = UnboundColumnType.DateTime;
                            break;
                        case ColumnUnboundType.Boolean:
                            ColumnType = UnboundColumnType.Boolean;
                            break;
                    }

                    RaisePropertyChanged(() => UnboundType);
                }
            }

            public override string ToString()
            {
                return Name;
            }
        }
        #endregion

        private readonly GridView _GridView;
        
        public UnboundExpressionEditor()
        {
            InitializeComponent();
        }

        public UnboundExpressionEditor(GridView gridView) : this()
        {
            _GridView = gridView ?? throw new ArgumentNullException(nameof(gridView), "GridView is not defined");

            foreach (var column in _GridView.Columns.Where(c => c.UnboundType != UnboundColumnType.Bound && c.UnboundType != UnboundColumnType.Object))
            {
                var comboItem = new ComboItem()
                {
                    Column     = column, 
                    Name       = column.Caption,
                    ColumnType = column.UnboundType
                };
                bindingColumns.Add(comboItem);
            }

            if (bindingColumns.Count > 0)
                bindingColumns.Position = 0;
        }
        
        protected void AddColumn()
        {
            var columnNames = _GridView.Columns.Select(c => c.FieldName ?? c.GetTextCaption()).ToList();
            var caption     = Utils.AddUniqueString(columnNames, "Column1", StringComparison.CurrentCultureIgnoreCase, true);

            var newColumn                       = _GridView.Columns.AddVisible(caption);
            newColumn.Name                      = caption;
            newColumn.Caption                   = caption;
            newColumn.UnboundType               = UnboundColumnType.String;
            newColumn.ShowUnboundExpressionMenu = true;
            newColumn.Visible                   = true;
            newColumn.Width                     = 100;
            
            var comboItem = new ComboItem()
            {
                Column     = newColumn, 
                Name       = newColumn.Caption,
                ColumnType = newColumn.UnboundType
            };

            int pos = bindingColumns.Add(comboItem);
            bindingColumns.Position = pos;
        }
        
        protected void DeleteColumn()
        {
            if (bindingColumns.Current is ComboItem column)
            {
                _GridView.Columns.Remove(column.Column);
                bindingColumns.Remove(column);
            }
        }

        private void BtnAddColumn_Click(object sender, EventArgs e)
        {
            AddColumn();
        }

        private void BtnDeleteColumn_Click(object sender, EventArgs e)
        {
            DeleteColumn();
        }

        private void BindingColumns_CurrentChanged(object sender, EventArgs e)
        {
            if (bindingColumns.Current is ComboItem column)
            {
                pnlExpression.StartEdit(column.Column);
                lookUpColumns.EditValue = column;
            }
            else
            {
                pnlExpression.StartEdit(null);
                lookUpColumns.EditValue = null;
            }
        }

        private void PnlExpression_UpdateColumn(object sender, EventArgs e)
        {
            if (bindingColumns.Current is not ComboItem column)
                return;

            column.Name        = txtName.Text;
            column.UnboundType = (ColumnUnboundType)radioType.SelectedIndex;

            column.Column.Caption     = column.Name;
            column.Column.UnboundType = column.ColumnType;
        }

        private void PnlExpression_CancelUpdate(object sender, EventArgs e)
        {
            if (bindingColumns.Current is not ComboItem column)
                return;

            column.Column.Caption     = column.Name;
            column.Column.UnboundType = column.ColumnType;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}