using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using System.Drawing;
using System.Globalization;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Add, "BookParagraphStyle")]
    public class AddBookParagraphStyleCmdlet : BaseBookParagraphPropertiesCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of the style")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "Replace existing style if it exists")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Linked style for the current style")]
        public string LinkedStyle { get; set; }

        [Parameter(HelpMessage = "Default style for a paragraph that immediately follows the current paragraph")]
        public string NextStyle { get; set; }

        [Parameter(HelpMessage = "Style form which the current style inherits")]
        public string Parent { get; set; }

        [Parameter(HelpMessage = "Whether the style is primary for the document.")]
        public SwitchParameter Primary { get; set; }


        protected override void DoProcessRecord(Document book)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new Exception("Character style name must not be empty.");

            var style = book.ParagraphStyles[Name];
            if (!Replace)
                throw new Exception($"Paragraph style '{Name}' already exist.");

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                bool isNewStyle = (style == null);
                if (isNewStyle)
                {
                    style = book.ParagraphStyles.CreateNew();
                    style.Name = Name;
                }
                else
                    ((ParagraphPropertiesBase)style).Reset();

                //Assigning a style via the LinkedStyle property results in overwriting the corresponding settings of the style to which the style becomes linked. 
                //So you are advised to link styles first, and specify their settings afterwards.
                if (!string.IsNullOrWhiteSpace(LinkedStyle))
                {
                    var linkedStyle = book.CharacterStyles[LinkedStyle] ?? throw new Exception($"Character style '{LinkedStyle}' does not exist.");
                    style.LinkedStyle = linkedStyle;
                }

                if (!string.IsNullOrWhiteSpace(Parent))
                {
                    var parent = book.ParagraphStyles[Parent] ?? throw new Exception($"Paragraph style '{Parent}' does not exist.");
                    style.Parent = parent;
                }

                if (!string.IsNullOrWhiteSpace(NextStyle))
                {
                    var nextStyle = book.ParagraphStyles[NextStyle] ?? throw new Exception($"Paragraph style '{NextStyle}' does not exist.");
                    style.NextStyle = nextStyle;
                }

                style.Primary = Primary;

                base.SetParagraphProperties(style);

                if (isNewStyle)
                    book.ParagraphStyles.Add(style);
            }
        }
    }
}
