using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RE = System.Text.RegularExpressions;

namespace SpreadCommander.Common.Spreadsheet.Functions.Regex
{
    public class RegexReplaceFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public RegexReplaceFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Input",       "String to evaluate",         "string"),
                new CustomFunctionArgumentDescription("Pattern",     "Regular expression pattern", "string"),
                new CustomFunctionArgumentDescription("Replacement", "Replacement string",         "string")
            };
        }

        public string Name                                             => "REGEX.REPLACE";
        public string Description                                      => "Replaces all strings that match a specified regular expression with a specified replacement string.";
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
                var input       = parameters[0].ToString();
                var pattern     = parameters[1].ToString();
                var replacement = parameters[2].ToString() ?? string.Empty;

                if (input == null || pattern == null)
                    return null;

                var result = RE.Regex.Replace(input, pattern, replacement);
                return result;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
