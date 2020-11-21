using DevExpress.Compression;
using DevExpress.Data;
using DevExpress.Spreadsheet;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System.Drawing.Drawing2D;

namespace SpreadCommander.Documents.Viewers
{
    public partial class GridViewer
    {
        public void LoadGridData(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            GridData gridData = null;
            using (var zip = ZipArchive.Read(fileName))
            {
                var itemGrid = zip["GridControl.xml"];
                if (itemGrid != null)
                {
                    using var stream = itemGrid.Open();
                    using var memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    gridData = Utils.DeserializeObject<GridData>(memStream);
                }
            }

            LoadGridData(gridData);
        }

        public void LoadGridData(GridData data) =>
            LoadGridData(viewTable, data);

        public static void LoadGridData(GridView viewTable, GridData data)
        {
            using (new UsingProcessor(() => viewTable.BeginUpdate(), () => viewTable.EndUpdate()))
            {
                viewTable.GridControl.ForceInitialize();

                if (data.Parameters != null)
                {
                    var gridProperties = new GridProperties((GridView)viewTable);
                    gridProperties.ApplyParameters(data.Parameters);
                }

                if (data.ColumnOrder != null && data.ColumnOrder.Count > 0)
                {
                    var visibleColumns = new List<GridColumn>();
                    bool hasSummaries = false;

                    int i = 0;
                    foreach (var field in data.ColumnOrder)
                    {
                        if (string.IsNullOrWhiteSpace(field.ColumnName))
                            continue;

                        var column = viewTable.Columns.ColumnByFieldName(field.ColumnName);
                        if (column != null)
                        {
                            column.Visible      = true;
                            column.VisibleIndex = i++;
                            visibleColumns.Add(column);

                            column.Summary.Clear();
                            if (!string.IsNullOrWhiteSpace(field.Summary))
                            {
                                var summaries = Utils.SplitString(field.Summary, ',');
                                foreach (var summary in summaries)
                                {
                                    if (Enum.TryParse<SummaryItemType>(summary, out SummaryItemType summaryType))
                                    {
                                        column.Summary.Add(summaryType);
                                        hasSummaries = true;
                                    }
                                }
                            }
                        }
                    }
                    if (hasSummaries)
                        viewTable.OptionsView.ShowFooter = true;
                }

                using (new UsingProcessor(() => viewTable.BeginSort(), () => viewTable.EndSort()))
                {
                    viewTable.ClearGrouping();
                    viewTable.ClearSorting();

                    var sortInfos = new List<GridColumnSortInfo>();
                    int groupColumnCount = 0;

                    if (data.GroupBy != null && data.GroupBy.Count > 0)
                    {
                        foreach (var group in data.GroupBy)
                        {
                            if (string.IsNullOrWhiteSpace(group.ColumnName))
                                continue;

                            var sortAscending = true;
                            var colName       = group.ColumnName;
                            if (string.Compare(group.SortOrder, "desc", true) == 0)
                                sortAscending = false;

                            var column = viewTable.Columns.ColumnByFieldName(colName);
                            if (column != null)
                            {
                                var colSortInfo = new GridColumnSortInfo(column, sortAscending ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending);
                                sortInfos.Add(colSortInfo);
                                groupColumnCount++;
                            }
                        }
                    }

                    if (data.OrderBy != null && data.OrderBy.Count > 0)
                    {
                        foreach (var orderBy in data.OrderBy)
                        {
                            if (string.IsNullOrWhiteSpace(orderBy.ColumnName))
                                continue;

                            var sortAscending = true;
                            var colName       = orderBy.ColumnName;
                            if (string.Compare(orderBy.SortOrder, "desc", true) == 0)
                                sortAscending = false;

                            var column = viewTable.Columns.ColumnByFieldName(colName);
                            if (column != null)
                            {
                                var colSortInfo = new GridColumnSortInfo(column, sortAscending ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending);
                                sortInfos.Add(colSortInfo);
                            }
                        }
                    }

                    viewTable.SortInfo.ClearAndAddRange(sortInfos.ToArray(), groupColumnCount);
                }

                if (data.FormatConditions != null && data.FormatConditions.Count > 0)
                {
                    viewTable.FormatConditions.Clear();

                    foreach (var formatCondition in data.FormatConditions)
                    {
                        GridColumn column = null, targetColumn = null;
                        if (!string.IsNullOrWhiteSpace(formatCondition.ColumnName))
                            column = viewTable.Columns.ColumnByFieldName(formatCondition.ColumnName);
                        if (!string.IsNullOrWhiteSpace(formatCondition.TargetColumn))
                            targetColumn = viewTable.Columns.ColumnByFieldName(formatCondition.TargetColumn);


                        var formatRule = Utils.NonNullString(formatCondition.FormatRule).ToLower();
                        switch (formatRule)
                        {
                            case "expression":
                                var expression = formatCondition.Expression;
                                if (!string.IsNullOrWhiteSpace(expression))
                                {
                                    var formatExpression = new FormatConditionRuleExpression()
                                    {
                                        Expression = expression
                                    };

                                    var ruleExpression = new GridFormatRule
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = (column == null) || formatCondition.ApplyToRow,
                                        Rule          = formatExpression
                                    };
                                    ApplyFormatting(formatExpression, formatCondition);

                                    viewTable.FormatRules.Add(ruleExpression);
                                }
                                break;
                            case "databar":
                                if (column != null)
                                {
                                    var ruleDataBar = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow                                        
                                    };

                                    var formatDataBar = new FormatConditionRuleDataBar();

                                    if (string.IsNullOrWhiteSpace(formatCondition.BackColor))
                                        formatCondition.BackColor = formatCondition.DataBar;

                                    if (!string.IsNullOrWhiteSpace(formatCondition.AppearanceName))
                                        formatDataBar.PredefinedName = formatCondition.AppearanceName;
                                    else
                                        ApplyAppearance(formatDataBar.Appearance, formatCondition);

                                    ruleDataBar.Rule = formatDataBar;

                                    viewTable.FormatRules.Add(ruleDataBar);
                                }
                                break;
                            case "iconset":
                                if (column != null)
                                {
                                    if (!Enum.TryParse(formatCondition.IconSet, out IconSetType iconSetType))
                                        iconSetType = IconSetType.Arrows5;

                                    var ruleIconSet = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatIconSet = new FormatConditionRuleIconSet()
                                    {
                                        IconSet = GetPredefinedIconSet(iconSetType)
                                    };

                                    ruleIconSet.Rule = formatIconSet;

                                    viewTable.FormatRules.Add(ruleIconSet);
                                }
                                break;
                            case "colorscale":
                                if (column != null)
                                {
                                    string colorScale = formatCondition.ColorScale;
                                    if (string.IsNullOrWhiteSpace(colorScale))
                                        colorScale = "White,Red";

                                    if (!string.IsNullOrWhiteSpace(colorScale))
                                    {
                                        var scales = colorScale.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (scales != null && scales.Length == 2)
                                        {
                                            var ruleColorScale2 = new GridFormatRule()
                                            {
                                                Column        = column,
                                                ColumnApplyTo = targetColumn,
                                                ApplyToRow    = formatCondition.ApplyToRow
                                            };

                                            var formatColorScale2 = new FormatConditionRule2ColorScale
                                            {
                                                MinimumColor = ColorExtensions.FromHtmlColor(scales[0], Color.White),
                                                MaximumColor = ColorExtensions.FromHtmlColor(scales[1], Color.Black)
                                            };

                                            ruleColorScale2.Rule = formatColorScale2;

                                            viewTable.FormatRules.Add(ruleColorScale2);
                                        }
                                        else if (scales != null && scales.Length >= 3) //Use first 3 colors if more are specified
                                        {
                                            var ruleColorScale3 = new GridFormatRule()
                                            {
                                                Column        = column,
                                                ColumnApplyTo = targetColumn,
                                                ApplyToRow    = formatCondition.ApplyToRow
                                            };

                                            var formatColorScale3 = new FormatConditionRule3ColorScale
                                            {
                                                MinimumColor = ColorExtensions.FromHtmlColor(scales[0], Color.White),
                                                MiddleColor  = ColorExtensions.FromHtmlColor(scales[1], Color.Gray),
                                                MaximumColor = ColorExtensions.FromHtmlColor(scales[2], Color.Black)
                                            };

                                            ruleColorScale3.Rule = formatColorScale3;

                                            viewTable.FormatRules.Add(ruleColorScale3);
                                        }
                                    }
                                }
                                break;
                            case "aboveaverage":
                                if (column != null)
                                {
                                    var ruleAboveAverage = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatAboveAverage = new FormatConditionRuleAboveBelowAverage()
                                    {
                                        AverageType = FormatConditionAboveBelowType.Above
                                    };
                                    ApplyFormatting(formatAboveAverage, formatCondition);

                                    ruleAboveAverage.Rule = formatAboveAverage;

                                    viewTable.FormatRules.Add(ruleAboveAverage);
                                }
                                break;
                            case "belowaverage":
                                if (column != null)
                                {
                                    var ruleBelowAverage = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatBelowAverage = new FormatConditionRuleAboveBelowAverage()
                                    {
                                        AverageType = FormatConditionAboveBelowType.Below
                                    };
                                    ApplyFormatting(formatBelowAverage, formatCondition);

                                    ruleBelowAverage.Rule = formatBelowAverage;

                                    viewTable.FormatRules.Add(ruleBelowAverage);
                                }
                                break;
                            case "aboveorequalaverage":
                                if (column != null)
                                {
                                    var ruleAboveAverage = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatAboveAverage = new FormatConditionRuleAboveBelowAverage()
                                    {
                                        AverageType = FormatConditionAboveBelowType.EqualOrAbove
                                    };
                                    ApplyFormatting(formatAboveAverage, formatCondition);

                                    ruleAboveAverage.Rule = formatAboveAverage;

                                    viewTable.FormatRules.Add(ruleAboveAverage);
                                }
                                break;
                            case "beloworequalaverage":
                                if (column != null)
                                {
                                    var ruleBelowAverage = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatBelowAverage = new FormatConditionRuleAboveBelowAverage()
                                    {
                                        AverageType = FormatConditionAboveBelowType.EqualOrBelow
                                    };
                                    ApplyFormatting(formatBelowAverage, formatCondition);

                                    ruleBelowAverage.Rule = formatBelowAverage;

                                    viewTable.FormatRules.Add(ruleBelowAverage);
                                }
                                break;
                            case "unique":
                                if (column != null)
                                {
                                    var ruleUnique = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatUnique = new FormatConditionRuleUniqueDuplicate()
                                    {
                                        FormatType = FormatConditionUniqueDuplicateType.Unique
                                    };
                                    ApplyFormatting(formatUnique, formatCondition);

                                    ruleUnique.Rule = formatUnique;

                                    viewTable.FormatRules.Add(ruleUnique);
                                }
                                break;
                            case "duplicate":
                                if (column != null)
                                {
                                    var ruleDuplicate = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatDuplicate = new FormatConditionRuleUniqueDuplicate()
                                    {
                                        FormatType = FormatConditionUniqueDuplicateType.Unique
                                    };
                                    ApplyFormatting(formatDuplicate, formatCondition);

                                    ruleDuplicate.Rule = formatDuplicate;

                                    viewTable.FormatRules.Add(ruleDuplicate);
                                }
                                break;
                            case "top":
                                if (column != null)
                                {
                                    var rankTop      = formatCondition.Rank;
                                    var isPercent    = !string.IsNullOrWhiteSpace(rankTop) && rankTop.EndsWith("%");
                                    var strRankTop   = isPercent ? rankTop[0..^1].TrimEnd() : rankTop;
                                    var rankTopValue = Utils.ChangeType<decimal>(strRankTop, 0);

                                    var ruleTop = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatTop = new FormatConditionRuleTopBottom()
                                    {
                                        TopBottom = FormatConditionTopBottomType.Top,
                                        RankType  = isPercent ? FormatConditionValueType.Percent : FormatConditionValueType.Number,
                                        Rank      = rankTopValue
                                    };
                                    ApplyFormatting(formatTop, formatCondition);

                                    ruleTop.Rule = formatTop;

                                    viewTable.FormatRules.Add(ruleTop);
                                }
                                break;
                            case "bottom":
                                if (column != null)
                                {
                                    var rankBottom      = formatCondition.Rank;
                                    var isPercent2      = !string.IsNullOrWhiteSpace(rankBottom) && rankBottom.EndsWith("%");
                                    var strRankBottom   = isPercent2 ? rankBottom[0..^1].TrimEnd() : rankBottom;
                                    var rankBottomValue = Utils.ChangeType<decimal>(strRankBottom, 0);

                                    var ruleBottom = new GridFormatRule()
                                    {
                                        Column        = column,
                                        ColumnApplyTo = targetColumn,
                                        ApplyToRow    = formatCondition.ApplyToRow
                                    };

                                    var formatBottom = new FormatConditionRuleTopBottom()
                                    {
                                        TopBottom = FormatConditionTopBottomType.Bottom,
                                        RankType  = isPercent2 ? FormatConditionValueType.Percent : FormatConditionValueType.Number,
                                        Rank      = rankBottomValue
                                    };
                                    ApplyFormatting(formatBottom, formatCondition);

                                    ruleBottom.Rule = formatBottom;

                                    viewTable.FormatRules.Add(ruleBottom);
                                }
                                break;
                            case "dateoccuring":
                                if (column != null)
                                {
                                    var filterDateType = Utils.ChangeType<FilterDateType>(formatCondition.DateOccuring, FilterDateType.None);
                                    if (filterDateType != FilterDateType.None &&
                                        filterDateType != FilterDateType.SpecificDate &&
                                        filterDateType != FilterDateType.User)
                                    {
                                        var ruleDateOccuring = new GridFormatRule()
                                        {
                                            Column        = column,
                                            ColumnApplyTo = targetColumn,
                                            ApplyToRow    = formatCondition.ApplyToRow
                                        };

                                        var formatDateOccuring = new FormatConditionRuleDateOccuring()
                                        {
                                            DateType = filterDateType
                                        };
                                        ApplyFormatting(formatDateOccuring, formatCondition);

                                        ruleDateOccuring.Rule = formatDateOccuring;

                                        viewTable.FormatRules.Add(ruleDateOccuring);
                                    }
                                }
                                break;
                            case "rule":
                            case "condition":
                                if (column != null)
                                {
                                    var condition = Utils.ChangeType<DevExpress.XtraEditors.FormatCondition>(formatCondition.Condition, DevExpress.XtraEditors.FormatCondition.None);
                                    if (condition != DevExpress.XtraEditors.FormatCondition.None)
                                    {
                                        var ruleRuleValue = new GridFormatRule()
                                        {
                                            Column        = column,
                                            ColumnApplyTo = targetColumn,
                                            ApplyToRow    = formatCondition.ApplyToRow
                                        };

                                        var formatRuleValue = new FormatConditionRuleValue()
                                        {
                                            Condition  = condition,
                                            Expression = formatCondition.Expression,
                                            Value1     = Utils.ChangeType(column.ColumnType, formatCondition.Value1, null),
                                            Value2     = Utils.ChangeType(column.ColumnType, formatCondition.Value2, null)
                                        };
                                        ApplyFormatting(formatRuleValue, formatCondition);

                                        ruleRuleValue.Rule = formatRuleValue;

                                        viewTable.FormatRules.Add(ruleRuleValue);
                                    }
                                }
                                break;
                        }
                    }
                }
                
