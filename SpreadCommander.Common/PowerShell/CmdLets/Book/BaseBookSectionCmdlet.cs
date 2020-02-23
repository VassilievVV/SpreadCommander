using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    public class BaseBookSectionCmdlet: BaseBookCmdlet
    {
        [Parameter(HelpMessage = "Number of columns on a page. Must be greater than zero")]
        public int? ColumnCount { get; set; }

        [Parameter(HelpMessage = "Width of an individual column, measured in DocumentUnit")]
        public float[] ColumnWidths { get; set; }

        [Parameter(HelpMessage = "Spacing between adjacent columns, measured in DocumentUnit")]
        public float[] ColumnSpacing { get; set; }

        [Parameter(HelpMessage = "Whether the header and footer areas of the first page can be different from other pages in the section")]
        public bool? DifferentFirstPage { get; set; }

        [Parameter(HelpMessage = "Paper tray to use for the first page of a section")]
        public int? FirstPageTray { get; set; }

        [Parameter(HelpMessage = "Paper tray to use for all pages except the first page of a section")]
        public int? OtherPagesTray { get; set; }

        [Parameter(HelpMessage = "Line numbers in the left margin or to the left of each column in the document's Section")]
        public int? LineNumberingCountBy { get; set; }

        [Parameter(HelpMessage = "Distance between the line number and the start of the line")]
        public float? LineNumberingDistance { get; set; }

        [Parameter(HelpMessage = "When the line numbering should be reset to the line number specified by the LineNumberingStart value")]
        public LineNumberingRestart? LineNumberingRestartType { get; set; }

        [Parameter(HelpMessage = "Starting value used for the first line")]
        public int? LineNumberingStart { get; set; }

        [Parameter(HelpMessage = "Section's margins. 1 value if same for Left, Top, Right, Bottom; 4 values for Left, Top, Right, Bottom; 6 values for Left, Top, Right, Bottom, HeaderOffset, FooterOffset")]
        public float[] Margins { get; set; }

        [Parameter(HelpMessage = "Page's width")]
        public float? PageWidth { get; set; }

        [Parameter(HelpMessage = "Page's height")]
        public float? PageHeight { get; set; }

        [Parameter(HelpMessage = "Set if section's page shall be Landscape")]
        public SwitchParameter Landscape { get; set; }

        [Parameter(HelpMessage = "Set if section's page shall be Portrait")]
        public SwitchParameter Portrait { get; set; }

        [Parameter(HelpMessage = "Page's paper size")]
        public PaperKind? PaperKind { get; set; }

        [Parameter(HelpMessage = "Set if page numbering should be continued from the previous section or should start from the beginning")]
        public SwitchParameter ContinuePageNumbering { get; set; }

        [Parameter(HelpMessage = "Set if page numbering should be restart")]
        public SwitchParameter RestartPageNumbering { get; set; }

        [Parameter(HelpMessage = "Initial number from which the numbering starts")]
        public int? FirstPageNumber { get; set; }

        [Parameter(HelpMessage = "Format used to display page numbers")]
        public NumberingFormat? PageNumberingFormat { get; set; }

        [Parameter(HelpMessage = "Set to change the section's direction to right-to-left")]
        public SwitchParameter RightToLeft { get; set; }

        [Parameter(HelpMessage = "Set to change the section's direction to left-to-right")]
        public SwitchParameter LeftToRight { get; set; }

        [Parameter(HelpMessage = "Type of a section break")]
        public SectionStartType? StartType { get; set; }

        [Parameter(HelpMessage = "Establishes a link to the previous section's header so that they have the same content")]
        public SwitchParameter LinkHeaderToPrevious { get; set; }

        [Parameter(HelpMessage = "Establishes a link to the previous section's footer so that they have the same content")]
        public SwitchParameter LinkFooterToPrevious { get; set; }


        protected virtual void SetupSection(Section section)
        {
            if (DifferentFirstPage.HasValue)
                section.DifferentFirstPage = DifferentFirstPage.Value;

            if (FirstPageTray.HasValue)
                section.FirstPageTray = FirstPageTray.Value;
            if (OtherPagesTray.HasValue)
                section.OtherPagesTray = OtherPagesTray.Value;

            if (LineNumberingCountBy.HasValue)
                section.LineNumbering.CountBy = LineNumberingCountBy.Value;
            if (LineNumberingDistance.HasValue)
                section.LineNumbering.Distance = LineNumberingDistance.Value;
            if (LineNumberingRestartType.HasValue)
                section.LineNumbering.RestartType = LineNumberingRestartType.Value;
            if (LineNumberingStart.HasValue)
                section.LineNumbering.Start = LineNumberingStart.Value;

            switch (Margins?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    section.Margins.Left = section.Margins.Top = section.Margins.Right = section.Margins.Bottom = Margins[0];
                    break;
                case 4:
                    section.Margins.Left   = Margins[0];
                    section.Margins.Top    = Margins[1];
                    section.Margins.Right  = Margins[2];
                    section.Margins.Bottom = Margins[3];
                    break;
                case 6:
                    section.Margins.Left         = Margins[0];
                    section.Margins.Top          = Margins[1];
                    section.Margins.Right        = Margins[2];
                    section.Margins.Bottom       = Margins[3];
                    section.Margins.HeaderOffset = Margins[4];
                    section.Margins.FooterOffset = Margins[5];
                    break;
                default:
                    throw new Exception("Invalid count of values in Margins.");
            }

            if (PaperKind.HasValue)
                section.Page.PaperKind = PaperKind.Value;
            if (PageWidth.HasValue)
                section.Page.Width = PageWidth.Value;
            if (PageHeight.HasValue)
                section.Page.Height = PageHeight.Value;
            if (Landscape)
                section.Page.Landscape = true;
            if (Portrait)
                section.Page.Landscape = false;

            if (ContinuePageNumbering)
                section.PageNumbering.ContinueNumbering = true;
            if (RestartPageNumbering)
                section.PageNumbering.ContinueNumbering = false;
            if (FirstPageNumber.HasValue)
                section.PageNumbering.FirstPageNumber = FirstPageNumber.Value;
            if (PageNumberingFormat.HasValue)
                section.PageNumbering.NumberingFormat = PageNumberingFormat.Value;

            if (RightToLeft)
                section.RightToLeft = true;
            if (LeftToRight)
                section.RightToLeft = false;

            if (StartType.HasValue)
                section.StartType = StartType.Value;

            if ((ColumnWidths?.Length ?? 0) > 0)
            {
                var columns = section.Columns.CreateUniformColumns(section.Page, (ColumnSpacing?.Length ?? 0) > 0 ? ColumnSpacing[0] : 0.25f, ColumnWidths.Length);
                for (int i = 0; i < ColumnWidths.Length; i++)
                {
                    columns[i].Width = ColumnWidths[i];
                    if ((ColumnSpacing?.Length ?? 0) > i)
                        columns[i].Spacing = ColumnSpacing[i];
                }
                section.Columns.SetColumns(columns);
            }
            else if (ColumnCount.HasValue)
            {
                var columns = section.Columns.CreateUniformColumns(section.Page, (ColumnSpacing?.Length ?? 0) > 0 ? ColumnSpacing[0] : 0.25f, ColumnCount.Value);
                if (ColumnSpacing != null)
                {
                    for (int i = 0; i < Math.Min(ColumnCount.Value, ColumnSpacing.Length); i++)
                        columns[i].Spacing = ColumnSpacing[i];
                }
                section.Columns.SetColumns(columns);
            }

            if (LinkHeaderToPrevious)
                section.LinkHeaderToPrevious();
            if (LinkFooterToPrevious)
                section.LinkFooterToPrevious();
        }
    }
}