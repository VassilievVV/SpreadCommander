using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraVerticalGrid.Events;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraEditors.Repository;
using SpreadCommander.Common;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Documents.Extensions;

namespace SpreadCommander.Documents.Controls
{
    public partial class PropertyGridEx: DevExpress.XtraEditors.XtraUserControl
    {
        #region PropertyValueChangedEventArgs
        public class PropertyValueChangedEventArgs: EventArgs
        {
            private readonly CellValueChangedEventArgs	_args;
            private readonly PropertyDescriptor			_propDescriptor;

            public PropertyValueChangedEventArgs(CellValueChangedEventArgs args,
                PropertyDescriptor propDescriptor)
            {
                _args			= args;
                _propDescriptor = propDescriptor;
            }

            public PropertyDescriptor PropertyDescriptor
            {
                get {return _propDescriptor;}
            }

            public string PropertyName
            {
                get {return _propDescriptor.Name;}
            }

            public Type PropertyType
            {
                get {return _propDescriptor.PropertyType;}
            }

            public object Value 
            {
                get {return _args.Value;}
            }

            public int CellIndex
            {
                get {return _args.CellIndex;}
            }

            public int RecordIndex
            {
                get {return _args.RecordIndex;}
            }

            public BaseRow Row
            {
                get {return _args.Row;}
            }
        }
        #endregion


        public event EventHandler<PropertyValueChangedEventArgs> PropertyValueChanging;
        public event EventHandler<PropertyValueChangedEventArgs> PropertyValueChanged;

        public PropertyGridEx()
        {
            InitializeComponent();
        }

        [DefaultValue(true)]
        public bool ShowDescription
        {
            get {return btnDescription.Down;}
            set {btnDescription.Down = value;}
        }
        
        [DefaultValue(true)]
        public bool ShowCategories
        {
            get {return checkCategories.Checked;}
            set 
            {
                if (value)
                    checkCategories.Checked = true;
                else 
                    checkOrder.Checked = true;
            }
        }

        [DefaultValue(true)]
        public bool ShowButtons
        {
            get {return barTop.Visible;}
            set {barTop.Visible = value;}
        }

        [Browsable(false)]
        public object SelectedObject
        {
            get {return PropertyGridControl.SelectedObject;}
            set 
            {
                PropertyGridControl.SelectedObject = null;
                PropertyGridControl.SelectedObject = value;
            }
        }

        [Browsable(false)]
        public object[] SelectedObjects
        {
            get {return PropertyGridControl.SelectedObjects;}
            set 
            {
                PropertyGridControl.SelectedObjects = null;
                PropertyGridControl.SelectedObjects = value;
            }
        }

        [DefaultValue(true)]
        public bool AutoGenerateRows
        {
            get {return PropertyGridControl.AutoGenerateRows;}
            set {PropertyGridControl.AutoGenerateRows = value;}
        }

        [DefaultValue(true)]
        public bool Editable
        {	get {return PropertyGridControl.OptionsBehavior.Editable;}
            set {PropertyGridControl.OptionsBehavior.Editable = value;}
        }

        public void RetrieveFields()
        {
            PropertyGridControl.RetrieveFields();
        }

        private void PropertyGridEx_Load(object sender, EventArgs e)
        {
            if (!DesignMode && Parent is XtraDialogForm frm)
                frm.MakeXtraDialogFormResizeableWithDelay();
        }

        private void CheckCategories_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PropertyGridControl.OptionsView.ShowRootCategories = checkCategories.Checked;
        }

        private void CheckOrder_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PropertyGridControl.OptionsView.ShowRootCategories = checkCategories.Checked;
        }

        private void BtnDescription_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SplitContainer.PanelVisibility = btnDescription.Down ? SplitPanelVisibility.Both : SplitPanelVisibility.Panel1;
        }

        private void PropertyGridControl_CellValueChanging(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (e.Row != null && (e.Row is EditorRow || e.Row is MultiEditorRow))
                FirePropertyValueChanging(e);
        }

        private void PropertyGridControl_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (e.Row != null && (e.Row is EditorRow || e.Row is MultiEditorRow))
                FirePropertyValueChanged(e);
        }

        private void PropertyGridControl_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
        }

        protected virtual void FirePropertyValueChanging(CellValueChangedEventArgs args)
        {
            PropertyValueChanging?.Invoke(this, new PropertyValueChangedEventArgs(args, PropertyGridControl.GetPropertyDescriptor(args.Row)));
        }

        protected virtual void FirePropertyValueChanged(CellValueChangedEventArgs args)
        {
            PropertyValueChanged?.Invoke(this, new PropertyValueChangedEventArgs(args, PropertyGridControl.GetPropertyDescriptor(args.Row)));
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Printing.PrintControl(this, PropertyGridControl);
        }

        public override Size GetPreferredSize (Size proposedSize)
        {
            const int MinProposedWidth	= 150;
            const int MinProposedHeight = 350;

            return new Size(Math.Max(proposedSize.Width, MinProposedWidth), 
                Math.Max(proposedSize.Height, MinProposedHeight));
        }


        private void EditorCalc_ParseEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        {
            if (e.Value != null && PropertyGridControl.FocusedRow != null)
            {
                try
                {
                    Type valueType = PropertyGridControl.FocusedRow.Properties.RowType;
                    e.Value = Convert.ChangeType(e.Value, valueType);
                }
                catch (Exception)
                {
                    //Do nothing, validation error will be shown.
                }
            }
        }

        private void PropertyGridControl_CustomRecordCellEdit(object sender, GetCustomRowCellEditEventArgs e)
        {
            var rowType = e.Row.Properties.RowType;

            if (rowType == typeof(bool?))
                e.RepositoryItem = editorCheck;
            else if (rowType == typeof(Color?))
                e.RepositoryItem = editorColor;
            else if (rowType == typeof(DateTime?))
                e.RepositoryItem = editorDate;
            else if (rowType == typeof(int?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(long?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(byte?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(short?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(decimal?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(float?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(double?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(ushort?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(uint?))
                e.RepositoryItem = editorCalc;
            else if (rowType == typeof(ulong?))
                e.RepositoryItem = editorCalc;
        }
    }
}