                if (data.ComputedColumns != null)
                {
                    for (int i = viewTable.Columns.Count-1; i >= 0; i--)
                    {
                        if (viewTable.Columns[i].UnboundType != UnboundColumnType.Bound)
                            viewTable.Columns.RemoveAt(i);
                    }
                    
                    foreach (var computedColumn in data.ComputedColumns)
                    {
                        var caption = computedColumn.ColumnName;

                        var newColumn                       = viewTable.Columns.AddVisible(caption);
                        newColumn.Name                      = caption;
                        newColumn.Caption                   = caption;
                        newColumn.UnboundType               = ConvertUnboudColumnType(computedColumn.ReturnType);
                        newColumn.UnboundExpression         = computedColumn.Expression;
                        newColumn.ShowUnboundExpressionMenu = true;
                        newColumn.Visible                   = true;
                        newColumn.Width                     = 100;
                    }
                }

                viewTable.UpdateViewColumns();
            }


            static void ApplyFormatting(FormatConditionRuleAppearanceBase formatting, GridFormatCondition formatCondition)
            {
                if (!string.IsNullOrWhiteSpace(formatCondition.AppearanceName))
                    formatting.PredefinedName = formatCondition.AppearanceName;
                else
                    ApplyAppearance(formatting.Appearance, formatCondition);
            }

