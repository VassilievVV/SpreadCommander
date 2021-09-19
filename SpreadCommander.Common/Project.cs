using DevExpress.Charts.Native;
using DevExpress.Mvvm.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common
{
    public class Project
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        private static Project _CurrentProject;
        public static EventHandler ProjectChanged;
#pragma warning restore CA2211 // Non-constant fields should not be visible

        static Project()
        {
            Initialize();
        }

        public static string DefaultProjectPath
        {
            get
            {
                string dirDefault = ApplicationSettings.Default.DefaultProjectLocation;

                if (string.IsNullOrWhiteSpace(dirDefault) || !Directory.Exists(dirDefault))
                {
                    dirDefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        Parameters.ApplicationName, "DefaultProject");

                    Directory.CreateDirectory(dirDefault);
                }

                return dirDefault;
            }
        }

        private static void Initialize()
        {
            var dirDefault = DefaultProjectPath;

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

                CloseProject(_CurrentProject);

                _CurrentProject = value;

                OpenProject(_CurrentProject);

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

        public string Name => GetProjectNameFromPath(ProjectPath);

        public static string GetProjectNameFromPath(string projectPath)
        {
            projectPath = projectPath?.Trim()?.TrimEnd('\\', '/', ' ', '\t', '\r', '\n')?.TrimEnd();
            if (string.IsNullOrWhiteSpace(projectPath))
                return null;

            string projectName = Path.GetFileName(projectPath);
            return projectName;
        }

        public bool IsDefault => string.Compare(ProjectPath, ApplicationSettings.Default.DefaultProjectLocation, true) == 0;
        
        public static string FolderExamples => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
            Parameters.ApplicationName, "Examples");

        public bool IsExample => ProjectPath?.StartsWith(FolderExamples, StringComparison.CurrentCultureIgnoreCase) ?? false;

        public void Load(string directory)
        {
            ProjectPath = directory;
            PrepareProject(ProjectPath);
        }

#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0060 // Remove unused parameter
        public void Save(Stream stream)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1822 // Mark members as static
        {
            //Do nothing
        }

        public static string ProjectFileName => $"{Parameters.ApplicationName}.scproj";

        public static Project CreateNewProject(string directory, bool makeCurrent = false)
        {
            var result = new Project()
            {
                ProjectPath = directory
            };
            PrepareProject(directory);
            if (makeCurrent)
                Current = result;

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
                Current = result;

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

            CheckSubDirectory($".{Parameters.ApplicationName}");
            CheckSubDirectory(".Heap");
            CheckSubDirectory("bin");
            CheckSubDirectory("Modules");


            void CheckSubDirectory(string dir)
            {
                var subdir = Path.Combine(directory, dir);
                if (!Directory.Exists(subdir))
                    Directory.CreateDirectory(subdir);
            }
        }

        public string MapPath(string fileName)
        {
            if (fileName == "~")
                return ProjectPath;

            if (!string.IsNullOrWhiteSpace(fileName) && fileName.StartsWith("~\\"))
            {
                fileName = Path.Combine(ProjectPath, fileName[2..]);
            }
            return fileName;
        }

        public string CreateMappedPath(string fileName)
        {
            var projectPath = ProjectPath;
            if (!string.IsNullOrWhiteSpace(fileName) && fileName.StartsWith(projectPath, StringComparison.CurrentCultureIgnoreCase) &&
                fileName.Length > projectPath.Length)
            {
                int startPos = projectPath.Length + 1;
                while (fileName.Length > startPos && (fileName[startPos] == '\\' || fileName[startPos] == '/'))
                    startPos++;

                fileName = fileName[startPos..];
                fileName = Path.Combine("~", fileName);
            }
            return fileName;
        }

        /*
https://docs.microsoft.com/en-us/dotnet/core/dependency-loading/default-probing

TRUSTED_PLATFORM_ASSEMBLIES 	List of platform and application assembly file paths.
PLATFORM_RESOURCE_ROOTS 	    List of directory paths to search for satellite resource assemblies.
NATIVE_DLL_SEARCH_DIRECTORIES 	List of directory paths to search for unmanaged (native) libraries.
APP_PATHS 	                    List of directory paths to search for managed assemblies.
APP_NI_PATHS 	                List of directory paths to search for native images of managed assemblies.

Managed assembly default probing

When probing to locate a managed assembly, the AssemblyLoadContext.Default looks in order at:

    Files matching the AssemblyName.Name in TRUSTED_PLATFORM_ASSEMBLIES (after removing file extensions).
    Native image assembly files in APP_NI_PATHS with common file extensions.
    Assembly files in APP_PATHS with common file extensions.

Satellite (resource) assembly probing

To find a satellite assembly for a specific culture, construct a set of file paths.

For each path in PLATFORM_RESOURCE_ROOTS and then APP_PATHS, append the CultureInfo.Name string, a directory separator, the AssemblyName.Name string, and the extension '.dll'.

If any matching file exists, attempt to load and return it.

Unmanaged (native) library probing

When probing to locate an unmanaged library, the NATIVE_DLL_SEARCH_DIRECTORIES are searched looking for a matching library.
         */

        private static readonly string[] DependencyPaths        = { "APP_PATHS", "APP_NI_PATHS", "PLATFORM_RESOURCE_ROOTS", "NATIVE_DLL_SEARCH_DIRECTORIES" };
        private static readonly string[] ModulesDependencyPaths = { "PSModulePath" };

        private static void CloseProject(Project project)
        {
            if (project == null)
                return;

            string binPath = Path.Combine(project.ProjectPath, "bin");

            foreach (var path in DependencyPaths)
                RemoveEnvironmentVariable(path, binPath);


            static void RemoveEnvironmentVariable(string name, string value)
            {
                var vars = new List<string>(Utils.SplitString(Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process), ';'));
                if (vars.Count <= 0)
                    return;

                for (int i = vars.Count-1; i >= 0; i--)
                {
                    if (string.Compare(vars[i], value, true) == 0)
                        vars.RemoveAt(i);
                }

                var newValue = vars.ConcatStringsWithDelimiter(";");
                if (string.IsNullOrWhiteSpace(newValue))
                    newValue = null;
                Environment.SetEnvironmentVariable(name, newValue, EnvironmentVariableTarget.Process);
            }
        }

        private static void OpenProject(Project project)
        {
            if (project == null)
                return;

            string binPath = Path.Combine(project.ProjectPath, "bin");
            foreach (var path in DependencyPaths)
                AddEnvironmentVariable(path, binPath);

            string modulePath = Path.Combine(project.ProjectPath, "Modules");
            foreach (var path in ModulesDependencyPaths)
                AddEnvironmentVariable(path, modulePath);


            static void AddEnvironmentVariable(string name, string value)
            {
                var vars = new List<string>(Utils.SplitString(Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process), ';'));
                string existingValue = vars.FirstOrDefault(str => string.Compare(str, value, true) == 0);
                if (!string.IsNullOrWhiteSpace(existingValue))
                    return;

                vars.Insert(0, value);

                var newValue = vars.ConcatStringsWithDelimiter(";");
                if (string.IsNullOrWhiteSpace(newValue))
                    newValue = null;
                Environment.SetEnvironmentVariable(name, newValue, EnvironmentVariableTarget.Process);
            }
        }
    }
}
