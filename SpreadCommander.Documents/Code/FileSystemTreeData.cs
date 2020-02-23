#pragma warning disable CRR0047
#pragma warning disable CRR0048

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
	public class BaseFileSystemTreeNode: TreeList.IVirtualTreeListData, IComparable
	{
#pragma warning disable CRRSP01 // A misspelled word has been found
		public static string[] AllowedExtensions = new string[]
		{
			".xlsx", ".xls", ".csv", ".txt", ".sql", ".ps", ".ps1", ".csx", ".fsx", ".r", ".py",
			".docx", ".doc", ".rtf", ".htm", ".html", ".mht", ".odt", ".epub",
			".png", ".tif", ".tiff", ".jpg", ".jpeg", ".gif", ".bmp",
			".scdash", ".scchart", ".scpivot"
		};
#pragma warning restore CRRSP01 // A misspelled word has been found

		public BaseFileSystemTreeNode(BaseFileSystemTreeNode parentNode)
		{
			ParentNode = parentNode;
		}

		public BaseFileSystemTreeNode ParentNode	{ get; }
		public virtual string Text					{ get; set; }
		public virtual string FullPath				{ get; set; }
		public BindingList<BaseFileSystemTreeNode> ChildNodes { get; private set; }

		public virtual bool CanDrag    => true;
		public virtual string DragText => FullPath;

		public virtual BindingList<BaseFileSystemTreeNode> ListChildNodes()
		{
			return null;
		}

		public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
		{
			if (info.Node is BaseFileSystemTreeNode node)
				info.CellData = node.Text;
		}

		public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
		{
			if (info.Node is BaseFileSystemTreeNode node)
			{
				node.ChildNodes = node.ListChildNodes();
				if (node.ChildNodes != null)
					info.Children = node.ChildNodes;
			}
		}

		public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
		{
			throw new NotImplementedException();
		}

		public T FindParentNode<T>() where T : BaseFileSystemTreeNode
		{
			BaseFileSystemTreeNode result = this;

			while (result != null)
			{
				if (result is T)
					return (T)result;

				result = result.ParentNode;
			}

			return null;
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
	}
	#endregion

	#region DirectoryTreeNode
	public class DirectoryTreeNode: BaseFileSystemTreeNode
	{
		public DirectoryInfo Info { get; private set; }

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

		public bool ShowFiles { get; set; } = true;

		public override BindingList<BaseFileSystemTreeNode> ListChildNodes()
		{
			var info = new DirectoryInfo(FullPath);
			if (!info.Exists)
				return null;

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
	}
	#endregion

	#region FileTreeNode
	public class FileTreeNode: BaseFileSystemTreeNode
	{
		public FileInfo Info { get; private set; }

		public FileTreeNode(DirectoryTreeNode parentNode) : base(parentNode)
		{
		}

		public FileTreeNode(DirectoryTreeNode parentNode, FileInfo info) : base(parentNode)
		{
			Info = info;
		}

		public override string Text { get => Info.Name; set { } }
		public override string FullPath { get => Info.FullName; set { } }
	}
	#endregion
}
