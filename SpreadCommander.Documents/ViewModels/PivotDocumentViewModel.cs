using DevExpress.Compression;
using DevExpress.Mvvm.POCO;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpreadCommander.Documents.ViewModels
{
    public class PivotDocumentViewModel: CustomConsoleDocumentViewModel
    {
        #region IPivotCallback
        public interface IPivotCallback
        {
            void LoadPivot(Stream stream);
            void SavePivot(Stream stream);

            DataTable ActiveTable   { get; set; }
            string TableName        { get; set; }
        }
        #endregion

        public new const string ViewName = "PivotDocument";

        public PivotDocumentViewModel() : base()
        {
        }

        public static PivotDocumentViewModel Create() =>
            ViewModelSource.Create<PivotDocumentViewModel>(() => new PivotDocumentViewModel());

        public override string DefaultExt => ".scpivot";
        public override string FileFilter => "Pivot (*.scpivot)|*.scpivot|All files (*.*)|*.*";
        public override string DocumentType    => ViewName;
        public override string DocumentSubType => Engine?.EngineName;

        public override Type[] CustomControlTypes { get; } = new Type[] { typeof(Console.ConsolePivotControl), typeof(Console.ConsoleFixedDataSourceChartControl) };

        public IPivotCallback PivotCallback { get; set; }

        private ChartDocumentViewModel.IChartCallback _ChartCallback;
        public ChartDocumentViewModel.IChartCallback ChartCallback
        {
            get => _ChartCallback;
            set
            {
                if (_ChartCallback == value)
                    return;

                _ChartCallback = value;

                if (value != null)
                    value.DataSource = _DrillDownDataSource;
            }
        }

        public override string SaveScriptAndControlCaption => "Save script and Pivot";

        public override void InitializeCustomControls()
        {
            if (PivotCallback != null && !string.IsNullOrWhiteSpace(FileName) && File.Exists(FileName))
                LoadCustomControlsFromFile(FileName);
        }

        private object _DrillDownDataSource;
        public void SetPivotDataSource(object dataSource)
        {
            _DrillDownDataSource = dataSource;
            var callback         = ChartCallback;
            if (callback != null)
                callback.DataSource = dataSource;
        }

        //If fileName == null - clear custom controls
        public override void LoadCustomControlsFromFile(string fileName) => LoadPivot(fileName);

        public void LoadPivot(string fileName)
        {
            var callback      = PivotCallback;
            var chartCallback = ChartCallback;

            if (callback == null || chartCallback == null)
                return;

            if (fileName == null)
            {
                //callback?.ClearPivot();
                return;
            }

            fileName = Project.Current.MapPath(fileName);

            string tableName = null;

            using (var zip = ZipArchive.Read(fileName))
            {
                var itemPivot = zip["Pivot.xml"];
                if (itemPivot != null)
                {
                    using var stream = itemPivot.Open();
                    using var memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    PivotCallback.LoadPivot(memStream);

                    tableName = ReadTableName(memStream);
                    if (string.IsNullOrWhiteSpace(tableName))
                        tableName = null;
                }

                var itemChart = zip["Chart.xml"];
                if (itemChart != null)
                {
                    using var stream = itemChart.Open();
                    using var memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    var chart = chartCallback.Chart;
                    var dataSource = chart.DataContainer.DataSource;
                    var dataMember = chart.DataContainer.DataMember;
                    try
                    {
                        chart.LoadLayout(memStream);
                    }
                    finally
                    {
                        chart.DataContainer.DataSource = dataSource;
                        chart.DataContainer.DataMember = dataMember;
                    }
                }
            }

            callback.TableName = tableName;
            if (!string.IsNullOrWhiteSpace(tableName))
                callback.ActiveTable = GridDataSet?.Tables[tableName];
            else
                callback.ActiveTable = null;


            static string ReadTableName(MemoryStream stream)
            {
                stream.Seek(0, SeekOrigin.Begin);

                var doc = new XmlDocument();
                doc.Load(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var nodeTableName = doc.SelectSingleNode("/XtraSerializer/@TableName");
                if (nodeTableName != null)
                    return nodeTableName.Value;

                return null;
            }
        }

        public override void SaveCustomControlsToFile(string fileName) => SavePivot(fileName);

        public void SavePivot(string fileName)
        {
            if (fileName == null)
                return;

            fileName = Project.Current.MapPath(fileName);

            var callback      = PivotCallback;
            var chartCallback = ChartCallback;

            using (var zip = new ZipArchive())
            {
                MemoryStream streamPivot = null, streamChart = null;
                try
                {
                    using (MemoryStream streamPivotInit = new MemoryStream())
                    {
                        callback.SavePivot(streamPivotInit);
                        streamPivotInit.Seek(0, SeekOrigin.Begin);

                        streamPivot = WriteTableName(streamPivotInit);
                        streamPivot.Seek(0, SeekOrigin.Begin);

                        zip.AddStream("Pivot.xml", streamPivot);
                    }

                    streamChart = new MemoryStream();
                    chartCallback.Chart.SaveLayout(streamChart);
                    streamChart.Seek(0, SeekOrigin.Begin);

                    zip.AddStream("Chart.xml", streamChart);

                    zip.Save(fileName);
                }
                finally
                {
                    streamPivot?.Dispose();
                    streamChart?.Dispose();
                }
            }


            MemoryStream WriteTableName(MemoryStream stream)
            {
                stream.Seek(0, SeekOrigin.Begin);

                var doc = new XmlDocument();
                doc.Load(stream);
                stream.Seek(0, SeekOrigin.Begin);

                if (doc.SelectSingleNode("/XtraSerializer") is XmlElement nodePivot)
                    nodePivot.SetAttribute("TableName", Utils.NonNullString(callback.TableName));

                var result = new MemoryStream((int)stream.Length + 1024);
                Utils.SaveXmlDocument(doc, result);
                result.Seek(0, SeekOrigin.Begin);

                return result;
            }
        }
    }
}
