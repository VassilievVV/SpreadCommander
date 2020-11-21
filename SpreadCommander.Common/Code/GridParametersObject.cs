using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using DevExpress.XtraEditors;

namespace SpreadCommander.Common.Code
{
    public class GridParametersObject
    {
        public enum GridMultiSelectMode { RowSelect = 0, CellSelect = 1, CheckBoxRowSelect = 2 }

        public GridParametersObject()
        {
            AllowIncrementalSearch			= true;
            FixedLineWidth					= 2;
            ShowFooter						= true;
            ShowGroupPanel					= true;
            ShowAutoFilter					= true;
            AllowExpandEmptyDetail			= false;
            AllowOnlyOneMasterRowExpanded	= false;
            AutoZoomDetail					= false;
            EnableAppearanceEvenRow			= false;
            EnableAppearanceOddRow			= true;
            ShowHorzLines					= true;
            ShowVertLines					= true;
            ShowIndicator					= true;
            RowSeparatorHeight				= 0;
            AllowCellMerge					= false;
            ColumnAutoWidth					= false;
            RowAutoHeight					= false;
            RowHeight						= -1;
            AutoCalcPreviewLineCount		= false;
            //MultiSelectMode					= GridMultiSelectMode.CellSelect;
            ShowSplitGrid					= false;
            FindPanelBehavior               = FindPanelBehavior.Search;
        }

#pragma warning disable CA1822 // Mark members as static
        public string Name
#pragma warning restore CA1822 // Mark members as static
        {
            get {return "Grid";}
        }

#pragma warning disable CA1822 // Mark members as static
        public string DisplayName
#pragma warning restore CA1822 // Mark members as static
        {
            get {return "Grid";}
        }

        [DefaultValue(true)]
        [Category("Behaviour")]
        [DisplayName("Incremental search")]
        [Description("Whether end users can locate rows by typing the desired column value.")]
        public bool AllowIncrementalSearch {get; set;}

        [DefaultValue(2)]
        [Category("Behaviour")]
        [DisplayName("Fixed line width")]
        [Description("Width of frozen panel separators.")]
        public int FixedLineWidth {get; set;}

        [DefaultValue(true)]
        [Category("Behaviour")]
        [DisplayName("Footer")]
        [Description("Whether the view footer is displayed.")]
        public bool ShowFooter {get; set;}

        [DefaultValue(true)]
        [Category("Behaviour")]
        [DisplayName("Group panel")]
        [Description("Whether the group panel is displayed.")]
        public bool ShowGroupPanel {get; set;}

        [DefaultValue(true)]
        [Category("Behaviour")]
        [DisplayName("Auto filter")]
        [Description("Whether the auto filter row is displayed.")]
        public bool ShowAutoFilter {get; set;}

        [DefaultValue(false)]
        [Category("Details")]
        [DisplayName("Expand empty")]
        [Description("Whether master rows can be expanded when details have no data.")]
        public bool AllowExpandEmptyDetail {get; set;}

        [DefaultValue(false)]
        [Category("Details")]
        [DisplayName("Expand single row")]
        [Description("Whether several master rows can be expanded simultaneously.")]
        public bool AllowOnlyOneMasterRowExpanded {get; set;}

        [DefaultValue(false)]
        [Category("Details")]
        [DisplayName("Auto zoom")]
        [Description("Whether details are automatically maximized to fit the entire grid control's area when corresponding master rows are expanded.")]
        public bool AutoZoomDetail {get; set;}

        [DefaultValue(false)]
        [Category("Appearance")]
        [DisplayName("Highlight even rows")]
        [Description("Whether the even rows are painted using the appearance settings provided by the GridViewAppearances.EvenRow property.")]
        public bool EnableAppearanceEvenRow {get; set;}

        [DefaultValue(true)]
        [Category("Appearance")]
        [DisplayName("Highlight odd rows")]
        [Description("Whether the odd rows are painted using the appearance settings provided by the GridViewAppearances.OddRow property.")]
        public bool EnableAppearanceOddRow {get; set;}

        [DefaultValue(true)]
        [Category("Appearance")]
        [DisplayName("Horizontal lines")]
        [Description("Whether horizontal grid lines are displayed.")]
        public bool ShowHorzLines {get; set;}

        [DefaultValue(true)]
        [Category("Appearance")]
        [DisplayName("Vertical lines")]
        [Description("Whether vertical grid lines are displayed.")]
        public bool ShowVertLines {get; set;}

        [DefaultValue(true)]
        [Category("Appearance")]
        [DisplayName("Indicator")]
        [Description("Whether the row indicator panel is displayed.")]
        public bool ShowIndicator {get; set;}

        [DefaultValue(0)]
        [Category("Appearance")]
        [DisplayName("Row separator height")]
        [Description("Distance between rows. Clears merge cells when greater than zero.")]
        public int RowSeparatorHeight {get; set;}

        [DefaultValue(false)]
        [Category("Appearance")]
        [DisplayName("Merge cells")]
        [Description("Whether neighboring cells with identical values can merge.")]
        public bool AllowCellMerge {get; set;}

        [DefaultValue(false)]
        [Category("Appearance")]
        [DisplayName("Column auto width")]
        [Description("Whether the number of text lines within preview sections is calculated automatically depending on their contents.")]
        public bool ColumnAutoWidth {get; set;}

        [DefaultValue(false)]
        [Category("Appearance")]
        [DisplayName("Row auto height")]
        [Description("Whether the height of each data row is automatically adjusted to completely display the contents of its cells.")]
        public bool RowAutoHeight {get; set;}

        [DefaultValue(-1)]
        [Category("Appearance")]
        [DisplayName("Row height")]
        [Description("Height of cells within data rows. Set to -1 to use default value.")]
        public int RowHeight {get; set;}

        [DefaultValue(false)]
        [Category("Preview field")]
        [DisplayName("Auto line count")]
        [Description("Whether the number of text lines within preview sections is calculated automatically depending on their contents.")]
        public bool AutoCalcPreviewLineCount {get; set;}

        [Category("Edit")]
        [DisplayName("Export to old Excel")]
        [Description("Export to Excel 2003 and earlier format. Select that option if you cannot open files in Excel 2007 and later format.")]
        public bool ExportToOldExcel {get; set;}

        /*
        [DefaultValue(GridMultiSelectMode.CellSelect)]
        [Category("Behaviour")]
        [DisplayName("Multi-select mode")]
        [Description("Whether multiple cells or rows can be selected.")]
        public GridMultiSelectMode MultiSelectMode {get; set;}
        */

        [DefaultValue(false)]
        [Category("Behaviour")]
        [DisplayName("Split grid")]
        [Description("Whether grid is splitted.")]
        public bool ShowSplitGrid {get; set;}

        [DefaultValue(FindPanelBehavior.Search)]
        [Category("Behaviour")]
        [DisplayName("Find behaviour")]
        [Description("Whether records that do not match a query in the Find Panel are hidden.")]
        public FindPanelBehavior FindPanelBehavior {get; set;}
    }
}