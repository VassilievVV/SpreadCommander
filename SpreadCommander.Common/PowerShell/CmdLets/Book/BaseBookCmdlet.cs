#pragma warning disable CRR0050

using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.API.Native.Implementation;
using SpreadCommander.Common.Book;
using SpreadCommander.Common.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    public class BaseBookCmdlet: SCCmdlet
    {
        public const int DefaultDPI = 300;

        [Parameter(HelpMessage = "Target book. By default - write into host's book")]
        public SCBookContext Book { get; set; }

        protected IRichEditDocumentServer GetCmdletBookServer() => Book?.BookServer ?? HostBookServer;
        protected Document GetCmdletBook()
        {
            var result = Book?.Document ?? HostBook;
            if (result != null)
                result.Unit = DevExpress.Office.DocumentUnit.Document;
            return result;
        }

        protected override bool NeedSynchronization() => (Book == null || Book == HostBook);

        public virtual void ScrollToCaret()
        {
            ExternalHost?.ScrollToCaret();
        }

        protected override void DoReportError(string errorMessage)
        {
            base.DoReportError(errorMessage);
            ScrollToCaret();
        }

        protected void ResetBookFormatting(Document book)
        {
            var cp = book.BeginUpdateCharacters(book.CreateRange(book.Range.End, 0));
            try
            {
                cp.Reset();
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }
        }

        protected virtual void ExpandFieldsInBookRange(DocumentRange range, Hashtable snippets = null)
        {
            var doc = range.BeginUpdateDocument();
            try
            {
                var fieldRanges = doc.FindAll(new Regex("(?:{{|(?:{(?:}}|[^}])*}(?!})))"), range);
                if (fieldRanges == null || fieldRanges.Length <= 0)
                    return;

                foreach (var fieldRange in fieldRanges)
                {
                    var fieldText = doc.GetText(fieldRange);
                    if (fieldText == "{{")
                    {
                        doc.Replace(fieldRange, "{");
                        continue;
                    }
                    
                    var code = Utils.TrimString(UnpackField(fieldText));

                    if (code.StartsWith("#"))   //Fields starting with # - #FILE, #IMAGE, #LATEX etc. - calculate field and insert its content instead of field itself
                    {
                        var codeName = Regex.Match(code, @"(?<=^#)[\w_\-]+").Value;

                        using var book        = new SCBook() 
                        { 
                            DefaultSpreadsheet                = this.HostSpreadsheet, 
                            NeedSynchronizeDefaultSpreadsheet = true, 
                            Snippets                          = ConvertSnippets() 
                        } ;
                        var helperFieldRange  = book.Document.AppendText($"DOCVARIABLE {code.Substring(1)}");
                        var helperField       = book.Document.Fields.Create(helperFieldRange);
                        helperField.ShowCodes = true;
                        helperField.Update();

                        var helperRangeBytes = book.Document.GetOpenXmlBytes(helperField.ResultRange);

                        //Insert generated content into document
                        doc.Delete(fieldRange);

                        if (helperRangeBytes != null && helperRangeBytes.Length > 0)
                        {
                            using var stream = new MemoryStream(helperRangeBytes);
                            var insRange = doc.InsertDocumentContent(fieldRange.Start, stream, DocumentFormat.OpenXml);
                            
                            //For FOOTNOTE and ENDNOTE - delete final line-break
                            if (string.Compare(codeName, "footnote", true) == 0 || string.Compare(codeName, "endnote", true) == 0)
                                doc.Delete(doc.CreateRange(insRange.End.ToInt() - 1, 1));
                        }
                    }
                    else
                    {
                        doc.Delete(fieldRange);
                        var field = doc.Fields.Create(fieldRange.Start, code);
                        field.Update();
                    }
                }
            }
            finally
            {
                range.EndUpdateDocument(doc);
            }


            static string UnpackField(string value)
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                value = value.Trim();
                if (value.Length <= 1)
                    return Utils.NonNullString(value);

                switch (value[0])
                {
                    case '{':
                        if (value[^1] == '}')
                            return value[1..^1].Replace("}}", "}");
                        break;
                }

                return value;
            }

            StringNoCaseDictionary<object> ConvertSnippets()
            {
                if (snippets == null)
                    return null;

                var result = new StringNoCaseDictionary<object>();

                foreach (DictionaryEntry keyValuePair in snippets)
                {
                    var value = keyValuePair.Value;
                    if (value is PSObject psObject)
                        value = psObject.BaseObject;
                    
                    result[Convert.ToString(keyValuePair.Key)] = value;
                }

                return result;
            }

        }
    }
}
