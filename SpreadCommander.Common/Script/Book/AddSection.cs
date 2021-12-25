using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class AddSectionOptions : SectionOptions
    {
    }

    public partial class SCBook
    {
        public SCBook AddSection(SectionOptions options = null)
        {
            ExecuteSynchronized(options, () => DoAddSection(options));
            return this;
        }

        protected void DoAddSection(SectionOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            var book = options.Book?.Document ?? Document;

            var section = book.AppendSection();
            if (options != null)
                SetupSection(section, options);

            if (section?.Range?.End != null)
            {
                book.CaretPosition = section.Range.End;
                ScrollToCaret();
            }
        }
    }
}
