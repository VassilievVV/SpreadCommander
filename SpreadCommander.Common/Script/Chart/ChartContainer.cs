using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    internal class ChartContainer : SpreadCommander.Common.Code.ChartContainer
    {
        protected override void ProcessBoundDataChanged()
        {
            if (Chart.Tag is not SCChart chart)
                return;

            foreach (DevExpress.XtraCharts.Series series in Chart.Series)
            {
                if (series.Tag is Series.SeriesOptions options)
                    options.BoundDataChanged(chart, series);
            }

            foreach (Annotation annotation in Chart.AnnotationRepository)
            {
                if (annotation.Tag is AnnotationOptions options)
                    options.BoundDataChanged(chart, annotation);
            }
        }
    }
}
