using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions
{
    public class RegexSplitFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public RegexSplitFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Input",      "String to evaluate",         "string"),
                new CustomFunctionArgumentDescription("Pattern",    "Regular expression pattern", "string"),
                new CustomFunctionArgumentDescription("Horizontal", "Whether to return array in horizontal or vertical array. Default - vertical.", "bool")
            };
        }

        public string Name                                             => "REGEX.SPLIT";
        public string Description                                      => "Splits an input string into an array of substrings at the positions defined by a regular expression pattern";
        public ParameterInfo[] Parameters                              => _Parameters;
        public CustomFunctionArgumentsDescriptionsCollection Arguments => _ArgumentDescriptions;
        public ParameterType ReturnType                                => ParameterType.Array;
        public bool Volatile                                           => false;

        public string GetName(CultureInfo culture) => Name;

        public ParameterValue Evaluate(IList<ParameterValue> parameters, EvaluationContext context)
        {
            if (parameters.Count < 2)
                return ParameterValue.ErrorInvalidValueInFunction;

            foreach (var parameter in parameters)
                if (parameter.IsError)
                    return ParameterValue.ErrorInvalidValueInFunction;

            try
            {
                var input   = parameters[0].ToString();
                var pattern = parameters[1].ToString();

                if (input == null || pattern == null)
                    return new CellValue[,] { };

#pragma warning disable IDE0075 // Simplify conditional expression
                bool horizontal = parameters.Count > 2 ? parameters[2].BooleanValue : false;
#pragma warning restore IDE0075 // Simplify conditional expression

                var split = Regex.Split(input, pattern);

                if (split == null || split.Length <= 0)
                    return new CellValue[,] { };

                if (!horizontal)
                {
                    var result = new CellValue[split.Length, 1];
                    for (int i = 0; i < split.Length; i++)
                        result[i, 0] = split[i];

                    return result;
                }
                else
                {
                    var result = new CellValue[1, split.Length];
                    for (int i = 0; i < split.Length; i++)
                        result[0, i] = split[i];

                    return result;
                }
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}