using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Clear, "Book")]
    public class ClearBookCmdlet: BaseBookCmdlet
    {
        protected override void EndProcessing()
        {
            var book = GetCmdletBook();
            ExecuteSynchronized(() => DoClearBook(book));
        }

        protected virtual void DoClearBook(Document book)
        {
            book.Text = string.Empty;
        }
    }
}
