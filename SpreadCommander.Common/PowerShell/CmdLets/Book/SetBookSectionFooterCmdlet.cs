using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Set, "BookSectionFooter")]
    public class SetBookSectionFooterCmdlet: BaseBookHeaderFooterCmdlet
    {
        protected override DocumentType HeaderFooter => DocumentType.Footer;


        protected override void ProcessRecord()
        {
            var book = GetCmdletBook();
            ExecuteSynchronized(() => SetupHeaderFooter(book));
        }
    }
}
