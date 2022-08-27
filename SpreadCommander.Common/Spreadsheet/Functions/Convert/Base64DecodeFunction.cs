using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Spreadsheet.Functions.Convert
{
    public class Base64DecodeFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public Base64DecodeFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Value",      "String to be decoded.", "string"),
                new CustomFunctionArgumentDescription("Encoding",   "Encoding.",             "string")
            };
        }

        public string Name                                             => "BASE64.DECODE";
        public string Description                                      => "Decodes Base64-encoded string.";
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
                var value = parameters[0]?.ToString();
                if (value == null)
                    return null;

                var encoding = Encoding.UTF8;
                bool isHex   = false;
                if (parameters.Count >= 2)
                {
                    var strEncoding = System.Convert.ToString(parameters[1]);
                    if (string.Compare(strEncoding, "HEX", true) == 0)
                        isHex = true;
                    else
                        encoding = Utils.GetEncoding(strEncoding);
                }

                byte[] data = System.Convert.FromBase64String(value.ToString());

                if (data == null)
                    return null;

                string result;
                if (isHex)
                    result = System.Convert.ToHexString(data);
                else
                    result = encoding.GetString(data);
                return result;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
