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
using System.IO;
using DevExpress.XtraGrid.Views.WinExplorer;
using SpreadCommander.Common;
using SpreadCommander.Documents.Viewers;
using SpreadCommander.Common.Code;
using System.Diagnostics;

namespace SpreadCommander.Documents.Controls
{
    public partial class HeapControl : DevExpress.XtraEditors.XtraUserControl
    {
        public enum FileType { Unknown, Spreadsheet, Text, Book, SqlScript, Script, Dashboard, Image, PDF }

        #region FileItem
        public class FileItem: INotifyPropertyChanged
        {
            public FileItem(FileInfo fileInfo)
            {
                _FileInfo = fileInfo;
            }

            private FileInfo _FileInfo;

            public string FileName    => FileInfo.Name;
            public string Extension   => FileInfo.Extension?.ToLower();
            public string Description => $"{FileInfo.CreationTime.ToShortDateString()}  {GetFileSize(FileInfo.Length)}";

            public event PropertyChangedEventHandler PropertyChanged;

            public FileInfo FileInfo
            {
                get => _FileInfo;
                set
                {
                    if (value == _FileInfo)
                        return;

                    _FileInfo = value;

                    FirePropertyChanged("FileName");
                    FirePropertyChanged("Extension");
                    FirePropertyChanged("Description");
                    FirePropertyChanged("FileType");
                    FirePropertyChanged("GroupName");
                    FirePropertyChanged("ImageIndex");
                    FirePropertyChanged("Enabled");
                }
            }

            protected void FirePropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            public static string GetFileSize(long size)
            {
                if (size > 1_073_741_824)   //1GB
                    return (Convert.ToDouble(size) / 1_073_741_824.0).ToString("0.##") + " GB";
                if (size > 1_048_576)   //1MB
                    return (Convert.ToDouble(size) / 1_048_576.0).ToString("0.##") + "MB";
                if (size > 1_024)   //1KB
                    return (Convert.ToDouble(size) / 1024.0).ToString("0.##") + "kB";
                return $"{size} bytes";
            }

            public FileType FileType
            {
                get
                {
                    switch (Extension)
                    {
                        case ".xlsx":
                        case ".xls":
                            return FileType.Spreadsheet;
                        case ".csv":
                        case ".txt":
                            return FileType.Text;
                        case ".sql":
                            return FileType.SqlScript;
                        case ".ps":
                        case ".ps1":
                        case ".csx":
                        case ".fsx":
                        case ".r":
                        case ".py":
                            return FileType.Script;
                        case ".docx":
                        case ".doc":
                        case ".rtf":
                        case ".htm":
                        case ".html":
                        case ".mht":
                        case ".odt":
                        case ".epub":
                            return FileType.Book;
                        case ".png":
                        case ".tif":
                        case ".tiff":
                        case ".jpg":
                        case ".jpeg":
                        case ".gif":
                        case ".bmp":
                            return FileType.Image;
                        case ".dash":
                            return FileType.Dashboard;
                        case ".pdf":
                            return FileType.PDF;
                        default:
                            return FileType.Unknown;
                    }
                }
            }

            public string GroupName
            {
                get
                {
                    switch (FileType)
                    {
                        case FileType.Spreadsheet:
                            return "Spreadsheet";
                        case FileType.Text:
                            return "CSV";
                        case FileType.Book:
                            return "Book";
                        case FileType.SqlScript:
                            return "SQL script";
                        case FileType.Script:
                            return "Script";
                        case FileType.Dashboard:
                            return "Dashboard";
                        case FileType.Image:
                            return "Image";
                        case FileType.PDF:
                            return "PDF";
                        default:
                            return "Others";
                    }
                }
            }

