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
using DevExpress.XtraTreeList.Nodes;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.ViewModels;

namespace SpreadCommander.Documents.Controls
{
    public partial class ProjectBrowser : DevExpress.XtraEditors.XtraUserControl
    {
        #region SelectedItemChangedEventArgs
        public class SelectedItemChangedEventArgs: EventArgs
        {
            public string FullPath { get; set; }
        }
        #endregion


        private FileSystemWatcher _Watcher;

        public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

        public ProjectBrowser()
        {
            InitializeComponent();

            Disposed += (s, e) =>
            {
                if (_Watcher != null)
                    _Watcher.EnableRaisingEvents = false;

                _Watcher?.Dispose();
                _Watcher = null;
            };
        }

        [DefaultValue(false)]
        public bool ShowFiles { get; set; }

        public DirectoryInfo SelectedFolder
        {
            get
            {
                var result = (treeProjectFiles.GetFocusedRow() as DirectoryTreeNode)?.Info as DirectoryInfo;
                return result;
            }
        }

        public BaseFileSystemTreeNode SelectedItem => treeProjectFiles.GetFocusedRow() as BaseFileSystemTreeNode;

        public DirectoryTreeNode RootItem => treeProjectFiles.DataSource as DirectoryTreeNode;

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

            if (_Watcher != null)
            {
                _Watcher.EnableRaisingEvents = false;

                _Watcher.Changed -= Watcher_Changed;
                _Watcher.Created -= Watcher_Created;
                _Watcher.Deleted -= Watcher_Deleted;
                _Watcher.Renamed -= Watcher_Renamed;

                _Watcher.Dispose();
                _Watcher = null;
            }

            var project = Project.Current;

            if (project == null)
                return;

            var data = new Code.DirectoryTreeNode(project.ProjectPath) { ShowFiles = this.ShowFiles };

            treeProjectFiles.DataSource = data;
            treeProjectFiles.RefreshDataSource();

            try
            {
                _Watcher = new FileSystemWatcher()
                {
                    Path                  = project.ProjectPath,
                    IncludeSubdirectories = true
                };
                _Watcher.Changed += Watcher_Changed;
                _Watcher.Created += Watcher_Created;
                _Watcher.Deleted += Watcher_Deleted;
                _Watcher.Renamed += Watcher_Renamed;

                _Watcher.EnableRaisingEvents = true;
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        private void ProjectBrowser_Load(object sender, EventArgs e)
        {
            ProjectChanged();
        }

        private void TreeProjectFiles_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (treeProjectFiles.GetDataRecordByNode(e.Node) is not BaseFileSystemTreeNode node)
                return;

            if (node is DirectoryTreeNode)
                e.Appearance.FontStyleDelta = FontStyle.Bold;
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

        private void TreeProjectFiles_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.NodeValue1 is string && e.NodeValue2 is string)
                e.Result = StringLogicalComparer.Compare(Convert.ToString(e.NodeValue1), Convert.ToString(e.NodeValue2));
        }

        private void TreeProjectFiles_DoubleClick(object sender, EventArgs e)
        {
            var node = treeProjectFiles.FocusedNode;

            if (treeProjectFiles.GetDataRecordByNode(node) is not FileTreeNode fileNode)
                return;

            if (!string.IsNullOrWhiteSpace(fileNode.FullPath) && File.Exists(fileNode.FullPath))
                BaseDocumentViewModel.MainDocumentParent.OpenDocumentFile(fileNode.FullPath);
        }

