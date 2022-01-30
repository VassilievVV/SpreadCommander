using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class ViewRichTextService: IViewRichTextService
    {
        private readonly IWin32Window _Owner;

        public ViewRichTextService(IWin32Window owner)
        {
            _Owner = owner;
        }

        public void ViewDocument(Stream stream)
        {
            var viewer = new RichTextViewer();
            viewer.FormClosed += (s, _) => (s as IDisposable)?.Dispose();
            viewer.LoadDocument(stream);
            viewer.Show(_Owner);
        }

        public void ViewHtmlText(string htmlText)
        {
            var viewer = new RichTextViewer();
            viewer.FormClosed += (s, _) => (s as IDisposable)?.Dispose();
            viewer.LoadHtmlText(htmlText);
            viewer.Show(_Owner);
        }
    }
}
