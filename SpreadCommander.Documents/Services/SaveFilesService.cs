using SpreadCommander.Documents.Code;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Documents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class SaveFilesService: ISaveFilesService
    {
        private readonly IWin32Window _Owner;

        public SaveFilesService(IWin32Window owner)
        {
            _Owner = owner;
        }

        public DialogResult SaveFiles(IList<SaveFileData> filesData)
        {
            if (filesData == null || filesData.Count <= 0)
                return DialogResult.OK; //If there are no files to save - treat it as if files were saved.

            using var frmSaveFiles = new SaveFilesForm();
            frmSaveFiles.AddFiles(filesData);
            var result = frmSaveFiles.ShowDialog(_Owner);
            return result;
        }

        public DialogResult SaveFiles(BaseDocumentViewModel viewModel)
        {
            var filesData = new List<SaveFileData>()
            {
                new SaveFileData()
                {
                    ViewModel    = viewModel,
                    Title        = Convert.ToString(viewModel.Title),
                    FileName     = viewModel.FileName
                }
            };

            var result = SaveFiles(filesData);
            return result;
        }
    }
}
