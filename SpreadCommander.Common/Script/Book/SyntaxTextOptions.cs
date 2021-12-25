using Alsing.Windows.Forms.Document;
using Alsing.Windows.Forms.Document.SyntaxDefinition;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Script.Book
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

    public class SyntaxTextOptions : CommentOptions
    {
        [Description("Syntax name used for the text")]
        public SyntaxName? Syntax { get; set; }

        [Description("Add line breaks after each line or no")]
        public bool NoLineBreaks { get; set; }

        [Description("Paragraph style")]
        public string ParagraphStyle { get; set; }
    }

    public partial class SCBook
    {
        public SCBook WriteSyntaxText(string text, SyntaxName syntaxName, SyntaxTextOptions options = null)
        {
            ExecuteSynchronized(options, () => DoWriteSyntaxText(text, syntaxName, options));
            return this;
        }

        protected void DoWriteSyntaxText(string text, SyntaxName syntaxName, SyntaxTextOptions options)
        {
            options ??= new SyntaxTextOptions();

            var book = options.Book?.Document ?? Document;

            string strSyntax = null;
            switch (syntaxName)
            {
                case SyntaxName.CSharp:
                    strSyntax = "C#";
                    break;
                case SyntaxName.CPlusPlus:
                    strSyntax = "C++";
                    break;
                case SyntaxName.Delphi:
                    strSyntax = "Delphi";
                    break;
                case SyntaxName.DOSBatch:
                    strSyntax = "DOSBatch";
                    break;
                case SyntaxName.FSharp:
                    strSyntax = "F#";
                    break;
                case SyntaxName.Fortran90:
                    strSyntax = "Fortran90";
                    break;
                case SyntaxName.Java:
                    strSyntax = "Java";
                    break;
                case SyntaxName.JavaScript:
                    strSyntax = "JavaScript";
                    break;
                case SyntaxName.LotusScript:
                    strSyntax = "LotusScript";
                    break;
                case SyntaxName.MSIL:
                    strSyntax = "MSIL";
                    break;
                case SyntaxName.Perl:
                    strSyntax = "Perl";
                    break;
                case SyntaxName.PHP:
                    strSyntax = "PHP";
                    break;
                case SyntaxName.PowerShell:
                    strSyntax = "PowerShell";
                    break;
                case SyntaxName.Python:
                    strSyntax = "Python";
                    break;
                case SyntaxName.R:
                    strSyntax = "R";
                    break;
                case SyntaxName.SQL:
                    strSyntax = "SqlScript";
                    break;
                case SyntaxName.VBNet:
                    strSyntax = "VB.Net";
                    break;
                case SyntaxName.XML:
                    strSyntax = "XML";
                    break;
            }

            book.BeginUpdate();
            try
            {
                string rtfText;
                DocumentRange range;

                if (!string.IsNullOrWhiteSpace(strSyntax))
                {
                    using (var syntaxDocument = new SyntaxDocument())
                    {
                        SyntaxDefinition syntaxDefinition = LoadSyntax(strSyntax);

                        syntaxDocument.Parser.Init(syntaxDefinition);
                        syntaxDocument.ReParse();

                        syntaxDocument.Text = text;
                        rtfText = syntaxDocument.ExportToRTF(null, "Lucida Console");
                    }

                    range = book.AppendRtfText(rtfText, InsertOptions.KeepSourceFormatting);
                }
                else
                    range = book.AppendText(text);

                if (!string.IsNullOrWhiteSpace(options.ParagraphStyle))
                {
                    var style = book.ParagraphStyles[options.ParagraphStyle] ?? throw new Exception($"Paragraph style '{options.ParagraphStyle}' does not exist.");
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

                Script.Book.SCBook.AddComments(book, range, options);
                WriteRangeToConsole(book, range);

                if (range?.End != null)
                {
                    book.CaretPosition = range.End;
                    Script.Book.SCBook.ResetBookFormatting(book, range.End);
                    ScrollToCaret();
                }
            }
            finally
            {
                book.EndUpdate();
            }


            SyntaxDefinition LoadSyntax(string syntaxFileName)
            {
                if (string.IsNullOrWhiteSpace(syntaxFileName))
                    return null;

                using var streamSyntax = Utils.GetEmbeddedResource(System.Reflection.Assembly.GetAssembly(this.GetType()), $"SyntaxFiles.{syntaxFileName}.syn");
                streamSyntax.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(streamSyntax);
                var syntax = SyntaxDefinition.FromSyntaxXml(reader.ReadToEnd());
                return syntax;
            }
        }
    }
}
