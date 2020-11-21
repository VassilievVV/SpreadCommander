using DevExpress.Data.Filtering;
using DevExpress.LookAndFeel;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Extensions;
using System.Globalization;
using DevExpress.XtraEditors;

namespace SpreadCommander.Common.Spreadsheet
{
    public static partial class SpreadsheetUtils
    {
        public static void ApplyGridFormatCondition(Worksheet worksheet, GridFormatCondition formatCondition, CellRange formatRange,
            DevExpress.Export.Xl.CriteriaOperatorToXlExpressionConverter operatorConverter = null)
        {
            var conditionalFormattings = worksheet.ConditionalFormattings;

            var formatRule = Utils.NonNullString(formatCondition.FormatRule).ToLower();
            switch (formatRule)
            {
                case "expression":
                    var expression = formatCondition.Expression;
                    if (!string.IsNullOrWhiteSpace(expression))
                    {
                        if (operatorConverter != null)	//If there is no operatorConverter - there is no Table and so expression is not allowed.
                        {
                            expression = AdjustFormatExpression(operatorConverter, expression);
                            var formatExpression = conditionalFormattings.AddFormulaExpressionConditionalFormatting(
                                formatRange, expression);
                            ApplyFormatting(formatExpression);
                        }
                    }
                    break;
                case "databar":
                    var formatDataBar = conditionalFormattings.AddDataBarConditionalFormatting(formatRange,
                        conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax),
                        conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax),
                        ColorExtensions.FromHtmlColor(formatCondition.BackColor, Color.Green));

                    var borderColor = ColorExtensions.FromHtmlColor(formatCondition.BorderColor, Color.Empty);
                    if (borderColor != Color.Empty)
                        formatDataBar.BorderColor = borderColor;

                    formatDataBar.GradientFill = true;

                    ApplyOptionalFormatting();
                    break;
                case "iconset":
                    if (!Enum.TryParse(formatCondition.IconSet, out IconSetType iconSetType))
                    {
                        iconSetType = IconSetType.Arrows5;
                        formatCondition.IconSet = "Arrows5";
                    }

                    if (!string.IsNullOrWhiteSpace(formatCondition.IconSet) &&
                        int.TryParse(new string(formatCondition.IconSet[^1], 1), out int stepCount))
                    {
                        var listSteps = new List<ConditionalFormattingIconSetValue>();
                        for (int i = 0; i < stepCount; i++)
                        {
                            var step = worksheet.ConditionalFormattings.CreateIconSetValue(ConditionalFormattingValueType.Percent,
                                Convert.ToInt32(Convert.ToDouble(i) / Convert.ToDouble(stepCount) * 100.0).ToString(CultureInfo.InvariantCulture),
                                ConditionalFormattingValueOperator.GreaterOrEqual);
                            listSteps.Add(step);
                        }

                        var formatIconSet = conditionalFormattings.AddIconSetConditionalFormatting(formatRange, iconSetType, listSteps.ToArray());
                        formatIconSet.ShowValue = true;
                    }

                    ApplyOptionalFormatting();
                    break;
                case "colorscale":
                    string colorScale = formatCondition.ColorScale;
                    if (string.IsNullOrWhiteSpace(colorScale))
                        colorScale = "White,Red";