            static void ApplyAppearance(AppearanceObjectEx appearance, GridFormatCondition formatCondition)
            {
                Color backColor = ColorExtensions.FromHtmlColor(formatCondition.BackColor, Color.Empty);
                if (backColor != Color.Empty)
                    appearance.BackColor = backColor;

                Color foreColor = ColorExtensions.FromHtmlColor(formatCondition.ForeColor, Color.Empty);
                if (foreColor != Color.Empty)
                    appearance.ForeColor = foreColor;

                Color backColor2 = ColorExtensions.FromHtmlColor(formatCondition.BackColor2, Color.Empty);
                if (backColor2 != Color.Empty)
                    appearance.BackColor2 = backColor2;

                Color borderColor = ColorExtensions.FromHtmlColor(formatCondition.BorderColor, Color.Empty);
                if (borderColor != Color.Empty)
                    appearance.BorderColor = borderColor;

                if (!string.IsNullOrWhiteSpace(formatCondition.Gradient) && Enum.TryParse<LinearGradientMode>(formatCondition.Gradient, true,
                    out LinearGradientMode gradientMode))
                    appearance.GradientMode = gradientMode;

                if (!string.IsNullOrWhiteSpace(formatCondition.Font))
                {
                    try
                    {
                        var appearanceFont = appearance.Font != null ? Utils.FontToString(appearance.Font) : string.Empty;
                        if (string.Compare(appearanceFont, formatCondition.Font, true) != 0)
                        {
                            var font = Utils.StringToFont(formatCondition.Font, out Color fontColor);
                            if (font != null)
                                appearance.Font = font;
                            if (fontColor != Color.Empty && foreColor == Color.Empty)
                                appearance.ForeColor = fontColor;
                        }
                    }
                    catch (Exception)
                    {
                        //Do nothing
                    }
                }
            }


