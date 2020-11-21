using DevExpress.Mvvm;
using Newtonsoft.Json;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpreadCommander.Documents.Code
{
    [DataContract(Namespace = Parameters.ApplicationNamespace)]
    public class SavedProject: BindableBase
    {
        [DataMember()]
        public string Directory
        {
            get => GetProperty(() => Directory);
            set => SetProperty(() => Directory, value);
        }

        [DataMember()]
        public DateTime? LastAccess
        {
            get => GetProperty(() => LastAccess);
            set => SetProperty(() => LastAccess, value);
        }

        [XmlIgnore()]
        [JsonIgnore()]
        public bool IsFavorite
        {
            get => GetProperty(() => IsFavorite);
            set => SetProperty(() => IsFavorite, value);
        }

        [XmlIgnore()]
        [JsonIgnore()]
        public string ProjectName => Project.GetProjectNameFromPath(Directory);

        [XmlIgnore()]
        [JsonIgnore()]
        public bool IsExist
        {
            get
            {
                var path = Directory;
                return (!string.IsNullOrWhiteSpace(path) && System.IO.Directory.Exists(path));
            }
        }
    }

    public class SavedStorageNameComparer : IComparer<SavedProject>
    {
        public int Compare(SavedProject x, SavedProject y)
        {
            return StringLogicalComparer.Compare(x?.ProjectName, y?.ProjectName, false);
        }
    }

    public class SavedStorageLastAccessComparer : IComparer<SavedProject>
    {
        public int Compare(SavedProject x, SavedProject y)
        {
            if (x?.LastAccess == null && y?.LastAccess == null)
                return 0;
            if (x?.LastAccess == null)
                return 1;
            if (y?.LastAccess == null)
                return -1;

            return -DateTime.Compare(x.LastAccess.Value, y.LastAccess.Value);
        }
    }
}