        private TreeListHitInfo dragFiles_StartHitInfo;
        private void TreeProjectFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None)
                dragFiles_StartHitInfo = (sender as TreeList).CalcHitInfo(new Point(e.X, e.Y));
        }

        private void TreeProjectFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && dragFiles_StartHitInfo != null && dragFiles_StartHitInfo.HitInfoType == HitInfoType.Cell && dragFiles_StartHitInfo.Node != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(dragFiles_StartHitInfo.MousePoint.X - dragSize.Width / 2,
                    dragFiles_StartHitInfo.MousePoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
#pragma warning disable IDE0019 // Use pattern matching
                    var node = treeProjectFiles.GetDataRecordByNode(dragFiles_StartHitInfo.Node) as FileTreeNode;
#pragma warning restore IDE0019 // Use pattern matching
                    if (node == null || !node.CanDrag || BaseDocumentViewModel.MainDocumentParent.IsFileOpen(node.FullPath))
                        return;

                    string text = node.DragText;

                    var dragData = new DataObject();
                    dragData.SetText(text, TextDataFormat.Text);
                    dragData.SetText(text, TextDataFormat.UnicodeText);
                    dragData.SetData(typeof(FileTreeNode), node);
                    dragData.SetData(typeof(TreeListNode), dragFiles_StartHitInfo.Node);

                    (sender as TreeList).DoDragDrop(dragData, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        private void TreeProjectFiles_DragOver(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetData(typeof(FileTreeNode)) is FileTreeNode))
                return;

            var hitInfo = treeProjectFiles.CalcHitInfo(treeProjectFiles.PointToClient(new Point(e.X, e.Y)));

            var treeNode = hitInfo?.Node;
            if (treeNode == null)
                return;

            if (!(treeProjectFiles.GetDataRecordByNode(treeNode) is DirectoryTreeNode))
                return;

            e.Effect = (e.KeyState & 8) == 8 ? DragDropEffects.Copy : DragDropEffects.Move;
        }

        private void TreeProjectFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(FileTreeNode)) is not FileTreeNode sourceNode)
                return;

            if (e.Data.GetData(typeof(TreeListNode)) is not TreeListNode treeSourceNode)
                return;

            var hitInfo = treeProjectFiles.CalcHitInfo(treeProjectFiles.PointToClient(new Point(e.X, e.Y)));

            var treeNode = hitInfo?.Node;
            if (treeNode == null)
                return;

            if (treeProjectFiles.GetDataRecordByNode(treeNode) is not DirectoryTreeNode dirNode)
                return;

            var srcFile = sourceNode.FullPath;
            var dstFile = Path.Combine(dirNode.FullPath, Path.GetFileName(srcFile));

            switch (e.Effect)
            {
                case DragDropEffects.Copy:
                    File.Copy(srcFile, dstFile);
                    dirNode.ChildNodes.Add(new FileTreeNode(dirNode, new FileInfo(dstFile)));
                    break;
                case DragDropEffects.Move:
                    File.Move(srcFile, dstFile);
                    dirNode.ChildNodes.Add(new FileTreeNode(dirNode, new FileInfo(dstFile)));
                    sourceNode.ParentNode.ChildNodes.Remove(sourceNode);
                    break;
            }

            treeProjectFiles.RefreshNode(treeNode);
            treeProjectFiles.RefreshNode(treeSourceNode.ParentNode);
        }

        private void TreeProjectFiles_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (treeProjectFiles.GetDataRecordByNode(e.Node) is not BaseFileSystemTreeNode node)
                return;

            SelectedItemChanged?.Invoke(this, new SelectedItemChangedEventArgs() { FullPath = node.FullPath });
        }

        private (Code.BaseFileSystemTreeNode parentItem, Code.BaseFileSystemTreeNode childItem) FindProjectDirectoryAndFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return (null, null);

            if (treeProjectFiles.DataSource is not Code.DirectoryTreeNode dataSource)
                return (null, null);

            if (!fileName.StartsWith(dataSource.FullPath, StringComparison.CurrentCultureIgnoreCase))
                return (null, null);

            char[] dirSeparators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
            int startIndex = dataSource.FullPath.Length;
            while (startIndex < fileName.Length && dirSeparators.Contains(fileName[startIndex]))
                startIndex++;

            string subPath = fileName[startIndex..];
            if (string.IsNullOrWhiteSpace(subPath))
                return (null, null);

            Code.BaseFileSystemTreeNode parentItem = dataSource, childItem = null;
            var folders = subPath.Split(dirSeparators);
            foreach (var folder in folders)
            {
                if (childItem != null)
                    parentItem = childItem;
                childItem = dataSource.FindNode(folder);
            }

            return (parentItem, childItem);
        }

        private static void InvokeMethod(Action action)
        {
            var mainForm = Parameters.MainForm;
            if (mainForm?.IsHandleCreated ?? false)
                mainForm.Invoke((Delegate)action);
            else
                action();
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            InvokeMethod(() =>
            {
                (var _, var childItem) = FindProjectDirectoryAndFile(e.FullPath);
                if (childItem != null)
                    childItem.UpdateInfo();
            });
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            InvokeMethod(() =>
            {
                (var parentItem, var childItem) = FindProjectDirectoryAndFile(e.FullPath);
                if (parentItem is not DirectoryTreeNode dirNode)
                    return;

                if (childItem != null)
                    childItem.UpdateInfo();
                else 
                    dirNode.AddNode(e.Name);
            });
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            InvokeMethod(() =>
            {
                (var parentItem, var childItem) = FindProjectDirectoryAndFile(e.FullPath);
                if (parentItem != null && childItem != null && parentItem is DirectoryTreeNode dirNode)
                    dirNode.RemoveNode(childItem);
            });
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            InvokeMethod(() =>
            {
                (var oldParentItem, var oldChildItem) = FindProjectDirectoryAndFile(e.OldFullPath);
                (var parentItem, var childItem)       = FindProjectDirectoryAndFile(e.FullPath);

                if (oldParentItem is DirectoryTreeNode oldDirNode && oldChildItem != null)
                    oldDirNode.RemoveNode(oldChildItem);
                
                if (parentItem is not DirectoryTreeNode dirNode)
                    return;

                if (childItem != null)
                    childItem.UpdateInfo();
                else
                    dirNode.AddNode(e.Name);
            });
        }
    }
}
