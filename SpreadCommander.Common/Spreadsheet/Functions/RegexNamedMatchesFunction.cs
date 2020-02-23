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
    public class RegexNamedMatchesFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public RegexNamedMatchesFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Input",      "String to evaluate",         "string"),
                new CustomFunctionArgumentDescription("Pattern",    "Regular expression pattern", "string"),
                new CustomFunctionArgumentDescription("GroupName",  "Name of capturing group",    "string"),
                new CustomFunctionArgumentDescription("Horizontal", "Whether to return array in horizontal or vertical array. Default - vertical.", "bool")
            };
        }

        public string Name                                             => "REGEX.NAMEDMATCHES";
        public string Description                                      => "Returns array of named group values in regular expression matches";
        public ParameterInfo[] Parameters                              => _Parameters;
        public CustomFunctionArgumentsDescriptionsCollection Arguments => _ArgumentDescriptions;
        public ParameterType ReturnType                                => ParameterType.Array;
        public bool Volatile                                           => false;

        public string GetName(CultureInfo culture) => Name;

        public ParameterValue Evaluate(IList<ParameterValue> parameters, EvaluationContext context)
        {
            if (parameters.Count < 3)
                return ParameterValue.ErrorInvalidValueInFunction;

            foreach (var parameter in parameters)
                if (parameter.IsError)
                    return ParameterValue.ErrorInvalidValueInFunction;

            try
            {
                var input     = parameters[0].ToString();
                var pattern   = parameters[1].ToString();
                var groupName = parameters[2].ToString();

                if (input == null || pattern == null || string.IsNullOrWhiteSpace(groupName))
                    return null;

                bool horizontal = parameters.Count > 3 ? parameters[3].BooleanValue : false;

                var strMatches = new List<string>();

                var matches = Regex.Matches(input, pattern, RegexOptions.ExplicitCapture);
                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        var group = match.Groups[groupName];
                        if (group.Success)
                            strMatches.Add(group.Value);
                    }
                }

                if (strMatches.Count <= 0)
                    return new CellValue[,] { };

                if (!horizontal)
                {
                    var result = new CellValue[strMatches.Count, 1];
                    for (int i = 0; i < strMatches.Count; i++)
                        result[i, 0] = strMatches[i];

                    return result;
                }
                else
                {
                    var result = new CellValue[1, strMatches.Count];
                    for (int i = 0; i < strMatches.Count; i++)
                        result[0, i] = strMatches[i];

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
