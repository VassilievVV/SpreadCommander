using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;

namespace SpreadCommander.Common.ScriptEngines.ConsoleCommands
{
    public class FormatCondition: BaseCommand
    {
        public string TableName				{ get; set; }

        public string ColumnName			{ get; set; }

        public string FormatFilter			{ get; set; }

        public override string ToString()
        {
            return AsString();
        }

        public string AsString()
        {
            var result = new StringBuilder(1024);

            result.Append("FORMAT ");
            if (!string.IsNullOrWhiteSpace(TableName))
                result.Append("TABLE ").Append(Utils.QuoteString(TableName, "\"")).Append(' ');
            if (!string.IsNullOrWhiteSpace(ColumnName))
                result.Append("COLUMN ").Append(Utils.QuoteString(ColumnName, "\"")).Append(' ');
            if (!string.IsNullOrWhiteSpace(FormatFilter))
                result.Append("FOR ").Append(Utils.QuoteString(FormatFilter, "\"")).Append(' ');

            if (Properties.Count > 0)
            {
                result.AppendFormat("WITH ");
                bool firstProperty = true;
                foreach (KeyValuePair<string, string> property in Properties)
                {
                    if (!firstProperty)
                        result.Append(", ");
                    firstProperty = false;

                    result.Append(property.Key).Append('=').Append(Utils.QuoteString(property.Value, "\""));
                }
            }

            return result.ToString();
        }

        public override void Clear()
        {
            base.Clear();

            TableName    = null;
            ColumnName   = null;
            FormatFilter = null;
        }

        public static IList<FormatCondition> Parse(string value, bool skipErrors)
        {
            var result = new List<FormatCondition>();

            if (string.IsNullOrWhiteSpace(value))
                return result;

            var scanner = new Scanner();
            var parser  = new Parser(scanner);

            List<BaseCommand> commands = null;

            var tree = parser.Parse(value);
            if (tree.Errors.Count > 0)
            {
                var strErrors = new StringBuilder();

                foreach (var error in tree.Errors)
                {
                    if (strErrors.Length > 0)
                        strErrors.AppendLine();
                    strErrors.Append(error.Message);
                }

                if (skipErrors)
                    return result;
                else
                    throw new Exception(strErrors.ToString());
            }

            try
            {
                commands = tree.Eval() as List<BaseCommand>;
            }
            catch (Exception)
            {
                //Do nothing, skip invalid commands
            }

            if (commands != null)
                result.AddRange(commands.OfType<FormatCondition>());

            return result;
        }
    }
}