            public int ImageIndex
            {
                get
                {
                    switch (FileType)
                    {
                        case FileType.Spreadsheet:
                            return 2;
                        case FileType.Text:
                            return 3;
                        case FileType.Book:
                            return 6;
                        case FileType.SqlScript:
                            return 4;
                        case FileType.Script:
                            return 5;
                        case FileType.Dashboard:
                            return 8;
                        case FileType.Image:
                            return 7;
                        case FileType.PDF:
                            return 9;
                        default:
                            return 1;
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

            public bool Enabled
            {
                get
                {
                    switch (FileType)
                    {
                        case FileType.Spreadsheet:
                        case FileType.Text:
                        case FileType.Book:
                        case FileType.SqlScript:
                        case FileType.Script:
                        case FileType.Dashboard:
                        case FileType.Image:
                        case FileType.PDF:
                            return true;
                        default:
                            return false;
                    }
                }
            }
        }
        #endregion

        public HeapControl()
        {
            InitializeComponent();

            using (new UsingProcessor(() => _IsLoading = true, () => _IsLoading = false))
            { 
                ProjectDirectory       = Project.Current.ProjectPath;
                CurrentHeapFolder      = HeapFolderPrefix;
                _CurrentPreviewControl = gridFiles;
            }
        }

        public readonly string ProjectDirectory;

        public const string HeapFolderPrefix = @"Project\";

        private string _CurrentFileName;
        private Control _CurrentPreviewControl;
        private bool _IsLoading;

        public string CurrentHeapFolder
        {
            get => (string)barFolder.EditValue;
            set
            {
                if (Convert.ToString(barFolder.EditValue) != value)
                    barFolder.EditValue = value;

                var folder = Path.Combine(ProjectDirectory, GetHeapFolderPath(value));
                var folderExists = !string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder);

                try
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.Path                = folderExists ? folder : null;
                    watcher.EnableRaisingEvents = folderExists;

                    UpdateFileList();
                }
                catch (Exception)
                {
                }
            }
        }

        public string FileMask
        {
            get => (string)barMask.EditValue;
            set => barMask.EditValue = value;
        }

        private string GetHeapFolderPath(string initPath)
        {
            if (!string.IsNullOrWhiteSpace(initPath) && initPath.StartsWith(HeapFolderPrefix, StringComparison.CurrentCultureIgnoreCase))
                return initPath.Substring(HeapFolderPrefix.Length);
            return initPath;
        }

        private string GetCurrentHeapDirectory()
        {
            var path   = GetHeapFolderPath(CurrentHeapFolder);
            var result = Path.Combine(ProjectDirectory, path);
            return result;
        }

        private void UpdateFileList()
        {
            bindingFiles.Clear();
            PreviewFile(null);

            using (new UsingProcessor(() => _IsLoading = true, () => _IsLoading = false))
            {
                var currentDir = GetCurrentHeapDirectory();
                if (string.IsNullOrWhiteSpace(currentDir) || !Directory.Exists(currentDir))
                    return;

                var mask = UpdateFileMast(FileMask);

                var currentDirInfo = new DirectoryInfo(currentDir);
                FileInfo[] files;
                try
                {
                    files = currentDirInfo.GetFiles(mask, SearchOption.TopDirectoryOnly);
                }
                catch (Exception)
                {
                    files = currentDirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly);
                }

                var listFiles = files.ToList();
                listFiles.Sort((x, y) => StringLogicalComparer.Compare(x.Name, y.Name));

                using (new UsingProcessor(() => gridFiles.BeginUpdate(), () => gridFiles.EndUpdate()))
                {
                    foreach (var file in listFiles)
                    {
                        var fileItem = new FileItem(file);
                        bindingFiles.Add(fileItem);
                    }
                }
            }

            string UpdateFileMast(string mask)
            {
                if (string.IsNullOrWhiteSpace(mask))
                    return "*.*";

                var invalidChars = Path.GetInvalidFileNameChars();
                var result = new StringBuilder();

                foreach (char c in mask)
                    result.Append(invalidChars.Contains(c) ? '?' : c);

                return result.ToString();
            }
        }

        private void BarList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowStyleBar();
        }

        public void ShowStyleBar()
        {
            var useTransition = gridFiles.Parent != null;
            if (useTransition)
                transitionManager.StartTransition(panelHost);
            try
            {
                viewFiles.OptionsView.Style = WinExplorerViewStyle.List;
            }
            finally
            {
                if (useTransition)
                    transitionManager.EndTransition();
            }
        }

