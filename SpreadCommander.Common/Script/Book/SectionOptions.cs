using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class SectionOptions: BookOptions
    {
        [Description("Number of columns on a page. Must be greater than zero")]
        public int? ColumnCount { get; set; }

        [Description("Width of an individual column, measured in DocumentUnit")]
        public float[] ColumnWidths { get; set; }

        [Description("Spacing between adjacent columns, measured in DocumentUnit")]
        public float[] ColumnSpacing { get; set; }

        [Description("Whether the header and footer areas of the first page can be different from other pages in the section")]
        public bool? DifferentFirstPage { get; set; }

        [Description("Paper tray to use for the first page of a section")]
        public int? FirstPageTray { get; set; }

        [Description("Paper tray to use for all pages except the first page of a section")]
        public int? OtherPagesTray { get; set; }

        [Description("Line numbers in the left margin or to the left of each column in the document's Section")]
        public int? LineNumberingCountBy { get; set; }

        [Description("Distance between the line number and the start of the line")]
        public float? LineNumberingDistance { get; set; }

        [Description("When the line numbering should be reset to the line number specified by the LineNumberingStart value")]
        public LineNumberingRestart? LineNumberingRestartType { get; set; }

        [Description("Starting value used for the first line")]
        public int? LineNumberingStart { get; set; }

        [Description("Section's margins. 1 value if same for Left, Top, Right, Bottom; 4 values for Left, Top, Right, Bottom; 6 values for Left, Top, Right, Bottom, HeaderOffset, FooterOffset")]
        public float[] Margins { get; set; }

        [Description("Page's width")]
        public float? PageWidth { get; set; }

        [Description("Page's height")]
        public float? PageHeight { get; set; }

        [Description("Set if section's page shall be Landscape")]
        public bool? Landscape { get; set; }

        [Description("Page's paper size")]
        public PaperKind? PaperKind { get; set; }

        [Description("Set if page numbering should be continued from the previous section or should start from the beginning")]
        public bool? ContinuePageNumbering { get; set; }

        [Description("Initial number from which the numbering starts")]
        public int? FirstPageNumber { get; set; }

        [Description("Format used to display page numbers")]
        public NumberingFormat? PageNumberingFormat { get; set; }

        [Description("Set to change the section's direction to right-to-left")]
        public bool? RightToLeft { get; set; }

        [Description("Type of a section break")]
        public SectionStartType? StartType { get; set; }

        [Description("Establishes a link to the previous section's header so that they have the same content")]
        public bool LinkHeaderToPrevious { get; set; }

        [Description("Establishes a link to the previous section's footer so that they have the same content")]
        public bool LinkFooterToPrevious { get; set; }
    }

    public partial class SCBook
    {
        protected static void SetupSection(Section section, SectionOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            if (options.DifferentFirstPage.HasValue)
                section.DifferentFirstPage = options.DifferentFirstPage.Value;

            if (options.FirstPageTray.HasValue)
                section.FirstPageTray = options.FirstPageTray.Value;
            if (options.OtherPagesTray.HasValue)
                section.OtherPagesTray = options.OtherPagesTray.Value;

            if (options.LineNumberingCountBy.HasValue)
                section.LineNumbering.CountBy = options.LineNumberingCountBy.Value;
            if (options.LineNumberingDistance.HasValue)
                section.LineNumbering.Distance = options.LineNumberingDistance.Value;
            if (options.LineNumberingRestartType.HasValue)
                section.LineNumbering.RestartType = (DevExpress.XtraRichEdit.API.Native.LineNumberingRestart)options.LineNumberingRestartType.Value;
            if (options.LineNumberingStart.HasValue)
                section.LineNumbering.Start = options.LineNumberingStart.Value;

            switch (options.Margins?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    section.Margins.Left = section.Margins.Top = section.Margins.Right = section.Margins.Bottom = options.Margins[0];
                    break;
                case 4:
                    section.Margins.Left   = options.Margins[0];
                    section.Margins.Top    = options.Margins[1];
                    section.Margins.Right  = options.Margins[2];
                    section.Margins.Bottom = options.Margins[3];
                    break;
                case 6:
                    section.Margins.Left         = options.Margins[0];
                    section.Margins.Top          = options.Margins[1];
                    section.Margins.Right        = options.Margins[2];
                    section.Margins.Bottom       = options.Margins[3];
                    section.Margins.HeaderOffset = options.Margins[4];
                    section.Margins.FooterOffset = options.Margins[5];
                    break;
                default:
                    throw new Exception("Invalid count of values in Margins.");
            }

            if (options.PaperKind.HasValue)
                section.Page.PaperKind = options.PaperKind.Value;
            if (options.PageWidth.HasValue)
                section.Page.Width = options.PageWidth.Value;
            if (options.PageHeight.HasValue)
                section.Page.Height = options.PageHeight.Value;
            if (options.Landscape.HasValue)
                section.Page.Landscape = options.Landscape.Value;

            if (options.ContinuePageNumbering.HasValue)
                section.PageNumbering.ContinueNumbering = options.ContinuePageNumbering.Value;
            if (options.FirstPageNumber.HasValue)
                section.PageNumbering.FirstPageNumber = options.FirstPageNumber.Value;
            if (options.PageNumberingFormat.HasValue)
                section.PageNumbering.NumberingFormat = (DevExpress.XtraRichEdit.API.Native.NumberingFormat)options.PageNumberingFormat.Value;

            if (options.RightToLeft.HasValue)
                section.RightToLeft = options.RightToLeft.Value;

            if (options.StartType.HasValue)
                section.StartType = (DevExpress.XtraRichEdit.API.Native.SectionStartType)options.StartType.Value;

            if ((options.ColumnWidths?.Length ?? 0) > 0)
            {
                var columns = section.Columns.CreateUniformColumns(section.Page, (options.ColumnSpacing?.Length ?? 0) > 0 ? options.ColumnSpacing[0] : 0.25f, options.ColumnWidths.Length);
                for (int i = 0; i < options.ColumnWidths.Length; i++)
                {
                    columns[i].Width = options.ColumnWidths[i];
                    if ((options.ColumnSpacing?.Length ?? 0) > i)
                        columns[i].Spacing = options.ColumnSpacing[i];
                }
                section.Columns.SetColumns(columns);
            }
            else if (options.ColumnCount.HasValue)
            {
                var columns = section.Columns.CreateUniformColumns(section.Page, (options.ColumnSpacing?.Length ?? 0) > 0 ? options.ColumnSpacing[0] : 0.25f, options.ColumnCount.Value);
                if (options.ColumnSpacing != null)
                {
                    for (int i = 0; i < Math.Min(options.ColumnCount.Value, options.ColumnSpacing.Length); i++)
                        columns[i].Spacing = options.ColumnSpacing[i];
                }
                section.Columns.SetColumns(columns);
            }

            if (options.LinkHeaderToPrevious)
                section.LinkHeaderToPrevious();
            if (options.LinkFooterToPrevious)
                section.LinkFooterToPrevious();
        }
    }
}
