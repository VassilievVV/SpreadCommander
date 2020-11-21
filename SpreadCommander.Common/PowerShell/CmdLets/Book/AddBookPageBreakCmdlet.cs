using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Add, "BookPageBreak")]
    public class AddBookPageBreakCmdlet: BaseBookCmdlet
    {
        protected override void ProcessRecord()
        {
            var book = GetCmdletBook();
            ExecuteSynchronized(() => DoWritePageBreak(book));
        }

        protected static void DoWritePageBreak(Document book)
        {
            book.AppendText("\f");
            WriteTextToConsole("\f");
        }
    }
}
