using DevExpress.XtraEditors;
using SpreadCommander.Documents.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class EditObjectService : IEditObjectService
    {
        private readonly IWin32Window _Owner;

        public EditObjectService(IWin32Window owner)
        {
            _Owner = owner;
        }

        public void EditObject(object value) => EditObject(_Owner, value);

        public static void EditObject(IWin32Window owner, object value)
        {
            using var editor = new PropertyGridEx() { SelectedObject = value };
            XtraDialog.Show(owner, editor, "Edit object", MessageBoxButtons.OK);
        }
    }
}
