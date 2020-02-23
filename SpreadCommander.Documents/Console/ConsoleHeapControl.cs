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
using DevExpress.XtraBars.Ribbon;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Code;
using SpreadCommander.Common;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleHeapControl : ConsoleBaseControl, IRibbonHolder, IFileViewer
    {
        public ConsoleHeapControl()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);
        }

        RibbonControl IRibbonHolder.Ribbon            => Ribbon;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => ribbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => Ribbon.Visible;
            set
            {
                Ribbon.Visible          = value;
                ribbonStatusBar.Visible = value;
            }
        }

        private void BarShowFiles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.ShowFileList();
        }

        private void BarPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.PreviewCurrentFile();
        }

        private void BarOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.OpenCurrentFile();
        }

        private void BarNavigationFirst_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.MoveFirst();
        }

        private void BarNavigationPrevious_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.MovePrevious();
        }

        private void BarNavigationNext_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.MoveNext();
        }

        private void BarNavigationLast_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.MoveLast();
        }

        private void BarViewList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.ShowStyleBar();
        }

        private void BarViewTiles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Heap.ShowStyleTiles();
        }

        public void ViewFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            Heap.PreviewFile(fileName);

            FireModified(false);
        }
    }
}
