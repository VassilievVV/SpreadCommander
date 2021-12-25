using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class NewCharacterStyleOptions: CharacterStyleOptions
    {
        [Description("Name of the style.")]
        public string Name { get; set; }

        [Description("Replace existing style if it exists")]
        public bool Replace { get; set; }

        [Description("Linked style for the current style")]
        public string LinkedStyle { get; set; }

        [Description("Style form which the current style inherits")]
        public string Parent { get; set; }

        [Description("Whether the style is primary for the document.")]
        public bool Primary { get; set; }
    }

    public class AddCharacterStyleOptions: NewCharacterStyleOptions
    {
    }

    public partial class SCBook
    {
        public SCBook AddCharacterStyle(NewCharacterStyleOptions options)
        {
            ExecuteSynchronized(options, () => DoAddCharacterStyle(options));
            return this;
        }

        protected void DoAddCharacterStyle(NewCharacterStyleOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            var book = options.Book?.Document ?? Document;

            if (string.IsNullOrWhiteSpace(options.Name))
                throw new Exception("Character style name must not be empty.");

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var style = book.CharacterStyles[options.Name];
                if (!options.Replace)
                    throw new Exception($"Character style '{options.Name}' already exist.");

                bool isNewStyle = (style == null);
                if (isNewStyle)
                {
                    style      = book.CharacterStyles.CreateNew();
                    style.Name = options.Name;
                }
                else
                    style.Reset();

                //Assigning a style via the LinkedStyle property results in overwriting the corresponding settings of the style to which the style becomes linked. 
                //So you are advised to link styles first, and specify their settings afterwards.
                if (!string.IsNullOrWhiteSpace(options.LinkedStyle))
                {
                    var linkedStyle = book.ParagraphStyles[options.LinkedStyle] ?? throw new Exception($"Paragraph style '{options.LinkedStyle}' does not exist.");
                    style.LinkedStyle = linkedStyle;
                }

                if (!string.IsNullOrWhiteSpace(options.Parent))
                {
                    var parent   = book.CharacterStyles[options.Parent] ?? throw new Exception($"Character style '{options.Parent}' does not exist.");
                    style.Parent = parent;
                }

                style.Primary = options.Primary;

                this.SetCharacterOptions(style, options);

                if (isNewStyle)
                    book.CharacterStyles.Add(style);
            }
        }
    }
}
