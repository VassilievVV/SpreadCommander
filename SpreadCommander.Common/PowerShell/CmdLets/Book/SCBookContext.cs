using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    public class SCBookContext: IDisposable
    {
        protected internal SCBook SCBook { get; } = new SCBook();

        protected internal Document Document                  => SCBook.Document;
        protected internal IRichEditDocumentServer BookServer => SCBook.BookServer;

        public void Dispose()
        {
            SCBook.Dispose();
        }
    }
}
