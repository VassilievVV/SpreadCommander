using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Documents.Extensions;

namespace SpreadCommander.Documents.Code
{
	public static class GridUtils
	{
		public static void InitializeGridView(GridView gridView)
		{
			gridView.OptionsBehavior.AllowSortAnimation             = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsBehavior.Editable                       = false;
			gridView.OptionsBehavior.ReadOnly                       = true;
			gridView.OptionsClipboard.AllowCopy                     = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.AllowCsvFormat                = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.AllowExcelFormat              = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.AllowHtmlFormat               = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.AllowRtfFormat                = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.AllowTxtFormat                = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.ClipboardMode                 = DevExpress.Export.ClipboardMode.Formatted;
			gridView.OptionsClipboard.CopyCollapsedData             = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.CopyColumnHeaders             = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsClipboard.PasteMode                     = DevExpress.Export.PasteMode.None;
			gridView.OptionsDetail.AllowExpandEmptyDetails          = true;
			gridView.OptionsDetail.AllowOnlyOneMasterRowExpanded    = true;
			gridView.OptionsFilter.ColumnFilterPopupMode            = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode.Excel;
			gridView.OptionsFind.AlwaysVisible                      = true;
			gridView.OptionsFind.Behavior                           = DevExpress.XtraEditors.FindPanelBehavior.Search;
			gridView.OptionsFind.SearchInPreview                    = true;
			gridView.OptionsMenu.ShowAddNewSummaryItem              = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsMenu.ShowConditionalFormattingItem      = true;
			gridView.OptionsMenu.ShowFooterItem                     = true;
			gridView.OptionsMenu.ShowGroupSummaryEditorItem         = true;
			gridView.OptionsScrollAnnotations.ShowCustomAnnotations = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsScrollAnnotations.ShowErrors            = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsScrollAnnotations.ShowFocusedRow        = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsScrollAnnotations.ShowSelectedRows      = DevExpress.Utils.DefaultBoolean.True;
			gridView.OptionsSelection.MultiSelect                   = true;
			gridView.OptionsSelection.MultiSelectMode               = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
			gridView.OptionsView.BestFitMaxRowCount                 = 100;
			gridView.OptionsView.BestFitMode                        = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Fast;
			gridView.OptionsView.ColumnAutoWidth                    = false;
			gridView.OptionsView.EnableAppearanceEvenRow            = true;
			gridView.OptionsView.ShowAutoFilterRow                  = true;
			gridView.OptionsView.ShowGroupPanelColumnsAsSingleRow   = true;

			gridView.SetMonoWidthGridFont();
		}
	}
}
