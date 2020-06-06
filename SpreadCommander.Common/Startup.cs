using MathNet.Numerics;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common
{
    public static class Startup
    {
        public static void Initialize()
        {
            SCDispatcherService.InitializeUIDispatcherService();


#pragma warning disable CRRSP05 // A misspelled word has been found
            //Use a fast codepath on AMD Ryzen/TR CPUs
            if (ApplicationSettings.Default.OptimizeAmdMath)
                Environment.SetEnvironmentVariable("MKL_DEBUG_CPU_TYPE", "5", EnvironmentVariableTarget.Process);
#pragma warning restore CRRSP05 // A misspelled word has been found

            //Control.TryUseNativeOpenBLAS();
#if DEBUG
            Control.UseNativeMKL();
#else
            Control.TryUseNativeMKL();
#endif

            //Load MathNet.Symbolic assembly
            MathNet.Symbolics.Symbol.NewSymbol("a");
        }
    }
}
