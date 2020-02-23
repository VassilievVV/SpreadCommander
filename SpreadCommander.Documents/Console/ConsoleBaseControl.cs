using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.ViewModels;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleBaseControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler Modified;

        public ConsoleBaseControl()
        {
            InitializeComponent();
        }

        private BaseDocumentViewModel _ViewModel;
        internal virtual BaseDocumentViewModel ViewModel
        {
            get => _ViewModel;
            set
            {
                if (_ViewModel == value)
                    return;

                _ViewModel = value;
                ViewModelChanged();
            }
        }

        protected virtual void ViewModelChanged()
        {
            //Do nothing
        }

        protected virtual void FireModified(bool documentModified)
        {
            Modified?.Invoke(this, new EventArgs());
            if (documentModified)
                _ViewModel?.DocumentModified();
        }
    }
}
