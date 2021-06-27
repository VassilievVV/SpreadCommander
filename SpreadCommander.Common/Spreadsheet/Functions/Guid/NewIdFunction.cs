using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions.Guid
{
    public class NewIdFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public NewIdFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Format",    "Format specifier - N, D, B, P, X. If not specified - D is used.",         "string"),
            };
        }

        public string Name                                             => "NEWID";
        public string Description                                      => "Returns as new instance of GUID";
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

            try
            {
                var format = parameters.Count > 0 ? parameters[0]?.ToString() : "D";
                if (string.IsNullOrEmpty(format) || !FormatInRange(format, "N", "D", "B", "P", "X"))
                    format = "D";

                var result = System.Guid.NewGuid().ToString(format);
                return result;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }


            static bool FormatInRange(string format, params string[] values)
            {
                for (int i = 0; i < values.Length; i++)
                    if (string.Compare(format, values[i], true) == 0)
                        return true;
                return false;
            }
        }
    }
}
