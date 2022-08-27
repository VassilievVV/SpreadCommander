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
    public class Base64EncodeFunction : ICustomFunction, ICustomFunctionDescription
    {
        private readonly ParameterInfo[] _Parameters;
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;

        public Base64EncodeFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Value",      "String to be encoded.", "string"),
                new CustomFunctionArgumentDescription("Encoding",   "Encoding.",             "string")
            };
        }

        public string Name                                             => "BASE64.ENCODE";
        public string Description                                      => "Encodes string to Base64 string.";
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
                    {
                        isHex = true;

                        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            value = value[2..];
                    }
                    else
                        encoding = Utils.GetEncoding(strEncoding);
                }

                byte[] data;
                if (isHex)
                    data = System.Convert.FromHexString(value);
                else 
                    data = Utils.GetBytes(value, encoding);

                if (data == null)
                    return null;

                var result = System.Convert.ToBase64String(data, Base64FormattingOptions.None);
                return result;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
