#pragma warning disable CRR0047

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevExpress.XtraGrid.Columns;
using System.Globalization;
using DevExpress.XtraGrid.Views.Grid;
using SpreadCommander.Common;
using System.Reflection;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using SpreadCommander.Common.Code;
using DevExpress.XtraEditors;
using SpreadCommander.Documents.Controls;

namespace SpreadCommander.Documents.Code
{
	#region PreviewFieldNameConverter
	class PreviewFieldNameConverter: TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			else
				return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;
			else
				return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
				return value;
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is string)
				return value;
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			List<string> values = new List<string>();

			if (context != null && context.Instance != null && context.Instance is GridProperties)
			{
				GridProperties props = (GridProperties)context.Instance;
				GridView view = props.GridView;
				if (view != null)
				{
					for (int i = 0; i < view.Columns.Count; i++)
						values.Add(view.Columns[i].FieldName);

					values.Sort(StringLogicalComparer.DefaultComparer);
				}
			}

			return new StandardValuesCollection(values);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return true;
		}
	}
	#endregion

	public class GridProperties
	{
		private readonly GridView _view;

		public GridProperties(GridView view)
		{
			_view = view;
		}

		[Browsable(false)]
		public GridView GridView
		{
			get {return _view;}
		}

		[DefaultValue(true)]
		[Category("Behavior")]
		[DisplayName("Incremental search")]
		[Description("Whether end users can locate rows by typing the desired column value.")]
		public bool AllowIncrementalSearch
		{
			get {return _view.OptionsBehavior.AllowIncrementalSearch;}
			set {_view.OptionsBehavior.AllowIncrementalSearch = value;}
		}

		[DefaultValue(2)]
		[Category("Behavior")]
		[DisplayName("Fixed line width")]
		[Description("Width of frozen panel separators.")]
		public int FixedLineWidth
		{
			get {return _view.FixedLineWidth;}
			set {_view.FixedLineWidth = value;}
		}

		[DefaultValue(true)]
		[Category("Behavior")]
		[DisplayName("Footer")]
		[Description("Whether the view footer is displayed.")]
		public bool ShowFooter
		{
			get {return _view.OptionsView.ShowFooter;}
			set {_view.OptionsView.ShowFooter = value;}
		}

		[DefaultValue(true)]
		[Category("Behavior")]
		[DisplayName("Group panel")]
		[Description("Whether the group panel is displayed.")]
		public bool ShowGroupPanel
		{
			get {return _view.OptionsView.ShowGroupPanel;}
			set {_view.OptionsView.ShowGroupPanel = value;}
		}

		[DefaultValue(true)]
		[Category("Behavior")]
		[DisplayName("Auto filter")]
		[Description("Whether the auto filter row is displayed.")]
		public bool ShowAutoFilter
		{
			get {return _view.OptionsView.ShowAutoFilterRow;}
			set {_view.OptionsView.ShowAutoFilterRow = value;}
		}

		[DefaultValue(false)]
		[Category("Behavior")]
		[DisplayName("Find panel")]
		[Description("Whether the find panel is displayed.")]
		public bool ShowFindPanel
		{
			get {return _view.IsFindPanelVisible;}
			set 
			{
				if (_view.IsFindPanelVisible) 
					_view.HideFindPanel();
				else
					_view.ShowFindPanel();
			}
		}

		[DefaultValue(0)]
		[Category("Behavior")]
		[DisplayName("Fixed left columns")]
		[Description("Width of fixed columns on left side.")]
		public int FixedColumnsOnLeft
		{
			get
			{
				for (int i = 0; i < _view.Columns.Count; i++)
				{
					if (_view.Columns[i].Fixed != FixedStyle.Left)
						return i;
				}

				return 0;
			}
			set
			{
				for (int i = 0; i < _view.Columns.Count; i++)
				{
					switch (_view.Columns[i].Fixed)
					{
						case FixedStyle.None:
						case FixedStyle.Right:
							if (i < value)
								_view.Columns[i].Fixed = FixedStyle.Left;
							break;
						case FixedStyle.Left:
							if (i >= value)
								_view.Columns[i].Fixed = FixedStyle.None;
							break;
					}
				}
			}
		}

		[DefaultValue(0)]
		[Category("Behavior")]
		[DisplayName("Fixed right columns")]
		[Description("Width of fixed columns on right side.")]
		public int FixedColumnsOnRight
		{
			get
			{
				for (int i = _view.Columns.Count - 1; i >= 0; i++)
				{
					if (_view.Columns[i].Fixed != FixedStyle.Right)
						return _view.Columns.Count - 1 - i;
				}

				return 0;
			}
			set
			{
				int colCount = _view.Columns.Count;

				for (int i = 0; i < colCount; i++)
				{
					switch (_view.Columns[i].Fixed)
					{
						case FixedStyle.None:
						case FixedStyle.Left:
							if (i >= colCount - value)
								_view.Columns[i].Fixed = FixedStyle.Right;
							break;
						case FixedStyle.Right:
							if (i < colCount - value)
								_view.Columns[i].Fixed = FixedStyle.None;
							break;
					}
				}
			}
		}

		[DefaultValue(false)]
		[Category("Details")]
		[DisplayName("Expand empty")]
		[Description("Whether master rows can be expanded when details have no data.")]
		public bool AllowExpandEmptyDetail
		{
			get {return _view.OptionsDetail.AllowExpandEmptyDetails;}
			set {_view.OptionsDetail.AllowExpandEmptyDetails = value;}
		}

		[DefaultValue(false)]
		[Category("Details")]
		[DisplayName("Expand single row")]
		[Description("Whether several master rows can be expanded simultaneously.")]
		public bool AllowOnlyOneMasterRowExpanded
		{
			get {return _view.OptionsDetail.AllowOnlyOneMasterRowExpanded;}
			set {_view.OptionsDetail.AllowOnlyOneMasterRowExpanded = value;}
		}

		[DefaultValue(false)]
		[Category("Details")]
		[DisplayName("Auto zoom")]
		[Description("Whether details are automatically maximized to fit the entire grid control's area when corresponding master rows are expanded.")]
		public bool AutoZoomDetail
		{
			get {return _view.OptionsDetail.AutoZoomDetail;}
			set {_view.OptionsDetail.AutoZoomDetail = value;}
		}

		[DefaultValue(false)]
		[Category("Appearance")]
		[DisplayName("Highlight even rows")]
		[Description("Whether the even rows are painted using the appearance settings provided by the GridViewAppearances.EvenRow property.")]
		public bool EnableAppearanceEvenRow
		{
			get {return _view.OptionsView.EnableAppearanceEvenRow;}
			set {_view.OptionsView.EnableAppearanceEvenRow = value;}
		}

		[DefaultValue(true)]
		[Category("Appearance")]
		[DisplayName("Highlight odd rows")]
		[Description("Whether the odd rows are painted using the appearance settings provided by the GridViewAppearances.OddRow property.")]
		public bool EnableAppearanceOddRow
		{
			get {return _view.OptionsView.EnableAppearanceOddRow;}
			set {_view.OptionsView.EnableAppearanceOddRow = value;}
		}

		[DefaultValue(true)]
		[Category("Appearance")]
		[DisplayName("Horizontal lines")]
		[Description("Whether horizontal grid lines are displayed.")]
		public bool ShowHorzLines
		{
			get {return _view.OptionsView.ShowHorizontalLines == DefaultBoolean.False ? false : true;}
			set {_view.OptionsView.ShowHorizontalLines = value ? DefaultBoolean.True : DefaultBoolean.False;}
		}

		[DefaultValue(true)]
		[Category("Appearance")]
		[DisplayName("Vertical lines")]
		[Description("Whether vertical grid lines are displayed.")]
		public bool ShowVertLines
		{
			get {return _view.OptionsView.ShowVerticalLines == DefaultBoolean.False ? false : true;}
			set {_view.OptionsView.ShowVerticalLines = value ? DefaultBoolean.True : DefaultBoolean.False;}
		}

		[DefaultValue(true)]
		[Category("Appearance")]
		[DisplayName("Indicator")]
		[Description("Whether the row indicator panel is displayed.")]
		public bool ShowIndicator
		{
			get {return _view.OptionsView.ShowIndicator;}
			set {_view.OptionsView.ShowIndicator = value;}
		}

		[DefaultValue(0)]
		[Category("Appearance")]
		[DisplayName("Row separator height")]
		[Description("Distance between rows. Clears merge cells when greater than zero.")]
		public int RowSeparatorHeight
		{
			get {return _view.RowSeparatorHeight;}
			set {_view.RowSeparatorHeight = value;}
		}

		[DefaultValue(false)]
		[Category("Appearance")]
		[DisplayName("Merge cells")]
		[Description("Whether neighboring cells with identical values can merge.")]
		public bool AllowCellMerge
		{
			get {return _view.OptionsView.AllowCellMerge;}
			set {_view.OptionsView.AllowCellMerge = value;}
		}

		[DefaultValue(false)]
		[Category("Appearance")]
		[DisplayName("Column auto width")]
		[Description("Whether the number of text lines within preview sections is calculated automatically depending on their contents.")]
		public bool ColumnAutoWidth
		{
			get {return _view.OptionsView.ColumnAutoWidth;}
			set {_view.OptionsView.ColumnAutoWidth = value;}
		}

		[DefaultValue(false)]
		[Category("Appearance")]
		[DisplayName("Row auto height")]
		[Description("Whether the height of each data row is automatically adjusted to completely display the contents of its cells.")]
		public bool RowAutoHeight
		{
			get {return _view.OptionsView.RowAutoHeight;}
			set {_view.OptionsView.RowAutoHeight = value;}
		}

		[DefaultValue(-1)]
		[Category("Appearance")]
		[DisplayName("Row height")]
		[Description("Height of cells within data rows. Set to -1 to use default value.")]
		public int RowHeight
		{
			get {return _view.RowHeight;}
			set {_view.RowHeight = value;}
		}

		[DefaultValue("")]
		[Category("Preview field")]
		[DisplayName("Field name")]
		[Description("Name of the field whose values are displayed within preview sections.")]
		[TypeConverter(typeof(PreviewFieldNameConverter))]
		[ParenthesizePropertyName(true)]
		public string PreviewFieldName
		{
			get {return _view.PreviewFieldName;}
			set
			{
				string previewField = _view.PreviewFieldName;
				_view.PreviewFieldName = value;
				ShowPreview = !string.IsNullOrEmpty(value);
				if (!string.IsNullOrEmpty(previewField))
				{
					GridColumn col = _view.Columns[previewField];
					if (col != null)
						col.Visible = !ShowPreview;
				}
				if (ShowPreview)
				{
					GridColumn col = _view.Columns[value];
					if (col != null)
						col.Visible = false;
				}
			}
		}

		[DefaultValue(false)]
		[Category("Preview field")]
		[DisplayName("Visible")]
		[Description("Whether preview sections are displayed.")]
		public bool ShowPreview
		{
			get {return _view.OptionsView.ShowPreview;}
			set {_view.OptionsView.ShowPreview = value;}
		}

		[DefaultValue(false)]
		[Category("Preview field")]
		[DisplayName("Auto line count")]
		[Description("Whether the number of text lines within preview sections is calculated automatically depending on their contents.")]
		public bool AutoCalcPreviewLineCount
		{
			get {return _view.OptionsView.AutoCalcPreviewLineCount;}
			set {_view.OptionsView.AutoCalcPreviewLineCount = value;}
		}

		[DefaultValue(-1)]
		[Category("Preview field")]
		[DisplayName("Line count")]
		[Description("Number of text lines within preview sections. Set -1 to use default.")]
		public int PreviewLineCount
		{
			get {return _view.PreviewLineCount;}
			set {_view.PreviewLineCount = value;}
		}

		/*
		[DefaultValue(GridMultiSelectMode.CellSelect)]
		[Category("Behavior")]
		[DisplayName("Multi-select mode")]
		[Description("Whether multiple cells or rows can be selected.")]
		public GridMultiSelectMode MultiSelectMode 
		{ 
			get {return _view.OptionsSelection.MultiSelectMode;}
			set {_view.OptionsSelection.MultiSelectMode = value;}
		}
		*/

		[DefaultValue(false)]
		[Category("Behavior")]
		[DisplayName("Split grid")]
		[Description("Whether grid is splitted. Master-detail relationship is not shown in splitted mode.")]
		public bool ShowSplitGrid
		{
			get {return IsGridSplitted(_view.GridControl);}
			set {SetGridSplitMode(_view.GridControl, value);}
		}

		[DefaultValue(FindPanelBehavior.Search)]
		[Category("Behavior")]
		[DisplayName("Find Behavior")]
		[Description("Whether records that do not match a query in the Find Panel are hidden.")]
		public FindPanelBehavior FindPanelBehavior
		{
			get {return _view.OptionsFind.Behavior;}
			set {_view.OptionsFind.Behavior = value;}
		}

		public static void SetGridSplitMode(GridControl grid, bool showSplitGrid)
		{
			if (showSplitGrid)
			{
				if (!grid.IsSplitGrid)
					grid.CreateSplitContainer();
				var container = Utils.FindTypedParentControl<GridSplitContainer>(grid);
				DataGridControl.RegisterSplitContainer(container);
				if (container != null)
					container.ShowSplitView();
			}
			else
			{
				var container = Utils.FindTypedParentControl<GridSplitContainer>(grid);
				if (container != null)
					container.HideSplitView();
				if (grid.IsSplitGrid)
				{
					DataGridControl.UnregisterSplitContainer(container);
					grid.RemoveSplitContainer();
				}
			}
		}

		public static bool IsGridSplitted(GridControl grid)
		{
			if (grid == null || !grid.IsSplitGrid)
				return false;

			GridSplitContainer container = Utils.FindTypedParentControl<GridSplitContainer>(grid);
			if (container == null)
				return false;

			return container.IsSplitViewVisible;
		}

		public void ApplyParameters(GridParametersObject parameters)
		{
			AllowIncrementalSearch			= parameters.AllowIncrementalSearch;
			FixedLineWidth					= parameters.FixedLineWidth;
			ShowFooter						= parameters.ShowFooter;
			ShowGroupPanel					= parameters.ShowGroupPanel;
			ShowAutoFilter					= parameters.ShowAutoFilter;
			AllowExpandEmptyDetail			= parameters.AllowExpandEmptyDetail;
			AllowOnlyOneMasterRowExpanded	= parameters.AllowOnlyOneMasterRowExpanded;
			AutoZoomDetail					= parameters.AutoZoomDetail;
			EnableAppearanceEvenRow			= parameters.EnableAppearanceEvenRow;
			EnableAppearanceOddRow			= parameters.EnableAppearanceOddRow;
			ShowHorzLines					= parameters.ShowHorzLines;
			ShowVertLines					= parameters.ShowVertLines;
			ShowIndicator					= parameters.ShowIndicator;
			RowSeparatorHeight				= parameters.RowSeparatorHeight;
			AllowCellMerge					= parameters.AllowCellMerge;
			ColumnAutoWidth					= parameters.ColumnAutoWidth;
			RowAutoHeight					= parameters.RowAutoHeight;
			RowHeight						= parameters.RowHeight;
			AutoCalcPreviewLineCount		= parameters.AutoCalcPreviewLineCount;
			//MultiSelectMode					= (GridMultiSelectMode)parameters.MultiSelectMode;
			ShowSplitGrid					= parameters.ShowSplitGrid;
			FindPanelBehavior               = parameters.FindPanelBehavior;
		}

		public void SaveParameters(GridParametersObject parameters)
		{
			parameters.AllowIncrementalSearch			= AllowIncrementalSearch;
			parameters.FixedLineWidth					= FixedLineWidth;
			parameters.ShowFooter						= ShowFooter;
			parameters.ShowGroupPanel					= ShowGroupPanel;
			parameters.ShowAutoFilter					= ShowAutoFilter;
			parameters.AllowExpandEmptyDetail			= AllowExpandEmptyDetail;
			parameters.AllowOnlyOneMasterRowExpanded	= AllowOnlyOneMasterRowExpanded;
			parameters.AutoZoomDetail					= AutoZoomDetail;
			parameters.EnableAppearanceEvenRow			= EnableAppearanceEvenRow;
			parameters.EnableAppearanceOddRow			= EnableAppearanceOddRow;
			parameters.ShowHorzLines					= ShowHorzLines;
			parameters.ShowVertLines					= ShowVertLines;
			parameters.ShowIndicator					= ShowIndicator;
			parameters.RowSeparatorHeight				= RowSeparatorHeight;
			parameters.AllowCellMerge					= AllowCellMerge;
			parameters.ColumnAutoWidth					= ColumnAutoWidth;
			parameters.RowAutoHeight					= RowAutoHeight;
			parameters.RowHeight						= RowHeight;
			parameters.AutoCalcPreviewLineCount		    = AutoCalcPreviewLineCount;
			//parameters.MultiSelectMode					= (GridParametersObject.GridMultiSelectMode)MultiSelectMode;
			parameters.ShowSplitGrid					= ShowSplitGrid;
			parameters.FindPanelBehavior                = FindPanelBehavior;
		}
	}
}