using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using SpreadCommander.Common.Code;
using Markdig;
using System.Drawing;
using SpreadCommander.Common.PowerShell.CmdLets.Book;

namespace SpreadCommander.Common.Book
{
    public partial class SCBook
    {
        protected IRichEditDocumentServer AddDocument(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE FILE' requires filename as first argument.");

            var fileName = arguments[0].Value;
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("DOCVARIABLE FILE does not contain valid filename.");

            object snippet = null;

            if (Snippets?.ContainsKey(fileName) ?? false)
            {
                snippet = Snippets[fileName];
                if (snippet is SCBook)
                {
                }
                else if (snippet is SCBookContext)
                {
                }
                else if (snippet is Image)
                {
                }
                else
                    throw new Exception($"Specified snippet '{fileName}' is not supported. Snippet shall be either Book or System.Drawing.Image.");
            }
            else
                fileName = Project.Current.MapPath(fileName);

            if (snippet == null && !File.Exists(fileName))
                throw new Exception($"File '{fileName}' does not exist.");

            bool rebuild = false;

            if (arguments.Count > 1)
            {
                var properties = Utils.SplitNameValueString(arguments[1].Value, ';');

                foreach (var prop in properties)
                {
                    if (string.IsNullOrWhiteSpace(prop.Key))
                        continue;

                    switch (prop.Key.ToLower())
                    {
#pragma warning disable CRRSP01 // A misspelled word has been found
                        case "rebuild":
                        case "recalculate":
                        case "recalc":
                            var valueRecalc = prop.Value;
                            if (string.IsNullOrWhiteSpace(valueRecalc))
                                valueRecalc = bool.TrueString;
                            rebuild = bool.Parse(valueRecalc);
                            break;
#pragma warning restore CRRSP01 // A misspelled word has been found
                    }
                }
            }

            using (new UsingProcessor(() => CheckNestedFile(fileName), () => RemoveNestedFile(fileName)))
            {
#pragma warning disable IDE0068 // Use recommended dispose pattern
                var server = new RichEditDocumentServer();
#pragma warning restore IDE0068 // Use recommended dispose pattern

                if (snippet != null)
                {
                    SCBook book = null;
                    if (snippet is SCBook)
                        book = (SCBook)snippet;
                    else if (snippet is SCBookContext)
                        book = ((SCBookContext)snippet).SCBook;

                    if (book != null)
                        server.Document.AppendDocumentContent(book.Document.Range);
                    else if (snippet is Image image)
                        server.Document.Images.Append(image);
                    else
                        throw new Exception($"Specified snippet '{fileName}' is not supported. Snippet shall be either Book or System.Drawing.Image.");
                }
                else
                {
                    var ext = Path.GetExtension(fileName)?.ToLower();
                    switch (ext)
                    {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                        case "markdown":
                        case "mdown":
                        case "md":
                            var content     = File.ReadAllText(fileName);
                            var htmlContent = Markdown.ToHtml(content);
                            server.Document.AppendHtmlText(htmlContent, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
                            break;
                        default:
                            server.LoadDocument(fileName);
                            break;
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
                    }
                }

                if (rebuild)
                {
                    server.CalculateDocumentVariable += CalculateDocumentVariable;
                    using (new UsingProcessor(() => server.CalculateDocumentVariable += CalculateDocumentVariable,
                        () => server.CalculateDocumentVariable -= CalculateDocumentVariable))
                    {
                        server.Document.UpdateAllFields();
                    }
                }

                return server;
            }
        }
    }
}
