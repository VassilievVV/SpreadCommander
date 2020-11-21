using DevExpress.Compression;
using DevExpress.DashboardCommon;
using DevExpress.Mvvm.POCO;
using SpreadCommander.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpreadCommander.Documents.ViewModels
{
    public class DashboardDocumentViewModel: CustomConsoleDocumentViewModel
    {
        public new const string ViewName = "DashboardDocument";

        #region ICallback
        public interface IDashboardCallback
        {
            void DashboardModified();
            void ReloadData();

            Dashboard Dashboard { get; }
        }
        #endregion

        public DashboardDocumentViewModel() : base()
        {
        }

        public static DashboardDocumentViewModel Create() =>
            ViewModelSource.Create<DashboardDocumentViewModel>(() => new DashboardDocumentViewModel());

        public override string DefaultExt => ".scdash";
        public override string FileFilter => "Dashboard (*.scdash)|*.scdash|All files (*.*)|*.*";
        public override string DocumentType    => ViewName;
        public override string DocumentSubType => Engine?.EngineName;

        public override Type[] CustomControlTypes { get; } = new Type[] { typeof(Console.ConsoleDashboardControl) };

        public IDashboardCallback DashboardCallback { get; set; }
        public Dashboard Dashboard => DashboardCallback?.Dashboard;

        public override void InitializeCustomControls()
        {
            if (Dashboard != null && !string.IsNullOrWhiteSpace(FileName) && File.Exists(FileName))
                LoadCustomControlsFromFile(FileName);
        }

        //If fileName == null - clear custom controls
        public override void LoadCustomControlsFromFile(string fileName) => LoadDashboard(fileName);

        public void LoadDashboard(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);

            var dashboard = Dashboard;
            if (dashboard == null)
                return;

            XDocument doc = null;
            using (var zip = ZipArchive.Read(fileName))
            {
                var itemDashboard = zip["Dashboard.xml"];
                if (itemDashboard != null)
                {
                    using var stream = itemDashboard.Open();
                    using var memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    doc = XDocument.Load(memStream, LoadOptions.PreserveWhitespace);
                }
            }

            dashboard.LoadFromXDocument(doc);
        }

        public override void SaveCustomControlsToFile(string fileName) => SaveDashboard(fileName);

        public void SaveDashboard(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);

            var dashboard = Dashboard;
            if (dashboard == null)
                throw new Exception("Dashboard control is not available");

            var doc = dashboard.SaveToXDocument();

            using var zip = new ZipArchive();
            using MemoryStream streamDashboard = new MemoryStream();
            doc.Save(streamDashboard);
            streamDashboard.Seek(0, SeekOrigin.Begin);
            zip.AddStream("Dashboard.xml", streamDashboard);

            zip.Save(fileName);
        }

        protected static void RemoveObjectDataSources(XDocument doc)
        {
            //All object databases are parameters. So user will have to re-execute parameters next time
            var objDataSources = doc.Descendants("DataSources").Descendants("ObjectDataSource").ToList();
            foreach (var objDataSource in objDataSources)
                objDataSource.Remove();
        }
    }
}
