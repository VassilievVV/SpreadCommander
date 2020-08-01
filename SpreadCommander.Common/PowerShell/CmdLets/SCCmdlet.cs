using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.Host;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets
{
    public class SCCmdlet: PSCmdlet
    {
        #region DataSourceParameters
        public class DataSourceParameters
        {
            public bool IgnoreErrors    { get; set; }

            public string[] Columns     { get; set; }

            public Action DisposeAction { get; set; }
        }
        #endregion

        public enum ImageFormat { Png, Tiff, Bmp, Gif, Jpeg }

        public const ImageFormat DefaultImageFormat = ImageFormat.Png;

        public static readonly object LockObject = new object();

        private static int _ProgressActivityID;

        public SCCmdlet()
        {
        }

        protected ExternalHost ExternalHost
        {
            get
            {
                if (Host.PrivateData?.BaseObject is ExternalHost host)
                    return host;

                var propExtHost = Host.GetType().GetProperty("ExternalHost", BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
                var extHost     = propExtHost.GetMethod.Invoke(Host, null);
                return (extHost as SpreadCommanderHost)?.ExternalHost;
            }
        }

        protected ExternalHost CheckExternalHost()
        {
            var result = ExternalHost ?? throw new Exception("Cannot obtain SpreadCommander Host");
            return result;
        }

        protected IRichEditDocumentServer HostBookServer => CheckExternalHost().BookServer;
        protected Document HostBook                      => CheckExternalHost().Book;
        protected IWorkbook HostSpreadsheet              => CheckExternalHost().Spreadsheet;
        protected DataSet HostGridDataSet                => CheckExternalHost().GridDataSet;
        protected IFileViewer FileViewer                 => CheckExternalHost().FileViewer;

        protected virtual bool NeedSynchronization() => true;

        protected virtual void ExecuteSynchronized(Action action)
        {
            if (NeedSynchronization())
                CheckExternalHost().ExecuteMethodSync(action);
            else
                action();
        }

        protected void ReportError(string errorMessage) =>
            ExecuteSynchronized(() => DoReportError(errorMessage));

        protected void ReportError(Exception ex) =>
            ExecuteSynchronized(() => DoReportError(ex.Message));

        protected virtual void DoReportError(string errorMessage)
        {
            var book = HostBook;

            var range = book.AppendText($"ERROR: {errorMessage}{Environment.NewLine}");
            var cp = book.BeginUpdateCharacters(range);
            try
            {
                cp.ForeColor = Color.Red;
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }
        }

        protected virtual ImageFormat GetImageFormatFromFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return DefaultImageFormat;

            switch (Path.GetExtension(fileName)?.ToLower())
            {
                case ".png":
                    return ImageFormat.Png;
                case ".tif":
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                default:
                    return ImageFormat.Png;

            }
        }

        //IgnoreErrors - for list of PSObject
        protected virtual object GetDataSource(IList<PSObject> dataRecords, object dataSource, DataSourceParameters parameters)
        {
            var reader = GetDataSourceReader(dataRecords, dataSource, parameters);
            var table  = new DataTable("Table");
            table.Load(reader);
            return table;
        }

        protected virtual DbDataReader GetDataSourceReader(IList<PSObject> dataRecords, object dataSource, DataSourceParameters parameters)
        {
            if (dataSource == null && dataRecords != null && dataRecords.Count == 1 && (dataRecords[0].BaseObject is IList || dataRecords[0].BaseObject is IListSource))
                dataSource = dataRecords[0].BaseObject;

            DbDataReader result = null;

            if (dataSource != null)
            {
                if (dataSource is PSObject psDataSource)
                    dataSource = psDataSource.BaseObject;

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
            }
            else
            {
                var reader = new PSObjectDataReader(dataRecords)
                {
                    IgnoreErrors = parameters?.IgnoreErrors ?? false
                };

                result = reader;
            }

            if (result != null)
                return new DataReaderWrapper(result, new DataReaderWrapper.DataReaderWrapperParameters() { Columns = parameters?.Columns, CloseAction = parameters?.DisposeAction });

            throw new Exception("Cannot generate DataReader for provided data source.");
        }

        protected virtual void PreviewFile(string fileName)
        {
            var externalHost = ExternalHost;
            var fileViewer   = externalHost?.FileViewer;
            if (fileViewer == null)
                return;

            externalHost.ExecuteMethodSync(() => fileViewer.ViewFile(fileName));
        }

        protected int GetNextProgressActivityID()
        {
            var result = Interlocked.Add(ref _ProgressActivityID, 1);
            return result;
        }
    }
}