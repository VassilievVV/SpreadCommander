using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Console
{
    class Program
    {
        private static PowerShellScriptEngine Engine    { get; } = new PowerShellScriptEngine();
        private static RichEditDocumentServer Book      { get; } = new RichEditDocumentServer();
        private static IWorkbook Workbook               { get; } = SpreadsheetUtils.CreateWorkbook();
        private static DataSet DataSet                  { get; } = new DataSet("Script");

        private static string _ScriptFileName;
        private readonly static List<string> _OutputFiles = new List<string>();

        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture   = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            BaseScriptEngine.SetApplicationType(BaseScriptEngine.ScriptApplicationType.Console);

            if (args == null || args.Length <= 0)
            {
                DisplayHelp();
                System.Console.ReadKey();
                return;
            }

            if (!ProcessStartupArguments(args))
                return;

            StartProfiling();

            Startup.Initialize();
            DevExpress.XtraPrinting.Native.PrintingSettings.UseGdiPlusLineBreakAlgorithm = true;

            string script = LoadScript();

            Engine.RequestLine += Engine_RequestLine;

            Engine.BookServer  = Book;
            Engine.Workbook    = Workbook;
            Engine.GridDataSet = DataSet;

            Engine.ExecutionType = BaseScriptEngine.ScriptExecutionType.Script;

            try
            {
                var taskSource = new TaskCompletionSource<bool>();

                Engine.ExecutionFinished += callback;
                Engine.Start();
                Engine.SendCommand(script);

                taskSource.Task.Wait();     //Cannot await in event handler.

                OutputResults();


                void callback(object s, EventArgs e)
                {
                    Engine.ExecutionFinished -= callback;
                    taskSource.SetResult(true);
                }
            }
            finally
            {
                Book?.Dispose();
                Workbook?.Dispose();
                DataSet?.Dispose();

                Engine.BookServer  = null;
                Engine.Workbook    = null;
                Engine.GridDataSet = null;

                Engine.Dispose();
            }
        }

        public static void StartProfiling()
        {
            Utils.InitiateProfiling();
            Utils.StartProfile("Console_Main");
        }

        private static void Engine_RequestLine(object sender, BaseScriptEngine.ReadLineArgs e)
        {
            e.Line = System.Console.ReadLine();
        }

        private static bool ProcessStartupArguments(string[] args)
        {
            int counter = 0;
            for (int i = 0; i < args.Length; i++)
            {
                string arg = Utils.TrimString(args[i]);
                if (string.IsNullOrWhiteSpace(arg))
                    continue;

                if (arg.StartsWith('/') || arg.StartsWith('-'))
                {
                    var match = Regex.Match(arg, @"^\s*[/-](?<Name>\w+)(?:\s*[:=]\s*(?<Value>.*))?$");
                    if (!match.Success)
                        continue;

                    string argName  = Utils.NonNullString(match.Groups["Name"].Value).ToLower();
                    string argValue = match.Groups["Value"]?.Value?.Trim();
                    argValue = Utils.UnquoteString(argValue);

                    switch (argName)
                    {
                        case "h":
                        case "help":
                            DisplayHelp();
                            System.Console.ReadKey();
                            return false;
                        case "project":
                        case "pr":
                        case "p":   //Project directory
                            Project.LoadProjectFromDirectory(argValue, true);
                            break;
                        case "script":
                        case "s":
                            _ScriptFileName = argValue;
                            break;
                        case "out": //Output file
                            _OutputFiles.Add(argValue);
                            break;
                    }
                }
                else    //Unnamed parameters
                {
                    switch (counter)
                    {
                        case 0: //Script filename
                            _ScriptFileName = arg;
                            break;
                        case 1: //Project directory
                            Project.LoadProjectFromDirectory(arg, true);
                            break;
                    }

                    counter++;
                }
            }

            return true;
        }

        private static void DisplayHelp()
        {
            const string help =
@"Spread Commander Console
------------------------

Execute SpreadCommander PowerShell script. Not interactive. Designed to run script on-demand, for example using Windows Scheduler.

Syntax: SpreadComCon.exe <arguments>. Arguments have format -<Name>=<Value> or -<Name>:<Value> (for example -project=C:\MyProject).

Unnamed arguments:
    (1) Script file name. Can be in form '~\script.ps1' 
            where ~ is project directory.
    (2) Project directory.

Named arguments:
    -project, -pr, -p: Project directory.
    -script, -s:       Script file.
    -out:              Output file. Book can be saved into 
                           MS Word, text, RTF, ODT, PDF format 
                           (depending on file extension), 
                           Spreadsheet - into MS Excel, CSV format. 
                           Multiple output files are allowed.
                           '~\<filename>' is supported to save 
                           into project directory.
    -help, -h:         Output help (current text).

Press any key to exit ...";
            System.Console.WriteLine(help);
        }

        private static string LoadScript()
        {
            if (string.IsNullOrWhiteSpace(_ScriptFileName))
                return null;

            string scriptFile = Project.Current.MapPath(_ScriptFileName);
            if (string.IsNullOrWhiteSpace(scriptFile) || !File.Exists(scriptFile))
                return null;

            string result = File.ReadAllText(scriptFile);
            return result;
        }

        private static void OutputResults()
        {
            for (int i = 0; i < _OutputFiles.Count; i++)
            {
                string fileName = Project.Current.MapPath(_OutputFiles[i]);
                string ext      = Utils.NonNullString(Path.GetExtension(fileName)).ToLower();

                var bookFormat   = DevExpress.XtraRichEdit.DocumentFormat.Undefined;
                var spreadFormat = DevExpress.Spreadsheet.DocumentFormat.Undefined;
                switch (ext)
                {
                    case ".docx":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
                        break;
                    case ".doc":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.Doc;
                        break;
                    case ".txt":
                    case ".text":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
                        break;
                    case ".rtf":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.Rtf;
                        break;
                    case ".html":
                    case ".htm":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.Html;
                        break;
                    case ".mht":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.Mht;
                        break;
                    case ".odt":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.OpenDocument;
                        break;
                    case ".epub":
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.ePub;
                        break;
                    case ".pdf":
                        //Leave undefined, handle later
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.Undefined;
                        break;

                    case ".xlsx":
                        spreadFormat = DevExpress.Spreadsheet.DocumentFormat.Xlsx;
                        break;
                    case ".xls":
                        spreadFormat = DevExpress.Spreadsheet.DocumentFormat.Xls;
                        break;
                    case ".csv":
                        spreadFormat = DevExpress.Spreadsheet.DocumentFormat.Csv;
                        break;

                    default:
                        bookFormat = DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
                        break;
                }

                if (bookFormat != DevExpress.XtraRichEdit.DocumentFormat.Undefined)
                    Book.SaveDocument(fileName, bookFormat);
                if (spreadFormat != DevExpress.Spreadsheet.DocumentFormat.Undefined)
                    Workbook.SaveDocument(fileName, spreadFormat);

                //Special case to output into PDF
                if (ext == ".pdf")
                {
                    var pdfOptions = new PdfExportOptions()
                    {
                        ConvertImagesToJpeg   = false,
                        ImageQuality          = PdfJpegImageQuality.Highest,
                        PdfACompatibility     = PdfACompatibility.None,
                        ShowPrintDialogOnOpen = false
                    };

                    using var ps   = new PrintingSystem();
                    using var link = new PrintableComponentLink(ps)
                    {
                        Component = Book as IPrintable
                    };
                    link.CreateDocument();

                    ps.ExportToPdf(fileName, pdfOptions);
                }
            }
        }
    }
}