            static UnboundColumnType ConvertUnboudColumnType(ComputedColumn.ComputedColumnType returnType)
            {
                return returnType switch
                {
                    ComputedColumn.ComputedColumnType.String   => UnboundColumnType.String,
                    ComputedColumn.ComputedColumnType.Integer  => UnboundColumnType.Integer,
                    ComputedColumn.ComputedColumnType.Decimal  => UnboundColumnType.Decimal,
                    ComputedColumn.ComputedColumnType.DateTime => UnboundColumnType.DateTime,
                    ComputedColumn.ComputedColumnType.Boolean  => UnboundColumnType.Boolean,
                    _                                          => UnboundColumnType.String
                };
            }
        }

        public void SaveGridData(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName), "Filename cannot be empty");

            var gridData = SaveGridData();

            using var zip = new ZipArchive();
            using var memStream = new MemoryStream();
            Utils.SerializeObjectToStream<GridData>(gridData, memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            zip.AddStream("GridControl.xml", memStream);

            zip.Save(fileName);
        }

        public GridData SaveGridData() =>
            SaveGridViewData((GridView)viewTable);

        public static GridData SaveGridViewData(GridView viewTable)
        {
            var result = new GridData();

            var gridProperties = new GridProperties(viewTable);
            var gridParameters = new GridParametersObject();
            gridProperties.SaveParameters(gridParameters);
            result.Parameters = gridParameters;

            foreach (GridColumn gridColumn in viewTable.VisibleColumns)
            {
                var summaries = new StringBuilder();
                foreach (GridColumnSummaryItem summary in gridColumn.Summary)
                {
                    if (summary.SummaryType != SummaryItemType.None)
                    {
                        if (summaries.Length > 0)
                            summaries.Append(',');
                        summaries.Append(Enum.GetName(typeof(SummaryItemType), summary.SummaryType));
                    }
                }

                result.ColumnOrder.Add(new GridFormatColumn() { ColumnName = gridColumn.FieldName, Summary = summaries.ToString() });
            }

            foreach (GridColumn gridColumn in viewTable.GroupedColumns)
                result.GroupBy.Add(new GridFormatColumn() { ColumnName = gridColumn.FieldName, SortOrder = gridColumn.SortOrder == ColumnSortOrder.Descending ? "desc" : "asc" });

            foreach (GridColumn gridColumn in viewTable.SortedColumns)
                result.OrderBy.Add(new GridFormatColumn() { ColumnName = gridColumn.FieldName, SortOrder = gridColumn.SortOrder == ColumnSortOrder.Descending ? "desc" : "asc" });


            foreach (var formatRule in viewTable.FormatRules)
            {
                var columnName = !formatRule.ApplyToRow ? formatRule.Column?.FieldName : null;

                switch (formatRule.Rule)
                {
                    case FormatConditionRuleExpression formatExpression:
                        var formatConditionExpression = new GridFormatCondition()
                        {
                            FormatRule = "Expression",
                            ColumnName = columnName,
                            Expression = formatExpression.Expression
                        };
                        SetFormatting(formatExpression.Appearance, formatExpression.PredefinedName, formatConditionExpression);
                        result.FormatConditions.Add(formatConditionExpression);
                        break;
                    case FormatConditionRuleDataBar formatDataBar:
                        var formatConditionDataBar = new GridFormatCondition()
                        {
                            FormatRule = "DataBar",
                            ColumnName = columnName
                        };
                        SetFormatting(formatDataBar.Appearance, formatDataBar.PredefinedName, formatConditionDataBar);
                        result.FormatConditions.Add(formatConditionDataBar);
                        break;
                    case FormatConditionRuleIconSet formatIconSet:
                        var iconSetType = GetIconSetType(formatIconSet.IconSet);
                        var formatConditionIconSet = new GridFormatCondition()
                        {
                            FormatRule = "IconSet",
                            ColumnName = columnName,
                            IconSet = Enum.GetName(typeof(IconSetType), iconSetType)
                        };
                        result.FormatConditions.Add(formatConditionIconSet);
                        break;
                    case FormatConditionRule3ColorScale formatColorScale3:
                        var formatConditionColorScale3 = new GridFormatCondition()
                        {
                            FormatRule = "ColorScale",
                            ColumnName = columnName,
                            ColorScale = !string.IsNullOrWhiteSpace(formatColorScale3.PredefinedName) ? formatColorScale3.PredefinedName :
                                $"{formatColorScale3.MinimumColor.ToHtmlColor()},{formatColorScale3.MiddleColor.ToHtmlColor()},{formatColorScale3.MaximumColor.ToHtmlColor()}"
                        };
                        result.FormatConditions.Add(formatConditionColorScale3);
                        break;
                    case FormatConditionRule2ColorScale formatColorScale2:
                        var formatConditionColorScale2 = new GridFormatCondition()
                        {
                            FormatRule = "ColorScale",
                            ColumnName = columnName,
                            ColorScale = !string.IsNullOrWhiteSpace(formatColorScale2.PredefinedName) ? formatColorScale2.PredefinedName :
                                $"{formatColorScale2.MinimumColor.ToHtmlColor()},{formatColorScale2.MaximumColor.ToHtmlColor()}"
                        };
                        result.FormatConditions.Add(formatConditionColorScale2);
                        break;
                    case FormatConditionRuleAboveBelowAverage formatAboveBelowAverage:
                        var ruleNameAboveBelow = formatAboveBelowAverage.AverageType switch
                        {
                            FormatConditionAboveBelowType.Above        => "AboveAverage",
                            FormatConditionAboveBelowType.Below        => "BelowAverage",
                            FormatConditionAboveBelowType.EqualOrAbove => "AboveOrEqualAverage",
                            FormatConditionAboveBelowType.EqualOrBelow => "BelowOrEqualAverage",
                            _                                          => "AboveAverage"
                        };
                        var formatConditionAboveAverage = new GridFormatCondition()
                        {
                            FormatRule = ruleNameAboveBelow,
                            ColumnName = columnName
                        };
                        SetFormatting(formatAboveBelowAverage.Appearance, formatAboveBelowAverage.PredefinedName, formatConditionAboveAverage);
                        result.FormatConditions.Add(formatConditionAboveAverage);
                        break;
                    case FormatConditionRuleUniqueDuplicate formatUniqueDuplicate:
                        var formatConditionUniqueDuplicate = new GridFormatCondition()
                        {
                            FormatRule = formatUniqueDuplicate.FormatType == FormatConditionUniqueDuplicateType.Unique ? "Unique" : "Duplicate",
                            ColumnName = columnName
                        };
                        SetFormatting(formatUniqueDuplicate.Appearance, formatUniqueDuplicate.PredefinedName, formatConditionUniqueDuplicate);
                        result.FormatConditions.Add(formatConditionUniqueDuplicate);
                        break;
                    case FormatConditionRuleTopBottom formatTopBottom:
                        var formatConditionTopBottom = new GridFormatCondition()
                        {
                            FormatRule = formatTopBottom.TopBottom == FormatConditionTopBottomType.Top ? "Top" : "Bottom",
                            ColumnName = columnName,
                            Rank = formatTopBottom.Rank.ToString() + (formatTopBottom.RankType == FormatConditionValueType.Percent ? "%" : "")
                        };
                        SetFormatting(formatTopBottom.Appearance, formatTopBottom.PredefinedName, formatConditionTopBottom);
                        result.FormatConditions.Add(formatConditionTopBottom);
                        break;
                    case FormatConditionRuleDateOccuring formatDateOccuring:
                        var formatConditionDateOccuring = new GridFormatCondition()
                        {
                            FormatRule = "DateOccuring",
                            ColumnName = columnName,
                            DateOccuring = Enum.GetName(typeof(FilterDateType), formatDateOccuring.DateType)
                        };
                        SetFormatting(formatDateOccuring.Appearance, formatDateOccuring.PredefinedName, formatConditionDateOccuring);
                        result.FormatConditions.Add(formatConditionDateOccuring);
                        break;
                    case FormatConditionRuleValue formatRuleValue:
                        var formatConditionRule = new GridFormatCondition()
                        {
                            FormatRule = "Rule",
                            ColumnName = columnName,
                            Condition = Enum.GetName(typeof(DevExpress.XtraEditors.FormatCondition), formatRuleValue.Condition),
                            Value1 = Convert.ToString(formatRuleValue.Value1),
                            Value2 = Convert.ToString(formatRuleValue.Value2),
                            Expression = formatRuleValue.Expression
                        };
                        SetFormatting(formatRuleValue.Appearance, formatRuleValue.PredefinedName, formatConditionRule);
                        result.FormatConditions.Add(formatConditionRule);
                        break;
                }
            }

            return result;


            void SetFormatting(AppearanceObjectEx formatting, string formattingName, GridFormatCondition formatCondition)
            {
                if (!string.IsNullOrWhiteSpace(formattingName))
                    formatCondition.AppearanceName = formattingName;
                else
                {
                    if (formatting.BackColor != Color.Empty)
                        formatCondition.BackColor = formatting.BackColor.ToHtmlColor();

                    if (formatting.ForeColor != Color.Empty)
                        formatCondition.ForeColor = formatting.ForeColor.ToHtmlColor();

                    if (formatting.BackColor2 != Color.Empty)
                        formatCondition.BackColor2 = formatting.BackColor2.ToHtmlColor();

                    if (formatting.BorderColor != Color.Empty)
                        formatCondition.BorderColor = formatting.BorderColor.ToHtmlColor();

                    if (formatting.Font != null && formatting.Font != viewTable.Appearance.Row.Font)
                        formatCondition.Font = Utils.FontToString(formatting.Font);
                }
            }
        }

        public static void ApplyGridFormatRules(GridView viewTable, List<BaseCommand> commands)
        {
            var gridData = new GridData();
            gridData.ApplyGridFormatConditions(commands);

            LoadGridData(viewTable, gridData);
        }

        private static FormatConditionIconSet GetPredefinedIconSet(IconSetType iconSetType)
        {
            return iconSetType switch
            {
                IconSetType.Arrows3         => FormatPredefinedIconSets.Default.Arrows3Colored,
                IconSetType.ArrowsGray3     => FormatPredefinedIconSets.Default.Arrows3Gray,
                IconSetType.Flags3          => FormatPredefinedIconSets.Default.Flags3,
                IconSetType.TrafficLights13 => FormatPredefinedIconSets.Default.TrafficLights3Unrimmed,
                IconSetType.TrafficLights23 => FormatPredefinedIconSets.Default.TrafficLights3Rimmed,
                IconSetType.Signs3          => FormatPredefinedIconSets.Default.Signs3,
                IconSetType.Symbols3        => FormatPredefinedIconSets.Default.Symbols3Circled,
                IconSetType.Symbols23       => FormatPredefinedIconSets.Default.Symbols3Uncircled,
                IconSetType.Stars3          => FormatPredefinedIconSets.Default.Stars3,
                IconSetType.Triangles3      => FormatPredefinedIconSets.Default.Triangles3,
                IconSetType.Arrows4         => FormatPredefinedIconSets.Default.Arrows4Colored,
                IconSetType.ArrowsGray4     => FormatPredefinedIconSets.Default.Arrows4Gray,
                IconSetType.RedToBlack4     => FormatPredefinedIconSets.Default.RedToBlack,
                IconSetType.Rating4         => FormatPredefinedIconSets.Default.Ratings4,
                IconSetType.TrafficLights4  => FormatPredefinedIconSets.Default.TrafficLights4,
                IconSetType.Arrows5         => FormatPredefinedIconSets.Default.Arrows5Colored,
                IconSetType.ArrowsGray5     => FormatPredefinedIconSets.Default.Arrows5Gray,
                IconSetType.Rating5         => FormatPredefinedIconSets.Default.Ratings5,
                IconSetType.Quarters5       => FormatPredefinedIconSets.Default.Quarters5,
                IconSetType.Boxes5          => FormatPredefinedIconSets.Default.Boxes5,
                _                           => FormatPredefinedIconSets.Default.Arrows5Colored
            };
        }

        private static IconSetType GetIconSetType(FormatConditionIconSet iconSet)
        {
            return iconSet.Name switch
            {
                "Stars3"                 => IconSetType.Stars3,
                "Ratings4"               => IconSetType.Rating4,
                "Ratings5"               => IconSetType.Rating5,
                "Quarters5"              => IconSetType.Quarters5,
                "Boxes5"                 => IconSetType.Boxes5,
                "Arrows3Colored"         => IconSetType.Arrows3,
                "Arrows3Gray"            => IconSetType.ArrowsGray3,
                "Triangles3"             => IconSetType.Triangles3,
                "Arrows4Colored"         => IconSetType.Arrows4,
                "Arrows4Gray"            => IconSetType.ArrowsGray4,
                "Arrows5Colored"         => IconSetType.Arrows3,
                "Arrows5Gray"            => IconSetType.ArrowsGray5,
                "TrafficLights3Rimmed"   => IconSetType.TrafficLights23,
                "TrafficLights3Unrimmed" => IconSetType.TrafficLights13,
                "Signs3"                 => IconSetType.Signs3,
                "TrafficLights4"         => IconSetType.TrafficLights4,
                "RedToBlack"             => IconSetType.RedToBlack4,
                "Symbols3Uncircled"      => IconSetType.Symbols23,
                "Symbols3Circled"        => IconSetType.Symbols3,
                "Flags3"                 => IconSetType.Flags3,
                _                        => IconSetType.None
            };
        }
    }
}
