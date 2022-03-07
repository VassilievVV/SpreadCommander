using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class NewParagraphStyleOptions: ParagraphStyleOptions
    {
        [Description("Name of the style")]
        public string Name { get; set; }

        [Description("Replace existing style if it exists")]
        public bool Replace { get; set; }

        [Description("Linked style for the current style")]
        public string LinkedStyle { get; set; }

        [Description("Default style for a paragraph that immediately follows the current paragraph")]
        public string NextStyle { get; set; }

        [Description("Style form which the current style inherits")]
        public string Parent { get; set; }

        [Description("Whether the style is primary for the document.")]
        public bool Primary { get; set; }
    }

    public class AddParagraphStyleOptions: NewParagraphStyleOptions
    {
    }

    public partial class SCBook
    {
        public void AddParagraphStyle(NewParagraphStyleOptions options) =>
            ExecuteSynchronized(options, () => DoAddParagraphStyle(options));

        protected void DoAddParagraphStyle(NewParagraphStyleOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            var book = options.Book?.Document ?? Document;

            if (string.IsNullOrWhiteSpace(options.Name))
                throw new Exception("Character style name must not be empty.");

            var style = book.ParagraphStyles[options.Name];
            if (!options.Replace)
                throw new Exception($"Paragraph style '{options.Name}' already exist.");

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                bool isNewStyle = (style == null);
                if (isNewStyle)
                {
                    style      = book.ParagraphStyles.CreateNew();
                    style.Name = options.Name;
                }
                else
                    ((ParagraphPropertiesBase)style).Reset();

                //Assigning a style via the LinkedStyle property results in overwriting the corresponding settings of the style to which the style becomes linked. 
                //So you are advised to link styles first, and specify their settings afterwards.
                if (!string.IsNullOrWhiteSpace(options.LinkedStyle))
                {
                    var linkedStyle   = book.CharacterStyles[options.LinkedStyle] ?? throw new Exception($"Character style '{options.LinkedStyle}' does not exist.");
                    style.LinkedStyle = linkedStyle;
                }

                if (!string.IsNullOrWhiteSpace(options.Parent))
                {
                    var parent   = book.ParagraphStyles[options.Parent] ?? throw new Exception($"Paragraph style '{options.Parent}' does not exist.");
                    style.Parent = parent;
                }

                if (!string.IsNullOrWhiteSpace(options.NextStyle))
                {
                    var nextStyle   = book.ParagraphStyles[options.NextStyle] ?? throw new Exception($"Paragraph style '{options.NextStyle}' does not exist.");
                    style.NextStyle = nextStyle;
                }

                style.Primary = options.Primary;

                this.SetParagraphOptions(style, options);

                if (isNewStyle)
                    book.ParagraphStyles.Add(style);
            }
        }
    }
}
