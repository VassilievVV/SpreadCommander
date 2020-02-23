using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Set, "BookSection")]
    public class SetBookSectionCmdlet: BaseBookSectionCmdlet
    {
        [Parameter(HelpMessage = "1-based number of section to update. Negative numbers are allowed, -1 is the last section")]
        public int? SectionNum { get; set; }

        protected override void ProcessRecord()
        {
            var book = GetCmdletBook();
            ExecuteSynchronized(() => DoAddSection(book));
        }

        protected void DoAddSection(Document book)
        {
            Section section;
            int sectionNum = SectionNum ?? -1;
            if (sectionNum > 0 && sectionNum <= book.Sections.Count)
                section = book.Sections[sectionNum - 1];
            else if (sectionNum < 0 && -sectionNum <= book.Sections.Count)
                section = book.Sections[book.Sections.Count - sectionNum];
            else
                throw new Exception("Invalid SectionNum.");
            
            SetupSection(section);
        }
    }
}
