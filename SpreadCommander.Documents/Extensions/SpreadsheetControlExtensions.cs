#pragma warning disable CRR0047

using DevExpress.XtraSpreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Spreadsheet;
using SpreadCommander.Documents.Dialogs;
using System.Windows.Forms;
using DevExpress.XtraSpreadsheet.Model;
using DevExpress.Utils;
using DevExpress.Services;
using DevExpress.Portable.Input;

namespace SpreadCommander.Documents.Extensions
{
    public static class SpreadsheetControlExtensions
    {
        public static void InitializeSpreadsheet(this SpreadsheetControl spreadsheetControl)
        {
            SpreadsheetUtils.InitializeWorkbook(spreadsheetControl.Document);
            spreadsheetControl.Options.Behavior.FunctionNameCulture = FunctionNameCulture.English;

            spreadsheetControl.DocumentLoaded += (s, e) =>
                SpreadsheetUtils.InitializeWorkbook(((SpreadsheetControl)s).Document);

            spreadsheetControl.EmptyDocumentCreated += (s, e) =>
                SpreadsheetUtils.InitializeWorkbook(((SpreadsheetControl)s).Document);

            spreadsheetControl.CellBeginEdit    += SpreadRichTextEditForm.SpreadsheetControl_CellBeginEdit;
            spreadsheetControl.PopupMenuShowing += SpreadRichTextEditForm.SpreadsheetControl_PopupMenuShowing;

            spreadsheetControl.KeyDown += SpreadsheetControl_KeyDown;

            var oldMouseHandler = (IMouseHandlerService)spreadsheetControl.GetService(typeof(IMouseHandlerService));
            if (oldMouseHandler != null)
            {
                var newMouseHandler = new SCMouseHandlerService(spreadsheetControl, oldMouseHandler);
                spreadsheetControl.RemoveService(typeof(IMouseHandlerService));
                spreadsheetControl.AddService(typeof(IMouseHandlerService), newMouseHandler);
            }
        }

        private static void SpreadsheetControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Control.IsKeyLocked(Keys.Scroll))
            {
                var spreadsheet = sender as SpreadsheetControl ?? throw new ArgumentException("Sender is not Spreadsheet", nameof(sender));
                if (spreadsheet.ActiveWorksheet != spreadsheet.VisibleRange.Worksheet)
                    return;

                switch (e.KeyData)
                {
                    case Keys.Left:
                        if (spreadsheet.VisibleUnfrozenRange.LeftColumnIndex > 0)
                            spreadsheet.ActiveWorksheet.ScrollToColumn(spreadsheet.VisibleUnfrozenRange.LeftColumnIndex - 1);
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        spreadsheet.ActiveWorksheet.ScrollToColumn(spreadsheet.VisibleUnfrozenRange.LeftColumnIndex + 1);
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        spreadsheet.ActiveWorksheet.ScrollToRow(spreadsheet.VisibleUnfrozenRange.TopRowIndex + 1);
                        e.Handled = true;
                        break;
                    case Keys.Up:
                        if (spreadsheet.VisibleUnfrozenRange.TopRowIndex > 0)
                            spreadsheet.ActiveWorksheet.ScrollToRow(spreadsheet.VisibleUnfrozenRange.TopRowIndex - 1);
                        e.Handled = true;
                        break;
                }
            }
        }
    }

    #region SCMouseHandlerService
    internal class SCMouseHandlerService : MouseHandlerServiceWrapper
    {
        private readonly IServiceProvider provider;

        public SCMouseHandlerService(IServiceProvider provider, IMouseHandlerService service)
            : base(service)
        {
            this.provider = provider;
        }

        public override void OnMouseWheel(PortableMouseEventArgs e)
        {
            if (Control.ModifierKeys.HasFlag(Keys.Shift))
            {
                if (e.Delta == 0)
                    return;
                
                var spreadsheet = provider as SpreadsheetControl ?? throw new ArgumentException("MouseWheel provider is not Spreadsheet", nameof(provider));

                var cellsToMove = e.Delta / 100;
                if (cellsToMove == 0)
                    cellsToMove = Math.Abs(e.Delta); //Scroll to at least 1 column
                    
                if (spreadsheet.ActiveWorksheet == spreadsheet.VisibleRange.Worksheet)
                    spreadsheet.ActiveWorksheet.ScrollToColumn(Math.Max(spreadsheet.VisibleRange.LeftColumnIndex - cellsToMove, 0));

                return;
            }

            base.OnMouseWheel(e);
        }
    }
    #endregion
}
