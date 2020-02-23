#pragma warning disable CRR0050

using DevExpress.Data;
using DevExpress.Data.ExpressionEditor;
using DevExpress.DataAccess.UI.ExpressionEditor;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ExpressionEditor;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Internal;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.Columns;
using SpreadCommander.Common.Code;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Controls
{
    [ToolboxItem(true)]
    [DesignerCategory("")]
    public class UnboundExpressionPanel : PanelControl
    {
        private ExpressionEditorView _View;
        private object _Column;
        private IDataColumnInfo _ColumnContext;

        public event EventHandler UpdateColumn;
        public event EventHandler CancelUpdate;

        public UnboundExpressionPanel() : base()
        {
            BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }
        
        private void DestroyExpressionControls()
        {
            if (_View != null)
            {
                Controls.Remove(_View);

                _View.Ok     -= ExpressionView_Ok;
                _View.Cancel -= ExpressionView_Cancel;

                _View.Dispose();
                _View = null;
            }
        }

        private ExpressionEditorView CreateExpressionControl()
        {
            var control = new ExpressionEditorControl();

            if (_Column is GridColumn gridColumn)
                _ColumnContext = new GridColumnIDataColumnInfoWrapper(gridColumn, GridColumnIDataColumnInfoWrapperEnum.ExpressionEditor);
            if (_Column is TreeListColumn)
                _ColumnContext = _Column as IDataColumnInfo;
            control.Context = ExpressionEditorHelper.GetExpressionEditorContext(LookAndFeel, _ColumnContext);

            var expressionView = new ExpressionEditorView(control.LookAndFeel, control)
            {
                Dock            = DockStyle.Fill,
                TopLevel        = false,
                FormBorderStyle = FormBorderStyle.None
            };

            expressionView.Ok     += ExpressionView_Ok;
            expressionView.Cancel += ExpressionView_Cancel;
            expressionView.Visible = true;

            if (FindChildControl(expressionView, "okButton") is SimpleButton btnOK)
            {
                btnOK.Text    = "Commit";
                btnOK.ToolTip = "Save changes for editing column.";
            }
            if (FindChildControl(expressionView, "cancelButton") is SimpleButton btnCancel)
            {
                btnCancel.Text    = "Rollback";
                btnCancel.ToolTip = "Rollback changes for editing column.";
            }

            return expressionView;


            static Control FindChildControl(Control parent, string controlName)
            {
                foreach (Control child in parent.Controls)
                {
                    if (child.Name == controlName)
                        return child;

                    var grandChild = FindChildControl(child, controlName);
                    if (grandChild != null)
                        return grandChild;
                }

                return null;
            }
        }
        
        public void StartEdit(object columnObject)
        {
            using (new UsingProcessor(
                () =>
                {
                    WinAPI.LockWindowUpdate(this.Handle);
                    SuspendLayout();
                }, 
                () =>
                {
                    ResumeLayout();
                    WinAPI.LockWindowUpdate(IntPtr.Zero);
                }))
            {
                _Column = columnObject;
                DestroyExpressionControls();

                _View = CreateExpressionControl();
                Controls.Add(_View);

                string expression = string.Empty;
                if (_Column is GridColumn)
                    expression = (_Column as GridColumn).UnboundExpression;
                if (_Column is TreeListColumn)
                    expression = (_Column as TreeListColumn).UnboundExpression;

                _View.ExpressionString = UnboundExpressionConvertHelper.ConvertToCaption(_ColumnContext, expression);
            }
        }

        protected void FireUpdateColumn() =>
            UpdateColumn?.Invoke(this, new EventArgs());

        protected void FireCancelUpdate() =>
            CancelUpdate?.Invoke(this, new EventArgs());

        private void ExpressionView_Ok(object sender, EventArgs e)
        {
            string expression = UnboundExpressionConvertHelper.ConvertToFields(_ColumnContext, _View.ExpressionString);
            if (_Column is GridColumn gridColumn)
                gridColumn.UnboundExpression = expression;
            if (_Column is TreeListColumn treeListColumn)
                treeListColumn.UnboundExpression = expression;

            FireUpdateColumn();
        }
        
        private void ExpressionView_Cancel(object sender, EventArgs e)
        {
            if (_Column != null)
                StartEdit(_Column);

            FireCancelUpdate();
        }
    }
}
