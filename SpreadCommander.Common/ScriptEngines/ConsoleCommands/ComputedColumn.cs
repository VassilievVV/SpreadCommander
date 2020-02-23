using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;

namespace SpreadCommander.Common.ScriptEngines.ConsoleCommands
{
    public class ComputedColumn: BaseCommand
    {
        public enum ComputedColumnType { String, Integer, Decimal, DateTime, Boolean }

        public string TableName                 { get; set; }

        public string ColumnName                { get; set; }
        
        public ComputedColumnType ReturnType    { get; set; }

        public string Expression                { get; set; }

        public override string ToString()
        {
            return AsString();
        }

        public string AsString()
        {
            var result = new StringBuilder(1024);

            result.Append("COMPUTED COLUMN ").Append(Utils.QuoteString(ColumnName, "\"")).Append(' ').
                Append(Enum.GetName(typeof(ComputedColumnType), ReturnType)).Append(' ');
            if (!string.IsNullOrWhiteSpace(TableName))
                result.Append("IN ").Append(Utils.QuoteString(TableName, "\"")).Append(' ');
            result.Append(" = ").Append(Utils.QuoteString(Expression, "\"")).Append(' ');

            return result.ToString();
        }

        public override void Clear()
        {
            base.Clear();

            TableName  = null;
            ColumnName = null;
            Expression = null;
        }

        public static ComputedColumnType StringToReturnType(string value)
        {
            switch (value?.ToLower())
            {
                case "string":
                case "varchar":
                case "char":
                case "text":
                    return ComputedColumnType.String;
                case "integer":
                case "int":
                    return ComputedColumnType.Integer;
                case "decimal":
                case "numeric":
                case "double":
                case "float":
                    return ComputedColumnType.Decimal;
                case "datetime":
                case "date":
                    return ComputedColumnType.DateTime;
                case "boolean":
                case "bool":
                case "bit":
                    return ComputedColumnType.Boolean;
                default:
                    return ComputedColumnType.String;
            }
        }
        
        public static IList<ComputedColumn> Parse(string value, bool skipErrors)
        {
            var result = new List<ComputedColumn>();

            if (string.IsNullOrWhiteSpace(value))
                return result;

            var scanner = new Scanner();
            var parser = new Parser(scanner);

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
                result.AddRange(commands.OfType<ComputedColumn>());

            return result;
        }
    }
}
