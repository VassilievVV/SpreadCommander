using Alsing.Windows.Forms.Document;
using Alsing.Windows.Forms.Document.SyntaxDefinition;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using System.Collections;
using System.IO;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "SyntaxText")]
    public class WriteSyntaxTextCmdlet : BaseBookWithCommentsCmdlet
    {
        #region SyntaxName
        public enum SyntaxName
        {
            CSharp,
            CPlusPlus,
            Delphi,
            DOSBatch,
            FSharp,
            Fortran90,
            Java,
            JavaScript,
            LotusScript,
            MSIL,
            Perl,
            PHP,
            PowerShell,
            Python,
            R,
            SQL,
            VBNet,
            XML
        }
        #endregion

        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Text to write into book")]
        public string[] Text { get; set; }

        [Parameter(Position = 1, HelpMessage = "Syntax name used for the text")]
        public SyntaxName? Syntax { get; set; }

        [Parameter(HelpMessage = "Write each string individually, without using cache")]
        public SwitchParameter Stream { get; set; }

        [Parameter(HelpMessage = "Add line breaks after each line or no")]
        [Alias("NoBreaks")]
        public SwitchParameter NoLineBreaks { get; set; }

        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }


        private readonly StringBuilder _Output = new StringBuilder();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var text = Text;
            if (text == null || text.Length <= 0)
                text = new string[] { string.Empty };

            foreach (var line in text)
            {
                if (line != null)
                    _Output.Append(line);
                if (!NoLineBreaks)
                    _Output.AppendLine();

                if (Stream)
                    WriteBuffer(false);
            }
        }

        protected override void EndProcessing()
        {
            WriteBuffer(true);
        }

        protected void WriteBuffer(bool lastBlock)
        {
            WriteText(GetCmdletBook(), _Output, lastBlock);
            _Output.Clear();
        }

        protected void WriteText(Document book, StringBuilder buffer, bool lastBlock)
        {
            if (buffer.Length <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteText(book, buffer, lastBlock));
        }

        protected virtual void DoWriteText(Document book, StringBuilder buffer, bool lastBlock)
        {
            if (buffer.Length <= 0 && !lastBlock)
                return;

            string syntaxName = null;
            if (Syntax.HasValue)
            {
                switch (Syntax.Value)
                {
                    case SyntaxName.CSharp:
                        syntaxName = "C#";
                        break;
                    case SyntaxName.CPlusPlus:
                        syntaxName = "C++";
                        break;
                    case SyntaxName.Delphi:
                        syntaxName = "Delphi";
                        break;
                    case SyntaxName.DOSBatch:
                        syntaxName = "DOSBatch";
                        break;
                    case SyntaxName.FSharp:
                        syntaxName = "F#";
                        break;
                    case SyntaxName.Fortran90:
                        syntaxName = "Fortran90";
                        break;
                    case SyntaxName.Java:
                        syntaxName = "Java";
                        break;
                    case SyntaxName.JavaScript:
                        syntaxName = "JavaScript";
                        break;
                    case SyntaxName.LotusScript:
                        syntaxName = "LotusScript";
                        break;
                    case SyntaxName.MSIL:
                        syntaxName = "MSIL";
                        break;
                    case SyntaxName.Perl:
                        syntaxName = "Perl";
                        break;
                    case SyntaxName.PHP:
                        syntaxName = "PHP";
                        break;
                    case SyntaxName.PowerShell:
                        syntaxName = "PowerShell";
                        break;
                    case SyntaxName.Python:
                        syntaxName = "Python";
                        break;
                    case SyntaxName.R:
                        syntaxName = "R";
                        break;
                    case SyntaxName.SQL:
                        syntaxName = "SqlScript";
                        break;
                    case SyntaxName.VBNet:
                        syntaxName = "VB.Net";
                        break;
                    case SyntaxName.XML:
                        syntaxName = "XML";
                        break;
                }
            }

            book.BeginUpdate();
            try
            {
                var text = buffer.ToString();
                string rtfText;
                DocumentRange range;

                if (!string.IsNullOrWhiteSpace(syntaxName))
                {
                    using (var syntaxDocument = new SyntaxDocument())
                    {
                        SyntaxDefinition syntaxDefinition = LoadSyntax(syntaxName);

                        syntaxDocument.Parser.Init(syntaxDefinition);
                        syntaxDocument.ReParse();

                        syntaxDocument.Text = text;
                        rtfText = syntaxDocument.ExportToRTF(null, "Lucida Console");
                    }

                    range = book.AppendRtfText(rtfText, InsertOptions.KeepSourceFormatting);
                }
                else
                    range = book.AppendText(text);                

                if (!string.IsNullOrWhiteSpace(ParagraphStyle))
                {
                    var style = book.ParagraphStyles[ParagraphStyle] ?? throw new Exception($"Paragraph style '{ParagraphStyle}' does not exist.");
                    var pp = book.BeginUpdateParagraphs(range);
                    try
                    {
                        pp.Style = style;
                    }
                    finally
                    {
                        book.EndUpdateParagraphs(pp);
                    }
                }

                if (lastBlock)
                    AddComments(book, range);

                if (range?.End != null)
                {
                    book.CaretPosition = range.End;
                    ScrollToCaret();
                }
            }
            finally
            {
                ResetBookFormatting(book);
                book.EndUpdate();
            }


            SyntaxDefinition LoadSyntax(string syntaxFileName)
            {
                if (string.IsNullOrWhiteSpace(syntaxFileName))
                    return null;

                using Stream streamSyntax = Utils.GetEmbeddedResource(System.Reflection.Assembly.GetAssembly(this.GetType()), $"SyntaxFiles.{syntaxFileName}.syn");
                streamSyntax.Seek(0, SeekOrigin.Begin);

                using StreamReader reader = new StreamReader(streamSyntax);
                var syntax = SyntaxDefinition.FromSyntaxXml(reader.ReadToEnd());
                return syntax;
            }
        }
    }
}