        private void BarTiles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowStyleTiles();
        }

        public void ShowStyleTiles()
        {
            var useTransition = gridFiles.Parent != null;
            if (useTransition)
                transitionManager.StartTransition(panelHost);
            try
            {
                viewFiles.OptionsView.Style = WinExplorerViewStyle.Tiles;
            }
            finally
            {
                if (useTransition)
                    transitionManager.EndTransition();
            }
        }

        public void PreviewFile(string fileName)
        {
            if (_IsLoading)
                return;

            if ((_CurrentFileName == null && fileName == null) ||
                (string.Compare(_CurrentFileName, fileName, true) == 0))
                return;

            _CurrentFileName = fileName;

            using (new UsingProcessor(() => transitionManager.StartTransition(panelHost), () => transitionManager.EndTransition()))
            {
                if (_CurrentPreviewControl != null)
                {
                    _CurrentPreviewControl.Parent = null;
                    if (_CurrentPreviewControl != gridFiles)
                        _CurrentPreviewControl.Dispose();
                    _CurrentPreviewControl = null;
                }

                try
                {
                    if (_CurrentFileName == null)
                        _CurrentPreviewControl = gridFiles;
                    else
                        _CurrentPreviewControl = BaseViewer.CreateViewer(this, fileName, null, null);
                }
                catch (Exception ex)
                {
                    _CurrentPreviewControl = new OtherViewer("Cannot preview: " + ex.Message);
                }
                _CurrentPreviewControl.Dock   = DockStyle.Fill;
                _CurrentPreviewControl.Parent = panelHost;
            }
        }

        private void BarFiles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowFileList();
        }

        public void ShowFileList()
        {
            PreviewFile(null);
        }

        private void BarPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PreviewCurrentFile();
        }

        public void PreviewCurrentFile()
        {
            if (!(bindingFiles.Current is FileItem item))
                return;

            PreviewFile(item.FileInfo.FullName);
        }

        private void ViewFiles_ItemDoubleClick(object sender, WinExplorerViewItemDoubleClickEventArgs e)
        {
            if (!(viewFiles.GetRow(e.ItemInfo.Row.RowHandle) is FileItem item) || !item.Enabled)
                return;

            PreviewFile(item.FileInfo.FullName);
        }

        private void BarOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenCurrentFile();
        }

        public void OpenCurrentFile()
        {
            if (!(bindingFiles.Current is FileItem item))
                return;

            Process.Start(item.FileInfo.FullName);
        }

        private void ViewFiles_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            if (e.Value1 is string && e.Value2 is string)
            {
                e.Result  = StringLogicalComparer.Compare((string)e.Value1, (string)e.Value2);
                e.Handled = true;
            }
        }

        private void RepositoryFolder_NewNodeAdding(object sender, BreadCrumbNewNodeAddingEventArgs e)
        {
            e.Node.PopulateOnDemand = true;
        }

        private void RepositoryFolder_ValidatePath(object sender, BreadCrumbValidatePathEventArgs e)
        {
            var dir = Path.Combine(ProjectDirectory, GetHeapFolderPath(e.Path));
            e.ValidationResult = Directory.Exists(dir) ? BreadCrumbValidatePathResult.CreateNodes : BreadCrumbValidatePathResult.Cancel;
        }

        private void RepositoryFolder_QueryChildNodes(object sender, BreadCrumbQueryChildNodesEventArgs e)
        {
            var dir = Path.Combine(ProjectDirectory, GetHeapFolderPath(e.Node.Path));
            if (!Directory.Exists(dir))
                return;

            string[] subDirs = GetSubDirs(dir);
            for (int i = 0; i < subDirs.Length; i++)
                e.Node.ChildNodes.Add(CreateNode(subDirs[i]));
        }

        protected BreadCrumbNode CreateNode(string path)
        {
            string folderName = new DirectoryInfo(Path.Combine(ProjectDirectory, GetHeapFolderPath(path))).Name;
            return new BreadCrumbNode(folderName, folderName, true);
        }

        public static string[] GetSubDirs(string dir)
        {
            string[] subDirs = null;
            try
            {
                subDirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            {
            }
            return subDirs;
        }

        private void RepositoryFolder_PathChanged(object sender, BreadCrumbPathChangedEventArgs e)
        {
            PathChanged(e.Path);
        }

        private void PathChanged(string path)
        { 
            using (new UsingProcessor(() => transitionManager.StartTransition(panelHost), () => transitionManager.EndTransition()))
            { 
                bindingFiles.Clear();

                var dir = Path.Combine(ProjectDirectory, GetHeapFolderPath(path));
                var dirInfo = new DirectoryInfo(dir);
                if (!dirInfo.Exists)
                    return;

                var fileMask = FileMask;
                if (string.IsNullOrWhiteSpace(fileMask))
                    fileMask = "*.*";

                try
                {
                    var files = dirInfo.GetFiles(FileMask, SearchOption.TopDirectoryOnly);

                    using (new UsingProcessor(() => viewFiles.BeginDataUpdate(), () => viewFiles.EndDataUpdate()))
                    {
                        foreach (var file in files)
                        {
                            var fileItem = new FileItem(file);
                            bindingFiles.Add(fileItem);
                        }
                    }
                }
                catch (Exception)
                {
                };
            }
        }

        private void RepositoryFolder_PathRejected(object sender, BreadCrumbPathRejectedEventArgs e)
        {
            bindingFiles.Clear();
        }

        private void RepositoryFolder_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var dir = ProjectBrowser.SelectProjectFolder(this);
            if (dir == null || !dir.Exists)
                return;

            var folder = dir.FullName;
            if (!folder.StartsWith(ProjectDirectory, StringComparison.CurrentCultureIgnoreCase))
                return;

            folder = HeapFolderPrefix + folder.Substring(ProjectDirectory.Length + 1);
            CurrentHeapFolder = folder;
        }

        private FileItem FindFileItem(string fullFileName)
        {
            foreach (FileItem item in bindingFiles)
            {
                if (string.Compare(item.FileInfo.FullName, fullFileName, true) == 0)
                    return item;
            }

            return null;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var fileItem = FindFileItem(e.FullPath);
            if (fileItem != null)
                fileItem.FileInfo = new FileInfo(e.FullPath);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var fileItem = new FileItem(new FileInfo(e.FullPath));
            bindingFiles.Add(fileItem);
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var fileItem = FindFileItem(e.FullPath);
            if (fileItem != null)
                bindingFiles.Remove(fileItem);
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            var fileItem = FindFileItem(e.OldFullPath);
            if (fileItem != null)
                fileItem.FileInfo = new FileInfo(e.FullPath);
        }

        private void BarFileFirst_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MoveFirst();
        }

        public void MoveFirst()
        {
            if (bindingFiles.Count > 0)
                bindingFiles.Position = 1;
        }

        private void BarFilePrevious_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MovePrevious();
        }

        public void MovePrevious()
        {
            if (bindingFiles.Count > 0 && bindingFiles.Position > 0)
                bindingFiles.Position -= 1;
        }

        private void BarFileNext_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MoveNext();
        }

        public void MoveNext()
        {
            if (bindingFiles.Count > 0 && bindingFiles.Position < bindingFiles.Count - 1)
                bindingFiles.Position += 1;
        }

        private void BarFileLast_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MoveLast();
        }

        public void MoveLast()
        {
            if (bindingFiles.Count > 0)
                bindingFiles.Position = bindingFiles.Count - 1;
        }

        private void BindingFiles_CurrentChanged(object sender, EventArgs e)
        {
            if (!(bindingFiles.Current is FileItem item))
                return;

            if (_CurrentFileName != null)
                PreviewFile(item.FileInfo.FullName);
        }

        private void BarFolder_EditValueChanged(object sender, EventArgs e)
        {
            if (barFolder.EditValue is string)
                CurrentHeapFolder = (string)barFolder.EditValue;
        }

        private void RepositoryMask_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateFileList();
        }

        private void AddMaskAndExecute()
        {
            var mask = Convert.ToString(barMask.EditValue);

            bool hasMask = false;
            for (int i = 0; i < repositoryMask.Items.Count; i++)
            {
                var itemMask = Convert.ToString(repositoryMask.Items[i]);
                if (string.Compare(mask, itemMask, true) == 0)
                {
                    hasMask = true;
                    break;
                }
            }

            if (!hasMask)
                repositoryMask.Items.Add(mask);

            UpdateFileList();
        }

        private void RepositoryMask_Leave(object sender, EventArgs e)
        {
            AddMaskAndExecute();
        }

        private void RepositoryMask_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AddMaskAndExecute();
        }

        private void RepositoryFolder2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var dir = ProjectBrowser.SelectProjectFolder(this);
            if (dir == null || !dir.Exists)
                return;

            var folder = dir.FullName;
            if (!folder.StartsWith(ProjectDirectory, StringComparison.CurrentCultureIgnoreCase))
                return;

            folder = HeapFolderPrefix + folder.Substring(ProjectDirectory.Length + 1);
            CurrentHeapFolder = folder;

            PathChanged(folder);
        }
    }
}
