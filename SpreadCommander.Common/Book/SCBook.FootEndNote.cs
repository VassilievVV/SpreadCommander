using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.Text;
using SpreadCommander.Common.Code;
using System.Globalization;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Charts.Native;

namespace SpreadCommander.Common.Book
{
    public partial class SCBook
    {
        protected enum FootEndNoteType { FootNote, EndNote }
        private enum FootEndNoteFormat { Text, Html }

#pragma warning disable CA1822 // Mark members as static
        protected IRichEditDocumentServer AddFootEndNote(FootEndNoteType noteType, ArgumentCollection arguments)
#pragma warning restore CA1822 // Mark members as static
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE FOOTNOTE' requires at least one argument.");

            var format  = FootEndNoteFormat.Text;
            string mark = null;
            string text = arguments[0].Value;
            if (arguments.Count == 1)
                text = arguments[0].Value;

            if (arguments.Count > 1)
            {
                var properties = Utils.SplitNameValueString(arguments[1].Value, ';');

                foreach (var prop in properties)
                {
                    if (string.IsNullOrWhiteSpace(prop.Key))
                        continue;

                    switch (prop.Key.ToLower())
                    {
                        case "format":
                            if (string.Compare(prop.Value, "HTML", true) == 0)
                                format = FootEndNoteFormat.Html;
                            break;
                        case "html":
                            format = FootEndNoteFormat.Html;
                            break;
                        case "mark":
                            mark = prop.Value;
                            break;
                    }
                }
            }

            var server   = new RichEditDocumentServer();
            var document = server.Document;

            var pos = document.Range.Start;

            Note note = null;
            switch (noteType)
            {
                case FootEndNoteType.FootNote:
                    if (mark != null)
                        note = document.Footnotes.Insert(pos, mark);
                    else
                        note = document.Footnotes.Insert(pos);
                    break;
                case FootEndNoteType.EndNote:
                    if (mark != null)
                        note = document.Endnotes.Insert(pos, mark);
                    else
                        note = document.Endnotes.Insert(pos);
                    break;
            }

            var docNote = note.BeginUpdate();
            try
            {
                switch (format)
                {
                    case FootEndNoteFormat.Text:
                        docNote.AppendText(text);
                        break;
                    case FootEndNoteFormat.Html:
                        docNote.AppendHtmlText(text, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
                        break;
                }
            }
            finally
            {
                note.EndUpdate(docNote);
            }

            return server;
        }

        protected IRichEditDocumentServer AddFootNote(ArgumentCollection arguments) => AddFootEndNote(FootEndNoteType.FootNote, arguments);
        protected IRichEditDocumentServer AddEndNote(ArgumentCollection arguments)  => AddFootEndNote(FootEndNoteType.EndNote, arguments);
    }

}
