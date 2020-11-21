using DevExpress.Data.Filtering;
using DevExpress.Printing.ExportHelpers;
using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegEx = System.Text.RegularExpressions;
using SpreadCommander.Common.Extensions;
using System.Globalization;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Spreadsheet;
using SpreadCommander.Common.PowerShell.CmdLets;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class BaseScriptEngine
    {
        protected virtual void ExecuteMethodSync(ISynchronizeInvoke sync, Action function)
        {
            if (sync?.InvokeRequired ?? false)
                sync.Invoke(function, Array.Empty<object>());
            else
                function();
        }

        protected virtual void ClearLastParagraph(Document doc)
        {
            using (new UsingProcessor(() => doc.BeginUpdate(), () => doc.EndUpdate()))
            {
                if (doc.Paragraphs.Count > 0)
                {
                    var para = doc.Paragraphs[doc.Paragraphs.Count - 1];
                    doc.Delete(para.Range);
                    doc.Paragraphs.Append();
                }

                if (ApplicationType == ScriptApplicationType.Console)
                {
                    System.Console.CursorLeft = 0;
                    if (System.Console.CursorTop > 0)
                        System.Console.CursorTop--;
                }
            }
        }

        protected virtual void ClearDocument(Document doc)
        {
            using (new UsingProcessor(() => doc.BeginUpdate(), () => doc.EndUpdate()))
            {
                doc.Text = string.Empty;
            }
        }

        protected void ReportErrorSynchronized(Document doc, ISynchronizeInvoke sync, string errorMessage) =>
            ExecuteMethodSync(sync, () => DoReportError(doc, errorMessage));

        //Has to be called from synchronized code
        protected virtual void ReportError(Document doc, Exception ex) =>
            DoReportError(doc, ex.Message);

        protected virtual void DoReportError(Document doc, string errorMessage)
        {
            var range = doc.AppendText($"ERROR: {errorMessage}{Environment.NewLine}");
            var cp    = doc.BeginUpdateCharacters(range);
            try
            {
                cp.Reset();
                cp.ForeColor = Color.Red;
            }
            finally
            {
                doc.EndUpdateCharacters(cp);
            }

            SCCmdlet.WriteErrorToConsole(errorMessage);
        }

        protected void FlushTextBufferSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, StringBuilder buffer)
        {
            if (buffer.Length <= 0)
                return;

            ExecuteMethodSync(sync, () => FlushTextBuffer(doc, output, buffer));
        }

        protected void FlushTextBufferSynchronized(Document doc, ISynchronizeInvoke sync, Color foregroundColor, Color backgroundColor, StringBuilder buffer)
        {
            if (buffer.Length <= 0)
                return;

            ExecuteMethodSync(sync, () => FlushTextBuffer(doc, foregroundColor, backgroundColor, buffer));
        }

        protected virtual void FlushTextBuffer(Document doc, ScriptOutputMessage output, StringBuilder buffer)
        {
            FlushTextBuffer(doc, output.ForegroundColor, output.BackgroundColor, buffer);
        }

        protected virtual void FlushTextBuffer(Document doc, Color foregroundColor, Color backgroundColor, StringBuilder buffer)
        {
            if (buffer.Length <= 0)
                return;

            doc.BeginUpdate();
            try
            {
                var text = buffer.ToString();

                var range = doc.AppendText(text);
                if (foregroundColor != SystemColors.WindowText || backgroundColor != SystemColors.Window)
                {
                    var cp = doc.BeginUpdateCharacters(range);
                    try
                    {
                        if (foregroundColor != SystemColors.WindowText)
                            cp.ForeColor = foregroundColor;
                        if (backgroundColor != SystemColors.Window)
                            cp.BackColor = backgroundColor;
                    }
                    finally
                    {
                        doc.EndUpdateCharacters(cp);
                    }
                }

                if (range != null)
                {
                    doc.CaretPosition = range.End;
                    ScrollToCaret();
                }

                buffer.Clear();

                SCCmdlet.WriteErrorToConsole(text);
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected void AppendDocumentContentSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            ExecuteMethodSync(sync, () => AppendDocumentContent(doc, output, fileName));
        }

        protected virtual void AppendDocumentContent(Document doc, ScriptOutputMessage output, string fileName)
        {
            doc.BeginUpdate();
            try
            {
                fileName = Project.Current.MapPath(fileName);

                doc.AppendDocumentContent(fileName);
                var paragraph = doc.Paragraphs.Append();

                doc.CaretPosition = paragraph.Range.End;
                ScrollToCaret();
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected void AppendImageSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            ExecuteMethodSync(sync, () => AppendImage(doc, output, fileName));
        }

        protected virtual void AppendImage(Document doc, ScriptOutputMessage output, string fileName)
        {
            doc.BeginUpdate();
            try
            {
                fileName = Project.Current.MapPath(fileName);
                doc.Images.Append(DocumentImageSource.FromFile(fileName));
                var paragraph = doc.Paragraphs.Append();

                doc.CaretPosition = paragraph.Range.End;
                ScrollToCaret();
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected void AppendHtmlSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return;

            ExecuteMethodSync(sync, () => AppendHtml(doc, output, html));
        }

        protected virtual void AppendHtml(Document doc, ScriptOutputMessage output, string html)
        {
            doc.BeginUpdate();
            try
            {
                var range = doc.AppendHtmlText(html, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

                doc.CaretPosition = range.End;
                ScrollToCaret();
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected bool _ProcessingOutputQueue;
        protected virtual void StartProcessOutputQueue()
        {
            _ProcessingOutputQueue = true;
        }

        protected virtual void EndProcessOutputQueue()
        {
            _ProcessingOutputQueue = false;
        }

        public virtual void ProcessOutputQueue()
        {
            if (_ProcessingOutputQueue)
                return;

            using (new UsingProcessor(() => StartProcessOutputQueue(), () => EndProcessOutputQueue()))
            {
                var doc = Document;
                if (doc == null)
                    return; //Process queue in next call to ProcessOutputQueue(), when document will be available.

                if (OutputQueue.Count <= 0)
                    return;

                var sync = SynchronizeInvoke;
                DoProcessQueue();


                void DoProcessQueue()
                {
                    var bufferOutput = new StringBuilder(DefaultBufferCapacity);

                    while (OutputQueue.Count > 0)
                    {
                        var output = OutputQueue.Dequeue();
                        if (output == null)
                            continue;

                        //Merge output parts with same formatting
                        if (OutputQueue.Count > 0)
                        {
                            var nextOutput = OutputQueue.Peek();
                            if (nextOutput?.IsFormattingEqual(output) ?? false)
                            {
                                bufferOutput.Append(output.Text);
                                continue;
                            }
                        }

                        bufferOutput.Append(output.Text);
                        if (bufferOutput.Length > 0)
                        {
                            try
                            {
                                ProcessBuffer(doc, sync, output, bufferOutput);
                            }
                            catch (Exception ex)
                            {
                                ReportErrorSynchronized(doc, sync, ex.Message);
                            }
                        }
                    }
                }
            }
        }

        protected virtual void ProcessBuffer(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, StringBuilder buffer)
        {
            FlushTextBufferSynchronized(doc, sync, output, buffer);
        }

        protected virtual string LoadCsvAsHtml(BaseCommand command, string fileName, List<FormatCondition> formatConditions)
        {
            using var workbook = SpreadsheetUtils.CreateWorkbook();
            var ext = Path.GetExtension(fileName)?.ToLower();

            workbook.LoadDocument(fileName, ext == ".txt" ? DocumentFormat.Text : DocumentFormat.Csv);
            workbook.History.IsEnabled = false;

            var sheet = workbook.Sheets[0] as Worksheet;
            var range = sheet.GetDataRange();

            var table = sheet.Tables.Add(range, "Table", true);

            var style = command.GetProperty<string>("style");
            if (!string.IsNullOrWhiteSpace(style))
            {
                var tableStyle = style;
                if (!tableStyle.StartsWith("TableStyle"))
                    tableStyle = $"TableStyle{style}";
                if (Enum.TryParse(tableStyle, out BuiltInTableStyleId tableStyleID))
                    table.Style = workbook.TableStyles[tableStyleID];
                else if (Enum.TryParse(style, out BuiltInStyleId styleID))
                    range.Style = workbook.Styles[styleID];
            }

            var gridData = new GridData();
            gridData.ApplyGridFormatConditions(formatConditions);

            SpreadsheetUtils.ApplyGridFormatting(table, gridData);

            var options = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
            {
                SheetIndex = workbook.Sheets.IndexOf(sheet),
                Range      = range.GetReferenceA1(),
                Encoding   = Encoding.Unicode
            };

            using var stream = new MemoryStream();
            workbook.ExportToHtml(stream, options);
            stream.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(stream, Encoding.UTF8);
            var result = reader.ReadToEnd();
            return result;
        }
    }
}
