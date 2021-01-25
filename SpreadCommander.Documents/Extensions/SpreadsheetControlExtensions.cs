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
using System.Drawing;
using DevExpress.XtraSpreadsheet.Layout;
using System.IO;
using DevExpress.XtraSpreadsheet.Export;
using System.Text.RegularExpressions;

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

            spreadsheetControl.BeforeDragRange   += SpreadsheetControl_BeforeDragRange;
            spreadsheetControl.DragOver          += SpreadsheetControl_DragOver;
            spreadsheetControl.DragDrop          += SpreadsheetControl_DragDrop;

            var oldMouseHandler = (IMouseHandlerService)spreadsheetControl.GetService(typeof(IMouseHandlerService));
            if (oldMouseHandler != null)
                spreadsheetControl.RemoveService(typeof(IMouseHandlerService));
            var newMouseHandler = new SCMouseHandlerService(spreadsheetControl, oldMouseHandler);
            spreadsheetControl.AddService(typeof(IMouseHandlerService), newMouseHandler);
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

        private static string ConvertToHtmlFragment(string html)
        {
            if (string.IsNullOrEmpty(html))
                return null;

            const string MARKER_BLOCK =
@"Version:1.0
StartHTML:{0,8}
EndHTML:{1,8}
StartFragment:{2,8}
EndFragment:{3,8}
StartSelection:{2,8}
EndSelection:{3,8}
SourceURL:{4}
{5}";

            int prefixLength = string.Format(MARKER_BLOCK, 0, 0, 0, 0, string.Empty, string.Empty).Length;

            html = Regex.Replace(html, @"(?i)<body>\s*", "<body><!--StartFragment-->");
            html = Regex.Replace(html, @"(?i)\s*</body>", "<!--EndFragment--></body>");

            int startFragment = prefixLength + html.IndexOf("<!--StartFragment-->") + "<!--StartFragment-->".Length;
            int endFragment   = prefixLength + html.IndexOf("<!--EndFragment-->");

            return string.Format(MARKER_BLOCK, prefixLength, prefixLength + html.Length, startFragment, endFragment, string.Empty, html);
        }

        private static void SpreadsheetControl_BeforeDragRange(object sender, BeforeDragRangeEventArgs e)
        {
            if (Control.ModifierKeys.HasFlag(Keys.Shift) && Control.ModifierKeys.HasFlag(Keys.Control) &&
                Control.ModifierKeys.HasFlag(Keys.Alt))
            {
                e.Cancel = true;

                var spreadsheet = sender as SpreadsheetControl;
                var modified    = spreadsheet.Modified;
                try
                {
                    string txt = null, html = null;

                    spreadsheet.Options.Export.Txt.Range = e.Range.GetReferenceA1();

                    var htmlOptions = new HtmlDocumentExporterOptions()
                    {
                        EmbedImages   = true,
                        Encoding      = Encoding.UTF8,
                        Range         = e.Range.GetReferenceA1(),
                        ExportRootTag = DevExpress.XtraSpreadsheet.Export.Html.ExportRootTag.Html
                    };

                    using (var stream = new MemoryStream())
                    {
                        spreadsheet.Document.SaveDocument(stream, DocumentFormat.Text);
                        stream.Seek(0, SeekOrigin.Begin);
                        txt = spreadsheet.Options.Export.Txt.Encoding.GetString(stream.ToArray());

                        stream.Seek(0, SeekOrigin.Begin);
                        stream.SetLength(0);
                        spreadsheet.Document.ExportToHtml(stream, htmlOptions);
                        stream.Seek(0, SeekOrigin.Begin);
                        html = spreadsheet.Options.Export.Csv.Encoding.GetString(stream.ToArray());
                        html = ConvertToHtmlFragment(html);
                    }

                    var dragData = new DataObject();
                    dragData.SetText(txt, TextDataFormat.Text);
                    dragData.SetText(txt, TextDataFormat.UnicodeText);
                    dragData.SetText(html, TextDataFormat.Html);
                    spreadsheet.DoDragDrop(dragData, DragDropEffects.Copy);
                }
                finally
                {
                    spreadsheet.Options.Export.Txt.Range = null;
                    spreadsheet.Modified = modified;
                }
            }
        }

        private static void SpreadsheetControl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.UnicodeText) ||
                e.Data.GetDataPresent(DataFormats.Text) ||
                e.Data.GetDataPresent(DataFormats.CommaSeparatedValue))
            {
                var spreadsheet = sender as SpreadsheetControl;
                var point       = spreadsheet.PointToClient(new Point(e.X, e.Y));
                var cell        = spreadsheet.GetCellFromPoint(new PointF(point.X, point.Y));

                if (cell != null && spreadsheet.ActiveWorksheet != null)
                {
                    e.Effect = (e.KeyState & 8) == 8 ? DragDropEffects.Copy : DragDropEffects.Move;
                    e.Effect &= e.AllowedEffect;
                }
            }
        }

        private static void SpreadsheetControl_DragDrop(object sender, DragEventArgs e)
        {
            var spreadsheet = sender as SpreadsheetControl;
            var point       = spreadsheet.PointToClient(new Point(e.X, e.Y));
            var cell        = spreadsheet.GetCellFromPoint(new PointF(point.X, point.Y));
            if (cell == null || spreadsheet.ActiveWorksheet == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.UnicodeText) ||
                e.Data.GetDataPresent(DataFormats.Text))
            {
                string text;

                if (e.Data.GetDataPresent(DataFormats.UnicodeText))
                    text = Convert.ToString(e.Data.GetData(DataFormats.UnicodeText));
                else if (e.Data.GetDataPresent(DataFormats.Text))
                    text = Convert.ToString(e.Data.GetData(DataFormats.Text));
                else
                    return;

                if (string.IsNullOrEmpty(text))
                    return;

                using var workbook = SpreadsheetUtils.CreateWorkbook();
                workbook.LoadDocument(workbook.Options.Import.Txt.Encoding.GetBytes(text), DocumentFormat.Text);

                cell.CopyFrom(workbook.Worksheets[0].GetDataRange());
            }
            else if (e.Data.GetDataPresent(DataFormats.CommaSeparatedValue))
            {
#pragma warning disable IDE0019 // Use pattern matching
                using var csv = e.Data.GetData(DataFormats.CommaSeparatedValue) as MemoryStream;
#pragma warning restore IDE0019 // Use pattern matching
                if (csv != null)
                {
                    using var workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.LoadDocument(csv, DocumentFormat.Csv);

                    cell.CopyFrom(workbook.Worksheets[0].GetDataRange());
                }
            }
        }
    }

    #region SCMouseHandlerService
    internal class SCMouseHandlerService : MouseHandlerServiceWrapper
    {
        private readonly IServiceProvider provider;

        public SCMouseHandlerService(IServiceProvider provider, IMouseHandlerService service): base(service)
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
