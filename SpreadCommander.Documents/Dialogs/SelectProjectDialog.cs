using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.Code;
using System.IO;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using DevExpress.XtraLayout.Utils;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using System.Diagnostics;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class SelectProjectDialog : DevExpress.XtraEditors.XtraForm
    {
        private enum LibraryKind { Favorites, Recent, Library, Examples }

        private SavedProjectsStorage _Storage;
        private LibraryKind _CurrentLibrary;
        private bool _Starting = true;

        public SelectProjectDialog()
        {
            InitializeComponent();
        }

        public string SelectedProject { get; private set; }

        private void CleanupStorage()
        {
            switch (_CurrentLibrary)
            {
                case LibraryKind.Favorites:
                    //Remove projects unmarked from favorites
                    for (int i = _Storage.FavoriteProjects.Count-1; i >= 0; i--)
                    {
                        if (!_Storage.FavoriteProjects[i].IsFavorite)
                            _Storage.FavoriteProjects.RemoveAt(i);
                    }
                    break;
            }
        }

        private void ShowLibrary(LibraryKind kind, string projectPath = null)
        {
            using (new UsingProcessor(
                () => { if (!_Starting) transitionManager.StartTransition(layoutControlRoot); },
                () => { if (!_Starting) transitionManager.EndTransition(); } ))
            {
                layoutLookupLibraries.Visibility = (kind == LibraryKind.Library) ? LayoutVisibility.Always : LayoutVisibility.Never;

                CleanupStorage();

                bindingProjects.Clear();
                _CurrentLibrary = kind;

                switch (_CurrentLibrary)
                {
                    case LibraryKind.Favorites:
                        layoutControlGroupProjects.Text = "Favorite Projects";
                        _Storage.FavoriteProjects.Sort(new SavedStorageNameComparer());
                        LoadProjects(_Storage.FavoriteProjects);
                        break;
                    case LibraryKind.Recent:
                        layoutControlGroupProjects.Text = "Recent Projects";
                        _Storage.RecentProjects.Sort(new SavedStorageLastAccessComparer());
                        LoadProjects(_Storage.RecentProjects);
                        break;
                    case LibraryKind.Library:
                        layoutControlGroupProjects.Text = "Libraries";
                        if (!string.IsNullOrWhiteSpace(projectPath) && Directory.Exists(projectPath))
                            LoadProjectsFromDirectory(projectPath);
                        else if (bindingLibraries.Current is SavedLibrary library)
                            LoadProjectsFromDirectory(library.Directory);
                        break;
                    case LibraryKind.Examples:
                        layoutControlGroupProjects.Text = "Examples";
                        var folderExamples = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                            Parameters.ApplicationName, "Examples");
                        if (Directory.Exists(folderExamples))
                            LoadProjectsFromDirectory(folderExamples);
                        break;
                    default:
                        break;
                }
            }


            static bool IsSpreadCommanderProject(string directory)
            {
                if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                    return false;

                var projectFile = Path.Combine(directory, Project.ProjectFileName);
                return File.Exists(projectFile);
            }

            void LoadProjectsFromDirectory(string directory)
            {
                if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                    return;

                var projects = new List<SavedProject>();

                var folders  = Directory.GetDirectories(directory);
                foreach (var folder in folders)
                {
                    if (!IsSpreadCommanderProject(folder))
                        continue;

                    var project = new SavedProject()
                    {
                        Directory = folder
                    };

                    projects.Add(project);
                }

                LoadProjects(projects);
            }

            void LoadProjects(IList<SavedProject> projects)
            {
                bindingProjects.Clear();

                using (new UsingProcessor(() => gridProjects.BeginUpdate(), 
                    () => gridProjects.EndUpdate()))
                {
                    foreach (var project in projects)
                    {
                        project.IsFavorite = _Storage.HasFavoriteProject(project.Directory);
                        bindingProjects.Add(project);
                    }

                    if (bindingProjects.Count > 0)
                        bindingProjects.Position = 0;
                }
            }
        }

        private void LoadSavedProjects()
        {
            _Storage = SavedProjectsStorage.Load();

            bindingLibraries.Clear();
            foreach (var library in _Storage.Libraries)
                bindingLibraries.Add(library);

            if (bindingLibraries.Count > 0)
                lookupLibraries.EditValue = bindingLibraries[0];
        }

        private void SaveSavedProjects()
        {
            _Storage.Save();
        }

        private void SelectProjectDialog_Load(object sender, EventArgs e)
        {
            LoadSavedProjects();

            if (_Storage.FavoriteProjects.Count > 0)
                ShowLibrary(LibraryKind.Favorites);
            else if (_Storage.RecentProjects.Count > 0)
                ShowLibrary(LibraryKind.Recent);
            else if (_Storage.Libraries.Count > 0)
                ShowLibrary(LibraryKind.Library);
            else
                ShowLibrary(LibraryKind.Examples);

            _Starting = false;
        }

        private void SelectProjectDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing && e.CloseReason != CloseReason.None)
                return;

            if (DialogResult != DialogResult.OK)
                return;

            if (string.IsNullOrWhiteSpace(SelectedProject) && bindingProjects.Current is SavedProject project)
                SelectedProject = project.Directory;

            SaveSavedProjects();
        }

        private void AccordionControlFavorites_Click(object sender, EventArgs e)
        {
            ShowLibrary(LibraryKind.Favorites);
        }

        private void AccordionControlRecent_Click(object sender, EventArgs e)
        {
            ShowLibrary(LibraryKind.Recent);
        }

        private void AccordionControlLibraries_Click(object sender, EventArgs e)
        {
            ShowLibrary(LibraryKind.Library);
        }

        private void AccordionControlExamples_Click(object sender, EventArgs e)
        {
            ShowLibrary(LibraryKind.Examples);
        }

        private void AccordionControlOpen_Click(object sender, EventArgs e)
        {
            if (dlgFolderBrowser.ShowDialog(this) != DialogResult.OK)
                return;

            SelectedProject = dlgFolderBrowser.SelectedPath;
            DialogResult = DialogResult.OK;
        }

        private void LookupLibraries_EditValueChanged(object sender, EventArgs e)
        {
            ShowLibrary(LibraryKind.Library, (bindingLibraries.Current as SavedLibrary)?.Directory);
        }

        private void LookupLibraries_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
