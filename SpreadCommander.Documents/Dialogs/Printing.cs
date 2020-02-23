using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrintingLinks;
using SpreadCommander.Common;
using DevExpress.XtraPrinting.Native;
using System.Drawing;
using DevExpress.XtraEditors;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    public static class Printing
    {
        private static void PrintControl(IWin32Window owner, LinkBase link, UserLookAndFeel lookAndFeel)
        {
            using PrintingSystem ps = new PrintingSystem();
            link.PrintingSystemBase = ps;
            ps.Links.Add(link);

            using (new WaitCursor())
            {
                link.CreateDocument(ps);
            }

            using var printTool = new PrintTool(ps);
            printTool.ShowRibbonPreviewDialog(owner, lookAndFeel);
        }

        public static void PrintControl(IWin32Window owner, IPrintable control)
        {
            using PrintableComponentLink link = new PrintableComponentLink() { Component = control };
            PrintControl(owner, link, link is ISupportLookAndFeel ? ((ISupportLookAndFeel)link).LookAndFeel : null);
        }

        public static void PrintControl(IWin32Window owner, ListView control)
        {
            using ListViewLink link = new ListViewLink() { ListView = control };
            PrintControl(owner, link, link is ISupportLookAndFeel ? ((ISupportLookAndFeel)link).LookAndFeel : null);
        }

        public static void PrintControl(IWin32Window owner, TreeView control)
        {
            using TreeViewLink link = new TreeViewLink() { TreeView = control };
            PrintControl(owner, link, link is ISupportLookAndFeel ? ((ISupportLookAndFeel)link).LookAndFeel : null);
        }

        public static void PrintControl(IWin32Window owner, RichTextBox control)
        {
            using RichTextBoxLink link = new RichTextBoxLink() { RichTextBox = control, PrintFormat = RichTextPrintFormat.ClientPageSize };
            PrintControl(owner, link, link is ISupportLookAndFeel ? ((ISupportLookAndFeel)link).LookAndFeel : null);
        }

        public static void PrintControl(IWin32Window owner, PictureEdit control)
        {
            using PrintingSystem ps = new PrintingSystem();
            Link link = new Link(ps);
            ps.Links.Add(link);

            link.CreateDetailArea += (s, e) =>
            {
                Rectangle rect = new Rectangle(Point.Empty, control.Image.Size);
                PrintCellHelperInfo info = new PrintCellHelperInfo(
                    Color.Empty,
                    e.Graph.PrintingSystem,
                    control.EditValue,
                    control.Properties.Appearance,
                    control.Text,
                    rect,
                    e.Graph);

                e.Graph.DrawBrick((Brick)control.Properties.GetBrick(info), rect);
            };

            using (new WaitCursor())
            {
                link.CreateDocument(ps);
            }

            using var printTool = new PrintTool(ps);
            printTool.ShowRibbonPreview(owner, control.LookAndFeel);
        }

        public static void PrintText(IWin32Window owner, string text)
        {
            using RichTextBox box = new RichTextBox() { Visible = false };
            PSNativeMethods.ForceCreateHandle(box);
            box.Text = text;

            Printing.PrintControl(owner, box);
        }

        public static void PrintRtfText(IWin32Window owner, string rtf)
        {
            using RichTextBox box = new RichTextBox() { Visible = false };
            PSNativeMethods.ForceCreateHandle(box);
            box.Rtf = rtf;

            Printing.PrintControl(owner, box);
        }

        private static void ExecutePrintCommand(LinkBase link, PrintingSystemCommand command, object[] args)
        {
            using PrintingSystem ps = new PrintingSystem();
            using (new WaitCursor())
            {
                link.CreateDocument(ps);
            }

            using var printTool = new PrintTool(ps);
            if (args != null)
                printTool.PreviewRibbonForm.PrintControl.ExecCommand(command, args);
            else
                printTool.PreviewRibbonForm.PrintControl.ExecCommand(command);
        }

        private static void ExecutePrintCommand(LinkBase link, PrintingSystemCommand command)
        {
            ExecutePrintCommand(link, command, null);
        }

        public static void ExecutePrintCommand(IPrintable control, PrintingSystemCommand command, object[] args)
        {
            using PrintableComponentLink link = new PrintableComponentLink() { Component = control };
            ExecutePrintCommand(link, command, args);
        }

        public static void ExecutePrintCommand(RichTextBox control, PrintingSystemCommand command)
        {
            using RichTextBoxLink link = new RichTextBoxLink() { RichTextBox = control, PrintFormat = RichTextPrintFormat.ClientPageSize };
            ExecutePrintCommand(link, command);
        }

        public static void ExecutePrintCommandForText(string text, PrintingSystemCommand command)
        {
            using RichTextBox box = new RichTextBox() { Visible = false };
            PSNativeMethods.ForceCreateHandle(box);
            box.Text = text;

            ExecutePrintCommand(box, command);
        }

        public static void ExecutePrintCommandForRtf(string rtf, PrintingSystemCommand command)
        {
            using RichTextBox box = new RichTextBox() { Visible = false };
            PSNativeMethods.ForceCreateHandle(box);
            box.Rtf = rtf;

            ExecutePrintCommand(box, command);
        }
    }
}