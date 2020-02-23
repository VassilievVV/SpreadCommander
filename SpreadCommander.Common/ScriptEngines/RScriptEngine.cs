using Microsoft.Win32;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.ScriptEngines
{
    public class RScriptEngine: ProcessScriptEngine
    {
        public const string ScriptEngineName = "R";

        public override string EngineName => ScriptEngineName;
        public override string DefaultExt => ".r";
        public override string FileFilter => "R files (*.r)|*.r";
        public override string SyntaxFile => "R";

        public override bool LfInSendCommand => true;


        protected virtual string GetProcessPath(bool isScript)
        {
            var rKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\R-core\R64");
            if (rKey == null)
                rKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\R-core\R64");
            bool is_x64 = rKey != null;
            if (rKey == null)
                rKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\R-core\R");
            if (rKey == null)
                rKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\R-core\R");

            if (rKey == null)
                return null;

            string path = null;
            try
            {
                path = Convert.ToString(rKey.GetValue("InstallPath"));

                var version = Convert.ToString(rKey.GetValue("Current Version"));

                path = Path.Combine(path, "bin");

                if (StringLogicalComparer.Compare(version, "2.12.0.0", true) >= 0)
                    path = Path.Combine(path, is_x64 ? "x64" : "i386");

                var cmdFile = isScript ? "Rscript.exe" : "Rterm.exe";

                path = Path.Combine(path, cmdFile);
            }
            finally
            {
                rKey.Close();
            }

            return path;
        }

        public override string ProcessPath       => GetProcessPath(false);
        public override string ProcessScriptPath => GetProcessPath(true);

#pragma warning disable CRRSP01 // A misspelled word has been found
        //public override string ProcessArguments       => "--slave --vanilla –-no-Rconsole ";
#pragma warning restore CRRSP01 // A misspelled word has been found
        //public override string ProcessScriptArguments => "--vanilla";
    }
}
