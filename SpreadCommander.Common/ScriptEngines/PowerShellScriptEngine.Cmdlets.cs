using SpreadCommander.Common.PowerShell.CmdLets;
using SpreadCommander.Common.PowerShell.CmdLets.Book;
using SpreadCommander.Common.PowerShell.CmdLets.Grid;
using SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet;
using SpreadCommander.Common.PowerShell.CmdLets.Charts;
using SpreadCommander.Common.PowerShell.CmdLets.Test;
using SpreadCommander.Common.PowerShell.CmdLets.Map;
using SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet.Chart;
using SpreadCommander.Common.PowerShell.CmdLets.Heap;
using SpreadCommander.Common.PowerShell.CmdLets.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.PowerShell.CmdLets.ImportExport;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class PowerShellScriptEngine
    {
        public static void InitializeRunspaceConfiguration(InitialSessionState config)
        {
            config.LanguageMode                  = PSLanguageMode.FullLanguage;
            config.UseFullLanguageModeInDebugger = true;

            var cmdlets = new List<SessionStateCmdletEntry>();

            AddCmdletConfigurationEntries(new List<Type>()
            {
                //Book
                typeof(WriteTextCmdlet),
                typeof(WriteHtmlCmdlet),
                typeof(WriteImageCmdlet),
                typeof(WriteContentCmdlet),
                typeof(WriteLatexCmdlet),
                typeof(AddBookCharacterStyleCmdlet),
                typeof(AddBookParagraphStyleCmdlet),
                typeof(SetBookDefaultCharacterPropertiesCmdlet),
                typeof(SetBookDefaultParagraphPropertiesCmdlet),
                typeof(SetBookDefaultPropertiesCmdlet),
                typeof(AddBookPageBreakCmdlet),
                typeof(AddBookSectionCmdlet),
                typeof(SetBookSectionCmdlet),
                typeof(SetBookSectionHeaderCmdlet),
                typeof(SetBookSectionFooterCmdlet),
                typeof(AddBookTocCmdlet),
                typeof(AddBookShapeCmdlet),
                typeof(NewBookCmdlet),
                typeof(MergeBookCmdlet),
                typeof(SaveBookCmdlet),
                typeof(WriteSpreadTableCmdlet),
                typeof(WriteDataTableCmdlet),
                //typeof(OutBookTemplateCmdlet),
                typeof(WriteSyntaxTextCmdlet),
                typeof(WriteErrorMessageCmdlet),
                typeof(WriteMarkdownCmdlet),
                typeof(ClearBookCmdlet),

                //Spreadsheet
                typeof(OutSpreadTableCmdlet),
                typeof(NewSpreadPivotCmdlet),
                typeof(NewSimpleSpreadChartCmdlet),
                typeof(NewSpreadChartCmdlet),
                typeof(NewSpreadsheetCmdlet),
                typeof(SaveSpreadsheetCmdlet),
                typeof(MergeSpreadsheetCmdlet),
                typeof(OutSpreadTemplateCmdlet),
                typeof(GetSpreadTableCmdlet),
                typeof(SaveSpreadChartCmdlet),
                typeof(ClearSpreadsheetCmdlet),

                //Chart
                typeof(NewChartCmdlet),
                typeof(AddChartSeriesCmdlet),
                typeof(AddChartTitleCmdlet),
                typeof(AddChartPaneCmdlet),
                typeof(SetChartLegendCmdlet),
                typeof(AddChartLegendCmdlet),
                typeof(SetChartSeriesLabelCmdlet),
                typeof(SetChartTotalLabelCmdlet),
                typeof(WriteChartCmdlet),
                typeof(AddChartAxisCmdlet),
                typeof(SetChartAxisCmdlet),
                typeof(SetChartDefaultPaneCmdlet),
                typeof(SetChartAxisLabelCmdlet),
                typeof(SetChartAxisTitleCmdlet),
                typeof(AddChartAxisCustomLabelCmdlet),
                typeof(AddChartConstantLineCmdlet),
                typeof(AddChartIndicatorCmdlet),
                typeof(AddChartScaleBreakCmdlet),
                typeof(AddChartSeriesTitleCmdlet),
                typeof(AddChartAnnotationCmdlet),
                typeof(AddChartSeriesColorizerCmdlet),
                typeof(AddChartSegmentColorizerCmdlet),
                typeof(SaveChartCmdlet),
                typeof(SaveChartTemplateCmdlet),

                //Grid
                typeof(OutDataCmdlet),
                typeof(OutDataSetCmdlet),
                typeof(RemoveDataTableCmdlet),
                typeof(ClearDataCmdlet),

                //Map
                typeof(NewMapCmdlet),
                typeof(WriteMapCmdlet),
                typeof(AddMapLayerVectorFileCmdlet),
                typeof(AddMapLayerSqlCmdlet),
                typeof(AddMapLayerWktCmdlet),
                typeof(AddMapLayerImageCmdlet),
                typeof(AddMapLayerVectorDataCmdlet),
                typeof(AddMapLayerVectorItemsCmdlet),
                typeof(AddMapItemCmdlet),
                typeof(AddMapChoroplethColorizerCmdlet),
                typeof(AddMapGraphColorizerCmdlet),
                typeof(AddMapKeyColorizerCmdlet),
                typeof(AddMapLegendCmdlet),
                typeof(AddMapLayerSearchCmdlet),
                typeof(AddMapLayerRouteCmdlet),
                typeof(AddMapOverlayCmdlet),
                typeof(AddMapMiniMapCmdlet),
                typeof(AddMapClustererCmdlet),
                typeof(SaveMapCmdlet),

                //Heap
                typeof(OutFileViewerCmdlet),

                //Script
                typeof(InvokeSCScriptCmdlet),
                typeof(GetDbConnectionCmdlet),
                typeof(SetDbConnectionCmdlet),
                typeof(InvokeSqlScriptCmdlet),
                typeof(InvokeSqlQueryCmdlet),
                typeof(ExportTableToDatabaseCmdlet),
                typeof(GetSCHostCmdlet),
                typeof(ConvertToObjectListCmdlet),
                typeof(ConvertToVectorCmdlet),
                typeof(ConvertToPivotCmdlet),
                typeof(ConvertToUnPivotCmdlet),

                //ImportExport
                typeof(ImportDelimitedText),
                typeof(ImportFixedLengthText),
                typeof(ExportDelimitedText),
                typeof(ExportFixedLengthText),
                typeof(ImportDbf),
                typeof(ExportDbf)

#if DEBUG
                , typeof(TestCmdlet)
#endif
            });

            config.Commands.Add(cmdlets);


            void AddCmdletConfigurationEntries(IEnumerable<Type> types)
            {
                foreach (var type in types)
                    AddCmdletConfigurationEntry(type);
            }

            void AddCmdletConfigurationEntry(Type cmdletType)
            {
                var attrsCmdlet = cmdletType.GetCustomAttributes(typeof(CmdletAttribute), true);
                if (attrsCmdlet == null || attrsCmdlet.Length <= 0)
                    throw new Exception($"Cannot register cmdlet: {cmdletType.Name}");

                var attrCmdlet = ((CmdletAttribute)attrsCmdlet[0]);
                var cmdletName = $"{attrCmdlet.VerbName}-{attrCmdlet.NounName}";

                var cmdletEntry = new SessionStateCmdletEntry(cmdletName, cmdletType, null);
                cmdlets.Add(cmdletEntry);
            }
        }
    }
}
