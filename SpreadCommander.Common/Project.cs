using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common
{
    public class Project
    {
        private static Project _CurrentProject;
        public static EventHandler ProjectChanged;

        static Project()
        {
            Initialize();
        }

        private static void Initialize()
        {
            var dirDefault = ApplicationSettings.Default.DefaultProjectLocation;

            if (string.IsNullOrWhiteSpace(dirDefault) || !Directory.Exists(dirDefault))
            {
                dirDefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    Parameters.ApplicationName, "DefaultProject");

                Directory.CreateDirectory(dirDefault);
            }

            var project = new Project();
            project.Load(dirDefault);

            Current = project;
        }

        public static Project Current
        {
            get => _CurrentProject;
            set
            {
                if (_CurrentProject == value)
                    return;

                _CurrentProject = value;

                ProjectChanged?.Invoke(_CurrentProject, EventArgs.Empty);
            }
        }

        public string ProjectFile
        {
            get
            {
                var dir = ProjectPath;
                return dir != null ? Path.Combine(dir, ProjectFileName) : null;
            }
        }

        public string ProjectPath { get; set; }

        public string Name => Path.GetFileName(ProjectPath);

        public void Load(string directory)
        {
            ProjectPath = directory;
            PrepareProject(ProjectPath);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public void Save(Stream stream)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //Do nothing
        }

#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
        public static string ProjectFileName => $"{Parameters.ApplicationName}.scproj";
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found

        public static Project CreateNewProject(string directory, bool makeCurrent = false)
        {
            var result = new Project()
            {
                ProjectPath = directory
            };
            PrepareProject(directory);
            if (makeCurrent)
                _CurrentProject = result;

            return result;
        }

        public static Project LoadProjectFromDirectory(string directory, bool makeCurrent = true)
        {
            var result = new Project()
            {
                ProjectPath = directory
            };
            PrepareProject(directory);
            if (makeCurrent)
                _CurrentProject = result;

            return result;
        }

        public static void PrepareProject(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new Exception("Project directory does not exist");


            var projectFile = Path.Combine(directory, ProjectFileName);
            if (!File.Exists(projectFile))
            {
                using var fs = File.Create(projectFile);
                var project = new Project() { ProjectPath = directory };
                project.Save(fs);
            }

            var dirSpreadCommander = Path.Combine(directory, $".{Parameters.ApplicationName}");
            if (!Directory.Exists(dirSpreadCommander))
                Directory.CreateDirectory(dirSpreadCommander);

            var dirHeap = Path.Combine(directory, ".Heap");
            if (!Directory.Exists(dirHeap))
                Directory.CreateDirectory(dirHeap);
        }

        public string MapPath(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName) && fileName.StartsWith("~\\"))
            {
                fileName = Path.Combine(ProjectPath, fileName.Substring(2));
            }
            return fileName;
        }

        public string CreateMappedPath(string fileName)
        {
            var projectPath = ProjectPath;
            if (!string.IsNullOrWhiteSpace(fileName) && fileName.StartsWith(projectPath, StringComparison.CurrentCultureIgnoreCase))
            {
                fileName = "~" + fileName.Substring(projectPath.Length);
            }
            return fileName;
        }
    }
}
