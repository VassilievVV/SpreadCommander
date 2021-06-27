using DevExpress.Spreadsheet;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Extensions
{
    public static class SpreadsheetExtensions
    {
        public static Table GetSelectedTable(this IWorkbook workbook)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));

            var sheet = workbook.Worksheets.ActiveWorksheet;
            if (sheet == null)
                return null;

            var selection     = sheet.Selection.GetRangeWithAbsoluteReference();
            var selectedTable = selection?.GetRangeTable();

            return selectedTable;
        }

        public static Table GetRangeTable(this CellRange range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            var sheet = range.Worksheet;
            Table rangeTable = null;

            foreach (var table in sheet.Tables)
            {
                if (range.IsIntersecting(table.Range))
                {
                    rangeTable = table;
                    break;
                }
            }

            if (rangeTable == null)
                return null;

            return rangeTable;
        }

        public static CellRange ExpandToTableRows(this CellRange range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            CellRange dataRange;
            var sheet        = range.Worksheet;
            var resultRanges = new List<CellRange>();

            var table = range.GetRangeTable();
            if (table != null)
                dataRange = table.DataRange;
            else
                dataRange = range.Worksheet.GetDataRange();

            var topRowIndex    = dataRange.TopRowIndex;
            var bottomRowIndex = dataRange.BottomRowIndex;

            foreach (var area in range.Areas)
            {
                var resultArea = sheet.Range.FromLTRB(area.LeftColumnIndex, topRowIndex, area.RightColumnIndex, bottomRowIndex);
                resultRanges.Add(resultArea);
            }

            var result = sheet.Range.Union(resultRanges.ToArray());
            return result;
        }

        public static CellRange ExpandToTableColumn (this CellRange range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            CellRange dataRange;
            var sheet        = range.Worksheet;
            var resultRanges = new List<CellRange>();

            var table = range.GetRangeTable();
            if (table != null)
                dataRange = table.DataRange;
            else
                dataRange = range.Worksheet.GetDataRange();

            var leftColumnIndex  = dataRange.LeftColumnIndex;
            var rightColumnIndex = dataRange.RightColumnIndex;

            foreach (var area in range.Areas)
            {
                var resultArea = sheet.Range.FromLTRB(leftColumnIndex, area.TopRowIndex, rightColumnIndex, area.BottomRowIndex);
                resultRanges.Add(resultArea);
            }

            var result = sheet.Range.Union(resultRanges.ToArray());
            return result;
        }

        public static void CopyToTableRows(this CellRange range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            var tableRange = range.ExpandToTableRows()?.Exclude(range);
            if (tableRange != null)
                tableRange.CopyFrom(range);
        }

        public static void CopyToTableColumns(this CellRange range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            var tableRange = range.ExpandToTableColumn()?.Exclude(range);
            if (tableRange != null)
                tableRange.CopyFrom(range);
        }

        public static DataTable ExportToDataTable(this Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var result = SpreadsheetUtils.GetDataTable(table);
            return result;
        }
    }
}
