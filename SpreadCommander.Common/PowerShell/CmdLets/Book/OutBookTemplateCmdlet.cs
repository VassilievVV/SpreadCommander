using DevExpress.Snap;
using DevExpress.Snap.Core.API;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Book;
using SpreadCommander.Common.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsData.Out, "BookTemplate")]
    public class OutBookTemplateCmdlet: BaseBookWithCommentsCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "File with spreadsheet template.")]
        public string TemplateFile { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Merge parameters")]
        public Hashtable Parameters { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "If set - new spreadsheet will be generated and sent to the pipe")]
        public SwitchParameter OutputBook{ get; set; }

        [Parameter(HelpMessage = "Whether all styles contained in the template are copied to the resulting document")]
        public SwitchParameter CopyTemplateStyles { get; set; }

        [Parameter(HelpMessage = "Whether headers (footers) in the merged document have the same content in all sections")]
        public SwitchParameter DoNotLinkHeaderFooter { get; set; }

        [Parameter(HelpMessage = "Behavior of numbered lists when the mail merge operation is performed")]
        public DevExpress.Snap.Core.Options.MailMergeNumberingRestart? NumberedListRestart { get; set; }


        private readonly List<PSObject> _Output = new List<PSObject>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Output.Add(obj);
        }

        protected override void EndProcessing()
        {
            WriteTable();

            if (PassThru)
                WriteObject(_Output, true);
        }

        protected void WriteTable()
        {
            WriteTable(GetCmdletBook());
        }

        protected void WriteTable(Document book)
        {
            var dataSource = GetDataSource(_Output, DataSource, IgnoreErrors);

            if (string.IsNullOrWhiteSpace(TemplateFile))
                throw new Exception("Template file is not specified.");

            var templateFile = Project.Current.MapPath(TemplateFile);
            if (!File.Exists(templateFile))
                throw new Exception($"Template file '{TemplateFile}' does not exist.");

            MemoryStream documentData;
            using (var template = SCBook.NewSnapSCBook())
            {
                lock(LockObject)
                    template.Document.LoadDocument(templateFile);

                var mergeOptions = (template.BookServer as SnapDocumentServer).CreateSnapMailMergeExportOptions();
                mergeOptions.DataSource                 = dataSource;
                mergeOptions.DataMember                 = null;
                mergeOptions.CopyTemplateStyles         = CopyTemplateStyles;
                mergeOptions.HeaderFooterLinkToPrevious = !DoNotLinkHeaderFooter;
                if (NumberedListRestart.HasValue)
                    mergeOptions.NumberedListRestart    = NumberedListRestart.Value;

                var snapDocument = template.Document as SnapDocument;
                snapDocument.Parameters.Clear();
                if (Parameters != null)
                {
                    foreach (DictionaryEntry keyValuePair in Parameters)
                    {
                        var parameter = new Parameter()
                        {
                            Name  = Convert.ToString(keyValuePair.Key),
                            Value = keyValuePair.Value,
                            Type  = keyValuePair.Value?.GetType() ?? typeof(string)
                        };
                        snapDocument.Parameters.Add(parameter);
                    }
                }

                documentData = new MemoryStream();
                snapDocument.SnapMailMerge(mergeOptions, documentData, DocumentFormat.OpenXml);
                documentData.Seek(0, SeekOrigin.Begin);
            }

            ExecuteSynchronized(() => DoWriteBook(book, documentData));

            if (OutputBook)
            {
                documentData.Seek(0, SeekOrigin.Begin);

                var outBook = new SCBookContext();
                outBook.Document.LoadDocument(documentData, DocumentFormat.OpenXml);

                WriteObject(outBook);
            }
        }

        protected virtual void DoWriteBook(Document book, Stream mergeDocument)
        {
            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var range = book.AppendDocumentContent(mergeDocument, DocumentFormat.OpenXml);
                AddComments(book, range);
            }
        }
    }
}
