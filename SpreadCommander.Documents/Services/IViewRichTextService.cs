using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
    public interface IViewRichTextService
    {
        void ViewDocument(Stream stream);
        void ViewHtmlText(string htmlText);
    }
}
