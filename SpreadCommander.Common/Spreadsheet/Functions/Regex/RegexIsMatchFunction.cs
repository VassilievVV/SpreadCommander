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
    public class RegexIsMatchFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public RegexIsMatchFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Input",    "String to evaluate",         "string"),
                new CustomFunctionArgumentDescription("Pattern",  "Regular expression pattern", "string")
            };
        }

        public string Name                                             => "REGEX.ISMATCH";
        public string Description                                      => "Whether input string matches regex pattern or no";
        public ParameterInfo[] Parameters                              => _Parameters;
        public CustomFunctionArgumentsDescriptionsCollection Arguments => _ArgumentDescriptions;
        public ParameterType ReturnType                                => ParameterType.Value;
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
                var input    = parameters[0]?.ToString();
                var pattern  = parameters[1]?.ToString();

                if (input == null || pattern == null)
                    return false;

                var result = RE.Regex.IsMatch(input, pattern, RegexOptions.ExplicitCapture);
                return result;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
