using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Spreadsheet.Functions.String
{
    public class QuoteStringFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public QuoteStringFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Value",      "String to be quoted", "string"),
                new CustomFunctionArgumentDescription("Quote",      "Quote character.",    "string")
            };
        }

        public string Name                                             => "QUOTESTRING";
        public string Description                                      => "Quotes string.";
        public ParameterInfo[] Parameters                              => _Parameters;
        public CustomFunctionArgumentsDescriptionsCollection Arguments => _ArgumentDescriptions;
        public ParameterType ReturnType                                => ParameterType.Value;
        public bool Volatile                                           => false;

        public string GetName(CultureInfo culture) => Name;

        public ParameterValue Evaluate(IList<ParameterValue> parameters, EvaluationContext context)
        {
            foreach (var parameter in parameters)
                if (parameter.IsError)
                    return ParameterValue.ErrorInvalidValueInFunction;

            if (parameters.Count < 1)
                return ParameterValue.ErrorInvalidValueInFunction;

            try
            {
                string value = parameters[0]?.ToString();

                var quote = parameters.Count >= 2 ? parameters[1]?.ToString() : "\"";
                if (string.IsNullOrWhiteSpace(quote))
                    quote = "\"";

                var result = Utils.QuoteString(value?.ToString(), quote);
                return result ?? string.Empty;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
