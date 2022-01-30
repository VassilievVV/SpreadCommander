using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Book;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public partial class SCBook : ScriptHostObject, IDisposable
    {
        protected InternalBook CommonBook { get; private set; }

        protected internal Document Document => CommonBook.Document;

        public SCBook(): base()
        {
            CommonBook = new InternalBook();
        }

        public SCBook(ScriptHost host): base(host)
        {
            CommonBook = new InternalBook(host.Engine.BookServer);
        }

        public void Dispose()
        {
            CommonBook?.Dispose();
            CommonBook = null;
        }

        protected override bool NeedSynchronization(BookOptions options)
        {
            bool result = base.NeedSynchronization(options);
            
            if (options is SpreadTableOptions spreadOptions)
                result |= (spreadOptions._Spreadsheet != null && spreadOptions._Spreadsheet == Host?.Spreadsheet) ||
                    (spreadOptions._Spreadsheet == null && string.IsNullOrWhiteSpace(spreadOptions._FileName));
            
            return result;
        }

        protected internal static void ResetBookFormatting(Document book, DocumentPosition pos)
        {
            var cp = book.BeginUpdateCharacters(pos, 0);
            try
            {
                cp.Reset();
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }
        }

        protected internal static void ResetBookFormatting(Document book) =>
            ResetBookFormatting(book, book.Range.End);

        protected static void FormatText(Document book, int start, int length)
        {
            var cp = book.BeginUpdateCharacters(start, length);
            try
            {
                cp.Reset();
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }
        }

        protected static void FormatText(Document book, DocumentRange range)
        {
            var cp = book.BeginUpdateCharacters(range);
            try
            {
                cp.Reset();
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }
        }

        protected internal void ScrollToCaret()
        {
            Host?.Engine?.ScrollToCaret();
        }

        protected internal static void AddComments(Document book, DocumentRange range, CommentOptions options)
        {
            if (options == null)
                return;

            if (!string.IsNullOrWhiteSpace(options.Comment))
            {
                var bookComment = book.Comments.Create(range, Environment.UserName);
                var docComment = bookComment.BeginUpdate();
                try
                {
                    if (options.CommentHtml)
                        docComment.AppendHtmlText(options.Comment, InsertOptions.KeepSourceFormatting);
                    else
                        docComment.AppendText(options.Comment);
                }
                finally
                {
                    bookComment.EndUpdate(docComment);
                }
            }

            if (!string.IsNullOrWhiteSpace(options.Bookmark))
            {
                var oldBookmark = book.Bookmarks[options.Bookmark];
                if (oldBookmark != null)
                    book.Bookmarks.Remove(oldBookmark);

                book.Bookmarks.Create(range, options.Bookmark);
            }

            if (!string.IsNullOrWhiteSpace(options.Hyperlink))
            {
                var hyperlinkBookmark = book.Bookmarks[options.Hyperlink];
                var link = book.Hyperlinks.Create(range);
                if (hyperlinkBookmark != null)
                    link.Anchor = hyperlinkBookmark.Name;
                else
                    link.NavigateUri = options.Hyperlink;

                if (!string.IsNullOrWhiteSpace(options.HyperlinkTooltip))
                    link.ToolTip = options.HyperlinkTooltip;

                if (!string.IsNullOrWhiteSpace(options.HyperlinkTarget))
                    link.Target = options.HyperlinkTarget;
            }
        }

        protected internal static void ExpandFieldsInBookRange(DocumentRange range, IWorkbook defaultSpreadsheet, Hashtable snippets = null)
        {
            var doc = range.BeginUpdateDocument();
            try
            {
                var fieldRanges = doc.FindAll(new Regex("(?:{{|(?:{(?:}}|[^}])*}(?!})))"), range);
                if (fieldRanges == null || fieldRanges.Length <= 0)
                    return;

                foreach (var fieldRange in fieldRanges)
                {
                    var fieldText = doc.GetText(fieldRange);
                    if (fieldText == "{{")
                    {
                        doc.Replace(fieldRange, "{");
                        continue;
                    }

                    var code = Utils.TrimString(UnpackField(fieldText));

                    if (code.StartsWith("#"))   //Fields starting with # - #FILE, #IMAGE, #LATEX etc. - calculate field and insert its content instead of field itself
                    {
                        var codeName = Regex.Match(code, @"(?<=^#)[\w_\-]+").Value;

                        using var book = new InternalBook()
                        {
                            DefaultSpreadsheet                = defaultSpreadsheet,
                            NeedSynchronizeDefaultSpreadsheet = true,
                            Snippets                          = ConvertSnippets()
                        };
                        var helperFieldRange = book.Document.AppendText($"DOCVARIABLE {code[1..]}");
                        var helperField = book.Document.Fields.Create(helperFieldRange);
                        helperField.ShowCodes = true;
                        helperField.Update();

                        var helperRangeBytes = book.Document.GetOpenXmlBytes(helperField.ResultRange);

                        //Insert generated content into document
                        doc.Delete(fieldRange);

                        if (helperRangeBytes != null && helperRangeBytes.Length > 0)
                        {
                            using var stream = new MemoryStream(helperRangeBytes);
                            var insRange = doc.InsertDocumentContent(fieldRange.Start, stream, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);

                            //For FOOTNOTE and ENDNOTE - delete final line-break
                            if (string.Compare(codeName, "footnote", true) == 0 || string.Compare(codeName, "endnote", true) == 0)
                                doc.Delete(doc.CreateRange(insRange.End.ToInt() - 1, 1));
                        }
                    }
                    else
                    {
                        doc.Delete(fieldRange);
                        var field = doc.Fields.Create(fieldRange.Start, code);
                        field.Update();
                    }
                }
            }
            finally
            {
                range.EndUpdateDocument(doc);
            }


            static string UnpackField(string value)
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                value = value.Trim();
                if (value.Length <= 1)
                    return Utils.NonNullString(value);

                switch (value[0])
                {
                    case '{':
                        if (value[^1] == '}')
                            return value[1..^1].Replace("}}", "}");
                        break;
                }

                return value;
            }

            StringNoCaseDictionary<object> ConvertSnippets()
            {
                if (snippets == null)
                    return null;

                var result = new StringNoCaseDictionary<object>();

                foreach (DictionaryEntry keyValuePair in snippets)
                {
                    var value = keyValuePair.Value;
                    if (value is System.Management.Automation.PSObject psObject)
                        value = psObject.BaseObject;

                    result[Convert.ToString(keyValuePair.Key)] = value;
                }

                return result;
            }
        }
    }
}
