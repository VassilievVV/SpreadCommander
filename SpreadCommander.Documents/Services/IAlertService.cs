using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
	public interface IAlertService
	{
		void Show(string caption, string text, string hotTrackedText = null, Image image = null);
	}
}
