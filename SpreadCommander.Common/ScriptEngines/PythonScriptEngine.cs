using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.ScriptEngines
{
	public class PythonScriptEngine: ProcessScriptEngine
	{
		public const string ScriptEngineName = "Python";

		public override string EngineName => ScriptEngineName;
		public override string DefaultExt => ".py";
		public override string FileFilter => "Python files (*.py)|*.py";
		public override string SyntaxFile => "Python";

		public override string ProcessPath
		{
			get
			{
				const string software = "SOFTWARE";

				var key = Registry.CurrentUser.OpenSubKey(software);
				if (key == null)
					key = Registry.LocalMachine.OpenSubKey(software);
				if (key == null)
					return null;

				var pythonCoreKey = key.OpenSubKey(@"Python\PythonCore");
				if (pythonCoreKey == null)
					pythonCoreKey = key.OpenSubKey(@"Wow6432Node\Python\PythonCore");
				if (pythonCoreKey == null)
					return null;

				var subKeys = new List<string>(pythonCoreKey.GetSubKeyNames());
				subKeys.Sort(StringLogicalComparer.DefaultComparer);
				var targetVersion = subKeys.LastOrDefault();

				var installPathKey = pythonCoreKey.OpenSubKey($@"{targetVersion}\InstallPath");
				if (installPathKey == null)
					return null;

				var result = Convert.ToString(installPathKey.GetValue(string.Empty));
				if (string.IsNullOrWhiteSpace(result))
					result = Convert.ToString(installPathKey.GetValue("ExecutablePath"));

				if (!string.IsNullOrWhiteSpace(result))
					result = Path.Combine(result, "python.exe");

				return result;
			}
		}

		public override string ProcessArguments => "--i";
	}
}
