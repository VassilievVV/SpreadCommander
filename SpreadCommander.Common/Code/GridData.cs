using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;

namespace SpreadCommander.Common.Code
{
    public class GridData
    {
        public List<GridFormatColumn> ColumnOrder   { get; set; } = new List<GridFormatColumn>();
        public List<GridFormatColumn> GroupBy       { get; set; } = new List<GridFormatColumn>();
        public List<GridFormatColumn> OrderBy       { get; set; } = new List<GridFormatColumn>();
        public string Filter                        { get; set; }

        public List<GridFormatCondition> FormatConditions   { get; } = new List<GridFormatCondition>();
        public List<ComputedColumn> ComputedColumns         { get; } = new List<ComputedColumn>();
        public GridParametersObject Parameters              { get; set; } = new GridParametersObject();

        public void ApplyGridFormatConditions(List<ConsoleCommands.FormatCondition> commands)
        {
            var listCommands = new List<ConsoleCommands.BaseCommand>();
            listCommands.AddRange(commands);
            ApplyGridFormatConditions(listCommands);
        }

        public void ApplyGridFormatConditions(List<ConsoleCommands.BaseCommand> commands)
        {
            FormatConditions.Clear();
            ComputedColumns.Clear();

            var gridFormatConditions = LoadGridFormatConditions(commands);
            FormatConditions.AddRange(gridFormatConditions);

            ComputedColumns.AddRange(commands.OfType<ComputedColumn>());
        }

        public static List<GridFormatCondition> LoadGridFormatConditions(List<ConsoleCommands.BaseCommand> commands)
        {
            var result = new List<GridFormatCondition>();

            foreach (ConsoleCommands.FormatCondition formatCondition in commands.Where(c => c is ConsoleCommands.FormatCondition))
            {
                var gridFormatCondition = new GridFormatCondition()
                {
                    TableName        = formatCondition.TableName,
                    ColumnName       = formatCondition.ColumnName,
                    Expression       = formatCondition.FormatFilter,

                    TargetColumn     = formatCondition.GetProperty<string>("TargetColumn"),
                    ApplyToRow       = formatCondition.GetProperty<bool>("ApplyToRow"),

                    AppearanceName   = formatCondition.GetProperty<string>("AppearanceName"),
                    BackColor        = formatCondition.GetProperty<string>("BackColor"),
                    BackColor2       = formatCondition.GetProperty<string>("BackColor2"),
                    BorderColor      = formatCondition.GetProperty<string>("BorderColor"),
                    ColorScale       = formatCondition.GetProperty<string>("ColorScale"),
                    Condition        = formatCondition.GetProperty<string>("Condition"),
                    DataBar          = formatCondition.GetProperty<string>("DataBar"),
                    DateOccuring     = formatCondition.GetProperty<string>("DateOccuring"),
                    Font             = formatCondition.GetProperty<string>("Font"),
                    FormatRule       = formatCondition.GetProperty<string>("Rule"),
                    ForeColor        = formatCondition.GetProperty<string>("ForeColor"),
                    Gradient         = formatCondition.GetProperty<string>("Gradient"),
                    IconSet          = formatCondition.GetProperty<string>("IconSet"),
                    Rank             = formatCondition.GetProperty<string>("Rank"),
                    Value1           = formatCondition.GetProperty<string>("Value1"),
                    Value2           = formatCondition.GetProperty<string>("Value2")
                };

                if (!string.IsNullOrWhiteSpace(gridFormatCondition.FormatRule))
                {
                    //Do nothing
                }
                else if (!string.IsNullOrWhiteSpace(gridFormatCondition.Expression))
                    gridFormatCondition.FormatRule = "Expression";
                else
                    gridFormatCondition.FormatRule = FirstPropertyName(formatCondition,
                        new string[] { "DataBar", "IconSet", "ColorScale", "AboveAverage", "BelowAverage", "AboveOrEqualAverage", "BelowOrEqualAverage",
                            "Unique", "Duplicate", "Top", "Bottom", "DateOccuring", "Condition" });

                result.Add(gridFormatCondition);
            }

            return result;


            static string FirstPropertyName(ConsoleCommands.FormatCondition formatCondition, string[] propertyNames)
            {
                foreach (var propertyName in propertyNames)
                    if (formatCondition.HasProperty(propertyName))
                        return propertyName;
                return null;
            }
        }
    }
}
