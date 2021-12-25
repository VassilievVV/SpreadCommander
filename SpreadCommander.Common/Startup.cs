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

            //Use a fast CodePath on AMD Ryzen/TR CPUs
            if (ApplicationSettings.Default.OptimizeAmdMath)
                Environment.SetEnvironmentVariable("MKL_DEBUG_CPU_TYPE", "5", EnvironmentVariableTarget.Process);

            //Control.TryUseNativeOpenBLAS();
#if DEBUG
            Control.UseNativeMKL();
#else
            Control.TryUseNativeMKL();
#endif

            //Load MathNet.Symbolic assembly
            //var typeSymbol = typeof(MathNet.Symbolics.Symbol);
            var symbol = MathNet.Symbolics.SymbolicExpression.Variable("x");

            //Load MathNet.Numerics.Data.Text assembly
            //var typeDelimitedReader = typeof(MathNet.Numerics.Data.Text.DelimitedReader);
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            var _ = MathNet.Numerics.Data.Text.Compression.GZip;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
        }
    }
}
