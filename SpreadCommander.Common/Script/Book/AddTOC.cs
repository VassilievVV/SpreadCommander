using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class TocOptions: CommentOptions
    {
        [Description("Includes entries only from the portion of the document marked by the bookmark named by text in this switch's field-argument")]
        public bool Bookmarks { get; set; }

        [Description("Inserts table of contents entries as hyperlinks")]
        public bool Hyperlinks { get; set; }

        [Description("Omits page numbers from the table of contents. Page numbers are omitted from all levels unless a range of entry levels is specified. For example, '1-1' omits page numbers from level 1")]
        public string OmitPageNumbers { get; set; }

        [Description("Uses the applied paragraph outline level")]
        public bool UseParagraphOutline { get; set; }

        [Description("Builds a table of contents from paragraphs with specified outline levels. For example, '1 - 3' lists only paragraphs with outline levels 1 through 3")]
        public string ParagraphOutline { get; set; }

        [Description("Builds a table of contents from paragraphs formatted with specified styles. For example, 'Title, 1, subtitle, 2, customTitle, 3' builds a table of contents from paragraphs formatted with the styles 'Title', 'subtitle' and 'customTitle'. The number after each style name indicates the entry level corresponding to that style")]
        public string Styles { get; set; }

        [Description("Builds a table of contents from items that are numbered by SEQ fields. The sequence identifier designated by text in this switch's field-argument shall match the identifier in the corresponding SEQ field")]
        public string SeqFields { get; set; }

        [Description("For entries numbered with a SEQ field, adds a prefix to the page number. The prefix depends on the type of entry")]
        public string SeqPrefixes { get; set; }

        [Description("When used with SeqPrefix, defines the separator between sequence and page numbers. The default separator is a hyphen (-)")]
        public string Separator { get; set; }

        [Description("Builds a table of contents from TC fields. TC field identifier must exactly match the text in this switch's field-argument")]
        public string TcFields { get; set; }

        [Description("Used with TcFields. Includes TC fields that assign entries to one of the specified levels")]
        public string TcLevels { get; set; }
    }

    public class AddTocOptions: TocOptions
    {
    }

    public partial class SCBook
    {
        public void AddTOC(TocOptions options = null) =>
            ExecuteSynchronized(options, () => DoAddTOC(options));

        protected virtual void DoAddTOC(TocOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            var code = new StringBuilder("TOC", 512);

            if (options != null)
            {
                if (options.Bookmarks)
                    code.Append(" \\b");
                if (options.Hyperlinks)
                    code.Append(" \\h");
                if (!string.IsNullOrWhiteSpace(options.OmitPageNumbers))
                    code.Append($" \\n {QuoteString(options.OmitPageNumbers)}");
                if (options.UseParagraphOutline)
                    code.Append(" \\u");
                if (!string.IsNullOrWhiteSpace(options.ParagraphOutline))
                    code.Append("$ \\o {QuoteString(options.ParagraphOutline)}");
                if (!string.IsNullOrWhiteSpace(options.Styles))
                    code.Append("$ \\t {QuoteString(options.Styles)}");
                if (!string.IsNullOrWhiteSpace(options.SeqFields))
                    code.Append("$ \\c {QuoteString(options.SeqFields)}");
                if (!string.IsNullOrWhiteSpace(options.SeqPrefixes))
                    code.Append("$ \\s {QuoteString(options.SeqPrefixes)}");
                if (!string.IsNullOrWhiteSpace(options.Separator))
                    code.Append("$ \\d  {QuoteString(options.Separator)}");
                if (!string.IsNullOrWhiteSpace(options.TcFields))
                    code.Append("$ \\f  {QuoteString(options.TcFields)}");
                if (!string.IsNullOrWhiteSpace(options.TcLevels))
                    code.Append("$ \\l  {QuoteString(TcLevels)}");
            }

            var field = book.Fields.Create(book.Range.End, code.ToString());
            field.Update();

            AddComments(book, field.ResultRange, options);

            if (field?.ResultRange?.End != null)
            {
                book.CaretPosition = field.ResultRange.End;
                ScrollToCaret();
            }


            static string QuoteString(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return string.Empty;

                return value.Replace(@"""", @"\""").Replace(@"\", @"\\");
            }
        }
    }
}
