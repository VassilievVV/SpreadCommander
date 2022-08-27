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
    public class RegexMatchFunction: ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public RegexMatchFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Input",    "String to evaluate",         "string"),
                new CustomFunctionArgumentDescription("Pattern",  "Regular expression pattern", "string"),
                new CustomFunctionArgumentDescription("MatchNum", "Match number, default is 0", "number")
            };
        }

        public string Name                                             => "REGEX.MATCH";
        public string Description                                      => "Searches the specified input string for the first occurrence of the regular expression.";
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
                var input    = parameters[0].ToString();
                var pattern  = parameters[1].ToString();
                var matchNum = System.Convert.ToInt32(parameters.Count > 2 ? parameters[2].NumericValue : 0);

                if (input == null || pattern == null)
                    return string.Empty;

                var match   = RE.Regex.Match(input, pattern, RegexOptions.ExplicitCapture);
                int counter = 0;
                while (counter < matchNum && match.Success)
                {
                    match = match.NextMatch();
                    counter++;
                }

                if (!match.Success)
                    return string.Empty;

                return match.Value ?? string.Empty;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
