using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Extensions
{
    public static class GridViewExtensions
    {
        public static void SetMonoWidthGridFont(this ColumnView view) =>
#pragma warning disable IDE0068 // Use recommended dispose pattern
            SetGridFont(view, new Font("Lucida Console", 8.25F));
#pragma warning restore IDE0068 // Use recommended dispose pattern

        public static void SetGridFont(this ColumnView view, Font font)
        {
            foreach (AppearanceObject ap in view.Appearance)
            {
                var apName = Utils.NonNullString(ap.Name);
                if (apName.Contains("Row") || apName.Contains("Cell"))
                    ap.Font = font;
            }
        }

        public static void UpdateViewColumns(this ColumnView columnView)
        {
            foreach (GridColumn column in columnView.Columns)
            {
                column.Caption = column.FieldName;

                column.SortMode = ColumnSortMode.Custom;
                if (column.Width > 300)
                    column.Width = 300;
            }
        }
    }
}
