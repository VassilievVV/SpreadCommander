using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public partial class SCBook
    {
        public SCBook SetSection(int sectionNum, SectionOptions options)
        {
            ExecuteSynchronized(options, () => DoSetSection(sectionNum, options));
            return this;
        }

        protected virtual void DoSetSection(int sectionNum, SectionOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            Section section;
            if (sectionNum > 0 && sectionNum <= book.Sections.Count)
                section = book.Sections[sectionNum - 1];
            else if (sectionNum < 0 && -sectionNum <= book.Sections.Count)
                section = book.Sections[book.Sections.Count - sectionNum];
            else
                throw new Exception("Invalid SectionNum.");

            SetupSection(section, options);
        }
    }
}
