using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class SpreadsheetTemplateService : ISpreadsheetTemplateService
    {
        private readonly IWin32Window _Owner;

        public SpreadsheetTemplateService(IWin32Window owner)
        {
            _Owner = owner;
        }

        public void EditSpreadsheetTemplate(object dataSource, string dataMember = null) =>
            EditSpreadsheetTemplate(_Owner, dataSource, dataMember);

        public static void EditSpreadsheetTemplate(IWin32Window owner, object dataSource, string dataMember = null)
        {
#pragma warning disable IDE0067 // Dispose objects before losing scope
            var frm = new SpreadsheetTemplateEditor();
#pragma warning restore IDE0067 // Dispose objects before losing scope
            frm.FormClosed += (s, e) =>
            {
                frm.Dispose();
            };
            frm.SetupMergeMail(dataSource, dataMember);
            frm.Show(owner);
        }
    }
}
