using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class ErrorMessageOptions : CommentOptions
    {
    }

    public partial class SCBook
    {
        public void WriteErrorMessage(string text, ErrorMessageOptions options = null) =>
            ExecuteSynchronized(options, () => DoWriteErrorMessage(text, options));

        protected void DoWriteErrorMessage(string text, ErrorMessageOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            book.BeginUpdate();
            try
            {
                var range = book.AppendText(text);

                var foregroundColor = Color.Red;
                var backgroundColor = SystemColors.Window;

                var cp = book.BeginUpdateCharacters(range);
                try
                {

                    if (foregroundColor != SystemColors.WindowText)
                        cp.ForeColor = foregroundColor;
                    if (backgroundColor != SystemColors.Window)
                        cp.BackColor = backgroundColor;
                }
                finally
                {
                    book.EndUpdateCharacters(cp);
                }

                AddComments(book, range, options);

                if (range?.End != null)
                {
                    book.CaretPosition = range.End;
                    ResetBookFormatting(book, range.End);
                    ScrollToCaret();
                }

                WriteErrorToConsole(text);
            }
            finally
            {
                book.EndUpdate();
            }
        }
    }
}
