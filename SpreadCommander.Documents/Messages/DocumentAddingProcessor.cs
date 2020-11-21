using DevExpress.Mvvm;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Messages
{
    public class DocumentAddingProcessor : IDisposable
    {
        #region ILoadingCompleted
        public interface ILoadingCompleted
        {
            event EventHandler LoadingCompleted;
        }
        #endregion

        private XtraForm _DocumentView;

        public DocumentAddingProcessor(XtraForm documentView)
        {
            _DocumentView = documentView ?? throw new ArgumentException(nameof(documentView));
            Messenger.Default.Send(new DocumentStartAddingMessage() { DocumentView = _DocumentView });

            if (_DocumentView is ILoadingCompleted loadingCompleted)
                loadingCompleted.LoadingCompleted += DocumentView_LoadingCompleted;
            else
                _DocumentView.Load += DocumentView_Load;
        }

        private void DocumentView_LoadingCompleted(object sender, EventArgs e)
        {
            if (_DocumentView is ILoadingCompleted loadingCompleted)
                loadingCompleted.LoadingCompleted -= DocumentView_LoadingCompleted;

            DocumentLoaded();            
        }

        private void DocumentView_Load(object sender, EventArgs e)
        {
            var doc = _DocumentView;
            if (doc != null)
                doc.Load -= DocumentView_Load;

            DocumentLoaded();
        }

        public void Dispose()
        {
            if (_DocumentView != null)
            {
                Task.Run(() =>
                {
                    //Give 5 seconds for document to finish loading. If it did not - stop animation.
                    Thread.Sleep(5000); 
                    DocumentLoaded();
                });
            }
        }

        protected virtual void DocumentLoaded()
        {
            var doc = _DocumentView;
            if (doc != null)
            {
                Task.Run(() =>
                {
                    //Let document finish loading
                    Thread.Sleep(100);
                    doc.Invoke((MethodInvoker)(() =>
                    {
                        Messenger.Default.Send(new DocumentEndAddingMessage() { DocumentView = _DocumentView });
                        _DocumentView = null;
                    }));
                });
            }
        }
    }
}
