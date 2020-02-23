using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using SpreadCommander.Common;
using System.Collections;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Documents.Code;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Documents.Viewers;
using SpreadCommander.Documents.Dialogs;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Utils;
using System.Globalization;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Documents.Services;
using System.Data.Common;
using SpreadCommander.Common.SqlScript;
using DevExpress.XtraBars.Ribbon;
using SpreadCommander.Common.Spreadsheet;
using DevExpress.XtraGrid.Extension;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleGridControl : ConsoleCustomControl, IExportSource, IRibbonHolder
    {
        public ConsoleGridControl()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);
        }

        private void ConsoleGridControl_Load(object sender, EventArgs e)
        {
        }

        protected override void DataSetChanged(DataSet oldDataSet, DataSet newDataSet)
        {
            base.DataSetChanged(oldDataSet, newDataSet);
            DataSetView.SetDataSet(newDataSet);
        }

        protected override void ViewModelChanged()
        {
            base.ViewModelChanged();
            DataSetView.TargetSpreadsheet = ViewModel as ISpreadsheetHolder;
        }

        RibbonControl IRibbonHolder.Ribbon            => Ribbon;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => RibbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => Ribbon.Visible;
            set
            {
                Ribbon.Visible          = value;
                RibbonStatusBar.Visible = value;
            }
        }

        public string[] GetTableNames() =>
            DataSetView.GetTableNames();

        public DbDataReader GetDataTable(string tableName) =>
            DataSetView.GetDataTable(tableName);

        private void DataSetView_Modified(object sender, EventArgs e)
        {
            FireModified(false);
        }

        private void DataSetView_RibbonUpdateRequest(object sender, RibbonUpdateRequestEventArgs e)
        {
            Ribbon.UnMergeRibbon();
            RibbonStatusBar.UnMergeStatusBar();

            if (e.RibbonHolder != null && !e.IsFloating)	//leave ribbon on document if it is floating
            {
                if (e.RibbonHolder.Ribbon != null)
                    Ribbon.MergeRibbon(e.RibbonHolder.Ribbon);
                if (e.RibbonHolder.RibbonStatusBar != null)
                    RibbonStatusBar.MergeStatusBar(e.RibbonHolder.RibbonStatusBar);
            }

            FireRibbonUpdateRequest();
        }
    }
}
