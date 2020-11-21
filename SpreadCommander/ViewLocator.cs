using DevExpress.Utils.MVVM.UI;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Documents.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander
{
    public class ViewLocator : IViewLocator
    {
        public static readonly ViewLocator DefaultLocator = new ViewLocator();

        public object Resolve(string name, params object[] parameters)
        {
            return name switch
            {
                BookDocumentViewModel.ViewName        => new BookDocumentView(),
                SpreadsheetDocumentViewModel.ViewName => new SpreadsheetDocumentView(),
                SqlScriptDocumentViewModel.ViewName   => new SqlScriptDocumentView(),
                ConsoleDocumentViewModel.ViewName     => new ConsoleDocumentView(),
                ChartDocumentViewModel.ViewName       => new ChartDocumentView(),
                PivotDocumentViewModel.ViewName       => new PivotDocumentView(),
                DashboardDocumentViewModel.ViewName   => new DashboardDocumentView(),
                ViewerDocumentViewModel.ViewName      => new ViewerDocumentView(),
                PdfDocumentViewModel.ViewName         => new PdfDocumentView(),
                PictureDocumentViewModel.ViewName     => new PictureDocumentView(),
                _                                     => throw new ArgumentException($"Cannot find view: {name}", nameof(name))
            };
        }
    }
}
