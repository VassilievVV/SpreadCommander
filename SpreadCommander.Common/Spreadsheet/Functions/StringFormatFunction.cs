using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions
{
    public class StringFormatFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public StringFormatFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.OptionalUnlimited)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Part1",      "Format parts", "string")
            };
        }

        public string Name                                             => "STRING.FORMAT";
        public string Description                                      => "Formats a string using .Net pattern, such as {0}.";
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
                var parts = new List<object>();

                string format = parameters[0].ToString();
                
                for (int i = 1; i < parameters.Count; i++)
                {
                    var parameter = parameters[i];
                    
                    object value = null;
                    if (parameter.IsBoolean)
                        value = parameter.BooleanValue;
                    else if (parameter.IsNumeric)
                        value = parameter.NumericValue;
                    else if (parameter.IsText)
                        value = parameter.TextValue;
                    
                    parts.Add(value);
                }

                var result = string.Format(format, parts.ToArray());
                return result;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
