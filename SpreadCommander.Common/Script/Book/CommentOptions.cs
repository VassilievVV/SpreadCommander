using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class CommentOptions: BookOptions
    {
        [Description("Comment for the text.")]
        public string Comment { get; set; }

        [Description("If set - Comment is treated as HTML")]
        public bool CommentHtml { get; set; }

        [Description("Bookmark in the document")]
        public string Bookmark { get; set; }

        [Description("Hyperlink to existing bookmark")]
        public string Hyperlink { get; set; }

        [Description("Text for the tooltip displayed when the mouse hovers over a hyperlink")]
        public string HyperlinkTooltip { get; set; }

        [Description("Target window or frame in which to display the web page content when the hyperlink is clicked")]
        public string HyperlinkTarget { get; set; }
    }
}
