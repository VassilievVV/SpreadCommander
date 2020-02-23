using DevExpress.XtraEditors;
using SpreadCommander.Common.ScriptEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Automation = System.Management.Automation;

namespace SpreadCommander.Common.Code
{
    public class CmdletIntellisenseHelp: ScriptIntellisenseHelp
    {
        public CmdletIntellisenseHelp(string cmdletName)
        {
            CmdletName = cmdletName;
        }

        private string CmdletName { get; }

        public override bool SupportsHelp       => true;
        public override bool SupportsOnlineHelp => true;

        public override string GetHelpHtmlContent(ScriptIntellisenseItem item)
        {
            string cmdletName = CmdletName ?? item?.Value;
            if (string.IsNullOrWhiteSpace(cmdletName))
                return null;

            using (var runspace = PowerShellScriptEngine.CreateRunspace())
            {
                using var ps = Automation.PowerShell.Create();
                ps.Runspace = runspace;
                ps.AddCommand("get-help").AddParameter("Name", cmdletName).AddParameter("Full").AddCommand("out-string").AddParameter("Width", 10000);

                var commands = ps.Invoke();
                var command  = commands?.FirstOrDefault();
                var result   = Convert.ToString(command?.BaseObject);

                if (string.IsNullOrWhiteSpace(result))
                    return null;

                result = HtmlEncode(result);

                var reHeaders = new Regex(@"(?m)(^\w[\w \t]+)");
                result = reHeaders.Replace(result, "<b><u>$1</u></b>");

                var reParameters = new Regex(@"(?m)^(?:[ \t]*(\-\w+(?:\s*\[?&lt;.*?&gt;\]?)?))\s*<br>\s*$");
                result = reParameters.Replace(result, "<b>$1</b><br>\r\n");

                var reCommonParameters = new Regex(@"(?m)^[ \t]*(&lt;CommonParameters&gt;)\s*<br>\s*$");
                result = reCommonParameters.Replace(result, "<b>$1</b><br>\r\n");

                result = result.Replace(" ", "&nbsp;");

                return result;
            }


            static string HtmlEncode(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return value;

                var result = value.Replace("<", "&lt;").Replace(">", "&gt;"). 
                    Replace(Environment.NewLine, $"<br>{Environment.NewLine}");
                return result;
            }
        }

        public override void ShowOnlineHelp(ScriptIntellisenseItem item)
        {
            string cmdletName = CmdletName ?? item?.Value;
            if (string.IsNullOrWhiteSpace(cmdletName))
            {
                XtraMessageBox.Show("Cmdlet name is empty.", "Cannot show help for cmdlet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var runspace = PowerShellScriptEngine.CreateRunspace();
                using var ps       = Automation.PowerShell.Create();
                ps.Runspace        = runspace;
                ps.AddCommand("get-help").AddParameter("Name", cmdletName).AddParameter("Online");

                ps.Invoke();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Cannot show online help", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
