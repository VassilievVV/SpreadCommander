using DevExpress.XtraBars.Alerter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
	public class AlertService : IAlertService
	{
		private readonly Form _Owner;
		private readonly AlertControl _Alert = new AlertControl();

		public AlertService(Form owner)
		{
			_Owner = owner;
		}

		public void Show(string caption, string text, string hotTrackedText = null, Image image = null)
		{
			_Alert.Show(_Owner, caption, text, hotTrackedText, image);
		}
	}
}
