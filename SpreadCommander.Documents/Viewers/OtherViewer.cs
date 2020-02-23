using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SpreadCommander.Documents.Viewers
{
	public partial class OtherViewer : BaseViewer
	{
		public OtherViewer()
		{
			InitializeComponent();
		}

		public OtherViewer(string messageText): base()
		{
			MessageText = messageText;
		}

		public override bool SupportDataSource => false;
		public override bool SupportZoom       => false;
		public override bool SupportFind       => false;
		public override bool SupportPrint      => false;

		public string MessageText
		{
			get => lblMessage.Text;
			set => lblMessage.Text = value;
		}
	}
}
