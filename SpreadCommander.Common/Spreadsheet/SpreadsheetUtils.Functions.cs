using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Functions;
using SpreadCommander.Common.Spreadsheet.Functions;
using SpreadCommander.Common.Spreadsheet.Functions.Hash;
using SpreadCommander.Common.Spreadsheet.Functions.Path;
using SpreadCommander.Common.Spreadsheet.Functions.Regex;
using SpreadCommander.Common.Spreadsheet.Functions.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet
{
    public static partial class SpreadsheetUtils
    {
        public static void RegisterCustomSpreadsheetFunctions()
        {
            using (var workbook = CreateWorkbook())
            {
                RegisterFunctions(workbook, new List<ICustomFunction>()
                {
                    new StringFormatFunction(),

                    new RegexMatchFunction(),
                    new RegexIsMatchFunction(),
                    new RegexNamedMatchFunction(),
                    new RegexMatchesFunction(),
                    new RegexReplaceFunction(),
                    new RegexNamedMatchesFunction(),
                    new RegexSplitFunction(),

                    new PathCombineFunction(),
                    new PathGetDirectoryNameFunction(),
                    new PathGetExtensionFunction(),
                    new PathGetFileNameFunction(),
                    new PathGetFileNameWithoutExtensionFunction(),

                    new HashMD5Function(),
                    new HashSHA1Function(),
                    new HashSHA256Function(),
                    new HashSHA384Function(),
                    new HashSHA512Function()
                });
            }


            static void RegisterFunctions(Workbook workbook, List<ICustomFunction> functions)
            {
                foreach (var function in functions)
                    RegisterFunction(workbook, function);
            }

            static void RegisterFunction(Workbook workbook, ICustomFunction function)
            {
                if (!workbook.Functions.GlobalCustomFunctions.Contains(function.Name))
                    workbook.Functions.GlobalCustomFunctions.Add(function);
            }
        }

        public static void RegisterCustomSpreadsheetFunctionDescriptions(IWorkbook workbook)
        {
            if (workbook.GetService(typeof(ICustomFunctionDescriptionsRegisterService)) is not ICustomFunctionDescriptionsRegisterService service)
                return;

            foreach (var function in workbook.Functions.GlobalCustomFunctions.OfType<ICustomFunctionDescription>())
                RegisterFunctionDescription(function);

            foreach (var function in workbook.Functions.CustomFunctions.OfType<ICustomFunctionDescription>())
                RegisterFunctionDescription(function);


            void RegisterFunctionDescription(ICustomFunctionDescription functionDescription)
            {
                service.UnregisterFunction(functionDescription.Name);
                service.RegisterFunctionDescriptions(functionDescription.Name, functionDescription.Description, functionDescription.Arguments);
            }
        }
    }
}
