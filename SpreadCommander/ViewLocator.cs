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
        public static ViewLocator DefaultLocator = new ViewLocator();

        public object Resolve(string name, params object[] parameters)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (name)
            {
                case BookDocumentViewModel.ViewName:
                    return new BookDocumentView();
                case SpreadsheetDocumentViewModel.ViewName:
                    return new SpreadsheetDocumentView();
                case SqlScriptDocumentViewModel.ViewName:
                    return new SqlScriptDocumentView();
                case ConsoleDocumentViewModel.ViewName:
                    return new ConsoleDocumentView();
                case ChartDocumentViewModel.ViewName:
                    return new ChartDocumentView();
                case PivotDocumentViewModel.ViewName:
                    return new PivotDocumentView();
                case DashboardDocumentViewModel.ViewName:
                    return new DashboardDocumentView();
                case ViewerDocumentViewModel.ViewName:
                    return new ViewerDocumentView();
                case PdfDocumentViewModel.ViewName:
                    return new PdfDocumentView();
                case PictureDocumentViewModel.ViewName:
                    return new PictureDocumentView();
            }

            throw new ArgumentException($"Cannot find view: {name}", nameof(name));
        }
#pragma warning restore IDE0066 // Convert switch statement to expression
    }
}
