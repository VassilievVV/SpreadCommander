using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class BookTemplateService: IBookTemplateService
    {
        private readonly IWin32Window _Owner;

        public BookTemplateService(IWin32Window owner)
        {
            _Owner = owner;
        }

        public void EditBookTemplate(object dataSource, string dataMember = null) =>
            EditBookTemplate(_Owner, dataSource, dataMember);

#pragma warning disable IDE0060 // Remove unused parameter
        public static void EditBookTemplate(IWin32Window owner, object dataSource, string dataMember = null)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //TODO: Uncomment when Snap will be implemented by DevExpress
            /*
            var frm = new BookTemplateEditor();
            frm.SetupMergeMail(dataSource, dataMember);
            frm.Show(owner);
            */
        }
    }
}
