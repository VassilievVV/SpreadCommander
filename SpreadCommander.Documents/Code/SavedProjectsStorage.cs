using SpreadCommander.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Code
{
    [DataContract(Namespace = Parameters.ApplicationNamespace)]
    public class SavedProjectsStorage
    {
        public readonly static object Lock = new object();

        private List<SavedLibrary> _Libraries;
        [DataMember()]
        public List<SavedLibrary> Libraries
        {
            get
            {
                if (_Libraries == null)
                    _Libraries = new List<SavedLibrary>();
                return _Libraries;
            }
            set
            {
                _Libraries = value;
            }
        }

        private List<SavedProject> _FavoriteProjects;
        public List<SavedProject> FavoriteProjects
        {
            get
            {
                if (_FavoriteProjects == null)
                    _FavoriteProjects = new List<SavedProject>();
                return _FavoriteProjects;
            }
            set
            {
                _FavoriteProjects = value;
            }
        }

        private List<SavedProject> _RecentProjects;
        public List<SavedProject> RecentProjects
        {
            get
            {
                if (_RecentProjects == null)
                    _RecentProjects = new List<SavedProject>();
                return _RecentProjects;
            }
            set
            {
                _RecentProjects = value;
            }
        }

        public static bool HasProject(IList<SavedProject> projects, string directory)
        {
            if (projects == null)
                return false;

            var project = projects.FirstOrDefault(p => string.Compare(p.Directory, directory, true) == 0);
            return (project != null);
        }

        public static bool HasLibrary(IList<SavedLibrary> libraries, string directory)
        {
            if (libraries == null)
                return false;

            var project = libraries.FirstOrDefault(l => string.Compare(l.Directory, directory, true) == 0);
            return (project != null);
        }

        public bool HasFavoriteProject(string directory) =>
            HasProject(FavoriteProjects, directory);

        public bool HasLibrary(string directory) =>
            HasLibrary(Libraries, directory);

        public SavedProject FindRecentProject(string directory) =>
            RecentProjects.FirstOrDefault(p => string.Compare(p.Directory, directory, true) == 0);

        public static string StorageFileName => Path.Combine(Parameters.ApplicationDataFolder, "SavedProjects.xml");

        public static SavedProjectsStorage Load()
        {
            SavedProjectsStorage storage = null;
            
            var storageFileName = StorageFileName;
            try
            {
                if (!string.IsNullOrWhiteSpace(storageFileName) && File.Exists(storageFileName))
                    lock (Lock)
                        storage = Utils.DeserializeObjectFromFile<SavedProjectsStorage>(storageFileName);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (storage == null)
                    storage = new SavedProjectsStorage();
            }

            foreach (var project in storage.FavoriteProjects)
                project.IsFavorite = true;

            return storage;
        }

        public void Save()
        {
            var storageFileName = StorageFileName;

            RecentProjects.Sort(new SavedStorageLastAccessComparer());

            if (RecentProjects.Count > 10)
                RecentProjects.RemoveRange(10, RecentProjects.Count - 10);

            lock (Lock)
                Utils.SerializeObjectToFile<SavedProjectsStorage>(this, storageFileName);
        }
    }
}
