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
    public class ChartContainer : IChartContainer, IRangeControlClientExtension, IChartDataProvider, IChartRenderProvider
    {
        private RangeControlClient rangeControlClient;
        private int _LoadingCounter;

        public Chart Chart													{ get; private set; }

        IRangeControlClientExtension RangeControlClient					   => rangeControlClient;
        public IChartDataProvider DataProvider                             => this;
        public IChartRenderProvider RenderProvider                         => this;
        public IChartEventsProvider EventsProvider                         => null;
        public IChartInteractionProvider InteractionProvider               => null;
        public ChartContainerType ControlType                              => ChartContainerType.XRControl;
        public bool DesignMode                                             => false;
        public bool Loading                                                => (_LoadingCounter > 0);
        public bool ShowDesignerHints                                      => false;
        public bool IsDesignControl                                        => false;
        public bool IsEndUserDesigner                                      => false;
        public bool ShouldEnableFormsSkins                                 => false;
        public IServiceProvider ServiceProvider                            => null;
        public IComponent Parent                                           => null;
        public CommandBasedKeyboardHandler<ChartCommandId> KeyboardHandler => null;
        public ISite Site { get; set; }

        event EventHandler UpdateUI;
        public event BoundDataChangedEventHandler BoundDataChanged;

        public ChartContainer()
        {
        }

        public void BeginLoad()
        {
            _LoadingCounter++;
        }

        public void EndLoad()
        {
            _LoadingCounter--;
        }

        protected virtual void RaiseBoundDataChanged(object sender, EventArgs e)
        {
            BoundDataChanged?.Invoke(sender, e);
        }

        #region IChartContainer
        event EventHandler IChartContainer.EndLoading { add { } remove { } }
        bool IChartContainer.GetActualRightToLeft() { return false; }
        bool IChartContainer.CanDisposeItems { get { return true; } }
        void IChartContainer.AfterLoadLayout() { }

        void IChartContainer.Invoke(Action action)
        {
            action();
        }

        IEndUserFilteringViewModelProvider IChartContainer.GetFilteringViewModelProvider(SeriesFilterHelper filterHelper)
        {
            return null;
        }

        void IChartContainer.ShowErrorMessage(string message, string title)
        {
            throw new NotImplementedException();
        }

        Point IChartContainer.PointToScreen(Point point)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICommandAwareControl
        //event EventHandler ICommandAwareControl<ChartCommandId>.BeforeDispose { add { } remove { } }
        //event EventHandler ICommandAwareControl<ChartCommandId>.UpdateUI { add { UpdateUI += value; } remove { UpdateUI -= value; } }
        #endregion
        
        #region IRangeControlClient
        event ClientRangeChangedEventHandler IRangeControlClient.RangeChanged
        {
            add
            {
                RangeControlClient.RangeChanged += value;
            }
            remove
            {
                RangeControlClient.RangeChanged -= value;
            }
        }

        bool IRangeControlClient.IsValidType(Type type)
        {
            return RangeControlClient.IsValidType(type);
        }

        bool IRangeControlClient.IsValid                => RangeControlClient.IsValid;
        string IRangeControlClient.InvalidText          => RangeControlClient.InvalidText;
        int IRangeControlClient.RangeBoxTopIndent       => RangeControlClient.RangeBoxTopIndent;
        int IRangeControlClient.RangeBoxBottomIndent    => RangeControlClient.RangeBoxBottomIndent;
        bool IRangeControlClient.IsCustomRuler          => RangeControlClient.IsCustomRuler;
        object IRangeControlClient.RulerDelta           => RangeControlClient.RulerDelta;
        double IRangeControlClient.NormalizedRulerDelta => RangeControlClient.NormalizedRulerDelta;

        bool IRangeControlClient.SupportOrientation(RangeControlClientOrientation orientation) =>
            RangeControlClient.SupportOrientation(orientation);

        object IRangeControlClient.GetValue(double normalizedValue) =>
            RangeControlClient.GetValue(normalizedValue);

        double IRangeControlClient.GetNormalizedValue(object value) =>
            RangeControlClient.GetNormalizedValue(value);

        string IRangeControlClient.RulerToString(int ruleIndex) =>
            RangeControlClient.RulerToString(ruleIndex);

        List<object> IRangeControlClient.GetRuler(RulerInfoArgs e) =>
            RangeControlClient.GetRuler(e);

        void IRangeControlClient.ValidateRange(NormalizedRangeInfo info) =>
            RangeControlClient.ValidateRange(info);
        void IRangeControlClient.OnRangeChanged(object rangeMinimum, object rangeMaximum) =>
            RangeControlClient.OnRangeChanged(rangeMinimum, rangeMaximum);

        void IRangeControlClient.DrawContent(RangeControlPaintEventArgs e) =>
            RangeControlClient.DrawContent(e);

        bool IRangeControlClient.DrawRuler(RangeControlPaintEventArgs e) =>
            RangeControlClient.DrawRuler(e);

        object IRangeControlClient.GetOptions() =>
            RangeControlClient.GetOptions();

        void IRangeControlClient.UpdateHotInfo(RangeControlHitInfo hitInfo) =>
            RangeControlClient.UpdateHotInfo(hitInfo);

        void IRangeControlClient.UpdatePressedInfo(RangeControlHitInfo hitInfo) =>
            RangeControlClient.UpdatePressedInfo(hitInfo);

        void IRangeControlClient.OnClick(RangeControlHitInfo hitInfo) =>
            RangeControlClient.OnClick(hitInfo);

        void IRangeControlClient.OnRangeControlChanged(IRangeControl rangeControl) =>
            RangeControlClient.OnRangeControlChanged(rangeControl);

        void IRangeControlClient.OnResize() =>
            RangeControlClient.OnResize();

        void IRangeControlClient.Calculate(Rectangle contentRect) =>
            RangeControlClient.Calculate(contentRect);

        double IRangeControlClient.ValidateScale(double newScale) =>
            RangeControlClient.ValidateScale(newScale);

        string IRangeControlClient.ValueToString(double normalizedValue) =>
            RangeControlClient.ValueToString(normalizedValue);

        Rectangle IRangeControlClient.CalculateSelectionBounds(RangeControlPaintEventArgs e, Rectangle rect) =>
            RangeControlClient.CalculateSelectionBounds(e, rect);

        void IRangeControlClient.DrawSelection(RangeControlPaintEventArgs e) =>
            RangeControlClient.DrawSelection(e);
        #endregion

        #region IRangeControlClientExtension
        object IRangeControlClientExtension.NativeValue(double normalizedValue) =>
            RangeControlClient.NativeValue(normalizedValue);
        #endregion

        public void Assign(Chart chart)
        {
            if (this.Chart == null)
            {
                this.Chart = chart;
                this.rangeControlClient = new RangeControlClient(chart);
            }
            else
                this.Chart.Assign(chart);
        }

        public void Changing()
        {
        }

        public void Changed()
        {
        }

        public void LockChangeService()
        {
        }

        public void UnlockChangeService()
        {
        }

        public void RaiseRangeControlRangeChanged(object minValue, object maxValue, bool invalidate)
        {
            rangeControlClient.RaiseRangeControlRangeChanged(minValue, maxValue, invalidate);
        }

        public void RaiseUIUpdated()
        {
            UpdateUI?.Invoke(this, new EventArgs());
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public object GetService(Type serviceType) => null;
#pragma warning restore IDE0060 // Remove unused parameter

        public void CommitImeContent()
        {
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public Command CreateCommand(ChartCommandId id) => null;
#pragma warning restore IDE0060 // Remove unused parameter

        public void Focus()
        {
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public bool HandleException(Exception e) => false;
#pragma warning restore IDE0060 // Remove unused parameter

        #region IChartDataProvider 
        object IChartDataProvider.ParentDataSource      => null;
        DataContext IChartDataProvider.DataContext      => null;
        bool IChartDataProvider.CanUseBoundPoints       => true;
        bool IChartDataProvider.SeriesDataSourceVisible => true;

        bool IChartDataProvider.ShouldSerializeDataSource(object dataSource)
        {
            return false;
        }

        void IChartDataProvider.OnBoundDataChanged(EventArgs e)
        {
            ProcessBoundDataChanged();
            RaiseBoundDataChanged(this, e);
        }

        void IChartDataProvider.OnPivotGridSeriesExcluded(PivotGridSeriesExcludedEventArgs e)
        {
        }

        void IChartDataProvider.OnPivotGridSeriesPointsExcluded(PivotGridSeriesPointsExcludedEventArgs e)
        {
        }

        #endregion

        #region IChartRenderProvider 
        object IChartRenderProvider.LookAndFeel   => UserLookAndFeel.Default; 

        public object DataAdapter { get => null; set { } }
        public object DataSource { get => null; set { } }

        public Rectangle DisplayBounds => new Rectangle(0, 0, 3840, 2160);

        public bool IsPrintingAvailable => false;

        public SizeF DpiScaleFactor { get; set; } = new SizeF(1f, 1f);

        public IBasePrintable Printable => throw new NotImplementedException();

        void IChartRenderProvider.Invalidate()
        {
        }

        void IChartRenderProvider.InvokeInvalidate()
        {
        }

        Bitmap IChartRenderProvider.LoadBitmap(string url) => null;

        ComponentExporter IChartRenderProvider.CreateComponentPrinter(IBasePrintable iPrintable) => null;
        #endregion


        protected virtual void ProcessBoundDataChanged()
        {
            if (!(Chart.Tag is ChartContext chartContext))
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
