using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
	public class ApplicationService: IApplicationService
	{
		public void DoEvents()
		{
			Application.DoEvents();
		}
	}
}
