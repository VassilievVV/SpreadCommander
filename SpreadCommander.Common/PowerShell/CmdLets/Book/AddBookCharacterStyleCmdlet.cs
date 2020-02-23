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
    [Cmdlet(VerbsCommon.Add, "BookCharacterStyle")]
    public class AddBookCharacterStyleCmdlet: BaseBookCharacterPropertiesCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of the style")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "Replace existing style if it exists")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Linked style for the current style")]
        public string LinkedStyle { get; set; }

        [Parameter(HelpMessage = "Style form which the current style inherits")]
        public string Parent { get; set; }


        protected override void DoProcessRecord(Document book)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new Exception("Character style name must not be empty.");

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var style = book.CharacterStyles[Name];
                if (!Replace)
                    throw new Exception($"Character style '{Name}' already exist.");

                bool isNewStyle = (style == null);
                if (isNewStyle)
                {
                    style = book.CharacterStyles.CreateNew();
                    style.Name = Name;
                }
                else
                    style.Reset();

                //Assigning a style via the LinkedStyle property results in overwriting the corresponding settings of the style to which the style becomes linked. 
                //So you are advised to link styles first, and specify their settings afterwards.
                if (!string.IsNullOrWhiteSpace(LinkedStyle))
                {
                    var linkedStyle = book.ParagraphStyles[LinkedStyle] ?? throw new Exception($"Paragraph style '{LinkedStyle}' does not exist.");
                    style.LinkedStyle = linkedStyle;
                }

                if (!string.IsNullOrWhiteSpace(Parent))
                {
                    var parent = book.CharacterStyles[Parent] ?? throw new Exception($"Character style '{Parent}' does not exist.");
                    style.Parent = parent;
                }

                base.SetCharacterProperties(style);

                if (isNewStyle)
                    book.CharacterStyles.Add(style);
            }
        }
    }
}
