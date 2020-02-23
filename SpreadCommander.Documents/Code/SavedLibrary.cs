using DevExpress.Mvvm;
using Newtonsoft.Json;
using SpreadCommander.Common;
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
    public class SavedLibrary: BindableBase
    {
        [DataMember()]
        public string Directory
        {
            get => GetProperty(() => Directory);
            set => SetProperty(() => Directory, value);
        }

        [XmlIgnore()]
        [JsonIgnore()]
        public string LibraryName
        {
            get
            {
                var path = Directory;
                if (string.IsNullOrWhiteSpace(path))
                    return null;

                var info = new DirectoryInfo(path);
                return info.Name;
            }
        }
    }
}
