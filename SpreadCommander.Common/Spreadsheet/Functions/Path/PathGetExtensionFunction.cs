using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;

namespace SpreadCommander.Common.Spreadsheet.Functions.Path
{
    public class PathGetExtensionFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public PathGetExtensionFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Path",      "Path to modify", "string")
            };
        }

        public string Name                                             => "PATH.GETEXTENSION";
        public string Description                                      => "Extension of the path string.";
        public ParameterInfo[] Parameters                              => _Parameters;
        public CustomFunctionArgumentsDescriptionsCollection Arguments => _ArgumentDescriptions;
        public ParameterType ReturnType                                => ParameterType.Value;
        public bool Volatile                                           => false;

        public string GetName(CultureInfo culture) => Name;

        public ParameterValue Evaluate(IList<ParameterValue> parameters, EvaluationContext context)
        {
            if (parameters.Count < 1)
                return ParameterValue.ErrorInvalidValueInFunction;

            foreach (var parameter in parameters)
                if (parameter.IsError)
                    return ParameterValue.ErrorInvalidValueInFunction;

            try
            {
                var path = parameters[0].ToString();

                var result = IO.Path.GetExtension(path);
                return result ?? string.Empty;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
