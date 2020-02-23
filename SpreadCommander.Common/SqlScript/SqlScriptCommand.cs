using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.SqlScript
{
    public class SqlScriptCommand
    {
        #region TableRelation
        public class TableRelation
        {
            public string RelationName		{ get; set; }
            public string ParentTable		{ get; set; }
            public string ChildTable		{ get; set; }
            public string[] ParentColumns	{ get; set; }
            public string[] ChildColumns	{ get; set; }
        }
        #endregion

        private string _Text;
        public string Text { get => _Text; set => SetCommandText(value); }

        public int StartLineNumber { get; set; }

        public List<SqlDirective> Directives { get; } = new List<SqlDirective>();
        public List<BaseCommand> Commands    { get; } = new List<BaseCommand>();
        public List<string> Parameters       { get; } = new List<string>();

        protected virtual void SetCommandText(string commandText)
        {
            _Text = null;
            Directives.Clear();
            Commands.Clear();
            Parameters.Clear();

            if (string.IsNullOrWhiteSpace(commandText))
                return;

            var cmdText       = new StringBuilder();
            var directiveText = new StringBuilder();
            int i             = 0;
            char c;
            bool handled;
            string directive;
            
            while (i < commandText.Length)
            {
                handled = false;

                c = commandText[i];

                switch (c)
                {
                    case '"':
                        cmdText.Append(c);
                        i++;
                        while (i < commandText.Length)
                        {
                            c = commandText[i];
                            cmdText.Append(c);
                            if (c == '"')
                            {
                                handled = true;
                                break;
                            }

                            i++;
                        }
                        break;
                    case '\'':
                        cmdText.Append(c);
                        i++;
                        while (i < commandText.Length)
                        {
                            c = commandText[i];
                            cmdText.Append(c);
                            if (c == '\'')
                            {
                                handled = true;
                                break;
                            }

                            i++;
                        }
                        break;
                    case '`':
                        cmdText.Append(c);
                        i++;
                        while (i < commandText.Length)
                        {
                            c = commandText[i];
                            cmdText.Append(c);
                            if (c == '`')
                            {
                                handled = true;
                                break;
                            }

                            i++;
                        }
                        break;
                    case '[':
                        cmdText.Append(c);
                        i++;
                        while (i < commandText.Length)
                        {
                            c = commandText[i];
                            cmdText.Append(c);
                            if (c == ']')
                            {
                                handled = true;
                                break;
                            }

                            i++;
                        }
                        break;

                    case '-':
                        if (i < commandText.Length - 2 && commandText[i+1] == '-' && commandText[i+2] == '#')
                        {
                            i++; i++; i++;
                            c = commandText[i];
                            char quoteChar = '\0';
                            while (i < commandText.Length && c != '\r' && c != '\n')
                            {
                                if (quoteChar != '\0' && quoteChar == c)
                                    quoteChar = '\0';
                                else if (quoteChar == '\0' && (c == '"' || c == '\'' || c == '`'))
                                    quoteChar = c;
                                else if (quoteChar == '\0' && c == '[')
                                    quoteChar = ']';

                                if (quoteChar == '\0' && c == ';')
                                {
                                    directive = directiveText.ToString();
                                    if (!string.IsNullOrWhiteSpace(directive))
                                        Directives.Add(new SqlDirective() { Text = directive });
                                    directiveText.Clear();
                                }
                                else
                                    directiveText.Append(c);
                                cmdText.Append(' ');
                                i++;
                                if (i < commandText.Length)
                                    c = commandText[i];
                            }

                            directive = directiveText.ToString();
                            if (!string.IsNullOrWhiteSpace(directive))
                                Directives.Add(new SqlDirective() { Text = directive });
                            directiveText.Clear();
                        }
                        break;
                    case '/':
                        if (i < commandText.Length - 2 && commandText[i+1] == '*' && commandText[i+2] == '#')
                        {
                            i++; i++; i++;
                            char quoteChar = '\0';
                            do
                            {
                                c = commandText[i];
                                if (i < commandText.Length - 2 && c == '#' && commandText[i + 1] == '*' && commandText[i + 2] == '/')
                                {
                                    i++; i++;
                                    handled = true;
                                    break;
                                }

                                if (quoteChar != '\0' && quoteChar == c)
                                    quoteChar = '\0';
                                else if (quoteChar == '\0' && (c == '"' || c == '\'' || c == '`'))
                                    quoteChar = c;
                                else if (quoteChar == '\0' && c == '[')
                                    quoteChar = ']';

                                if (quoteChar == '\0' && c == ';')
                                {
                                    directive = directiveText.ToString();
                                    if (!string.IsNullOrWhiteSpace(directive))
                                        Directives.Add(new SqlDirective() { Text = directive });
                                    directiveText.Clear();
                                }
                                else
                                    directiveText.Append(c);

                                if (c == '\r' || c == '\n')
                                    cmdText.Append(c);
                                else
                                    cmdText.Append(' ');
                                i++;
                            }
                            while (i < commandText.Length);

                            directive = directiveText.ToString();
                            if (!string.IsNullOrWhiteSpace(directive))
                                Directives.Add(new SqlDirective() { Text = directive });
                            directiveText.Clear();
                        }
                        break;
                }

                if (!handled && i < commandText.Length)
                    cmdText.Append(c);

                i++;
            }

            _Text = cmdText.ToString();
            directive = directiveText.ToString();
            if (!string.IsNullOrWhiteSpace(directive))
                Directives.Add(new SqlDirective() { Text = directive });

            ListCommands();
            ListParameters();
        }

        private void ListCommands()
        {
            Commands.Clear();

            foreach (var directive in Directives)
            {
                if (!string.IsNullOrWhiteSpace(directive.Text))
                {
                    var scanner = new Scanner();
                    var parser  = new Parser(scanner);

                    var tree = parser.Parse(directive.Text);
                    if (tree.Errors.Count > 0)
                        continue;

                    try
                    {
                        var commands = tree.Eval() as List<BaseCommand>;
                        Commands.AddRange(commands);
                    }
                    catch (Exception)
                    {
                        //Do nothing, skip invalid commands
                    }
                }
            }
        }

        private void ListParameters()
        {
            Parameters.Clear();

            var parameters = ListCommands<Parameter>();
            foreach (var parameter in parameters)
                Parameters.Add(parameter.Name);
        }

        public bool AllParameters
        {
            get
            {
                foreach (var parameter in Parameters)
                {
                    if (string.Compare(parameter, "ALL", true) == 0)
                        return true;
                }

                return false;
            }
        }

        public T[] ListCommands<T>() where T: BaseCommand
        {
            var result = Commands.Where(c => c is T).OfType<T>().ToArray();
            return result;
        }

        public string[] GetTableNames()
        {
            var tableNames = ListCommands<Table>();
            var result     = tableNames.Select(t => t.TableName);
            return result.ToArray();
        }

        public Table[] GetTables()
        {
            var result = ListCommands<Table>();
            return result.ToArray();
        }

        public Relation[] GetDataRelations()
        {
            var result = ListCommands<Relation>();
            return result.ToArray();
        }

        public FormatCondition[] GetTableFormatting()
        {
            var result = ListCommands<FormatCondition>();
            return result;
        }
        
        public ComputedColumn[] GetTableComputedColumns()
        {
            var result = ListCommands<ComputedColumn>();
            return result;
        }

        public string GetConnectionName()
        {
            var connections = ListCommands<ConsoleCommands.Connection>();
            return connections.Length > 0 ? connections[0].Name : null;
        }
    }
}
