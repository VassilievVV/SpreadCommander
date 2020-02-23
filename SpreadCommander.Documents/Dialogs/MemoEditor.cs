#pragma warning disable CRR0047

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using SpreadCommander.Common;
using System.Diagnostics;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    #region IMemoHolder
    public interface IMemoHolder
    {
        string MemoText {get; set;}
    }
    #endregion

    public partial class MemoEditor: BaseForm
    {
        public MemoEditor()
        {
            InitializeComponent();
        }

        public string MemoText
        {
            get {return Memo.Text;}
            set {Memo.Text = value; Memo.Select(0, 0);}
        }

        public string Caption
        {
            get {return lblCaption.Visible ? lblCaption.Text : null;}
            set 
            {
                lblCaption.Text = Utils.NonNullString(value); 
                layoutCaption.Visibility = !string.IsNullOrEmpty(value) ? LayoutVisibility.Always : LayoutVisibility.Never;
            }
        }

        public static void ShowMemo(IWin32Window owner, string caption, string memo)
        {
            using var frmMemo = new MemoEditor()
            {
                Caption  = caption,
                MemoText = memo
            };

            frmMemo.ShowDialog(owner);
        }
    }

    #region MemoTypeEditor
    public class MemoTypeEditor: UITypeEditor
    {
        private string GetText(List<string> values)
        {
            if (values == null)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            foreach (string str in values)
            {
                if (result.Length > 0)
                    result.Append(Environment.NewLine);
                result.Append(str);
            }
            return result.ToString();
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null)
                return value;

            using (MemoEditor editor = new MemoEditor())
            {
                Type propType = context.PropertyDescriptor.PropertyType;
                bool isList = (propType == typeof(List<string>)) || propType.IsSubclassOf(typeof(List<string>));
                if (isList)
                    editor.Caption = "Enter one value per line.";

                editor.MemoText = isList ? GetText(value as List<string>) : Utils.NonNullString(value as string);
                if (edSvc.ShowDialog(editor) == DialogResult.OK)
                {
                    string text = editor.MemoText;
                    if (text == null)
                        return null;

                    if (isList)
                    {
                        string[] lines = Utils.SplitStringToLines(text);
                        if (lines == null || lines.Length <= 0)
                            return null;
                        List<string> result = //new List<string>();
                            (List<string>)Activator.CreateInstance(context.PropertyDescriptor.PropertyType);
                        foreach (string str in lines)
                        {
                            if (!string.IsNullOrEmpty(str))
                                result.Add(str);
                        }

                        return result.Count > 0 ? result : null;
                    }

                    return text;
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null && context.PropertyDescriptor != null)
            {
                Type propType = context.PropertyDescriptor.PropertyType;
                if (propType == typeof(string) || propType == typeof(List<string>) || propType.IsSubclassOf(typeof(List<string>)))
                    return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }
    }
    #endregion
}