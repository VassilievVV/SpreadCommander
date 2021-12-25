using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Script.Book;
using SpreadCommander.Common.Script.Spreadsheet;
using SpreadCommander.Common.ScriptEngines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public enum ImageFileFormat { Png, Tiff, Bmp, Gif, Jpeg }

    public partial class ScriptHostObject
    {
        #region DataSourceParameters
        public class DataSourceParameters
        {
            public bool IgnoreErrors    { get; set; }

            public string[] Columns     { get; set; }

            public string[] SkipColumns { get; set; }

            public bool SkipAutoID      { get; set; }

            public Action DisposeAction { get; set; }
        }
        #endregion

        public const ImageFileFormat DefaultImageFormat = ImageFileFormat.Png;

        public const int DefaultDPI = 300;

        public ScriptHostObject()
        {
        }

        public ScriptHostObject(ScriptHost host)
        {
            Host = host;
        }

        public static readonly object LockObject = new();

        public ScriptHost Host { get; protected set; }

        protected virtual bool NeedSynchronization(BookOptions options) => options?.Book == null || options.Book == Host?.Book;

        protected virtual void ExecuteSynchronized(BookOptions options, Action action)
        {
            if (NeedSynchronization(options))
                ExecuteSynchronized(action);
            else
                action();
        }

        protected virtual bool NeedSynchronization(SpreadsheetOptions options) => options?.Spreadsheet == null || options.Spreadsheet == Host?.Spreadsheet;

        protected virtual void ExecuteSynchronized(SpreadsheetOptions options, Action action)
        {
            if (NeedSynchronization(options))
                ExecuteSynchronized(action);
            else
                action();
        }

        protected virtual void ExecuteSynchronized(Action action)
        {
            var sync = Host?.Engine?.SynchronizeInvoke ?? BaseScriptEngine.StaticSynchronizeInvoke;
            if (sync?.InvokeRequired ?? false)
                _ = sync.Invoke(action, Array.Empty<object>());
            else
                action();
        }

        protected internal static DataTable GetDataSource(object dataSource, DataSourceParameters parameters)
        {
            var reader = GetDataSourceReader(dataSource, parameters);
            var table = new DataTable("Table");
            table.Load(reader);
            return table;
        }

        protected internal static DbDataReader GetDataSourceReader(object dataSource, DataSourceParameters parameters)
        {
            if (dataSource == null)
                throw new ArgumentException("Data source is not assigned.");

            if (!AcceptObject(dataSource))
                throw new ArgumentException("Data source is not acceptable. Shall be IList, IListSource, DbDataReader, DataTable or DataView.");

            DbDataReader result = null;

            if (dataSource is DbDataReader dbReader)
                result = dbReader;
            else if (dataSource is DataTable dataTable)
                result = dataTable.CreateDataReader();
            else if (dataSource is DataView dataView)
                result = new DataViewReader(dataView);
            else
            {
                if (dataSource is IListSource listSource)
                    dataSource = listSource.GetList();

                if (dataSource is ITypedList typedList)
                    result = new TypedListDataReader(typedList);
                else if (dataSource is System.Collections.IList list && list.Count > 0 && list[0] != null)
                    result = new ObjectDataReader(list, list[0].GetType());
            }

            if (result != null)
                return new DataReaderWrapper(result, new DataReaderWrapper.DataReaderWrapperParameters() { Columns = parameters?.Columns, CloseAction = parameters?.DisposeAction });

            throw new Exception("Cannot generate DataReader for provided data source.");


            static bool AcceptObject(object obj)
            {
                return obj is IList || obj is IListSource ||
                    obj is DbDataReader ||
                    obj is DataTable || obj is DataView;
            }
        }

        protected void ReportError(string errorMessage) =>
            ExecuteSynchronized(() => DoReportError(errorMessage));

        protected void ReportError(Exception ex) =>
            ExecuteSynchronized(() => DoReportError(ex.Message));

        protected virtual void DoReportError(string errorMessage)
        {
            var book = Host?.Book?.Document;
            if (book == null)
                return;

            var range = book.AppendText($"ERROR: {errorMessage}{Environment.NewLine}");
            var cp    = book.BeginUpdateCharacters(range);
            try
            {
                cp.ForeColor = Color.Red;
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }

            WriteErrorToConsole(errorMessage);
        }

        public static void WriteTextToConsole(string text)
        {
            if (BaseScriptEngine.ApplicationType == ScriptEngines.BaseScriptEngine.ScriptApplicationType.Console)
            {
                System.Console.ResetColor();
                System.Console.WriteLine(text);
            }
        }

        public static void WriteErrorToConsole(string text)
        {
            if (BaseScriptEngine.ApplicationType == ScriptEngines.BaseScriptEngine.ScriptApplicationType.Console)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"ERROR: {text}");
                System.Console.ResetColor();
            }
        }

        public static void WriteRangeToConsole(Document doc, DocumentRange range)
        {
            if (BaseScriptEngine.ApplicationType == ScriptEngines.BaseScriptEngine.ScriptApplicationType.Console)
            {
                var text = doc.GetText(range);
                WriteTextToConsole(text);
            }
        }

        public static void ExecuteLocked(Action action, object lockObject)
        {
            if (lockObject != null)
                lock (lockObject)
                    action();
            else
                action();
        }

        protected virtual ImageFileFormat GetImageFormatFromFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return DefaultImageFormat;

            return (Path.GetExtension(fileName)?.ToLower()) switch
            {
                ".png"            => ImageFileFormat.Png,
                ".tif" or ".tiff" => ImageFileFormat.Tiff,
                ".bmp"            => ImageFileFormat.Bmp,
                ".gif"            => ImageFileFormat.Gif,
                ".jpg" or ".jpeg" => ImageFileFormat.Jpeg,
                _                 => ImageFileFormat.Png
            };
        }

        protected virtual void PreviewFile(string fileName)
        {
            var fileViewer = Host?.FileViewer;
            if (fileViewer == null)
                return;

            ExecuteSynchronized(() => fileViewer.ViewFile(fileName));
        }
    }
}
