using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using DevExpress.XtraRichEdit;
using DevExpress.XtraSpreadsheet.Export;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Book
{
    public partial class InternalBook
    {
        protected IRichEditDocumentServer AddSpreadTable(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE SPREADTABLE' requires filename as first argument.");
            if (arguments.Count <= 1)
                throw new Exception("'DOCVARIABLE SPREADTABLE' requires table or range as second argument.");

            var fileName = arguments[0].Value;
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("DOCVARIABLE SPREADTABLE does not contain valid filename.");

            object snippet = null;

            if (Snippets?.ContainsKey(fileName) ?? false)
            {
                snippet = Snippets[fileName];
                if (snippet is SCSpreadsheetContext)
                {
                }
                else
                    throw new Exception($"Specified snippet '{fileName}' is not supported. Snippet shall be Spreadsheet.");
            }
            else if (string.Compare(fileName, "$SPREAD", true) == 0)
            {
                //Do nothing
            }
            else
                fileName = Project.Current.MapPath(fileName);

            if (snippet == null && string.Compare(fileName, "$SPREAD", true) != 0 && !File.Exists(fileName))
                throw new Exception($"File '{fileName}' does not exist.");

            var tableName = arguments[1].Value;

            bool rebuild = false;

            if (arguments.Count > 2)
            {
                var properties = Utils.SplitNameValueString(arguments[2].Value, ';');

                foreach (var prop in properties)
                {
                    if (string.IsNullOrWhiteSpace(prop.Key))
                        continue;

                    switch (prop.Key.ToLower())
                    {
                        case "rebuild":
                        case "recalculate":
                        case "recalc":
                            var valueRebuild = prop.Value;
                            if (string.IsNullOrWhiteSpace(valueRebuild))
                                valueRebuild = bool.TrueString;
                            rebuild = bool.Parse(valueRebuild);
                            break;
                    }
                }
            }

            bool needDispose = true;
            IWorkbook workbook = null;
            try
            {
                //using var workbook = SpreadsheetUtils.CreateWorkbook();
                if (snippet is SCSpreadsheetContext spreadsheetContext)
                {
                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.Append(spreadsheetContext.Workbook);
                }
                else if (string.Compare(fileName, "$SPREAD", true) == 0)
                {
                    needDispose = false;
                    workbook    = this.DefaultSpreadsheet;

                    if (workbook == null)
                        throw new Exception("Current script does not support default (Host) spreadsheet.");
                }
                else
                {
                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.LoadDocument(fileName);
                }

                IRichEditDocumentServer result = null;
                if (workbook == this.DefaultSpreadsheet && NeedSynchronizeDefaultSpreadsheet)
                    SCDispatcherService.UIDispatcherServer.Invoke(() => ProcessWorkbook(workbook, tableName, rebuild, out result));
                else
                    ProcessWorkbook(workbook, tableName, rebuild, out result);
                return result;
            }
            finally
            {
                if (needDispose && workbook != null)
                    workbook.Dispose();
            }


            static void ProcessWorkbook(IWorkbook workbook, string tableName, bool rebuild, out IRichEditDocumentServer richEditDocumentServer)
            {
                richEditDocumentServer = null;

                if (rebuild)
                    workbook.CalculateFullRebuild();

                CellRange range = null;

                int p = tableName.IndexOf('!');
                if (p >= 0)
                {
                    var worksheetName      = tableName.Substring(0, p);
                    var worksheetTableName = tableName[(p + 1)..];

                    var worksheet = workbook.Worksheets[worksheetName];
                    if (worksheet == null)
                        return;

                    foreach (var table in worksheet.Tables)
                    {
                        if (string.Compare(table.Name, worksheetTableName, true) == 0)
                        {
                            range = table.Range;
                            break;
                        }
                    }

                    if (range == null)
                    {
                        foreach (var definedName in worksheet.DefinedNames)
                        {
                            if (string.Compare(definedName.Name, worksheetTableName, true) == 0)
                            {
                                range = definedName.Range;
                                break;
                            }
                        }
                    }

                    if (range == null)
                    {
                        try
                        {
                            //Try to get range that is not table or named range, may be it is Table!A1:C10 or similar
                            range = worksheet.Range[worksheetTableName];
                        }
                        catch (Exception)
                        {
                            //Invalid range
                            throw new Exception($"Invalid range '{worksheetTableName}' in table '{tableName}'");
                        }
                    }
                }
                else
                {
                    foreach (var sheet in workbook.Worksheets)
                    {
                        foreach (var table in sheet.Tables)
                        {
                            if (string.Compare(table.Name, tableName, true) == 0)
                            {
                                range = table.Range;
                                break;
                            }
                        }
                    }

                    foreach (var definedName in workbook.DefinedNames)
                    {
                        if (string.Compare(definedName.Name, tableName, true) == 0)
                        {
                            range = definedName.Range;
                            break;
                        }
                    }
                }

                if (range == null)
                    throw new Exception($"Cannot find table or range '{tableName}'");

                string htmlText;

                var options = new HtmlDocumentExporterOptions()
                {
                    EmbedImages             = true,
                    Encoding                = Encoding.UTF8,
                    ExportComments          = true,
                    ExportGridlines         = true,
                    ExportImages            = true,
                    ExportRootTag           = DevExpress.XtraSpreadsheet.Export.Html.ExportRootTag.Html,
                    OverrideImageResolution = 300,
                    SheetIndex              = workbook.Sheets.IndexOf(range.Worksheet),
                    Range                   = range.GetReferenceA1()
                };

                using (var stream = new MemoryStream())
                {
                    workbook.ExportToHtml(stream, options);
                    stream.Seek(0, SeekOrigin.Begin);

                    using var streamReader = new StreamReader(stream, Encoding.UTF8);
                    htmlText = streamReader.ReadToEnd();
                }


                var server = new RichEditDocumentServer();
                server.Document.AppendHtmlText(htmlText, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

                richEditDocumentServer = server;
            }
        }

        protected IRichEditDocumentServer AddSpreadChart(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE SPREADCHART' requires filename as first argument.");
            if (arguments.Count <= 1)
                throw new Exception("'DOCVARIABLE SPREADCHART' requires chart name with optional sheet name as second argument.");

            var fileName = Project.Current.MapPath(arguments[0].Value);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("DOCVARIABLE SPREADCHART does not contain valid filename.");

            object snippet = null;

            if (Snippets?.ContainsKey(fileName) ?? false)
            {
                snippet = Snippets[fileName];
                if (snippet is SCSpreadsheetContext)
                {
                }
                else
                    throw new Exception($"Specified snippet '{fileName}' is not supported. Spreadsheet.");
            }
            else if (string.Compare(fileName, "$SPREAD", true) == 0)
            {
                //Do nothing
            }
            else
                fileName = Project.Current.MapPath(fileName);

            if (snippet == null && string.Compare(fileName, "$SPREAD", true) != 0 && !File.Exists(fileName))
                throw new Exception($"File '{fileName}' does not exist.");

            var chartName = arguments[1].Value;

            float? scale = null, scaleX = null, scaleY = null;
            bool rebuild = false;
            Size size = Size.Empty;

            if (arguments.Count > 2)
            {
                var properties = Utils.SplitNameValueString(arguments[2].Value, ';');

                foreach (var prop in properties)
                {
                    if (string.IsNullOrWhiteSpace(prop.Key))
                        continue;

                    switch (prop.Key.ToLower())
                    {
                        case "rebuild":
                        case "recalculate":
                        case "recalc":
                            var valueRecalc = prop.Value;
                            if (string.IsNullOrWhiteSpace(valueRecalc))
                                valueRecalc = bool.TrueString;
                            rebuild = bool.Parse(valueRecalc);
                            break;
                        case "size":
                            size = ParseSize(prop.Value);
                            break;
                        case "scale":
                            scale = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
                        case "scalex":
                            scaleX = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
                        case "scaley":
                            scaleY = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
                    }
                }
            }

            bool needDispose = true;
            IWorkbook workbook = null;
            try
            {
                //using var workbook = SpreadsheetUtils.CreateWorkbook();
                if (snippet is SCSpreadsheetContext spreadsheetContext)
                {
                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.Append(spreadsheetContext.Workbook);
                }
                else if (string.Compare(fileName, "$SPREAD", true) == 0)
                {
                    needDispose = false;
                    workbook = this.DefaultSpreadsheet;

                    if (workbook == null)
                        throw new Exception("Current script does not support default (Host) spreadsheet.");
                }
                else
                {
                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.LoadDocument(fileName);
                }

                IRichEditDocumentServer result = null;
                if (workbook == this.DefaultSpreadsheet && NeedSynchronizeDefaultSpreadsheet)
                    SCDispatcherService.UIDispatcherServer.Invoke(() => ProcessWorkbook(workbook, chartName, rebuild, scale, scaleX, scaleY, size, out result));
                else
                    ProcessWorkbook(workbook, chartName, rebuild, scale, scaleX, scaleY, size, out result);
                return result;
            }
            finally
            {
                if (needDispose && workbook != null)
                    workbook.Dispose();
            }


            static void ProcessWorkbook(IWorkbook workbook, string chartName, bool rebuild, float? scale, float? scaleX, float? scaleY, Size size,
                out IRichEditDocumentServer richEditDocumentServer)
            {
                richEditDocumentServer = null;

                if (rebuild)
                    workbook.CalculateFullRebuild();

                ChartObject chart = null;

                int p = chartName.IndexOf('!');
                if (p >= 0)
                {
                    var worksheetName      = chartName.Substring(0, p);
                    var worksheetChartName = chartName[(p + 1)..];

                    var worksheet = workbook.Worksheets[worksheetName];
                    if (worksheet == null)
                        return;

                    foreach (var sheetChart in worksheet.Charts)
                    {
                        if (string.Compare(sheetChart.Name, worksheetChartName, true) == 0)
                        {
                            chart = sheetChart;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var chartSheet in workbook.ChartSheets)
                    {
                        if (string.Compare(chartSheet.Name, chartName, true) == 0)
                        {
                            chart = chartSheet.Chart;
                            break;
                        }
                    }
                }

                if (chart == null)
                    throw new Exception($"Cannot find chart '{chartName}'");

                var image = size.IsEmpty ? chart.GetImage() : chart.GetImage(size);

                var server = new RichEditDocumentServer();
                var docImage = server.Document.Images.Append(image.NativeImage);

                if (scale.HasValue)
                    docImage.ScaleX = docImage.ScaleY = scale.Value;
                else
                {
                    if (scaleX.HasValue)
                        docImage.ScaleX = scaleX.Value;
                    if (scaleY.HasValue)
                        docImage.ScaleY = scaleY.Value;
                }

                richEditDocumentServer = server;
            }
        }

        protected IRichEditDocumentServer AddSpreadPivot(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE SPREADPIVOT' requires filename as first argument.");
            if (arguments.Count <= 1)
                throw new Exception("'DOCVARIABLE SPREADPIVOT' requires pivot table name with optional sheet name as second argument.");

            var fileName = Project.Current.MapPath(arguments[0].Value);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("DOCVARIABLE SPREADPIVOT does not contain valid filename.");

            object snippet = null;

            if (Snippets?.ContainsKey(fileName) ?? false)
            {
                snippet = Snippets[fileName];
                if (snippet is SCSpreadsheetContext)
                {
                }
                else
                    throw new Exception($"Specified snippet '{fileName}' is not supported. Spreadsheet.");
            }
            else if (string.Compare(fileName, "$SPREAD", true) == 0)
            {
                //Do nothing
            }
            else
                fileName = Project.Current.MapPath(fileName);

            if (snippet == null && string.Compare(fileName, "$SPREAD", true) != 0 && !File.Exists(fileName))
                throw new Exception($"File '{fileName}' does not exist.");

            var pivotName = arguments[1].Value;

            bool rebuild  = false;
            bool dataOnly = false;

            if (arguments.Count > 2)
            {
                var properties = Utils.SplitNameValueString(arguments[2].Value, ';');

                foreach (var prop in properties)
                {
                    if (string.IsNullOrWhiteSpace(prop.Key))
                        continue;

                    switch (prop.Key.ToLower())
                    {
                        case "rebuild":
                        case "recalculate":
                        case "recalc":
                            var valueRecalc = prop.Value;
                            if (string.IsNullOrWhiteSpace(valueRecalc))
                                valueRecalc = bool.TrueString;
                            rebuild = bool.Parse(valueRecalc);
                            break;
                        case "dataonly":
                            var valueDataOnly = prop.Value;
                            if (string.IsNullOrWhiteSpace(valueDataOnly))
                                valueRecalc = bool.TrueString;
                            dataOnly = bool.Parse(valueDataOnly);
                            break;
                    }
                }
            }

            bool needDispose = true;
            IWorkbook workbook = null;
            try
            {
                //using var workbook = SpreadsheetUtils.CreateWorkbook();
                if (snippet is SCSpreadsheetContext spreadsheetContext)
                {
                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.Append(spreadsheetContext.Workbook);
                }
                else if (string.Compare(fileName, "$SPREAD", true) == 0)
                {
                    needDispose = false;
                    workbook = this.DefaultSpreadsheet;

                    if (workbook == null)
                        throw new Exception("Current script does not support default (Host) spreadsheet.");
                }
                else
                {
                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.LoadDocument(fileName);
                }

                IRichEditDocumentServer result = null;
                if (workbook == this.DefaultSpreadsheet && NeedSynchronizeDefaultSpreadsheet)
                    SCDispatcherService.UIDispatcherServer.Invoke(() => ProcessWorkbook(workbook, pivotName, rebuild, dataOnly, out result));
                else
                    ProcessWorkbook(workbook, pivotName, rebuild, dataOnly, out result);
                return result;
            }
            finally
            {
                if (needDispose && workbook != null)
                    workbook.Dispose();
            }


            static void ProcessWorkbook(IWorkbook workbook, string pivotName, bool rebuild, bool dataOnly, out IRichEditDocumentServer richEditDocumentServer)
            {
                richEditDocumentServer = null;

                if (rebuild)
                    workbook.CalculateFullRebuild();

                PivotTable pivot = null;

                int p = pivotName.IndexOf('!');
                if (p >= 0)
                {
                    var worksheetName      = pivotName.Substring(0, p);
                    var worksheetPivotName = pivotName[(p + 1)..];

                    var worksheet = workbook.Worksheets[worksheetName];
                    if (worksheet == null)
                        return;

                    foreach (var sheetPivot in worksheet.PivotTables)
                    {
                        if (string.Compare(sheetPivot.Name, worksheetPivotName, true) == 0)
                        {
                            pivot = sheetPivot;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var sheet in workbook.Sheets.OfType<Worksheet>())
                    {
                        foreach (var sheetPivot in sheet.PivotTables)
                        {
                            if (string.Compare(sheetPivot.Name, pivotName, true) == 0)
                            {
                                pivot = sheetPivot;
                                break;
                            }
                        }

                        if (pivot != null)
                            break;
                    }
                }

                if (pivot == null)
                    throw new Exception($"Cannot find pivot table '{pivotName}'");

                var range = dataOnly ? pivot.Location.DataRange : pivot.Location.Range;

                string htmlText;

                var options = new HtmlDocumentExporterOptions()
                {
                    EmbedImages             = true,
                    Encoding                = Encoding.UTF8,
                    ExportComments          = true,
                    ExportGridlines         = true,
                    ExportImages            = true,
                    ExportRootTag           = DevExpress.XtraSpreadsheet.Export.Html.ExportRootTag.Html,
                    OverrideImageResolution = 300,
                    SheetIndex              = workbook.Sheets.IndexOf(range.Worksheet),
                    Range                   = range.GetReferenceA1()
                };

                using (var stream = new MemoryStream())
                {
                    workbook.ExportToHtml(stream, options);
                    stream.Seek(0, SeekOrigin.Begin);

                    using var streamReader = new StreamReader(stream, Encoding.UTF8);
                    htmlText = streamReader.ReadToEnd();
                }

                var server = new RichEditDocumentServer();
                server.Document.AppendHtmlText(htmlText, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

                richEditDocumentServer = server;
            }
        }
    }
}
