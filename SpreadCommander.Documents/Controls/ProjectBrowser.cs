using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.Code;
using System.IO;
using DevExpress.XtraTreeList;
using SpreadCommander.Common;

namespace SpreadCommander.Documents.Controls
{
    public partial class ProjectBrowser : DevExpress.XtraEditors.XtraUserControl
    {
        public ProjectBrowser()
        {
            InitializeComponent();
        }

        public DirectoryInfo SelectedFolder
        {
            get
            {
                var result = (treeProjectFiles.FocusedValue as DirectoryTreeNode)?.Info;
                return result;
            }
        }

        public static DirectoryInfo SelectProjectFolder(IWin32Window owner)
        {
            var browser = new ProjectBrowser();

            if (XtraDialog.Show(owner, browser, "Browser") != DialogResult.OK)
                return null;

            return browser.SelectedFolder;
        }

        public void ProjectChanged()
        {
            treeProjectFiles.DataSource = null;

            var project = Project.Current;

            if (project == null)
                return;

            var data = new Code.DirectoryTreeNode(project.ProjectPath) { ShowFiles = false };

            treeProjectFiles.DataSource = data;
            treeProjectFiles.RefreshDataSource();
        }

        private void ProjectBrowser_Load(object sender, EventArgs e)
        {
            ProjectChanged();
        }

        private void TreeProjectFiles_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            //Do nothing
        }

        private void TreeProjectFiles_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            if (treeProjectFiles.GetDataRecordByNode(e.Node) is not BaseFileSystemTreeNode node)
                return;

            if (node is DirectoryTreeNode)
                e.NodeImageIndex = 0;
            else if (node is FileTreeNode nodeFile)
            {
                var ext = Path.GetExtension(nodeFile.Text)?.ToLower();
                e.NodeImageIndex = ext switch
                {
                    ".xlsx" or ".xls"                                                               => 2,
                    ".csv" or ".txt"                                                                => 3,
                    ".sql"                                                                          => 4,
                    ".ps" or ".ps1" or ".csx" or ".fsx" or ".r" or ".py"                            => 5,
                    ".docx" or ".doc" or ".rtf" or ".htm" or ".html" or ".mht" or ".odt" or ".epub" => 6,
                    ".png" or ".tif" or ".tiff" or ".jpg" or ".jpeg" or ".gif" or ".bmp"            => 7,
                    ".dash"                                                                         => 8,
                    ".pdf"                                                                          => 9,
                    _                                                                               => 1
                };
            }

            /*
            0	folder
            1	file
            2	spreadsheet
            3	csv
            4	sql
            5	script
            6	document
            7	image
            8	dashboard
            9	pdf
            */
        }
    }
}
