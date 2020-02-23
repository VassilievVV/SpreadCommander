using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Add, "BookSection")]
    public class AddBookSectionCmdlet: BaseBookSectionCmdlet
    {
        protected override void ProcessRecord()
        {
            var book = GetCmdletBook();
            ExecuteSynchronized(() => DoAddSection(book));
        }

        protected void DoAddSection(Document book)
        {
            var section = book.AppendSection();
            SetupSection(section);

            if (section?.Range?.End != null)
            {
                book.CaretPosition = section.Range.End;
                ScrollToCaret();
            }
        }
    }
}
