using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    public class BaseBookWithCommentsCmdlet: BaseBookCmdlet
    {
        [Parameter(HelpMessage = "Comment for the text.")]
        public string Comment { get; set; }

        [Parameter(HelpMessage = "If set - Comment is treated as HTML")]
        public SwitchParameter CommentHtml { get; set; }

        [Parameter(HelpMessage = "Bookmark in the document")]
        public string Bookmark { get; set; }

        [Parameter(HelpMessage = "Hyperlink to existing bookmark")]
        public string Hyperlink { get; set; }

        [Parameter(HelpMessage = "Text for the tooltip displayed when the mouse hovers over a hyperlink")]
        public string HyperlinkTooltip { get; set; }

        [Parameter(HelpMessage = "Target window or frame in which to display the web page content when the hyperlink is clicked")]
        public string HyperlinkTarget { get; set; }


        protected void AddComments(Document book, DocumentRange range)
        {
            var comment = Comment;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var bookComment = book.Comments.Create(range, Environment.UserName);
                var docComment = bookComment.BeginUpdate();
                try
                {
                    if (CommentHtml)
                        docComment.AppendHtmlText(comment, InsertOptions.KeepSourceFormatting);
                    else
                        docComment.AppendText(comment);
                }
                finally
                {
                    bookComment.EndUpdate(docComment);
                }
            }

            var bookmark = Bookmark;
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                var oldBookmark = book.Bookmarks[bookmark];
                if (oldBookmark != null)
                    book.Bookmarks.Remove(oldBookmark);

                book.Bookmarks.Create(range, bookmark);
            }

            var hyperlink = Hyperlink;
            if (!string.IsNullOrWhiteSpace(hyperlink))
            {
                var hyperlinkBookmark = book.Bookmarks[hyperlink];
                var link = book.Hyperlinks.Create(range);
                if (hyperlinkBookmark != null)
                    link.Anchor = hyperlinkBookmark.Name;
                else
                    link.NavigateUri = hyperlink;

                if (!string.IsNullOrWhiteSpace(HyperlinkTooltip))
                    link.ToolTip = HyperlinkTooltip;

                if (!string.IsNullOrWhiteSpace(HyperlinkTarget))
                    link.Target = HyperlinkTarget;
            }
        }
    }
}
