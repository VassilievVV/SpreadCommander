using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Common.Code
{
    public class SCDispatcherService : IDispatcherService, ISCDispatcherService
    {
        private readonly SynchronizationContext _SynchronizationContext;

        public static SCDispatcherService UIDispatcherServer { get; private set; }

        //Must be called from UI thread
        public static void InitializeUIDispatcherService()
        {
            UIDispatcherServer = new SCDispatcherService();
        }

        public SCDispatcherService()
        {
            _SynchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }

        public Task BeginInvoke(Action action)
        {
            var taskSource = new TaskCompletionSource<bool>();
            _SynchronizationContext.Post(callback, null);
            return taskSource.Task;


            void callback(object state)
            {
                try
                {
                    action.Invoke();
                    taskSource.SetResult(true);
                }
                catch (Exception ex)
                {
                    taskSource.SetException(ex);
                }
            }
        }

        public void Invoke(Action action)
        {
            _SynchronizationContext.Send(state => action.Invoke(), null);
        }
    }
}
