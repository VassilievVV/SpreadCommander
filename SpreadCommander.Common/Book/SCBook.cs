using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WpfMath;
using WpfMath.Converters;

namespace SpreadCommander.Common.Book
{
    public partial class SCBook: IDisposable
    {
        private readonly bool _OwnRichEdit = true;

        public IRichEditDocumentServer BookServer { get; private set; }
        public Document Document => BookServer?.Document;

        public IWorkbook DefaultSpreadsheet { get; set; }

        public bool NeedSynchronizeDefaultSpreadsheet { get; set; }

        private readonly StringNoCaseDictionary<int> _NestedFiles = new StringNoCaseDictionary<int>();

        public SCBook()
        {
            BookServer = new RichEditDocumentServer();
            InitializeRichEdit();
        }

        public SCBook(IRichEditDocumentServer server)
        {
            _OwnRichEdit = false;
            BookServer   = server;
            InitializeRichEdit();
        }

        /*
        public static SCBook NewSnapSCBook()
        {
            var server = new SnapDocumentServer();
            var result = new SCBook(server);
            return result;
        }
        */

        public static SCBook CreateHelperBook()
        {
            var result = new SCBook();
            return result;
        }

        public void Dispose()
        {
            if (BookServer != null)
            {
                BookServer.CalculateDocumentVariable -= CalculateDocumentVariable;

                if (!_OwnRichEdit)
                    BookServer?.Dispose();
            }

            BookServer = null;
        }

        public StringNoCaseDictionary<object> Snippets { get; set; }

        protected void InitializeRichEdit()
        {
            if (BookServer == null)
                return;

            BookServer.Options.DocumentCapabilities.OleObjects = DocumentCapability.Disabled;
            BookServer.Options.DocumentCapabilities.Macros     = DocumentCapability.Disabled;

            ProjectUriStreamProvider.RegisterProvider(BookServer);
            BookServer.CalculateDocumentVariable += CalculateDocumentVariable;
        }

        protected void CheckNestedFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            if (_NestedFiles.ContainsKey(fileName))
                throw new Exception($"Circular reference detected. Filename - '{fileName}'.");
            else
                _NestedFiles.Add(fileName, 1);
        }

        protected void RemoveNestedFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            if (_NestedFiles.ContainsKey(fileName))
                _NestedFiles.Remove(fileName);
        }

        protected void CalculateDocumentVariable(object sender, CalculateDocumentVariableEventArgs e)
        {
            if (sender == BookServer) //Check that on first call sender == RichEdit
                _NestedFiles.Clear();

            e.Value = CalculateDocumentVariable(e.VariableName, e.Arguments);
            e.Handled = e.Value != null;

            if (sender == BookServer)
                _NestedFiles.Clear();
        }

        public IRichEditDocumentServer CalculateDocumentVariable(string variableName, ArgumentCollection arguments)
        {
            IRichEditDocumentServer result = (Utils.NonNullString(variableName).ToLower()) switch
            {
                "file" or "document" => AddDocument(arguments),
                "image" or "picture" => AddImage(arguments),
                "svg"                => AddSvg(arguments),
                "latex" or "formula" => AddLatex(arguments),
                "spreadtable"        => AddSpreadTable(arguments),
                "spreadchart"        => AddSpreadChart(arguments),
                "spreadpivot"        => AddSpreadPivot(arguments),
                "footnote"           => AddFootNote(arguments),
                "endnote"            => AddEndNote(arguments),
                //Do not allow yet to execute scripts from Book for security reasons
                //"script"           => AddScript(arguments),
                _ => throw new Exception($"Invalid variable name for DOCVARIABLE: {variableName}")
            };
            return result;
        }

        protected static Size ParseSize(string sizeValue)
        {
            var reSize = new Regex(@"^\s*(?<Width>\d+)\s*x\s*(?<Height>\d+)\s*$");
            var match = reSize.Match(sizeValue);
            if (!match.Success)
                throw new Exception($"Invalid size: {sizeValue}.");

            var width  = int.Parse(match.Groups["Width"].Value, CultureInfo.InvariantCulture);
            var height = int.Parse(match.Groups["Height"].Value, CultureInfo.InvariantCulture);

            return new Size(width, height);
        }
    }
}
