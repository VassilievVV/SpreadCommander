using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class FSharpScriptEngine
    {
        public override void ListScriptIntellisenseItems(string fileName, string text, string[] lines, Point caretPosition, ScriptIntellisense intellisense)
        {
            var session = _Session;
            if (session == null)
                return;

            var ident = GetIdent();
 
            var completions = session.GetCompletions(ident);
            if (completions == null)
                return;

            var dictVariables = new Dictionary<string, int>();
            try
            {
                var values = session.GetBoundValues();
                foreach (var value in values)
                {
                    if (!string.IsNullOrWhiteSpace(value.Name))
                    {
                        if (dictVariables.ContainsKey(value.Name))
                            dictVariables[value.Name]++;
                        else
                            dictVariables.Add(value.Name, 1);
                    }
                }
            }
            catch (Exception)
            {
                //Do nothing
            }

            foreach (var completion in completions.Distinct().OrderBy(str => str, StringLogicalComparer.DefaultComparer))
            {
                if (string.IsNullOrWhiteSpace(completion))
                    continue;

                //printf does not work correctly
#pragma warning disable CRRSP06 // A misspelled word has been found
                if (completion.Contains("printf", StringComparison.CurrentCultureIgnoreCase))
#pragma warning restore CRRSP06 // A misspelled word has been found
                    continue;

                bool isVariable = dictVariables.ContainsKey(completion);

                var item = new ScriptIntellisenseItem()
                {
                    ItemType = isVariable ? ScriptIntellisenseItem.IntellisenseItemType.Variable : ScriptIntellisenseItem.IntellisenseItemType.Property,
                    Caption  = completion,
                    Value    = completion
                };

                intellisense.Items.Add(item);
            }


            string GetIdent()
            {
                var line = lines[caretPosition.Y];
                if (line == null)
                    return null;
                line = line[..Math.Min(line.Length, caretPosition.X)];
                if (string.IsNullOrWhiteSpace(line))
                    return null;
            
                var match = Regex.Match(line, @"\S+\s*$");
                if (!match.Success)
                    return null;

                return match.Value;
            }
        }
    }
}
