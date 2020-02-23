using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
    public interface ISCDispatcherService
    {
        Task BeginInvoke(Action action);
        void Invoke(Action action);
    }
}
