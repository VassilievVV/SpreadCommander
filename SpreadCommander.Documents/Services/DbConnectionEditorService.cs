using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class DbConnectionEditorService : IDbConnectionEditorService
    {
        private readonly IWin32Window _Owner;

        public DbConnectionEditorService(IWin32Window owner)
        {
            _Owner = owner;
        }

        public SelectedDbConnection SelectConnection()
        {
            using var frm = new DbConnectionEditor();
            if (frm.ShowDialog(_Owner) != DialogResult.OK)
                return null;

            var result = new SelectedDbConnection()
            {
                ConnectionName   = frm.SelectedConnectionName,
                Connection       = frm.SelectedConnection,
                Factory          = frm.SelectedFactory,
                ConnectionString = frm.SelectedConnectionString
            };
            return result;
        }
    }
}
