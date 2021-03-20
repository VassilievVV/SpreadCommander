using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions.Hash
{
    public class BaseHashFunction : IDisposable, ICustomFunction, ICustomFunctionDescription
    {
        private readonly CustomFunctionArgumentsDescriptionsCollection _ArgumentDescriptions;
        private readonly ParameterInfo[] _Parameters;

        protected HashAlgorithm _Hash;

        public BaseHashFunction()
        {
            _Parameters = new ParameterInfo[]
            {
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Required),
                new ParameterInfo(ParameterType.Value, ParameterAttributes.Optional)
            };

            _ArgumentDescriptions = new CustomFunctionArgumentsDescriptionsCollection
            {
                new CustomFunctionArgumentDescription("Value",    "String to calculate hash for.", "string"),
                new CustomFunctionArgumentDescription("Encoding", "Code page or encoding name. Default is UTF-8.", "string")
            };
        }

        public virtual void Dispose()
        {
            if (_Hash != null)
            {
                _Hash.Dispose();
                _Hash = null;
            }
        }

        public virtual string Name                                     => null;
        public virtual string Description                              => "Calculates hash of the string.";
        public ParameterInfo[] Parameters                              => _Parameters;
        public CustomFunctionArgumentsDescriptionsCollection Arguments => _ArgumentDescriptions;
        public ParameterType ReturnType                                => ParameterType.Value;
        public bool Volatile                                           => false;

        public string GetName(CultureInfo culture) => Name;

        protected virtual byte[] ComputeHash(byte[] data)
        {
            lock (_Hash)
            {
                var result = _Hash.ComputeHash(data);
                return result;
            }
        }

        public ParameterValue Evaluate(IList<ParameterValue> parameters, EvaluationContext context)
        {
            foreach (var parameter in parameters)
                if (parameter.IsError)
                    return ParameterValue.ErrorInvalidValueInFunction;

            if (parameters.Count < 1)
                return ParameterValue.ErrorInvalidValueInFunction;

            try
            {
                string value = parameters[0].ToString();
                string strEncoding = parameters.Count >= 2 ? parameters[1].ToString() : null;
                Encoding encoding = Encoding.UTF8;
                switch (strEncoding?.ToLower())
                {
                    case "unicode":
                        encoding = Encoding.Unicode;
                        break;
                    case "utf8":
                    case "utf-8":
                        encoding = Encoding.UTF8;
                        break;
                    case "utf32":
                    case "utf-32":
                        encoding = Encoding.UTF32;
                        break;
                    case "ascii":
                        encoding = Encoding.ASCII;
                        break;
                    default:
                        if (!string.IsNullOrWhiteSpace(strEncoding))
                        {
                            if (int.TryParse(strEncoding, out int codePage))
                                encoding = Encoding.GetEncoding(codePage);
                            else
                                encoding = Encoding.GetEncoding(strEncoding);
                        }
                        break;
                }

                var data = encoding.GetBytes(value);
                if (data == null)
                    return string.Empty;

                var hash   = ComputeHash(data);
                var result = Convert.ToHexString(hash);
                return result;
            }
            catch (Exception)
            {
                return ParameterValue.ErrorInvalidValueInFunction;
            }
        }
    }
}
