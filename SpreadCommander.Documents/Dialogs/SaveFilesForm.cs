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
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Common;
using SpreadCommander.Documents.Services;
using System.IO;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class SaveFilesForm : BaseForm
    {
        public SaveFilesForm()
        {
            InitializeComponent();
        }

        public void AddFile(SaveFileData fileData)
        {
            fileData.Selected = true;
            bindingFiles.Add(fileData);
        }

        public void AddFiles(IList<SaveFileData> filesData)
        {
            foreach (var fileData in filesData)
                AddFile(fileData);
        }

        public void AddFile(BaseDocumentViewModel viewModel, string fileName)
        {
            bindingFiles.Add(new SaveFileData()
            {
                ViewModel = viewModel,
                Title     = Convert.ToString(viewModel.Title),
                FileName  = Project.Current.CreateMappedPath(fileName)
            });
        }

        private void SaveFilesForm_Load(object sender, EventArgs e)
        {
            foreach (SaveFileData saveFile in bindingFiles)
                saveFile.FileName = Project.Current.CreateMappedPath(saveFile.FileName);
        }

        private void RepositoryItemFileName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (viewFiles.GetFocusedRow() is not SaveFileData fileData)
                return;

            string fileName = Convert.ToString(((ButtonEdit)sender).EditValue);

            dlgSave.DefaultExt = fileData.ViewModel.DefaultExt;
            dlgSave.Filter     = fileData.ViewModel.FileFilter;
            dlgSave.FileName   = Project.Current.MapPath(fileName);

            if (dlgSave.ShowDialog(this) != DialogResult.OK)
                return;

            ((ButtonEdit)sender).EditValue = Project.Current.CreateMappedPath(dlgSave.FileName);

            viewFiles.CloseEditor();
            viewFiles.UpdateCurrentRow();
        }

        private void SaveFilesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                return;

            var listSelectedFiles = new List<SaveFileData>();
            foreach (SaveFileData fileData in bindingFiles)
                if (fileData.Selected)
                    listSelectedFiles.Add(fileData);

            if (listSelectedFiles.Count <= 0)
                return;

            foreach (var fileData in listSelectedFiles)
            {
                if (string.IsNullOrWhiteSpace(fileData.FileName) || !Utils.IsFullFileNameValid(Project.Current.MapPath(fileData.FileName)))
                {
                    XtraMessageBox.Show(this, "One or more of filenames are not valid", "Invalid filename(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
            }

            var errors = new StringBuilder();

            foreach (var fileData in listSelectedFiles)
            {
                try
                {
                    fileData.ViewModel.SaveToFile(fileData.FileName);
                }
                catch (Exception ex)
                {
                    errors.AppendLine($"Cannot save file '{fileData.FileName}': {ex.Message}");
                }
            }

            if (errors.Length > 0)
                MemoEditor.ShowMemo(this, "Errors", errors.ToString());
        }

        private void ViewFiles_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (e.Row is not SaveFileData saveFile)
                return;

            if (!saveFile.Selected)
                return;

            if (string.IsNullOrWhiteSpace(saveFile.FileName))
            {
                e.Valid = false;
                viewFiles.SetColumnError(colFileName, "Filename cannot be empty");
            }
            else if (!Utils.IsFullFileNameValid(Project.Current.MapPath(saveFile.FileName)))
            {
                e.Valid = false;
                viewFiles.SetColumnError(colFileName, "Filename is not valid");
            }
            else
            {
                e.Valid = true;
                viewFiles.SetColumnError(colFileName, null, ErrorType.None);
            }
        }

        private void ViewFiles_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
    }
}