using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Console
{
	public interface IRibbonHolder
	{
		RibbonControl Ribbon			{ get; }
		RibbonStatusBar RibbonStatusBar { get; }
		bool IsRibbonVisible			{ get; set; }
	}
}
