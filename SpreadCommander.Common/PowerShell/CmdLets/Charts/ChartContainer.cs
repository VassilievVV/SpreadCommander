using DevExpress.Charts.Native;
using DevExpress.Data.Browsing;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Commands;
using DevExpress.Utils.Filtering.Internal;
using DevExpress.Utils.KeyboardHandler;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Commands;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    internal class ChartContainer : SpreadCommander.Common.Code.ChartContainer
    {
        protected override void ProcessBoundDataChanged()
        {
            if (Chart.Tag is not ChartContext chartContext)
                return;

            foreach (Series series in Chart.Series)
            {
                if (series.Tag is BaseSeriesContext seriesContext)
                    seriesContext.BoundDataChanged(chartContext, series);
            }

            foreach (Annotation annotation in Chart.AnnotationRepository)
            {
                if (annotation.Tag is AddChartAnnotationCmdlet annotationCmdlet)
                    annotationCmdlet.BoundDataChanged(chartContext, annotation);
            }
        }
    }
}