#pragma warning disable IDE0019 // Use pattern matching
            var currentLibrary = bindingLibraries.Current as SavedLibrary;
#pragma warning restore IDE0019 // Use pattern matching

            switch ((string)e.Button.Tag)
            {
                case "Add":
                    if (dlgFolderBrowser.ShowDialog(this) != DialogResult.OK)
                        return;

                    if (_Storage.HasLibrary(dlgFolderBrowser.SelectedPath))
                    {
                        XtraMessageBox.Show(this, $"Selected library already exists", "Library already exists",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var newLibrary = new SavedLibrary() { Directory = dlgFolderBrowser.SelectedPath };
                    _Storage.Libraries.Add(newLibrary);
                    var libIndex = bindingLibraries.Add(newLibrary);
                    bindingLibraries.Position = libIndex;
                    lookupLibraries.EditValue = newLibrary;
                    break;
                case "Remove":
                    if (currentLibrary == null)
                        return;

                    if (XtraMessageBox.Show(this, $"Do you want to remove library '{currentLibrary.LibraryName}'?", "Confirm removing library",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    bindingLibraries.Remove(currentLibrary);
                    _Storage.Libraries.Remove(currentLibrary);
                    break;
                case "Explore":
                    if (currentLibrary == null)
                        return;

                    Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"),
                        Utils.QuoteString(currentLibrary.Directory, "\""));
                    break;
            }
        }

        private void RepositoryItemIsFavorite_CheckedChanged(object sender, EventArgs e)
        {
            if (!(bindingProjects.Current is SavedProject project))
                return;

            project.IsFavorite = !project.IsFavorite;

            switch (_CurrentLibrary)
            {
                case LibraryKind.Favorites:
                    if (!project.IsFavorite && _Storage.FavoriteProjects.Contains(project))
                        _Storage.FavoriteProjects.Remove(project);
                    break;
                case LibraryKind.Recent:
                case LibraryKind.Library:
                case LibraryKind.Examples:
                    if (project.IsFavorite && !_Storage.HasFavoriteProject(project.Directory))
                        _Storage.FavoriteProjects.Insert(0, project);
                    break;
            }
        }
    }
}