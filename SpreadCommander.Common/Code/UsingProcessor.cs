using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
    public class UsingProcessor : IDisposable
    {
        private readonly Action OnWaitFinished;

        public UsingProcessor(Action onWaitStarted, Action onWaitFinished)
        {
            OnWaitFinished = onWaitFinished;
            onWaitStarted?.Invoke();
        }

        public void Dispose()
        {
            OnWaitFinished?.Invoke();
        }
    }
}
