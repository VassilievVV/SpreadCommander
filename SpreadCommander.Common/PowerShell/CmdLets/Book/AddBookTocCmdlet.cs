using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Add, "BookTOC")]
    public class AddBookTocCmdlet: BaseBookWithCommentsCmdlet
    {
        [Parameter(HelpMessage = "Includes entries only from the portion of the document marked by the bookmark named by text in this switch's field-argument")]
        [Alias("b")]
        public SwitchParameter Bookmarks { get; set; }

        [Parameter(HelpMessage = "Inserts table of contents entries as hyperlinks")]
        [Alias("h")]
        public SwitchParameter Hyperlinks { get; set; }

        [Parameter(HelpMessage = "Omits page numbers from the table of contents. Page numbers are omitted from all levels unless a range of entry levels is specified. For example, '1-1' omits page numbers from level 1")]
        [Alias("n")]
        public string OmitPageNumbers { get; set; }

        [Parameter(HelpMessage = "Uses the applied paragraph outline level")]
        [Alias("u")]
        public SwitchParameter UseParagraphOutline { get; set; }

        [Parameter(HelpMessage = "Builds a table of contents from paragraphs with specified outline levels. For example, '1 - 3' lists only paragraphs with outline levels 1 through 3")]
        [Alias("o")]
        public string ParagraphOutline { get; set; }

        [Parameter(HelpMessage = "Builds a table of contents from paragraphs formatted with specified styles. For example, 'Title, 1, subtitle, 2, customTitle, 3' builds a table of contents from paragraphs formatted with the styles 'Title', 'subtitle' and 'customTitle'. The number after each style name indicates the entry level corresponding to that style")]
        [Alias("t")]
        public string Styles { get; set; }

        [Parameter(HelpMessage = "Builds a table of contents from items that are numbered by SEQ fields. The sequence identifier designated by text in this switch's field-argument shall match the identifier in the corresponding SEQ field")]
        [Alias("c")]
        public string SeqFields { get; set; }

        [Parameter(HelpMessage = "For entries numbered with a SEQ field, adds a prefix to the page number. The prefix depends on the type of entry")]
        [Alias("s")]
        public string SeqPrefixes { get; set; }

        [Parameter(HelpMessage = "When used with SeqPrefix, defines the separator between sequence and page numbers. The default separator is a hyphen (-)")]
        [Alias("d")]
        public string Separator { get; set; }

        [Parameter(HelpMessage = "Builds a table of contents from TC fields. TC field identifier must exactly match the text in this switch's field-argument")]
        [Alias("f")]
        public string TcFields { get; set; }

        [Parameter(HelpMessage = "Used with TcFields. Includes TC fields that assign entries to one of the specified levels")]
        public string TcLevels { get; set; }


        protected override void ProcessRecord()
        {
            var book = GetCmdletBook();

            var code = new StringBuilder("TOC", 512);

            if (Bookmarks)
                code.Append(" \\b");
            if (Hyperlinks)
                code.Append(" \\h");
            if (!string.IsNullOrWhiteSpace(OmitPageNumbers))
                code.Append($" \\n {QuoteString(OmitPageNumbers)}");
            if (UseParagraphOutline)
                code.Append(" \\u");
            if (!string.IsNullOrWhiteSpace(ParagraphOutline))
                code.Append("$ \\o {QuoteString(ParagraphOutline)}");
            if (!string.IsNullOrWhiteSpace(Styles))
                code.Append("$ \\t {QuoteString(Styles)}");
            if (!string.IsNullOrWhiteSpace(SeqFields))
                code.Append("$ \\c {QuoteString(SeqFields)}");
            if (!string.IsNullOrWhiteSpace(SeqPrefixes))
                code.Append("$ \\s {QuoteString(SeqPrefixes)}");
            if (!string.IsNullOrWhiteSpace(Separator))
                code.Append("$ \\d  {QuoteString(Separator)}");
            if (!string.IsNullOrWhiteSpace(TcFields))
                code.Append("$ \\f  {QuoteString(TcFields)}");
            if (!string.IsNullOrWhiteSpace(TcLevels))
                code.Append("$ \\l  {QuoteString(TcLevels)}");

            ExecuteSynchronized(() => DoWriteToc(book, code.ToString()));


            static string QuoteString(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return string.Empty;

                return value.Replace(@"""", @"\""").Replace(@"\", @"\\");
            }
        }

        protected void DoWriteToc(Document book, string code)
        {
            var field = book.Fields.Create(book.Range.End, code);
            field.Update();
            AddComments(book, field.ResultRange);

            if (field?.ResultRange?.End != null)
            {
                book.CaretPosition = field.ResultRange.End;
                ScrollToCaret();
            }
        }
    }
}
