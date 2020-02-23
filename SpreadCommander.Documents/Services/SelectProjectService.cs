using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class SelectProjectService: ISelectProjectService
    {
        private readonly IWin32Window _Owner;

        public SelectProjectService(IWin32Window owner)
        {
            _Owner = owner;
        }


        public string SelectProject()
        {
            using var frmSelectProject = new SelectProjectDialog();
            
            if (frmSelectProject.ShowDialog(_Owner) != DialogResult.OK)
                return null;

            return frmSelectProject.SelectedProject;
        }
    }
}
