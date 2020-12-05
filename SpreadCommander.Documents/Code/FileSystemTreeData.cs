using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraTreeList;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Code
{
    #region BaseFileSystemTreeNode
    public class BaseFileSystemTreeNode: TreeList.IVirtualTreeListData, IComparable, INotifyPropertyChanged
    {
        public static readonly string[] AllowedExtensions = new string[]
        {
            ".xlsx", ".xls", ".csv", ".txt", ".sql", ".ps", ".ps1", ".csx", ".fsx", ".r", ".py",
            ".docx", ".doc", ".rtf", ".htm", ".html", ".mht", ".odt", ".epub",
            ".png", ".tif", ".tiff", ".jpg", ".jpeg", ".gif", ".bmp",
            ".scdash", ".scchart", ".scpivot"
        };

        public BaseFileSystemTreeNode(BaseFileSystemTreeNode parentNode)
        {
            ParentNode = parentNode;
        }

        protected static readonly object LockObject = new object();

        public BaseFileSystemTreeNode ParentNode	{ get; }
        public virtual string Text					{ get; set; }
        public virtual string FullPath				{ get; set; }
        public BindingList<BaseFileSystemTreeNode> ChildNodes { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool CanDrag    => true;
        public virtual string DragText => FullPath;

        public virtual FileSystemInfo Info { get; protected set; }

        public virtual BindingList<BaseFileSystemTreeNode> ListChildNodes()
        {
            return null;
        }

        public virtual void UpdateInfo()
        {
            //Do nothing
        }

        public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
        {
            if (info.Node is BaseFileSystemTreeNode node)
                info.CellData = node.Text;
        }

        public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
        {
            lock (LockObject)
            {
                if (info.Node is BaseFileSystemTreeNode node)
                {
                    node.ChildNodes = node.ListChildNodes();
                    if (node.ChildNodes != null)
                        info.Children = node.ChildNodes;
                }
            }
        }

        public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
        {
            throw new NotImplementedException();
        }

        public T FindParentNode<T>() where T : BaseFileSystemTreeNode
        {
            lock (LockObject)
            {
                BaseFileSystemTreeNode result = this;

                while (result != null)
                {
                    if (result is T t)
                        return t;

                    result = result.ParentNode;
                }

                return null;
            }
        }

        public int CompareTo(object obj)
        {
            //Place directories above files
            if (GetType() == obj.GetType())
                return StringLogicalComparer.Compare(Text, ((BaseFileSystemTreeNode)obj)?.Text);
            else if (GetType() == typeof(DirectoryTreeNode))
                return -1;
            else
                return 1;
        }

        protected void FirePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static bool IsFileNameAllowed(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            string ext  = Path.GetExtension(fileName)?.ToLower();
            bool result = !string.IsNullOrWhiteSpace(ext) && AllowedExtensions.Contains(ext);
            return result;
        }
    }
    #endregion

    #region DirectoryTreeNode
    public class DirectoryTreeNode: BaseFileSystemTreeNode
    {
        public override FileSystemInfo Info { get; protected set; }
        public DirectoryInfo DirectoryInfo => (DirectoryInfo)Info;

        public DirectoryTreeNode(DirectoryTreeNode parentNode): base(parentNode)
        {
        }

        public DirectoryTreeNode(DirectoryTreeNode parentNode, DirectoryInfo info) : base(parentNode)
        {
            Info = info;
        }

        //Top node
        public DirectoryTreeNode(string directory): base(null)
        {
            Info = new DirectoryInfo(directory);
        }

        public override string Text { get => Info.Name; set { } }
        public override string FullPath { get => Info.FullName; set { } }

        public override void UpdateInfo()
        {
            Info = new DirectoryInfo(FullPath);
        }

        public bool ShowFiles { get; set; } = true;

        public override BindingList<BaseFileSystemTreeNode> ListChildNodes()
        {
            var info = new DirectoryInfo(FullPath);
            if (!info.Exists)
                return new BindingList<BaseFileSystemTreeNode>();

            var result = new List<BaseFileSystemTreeNode>();

            foreach (var folder in info.GetDirectories())
            {
                var folderNode = new DirectoryTreeNode(this, folder) { ShowFiles = this.ShowFiles };
                result.Add(folderNode);
            }

            if (ShowFiles)
            {
                foreach (var file in info.GetFiles())
                {
                    if (!string.IsNullOrWhiteSpace(file.Extension) && AllowedExtensions.Contains(file.Extension.ToLower()))
                    {
                        var fileNode = new FileTreeNode(this, file);
                        result.Add(fileNode);
                    }
                }
            }

            result.Sort();
            return new BindingList<BaseFileSystemTreeNode>(result);
        }

        public BaseFileSystemTreeNode FindNode(string name)
        {
            lock (LockObject)
            {
                var childNodes = ChildNodes;
                if (childNodes == null)
                    return null;

                foreach (var childNode in childNodes)
                {
                    if (string.Compare(childNode.Text, name, true) == 0)
                        return childNode;
                }

                return null;
            }
        }

        private int FindNewNameIndex(string name, bool isDirectory)
        {
            var childNodes = ChildNodes;
            if (childNodes == null)
                return 0;

            int startIndex = 0;
            var names = new List<string>();
            foreach (var item in childNodes)
            {
                if (item is DirectoryTreeNode)
                {
                    startIndex++;
                    if (isDirectory)
                        names.Add(item.Text);
                }
                else if (!isDirectory)
                    names.Add(item.Text);
            }

            Utils.FindSortedStringIndex(names, name, false, out int result);
            if (!isDirectory)
                result += startIndex;
            result = Utils.ValueInRange(result, 0, childNodes.Count);
            return result;
        }

        public BaseFileSystemTreeNode AddNode(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            lock (LockObject)
            {
                var childNodes = ChildNodes;
                if (childNodes == null)
                    return null;

                string fullName = Path.Combine(FullPath, name);
                if (Directory.Exists(fullName))
                {
                    var folder     = new DirectoryInfo(fullName);
                    var folderNode = new DirectoryTreeNode(this, folder) { ShowFiles = this.ShowFiles };

                    int index = FindNewNameIndex(name, true);
                    childNodes.Insert(index, folderNode);

                    return folderNode;
                }
                else if (ShowFiles && File.Exists(fullName))
                {
                    string ext = Path.GetExtension(name);
                    if (!string.IsNullOrWhiteSpace(ext) && AllowedExtensions.Contains(ext.ToLower()))
                    {
                        var file     = new FileInfo(fullName);
                        var fileNode = new FileTreeNode(this, file);

                        int index = FindNewNameIndex(name, false);
                        childNodes.Insert(index, fileNode);

                        return fileNode;
                    }
                }

                return null;
            }
        }

        public void RemoveNode(BaseFileSystemTreeNode node)
        {
            lock (LockObject)
            {
                var childNodes = ChildNodes;
                if (childNodes == null)
                    return;

                if (childNodes.Contains(node))
                    childNodes.Remove(node);
            }
        }
    }
    #endregion

    #region FileTreeNode
    public class FileTreeNode: BaseFileSystemTreeNode
    {
        public override FileSystemInfo Info { get; protected set; }
        public FileInfo FileInfo => (FileInfo)Info;

        public FileTreeNode(DirectoryTreeNode parentNode) : base(parentNode)
        {
        }

        public FileTreeNode(DirectoryTreeNode parentNode, FileInfo info) : base(parentNode)
        {
            Info = info;
        }

        public override string Text { get => Info.Name; set { } }
        public override string FullPath { get => Info.FullName; set { } }

        public override void UpdateInfo()
        {
            Info = new FileInfo(FullPath);
        }
    }
    #endregion
}
