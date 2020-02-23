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
    public class RegexNamedMatchFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public RegexNamedMatchFunction()
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
                new CustomFunctionArgumentDescription("Input",     "String to evaluate",         "string"),
                new CustomFunctionArgumentDescription("Pattern",   "Regular expression pattern", "string"),
                new CustomFunctionArgumentDescription("GroupName", "Name of capturing group",    "string"),
                new CustomFunctionArgumentDescription("MatchNum",  "Match number, default is 0", "number")
            };
        }

        public string Name                                             => "REGEX.NAMEDMATCH";
        public string Description                                      => "Regular expression match, returns value of named group";
        public ParameterInfo[] Parameters                              => _Parameters;
        public CustomFunctionArgumentsDescriptionsCollection Arguments => _ArgumentDescriptions;
        public ParameterType ReturnType                                => ParameterType.Value;
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
                var matchNum  = Convert.ToInt32(parameters.Count > 3 ? parameters[3].NumericValue : 0);

                if (input == null || pattern == null || string.IsNullOrWhiteSpace(groupName))
                    return null;

                var match = Regex.Match(input, pattern, RegexOptions.ExplicitCapture);
                int counter = 0;
                while (counter < matchNum && match.Success)
                {
                    match = match.NextMatch();
                    counter++;
                }

                if (!match.Success)
                    return null;

                var group = match.Groups[groupName];
                if (!group.Success)
                    return null;

                return group.Value;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
