using SpreadCommander.Common;
using SpreadCommander.Common.ScriptEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.ViewModels
{
    public class CustomConsoleDocumentViewModel: ConsoleDocumentViewModel
    {
        public CustomConsoleDocumentViewModel() : base(new PowerShellScriptEngine())
        {
        }

        public CustomConsoleDocumentViewModel(BaseScriptEngine engine): base(engine)
        {
        }

        public virtual string SaveScriptAndControlCaption => "Save script and control";

        public override string GetScriptFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            if (Engine is PowerShellScriptEngine)
                return $"{fileName}.ps1";
            else if (Engine is FSharpScriptEngine)
                return $"{fileName}.fsx";

            throw new Exception("Script engine is not initialized or is not supported.");
        }

        public override bool SaveCustomControlsToFile()
        {
            if (string.IsNullOrWhiteSpace(FileName))
            {
                var fileName = AskForFileName();
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;
                FileName = fileName;
            }

            SaveCustomControlsToFile(FileName);
            return true;
        }
    }
}