                    if (!string.IsNullOrWhiteSpace(colorScale))
                    {
                        var scales = colorScale.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (scales != null && scales.Length == 2)
                        {
                            conditionalFormattings.AddColorScale2ConditionalFormatting(formatRange,
                                conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax), ColorExtensions.FromHtmlColor(scales[0], Color.White),
                                conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax), ColorExtensions.FromHtmlColor(scales[1], Color.Black));

                        }
                        else if (scales != null && scales.Length >= 3) //Use first 3 colors if more are specified
                        {
                            conditionalFormattings.AddColorScale3ConditionalFormatting(formatRange,
                                conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax), ColorExtensions.FromHtmlColor(scales[0], Color.White),
                                conditionalFormattings.CreateValue(ConditionalFormattingValueType.Percent, "50"), ColorExtensions.FromHtmlColor(scales[1], Color.Gray),
                                conditionalFormattings.CreateValue(ConditionalFormattingValueType.MinMax), ColorExtensions.FromHtmlColor(scales[2], Color.Black));
                        }
                    }

                    ApplyOptionalFormatting();
                    break;
                case "aboveaverage":
                    var formatAboveAverage = conditionalFormattings.AddAverageConditionalFormatting(formatRange, ConditionalFormattingAverageCondition.Above);
                    ApplyFormatting(formatAboveAverage);
                    break;
                case "belowaverage":
                    var formatBelowAverage = conditionalFormattings.AddAverageConditionalFormatting(formatRange, ConditionalFormattingAverageCondition.Below);
                    ApplyFormatting(formatBelowAverage);
                    break;
                case "aboveorequalaverage":
                    var formatAboveOrEqualAverage = conditionalFormattings.AddAverageConditionalFormatting(formatRange, ConditionalFormattingAverageCondition.AboveOrEqual);
                    ApplyFormatting(formatAboveOrEqualAverage);
                    break;
                case "beloworequalaverage":
                    var formatBelowOrEqualAverage = conditionalFormattings.AddAverageConditionalFormatting(formatRange, ConditionalFormattingAverageCondition.BelowOrEqual);
                    ApplyFormatting(formatBelowOrEqualAverage);
                    break;
                case "unique":
                    var formatUnique = conditionalFormattings.AddSpecialConditionalFormatting(formatRange, ConditionalFormattingSpecialCondition.ContainUniqueValue);
                    ApplyFormatting(formatUnique);
                    break;
                case "duplicate":
                    var formatDuplicate = conditionalFormattings.AddSpecialConditionalFormatting(formatRange, ConditionalFormattingSpecialCondition.ContainDuplicateValue);
                    ApplyFormatting(formatDuplicate);
                    break;
                case "top":
                    var rankTop = formatCondition.Rank;
                    var isPercent = !string.IsNullOrWhiteSpace(rankTop) && rankTop.EndsWith("%");
                    var strRankTop = isPercent ? rankTop[0..^1].TrimEnd() : rankTop;
                    var rankTopValue = Utils.ChangeType<decimal>(strRankTop, 0);

                    var formatTop = conditionalFormattings.AddRankConditionalFormatting(formatRange,
                        isPercent ? ConditionalFormattingRankCondition.TopByPercent : ConditionalFormattingRankCondition.TopByRank,
                        Convert.ToInt32(rankTopValue));
                    ApplyFormatting(formatTop);
                    break;
                case "bottom":
                    var rankBottom = formatCondition.Rank;
                    var isPercent2 = !string.IsNullOrWhiteSpace(rankBottom) && rankBottom.EndsWith("%");
                    var strRankBottom = isPercent2 ? rankBottom[0..^1].TrimEnd() : rankBottom;
                    var rankBottomValue = Utils.ChangeType<decimal>(strRankBottom, 0);

                    var formatBottom = conditionalFormattings.AddRankConditionalFormatting(formatRange,
                        isPercent2 ? ConditionalFormattingRankCondition.BottomByPercent : ConditionalFormattingRankCondition.BottomByRank,
                        Convert.ToInt32(rankBottomValue));
                    ApplyFormatting(formatBottom);
                    break;
                case "dateoccuring":
                    if (Enum.TryParse<FilterDateType>(formatCondition.DateOccuring, out FilterDateType filterDateType))
                    {
                        ConditionalFormattingTimePeriod timePeriod = ConditionalFormattingTimePeriod.Today;
                        bool correctPeriod = true;
                        switch (filterDateType)
                        {
                            case FilterDateType.NextWeek:
                                timePeriod = ConditionalFormattingTimePeriod.NextWeek;
                                break;
                            case FilterDateType.Tomorrow:
                                timePeriod = ConditionalFormattingTimePeriod.Tomorrow;
                                break;
                            case FilterDateType.Today:
                                timePeriod = ConditionalFormattingTimePeriod.Today;
                                break;
                            case FilterDateType.Yesterday:
                                timePeriod = ConditionalFormattingTimePeriod.Yesterday;
                                break;
                            case FilterDateType.EarlierThisWeek:
                                break;
                            case FilterDateType.LastWeek:
                                timePeriod = ConditionalFormattingTimePeriod.LastWeek;
                                break;
                            case FilterDateType.ThisWeek:
                                timePeriod = ConditionalFormattingTimePeriod.ThisWeek;
                                break;
                            case FilterDateType.ThisMonth:
                                timePeriod = ConditionalFormattingTimePeriod.ThisMonth;
                                break;
                            case FilterDateType.MonthAfter1:
                                timePeriod = ConditionalFormattingTimePeriod.NextMonth;
                                break;
                            case FilterDateType.MonthAgo1:
                                timePeriod = ConditionalFormattingTimePeriod.LastMonth;
                                break;
                            default:
                                correctPeriod = false;
                                break;
                        }

                        if (correctPeriod)
                        {
                            var formatTimePeriod = conditionalFormattings.AddTimePeriodConditionalFormatting(formatRange, timePeriod);
                            ApplyFormatting(formatTimePeriod);
                        }
                    }
                    break;
                case "rule":
                    if (Enum.TryParse<DevExpress.XtraEditors.FormatCondition>(formatCondition.Condition,
                        out DevExpress.XtraEditors.FormatCondition formatExpressionCondition))
                    {
                        ConditionalFormattingExpressionCondition expressionCondition = ConditionalFormattingExpressionCondition.EqualTo;
                        var correctCondition = true;
                        switch (formatExpressionCondition)
                        {
                            case FormatCondition.Equal:
                                expressionCondition = ConditionalFormattingExpressionCondition.EqualTo;
                                break;
                            case FormatCondition.NotEqual:
                                expressionCondition = ConditionalFormattingExpressionCondition.UnequalTo;
                                break;
                            case FormatCondition.Less:
                                expressionCondition = ConditionalFormattingExpressionCondition.LessThan;
                                break;
                            case FormatCondition.Greater:
                                expressionCondition = ConditionalFormattingExpressionCondition.GreaterThan;
                                break;
                            case FormatCondition.GreaterOrEqual:
                                expressionCondition = ConditionalFormattingExpressionCondition.GreaterThanOrEqual;
                                break;
                            case FormatCondition.LessOrEqual:
                                expressionCondition = ConditionalFormattingExpressionCondition.LessThanOrEqual;
                                break;
                            default:
                                correctCondition = false;
                                break;
                        }

                        if (correctCondition)
                        {
                            var formatExpression = conditionalFormattings.AddExpressionConditionalFormatting(formatRange, expressionCondition, formatCondition.Value1);
                            ApplyFormatting(formatExpression);
                        }
                    }
                    break;
                case "":
                    ApplyOptionalFormatting();
                    break;
            }


            static string AdjustFormatExpression(DevExpress.Export.Xl.CriteriaOperatorToXlExpressionConverter converter, string expression)
            {
                var op           = CriteriaOperator.Parse(expression);
                var xlExpression = converter.Execute(op);
                var result       = xlExpression.ToString();

                return result;
            }

            void ApplyOptionalFormatting()
            {
                if (!string.IsNullOrWhiteSpace(formatCondition.AppearanceName) ||
                    !string.IsNullOrWhiteSpace(formatCondition.BackColor) ||
                    !string.IsNullOrWhiteSpace(formatCondition.BorderColor) ||
                    !string.IsNullOrWhiteSpace(formatCondition.Font) ||
                    !string.IsNullOrWhiteSpace(formatCondition.ForeColor))
                {
                    if (operatorConverter != null)  //If there is no operatorConverter - there is no Table and so expression is not allowed.
                    {
                        var expr = "true";
                        var formatExpression = conditionalFormattings.AddFormulaExpressionConditionalFormatting(formatRange, expr);
                        ApplyFormatting(formatExpression);
                    }
                }
            }

            void ApplyFormatting(ConditionalFormatting rule)
            {
                var formatting = ((ISupportsFormatting)rule).Formatting;
                if (formatting == null)
                    return;

                if (!string.IsNullOrWhiteSpace(formatCondition.AppearanceName))
                {
                    var appearance = FormatPredefinedAppearances.Default.Find(UserLookAndFeel.Default, formatCondition.AppearanceName)?.Appearance;
                    if (appearance != null)
                    {
                        if (appearance.BackColor != Color.Empty)
                            formatting.Fill.BackgroundColor = appearance.BackColor;
                        if (appearance.Font != null)
                        {
                            formatting.Font.Name = appearance.Font.Name;
                            formatting.Font.Size = appearance.Font.Size;
                            formatting.Font.Bold = appearance.Font.Bold;
                            formatting.Font.Italic = appearance.Font.Italic;
                            formatting.Font.UnderlineType = appearance.Font.Underline ? UnderlineType.Single : UnderlineType.None;
                            formatting.Font.Strikethrough = appearance.Font.Strikeout;
                        }
                        if (appearance.ForeColor != Color.Empty)
                            formatting.Font.Color = appearance.ForeColor;
                        if (appearance.BorderColor != Color.Empty)
                            formatting.Borders.SetAllBorders(appearance.BorderColor, BorderLineStyle.Thin);
                    }
                    return;
                }

                Color backColor = ColorExtensions.FromHtmlColor(formatCondition.BackColor, Color.Empty);
                if (backColor != Color.Empty)
                    formatting.Fill.BackgroundColor = backColor;

                Color foreColor = ColorExtensions.FromHtmlColor(formatCondition.ForeColor, Color.Empty);
                if (foreColor != Color.Empty)
                    formatting.Font.Color = foreColor;

                Color borderColor = ColorExtensions.FromHtmlColor(formatCondition.BorderColor, Color.Empty);
                if (borderColor != Color.Empty)
                    formatting.Borders.SetAllBorders(borderColor, BorderLineStyle.Thin);

                string strFont = formatCondition.Font;
                if (!string.IsNullOrWhiteSpace(strFont))
                {
                    try
                    {
                        Utils.StringToSpreadsheetFont(strFont, formatting.Font);
                    }
                    catch (Exception)
                    {
                        //Do nothing
                    }
                }
            }
        }

        public static void FinishTableFormatting(Table table, bool applyStyle = true)
        {
            table.Range.AutoFitColumns();
            foreach (TableColumn column in table.Columns)
                if (column.Range.ColumnWidthInCharacters > 50)
                    column.Range.ColumnWidthInCharacters = 50;

            if (applyStyle)
                table.Style = table.Range.Worksheet.Workbook.TableStyles[BuiltInTableStyleId.TableStyleMedium2];
                    //table.Range.Worksheet.Workbook.TableStyles.DefaultStyle;
        }
    }
}
