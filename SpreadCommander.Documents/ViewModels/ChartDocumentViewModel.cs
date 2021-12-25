using DevExpress.XtraCharts.Native;
using DevExpress.Compression;
using DevExpress.Mvvm.POCO;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines;
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
    public class ChartDocumentViewModel: CustomConsoleDocumentViewModel, ChartDocumentViewModel.IModelWithChart
    {
        #region IChartCallback
        public interface IChartCallback
        {
            Chart Chart { get; }
            object DataSource { get; set; }
            DataTable ActiveTable { get; set; }
            string TableName { get; set; }
            void ClearChart();
        }
        #endregion

        #region IModelWithChart
        public interface IModelWithChart
        {
            void LoadChart(string fileName);
            void SaveChart(string fileName);
        }
        #endregion

        public new const string ViewName = "ChartDocument";

        public ChartDocumentViewModel(): base()
        {
        }

        public ChartDocumentViewModel(BaseScriptEngine engine) : base(engine)
        {
        }

        public static ChartDocumentViewModel Create() =>
            ViewModelSource.Create<ChartDocumentViewModel>(() => new ChartDocumentViewModel());

        public new static ChartDocumentViewModel Create(BaseScriptEngine engine) =>
            ViewModelSource.Create<ChartDocumentViewModel>(() => new ChartDocumentViewModel(engine));

        public override string DefaultExt      => ".scchart";
        public override string FileFilter      => "Chart (*.scchart)|*.scchart|All files (*.*)|*.*";
        public override string DocumentType    => ViewName;
        public override string DocumentSubType => Engine?.EngineName;
        
        public override Type[] CustomControlTypes { get; } = new Type[] { typeof(Console.ConsoleChartControl) };

        public IChartCallback ChartCallback { get; set; }

        public override string SaveScriptAndControlCaption => "Save script and chart";

        public override void InitializeCustomControls()
        {
            if (ChartCallback != null && !string.IsNullOrWhiteSpace(FileName) && File.Exists(FileName))
                LoadCustomControlsFromFile(FileName);
        }

        //If fileName == null - clear custom controls
        public override void LoadCustomControlsFromFile(string fileName) => LoadChart(fileName);

        public void LoadChart(string fileName)
        {
            var callback = ChartCallback;

            if (callback == null)
                return;

            if (fileName == null)
            {
                callback?.ClearChart();
                return;
            }

            fileName = Project.Current.MapPath(fileName);

            var chart    = callback?.Chart;
            if (chart == null)
                throw new Exception("Chart control is not available");

            string tableName = null;

            using (var zip = ZipArchive.Read(fileName))
            {
                var itemChart = zip["Chart.xml"];
                if (itemChart != null)
                {
                    using var stream = itemChart.Open();
                    using var memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    chart.LoadLayout(memStream);

                    tableName = ReadTableName(memStream);
                    if (string.IsNullOrWhiteSpace(tableName))
                        tableName = null;
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

                var nodeTableName = doc.SelectSingleNode("/ChartXmlSerializer/Chart/@TableName");
                if (nodeTableName != null)
                    return nodeTableName.Value;

                return null;
            }
        }

        public override void SaveCustomControlsToFile(string fileName) => SaveChart(fileName);

        public void SaveChart(string fileName)
        {
            if (fileName == null)
                return;

            fileName = Project.Current.MapPath(fileName);

            var callback = ChartCallback;
            var chart    = callback?.Chart;
            if (chart == null)
                throw new Exception("Chart control is not available");

            using (var zip = new ZipArchive())
            {
                using var streamChart = new MemoryStream();
                chart.SaveLayout(streamChart);
                streamChart.Seek(0, SeekOrigin.Begin);

                using var streamChart2 = WriteTableName(streamChart);
                streamChart2.Seek(0, SeekOrigin.Begin);

                zip.AddStream("Chart.xml", streamChart2);

                zip.Save(fileName);
            }


            Stream WriteTableName(MemoryStream stream)
            {
                stream.Seek(0, SeekOrigin.Begin);

                var doc = new XmlDocument();
                doc.Load(stream);
                stream.Seek(0, SeekOrigin.Begin);

                if (doc.SelectSingleNode("/ChartXmlSerializer/Chart") is XmlElement nodeChart)
                    nodeChart.SetAttribute("TableName", Utils.NonNullString(callback.TableName));

                var result = new MemoryStream((int)stream.Length + 1024);
                Utils.SaveXmlDocument(doc, result);
                result.Seek(0, SeekOrigin.Begin);

                return result;
            }
        }
    }
}
