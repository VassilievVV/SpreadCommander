using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
	public partial class ObjectListEditor: BaseForm
	{
		public ObjectListEditor(List<object> objects, int selectedIndex, Image image)
		{
			InitializeComponent();

			if (image != null)
				Images.AddImage(image);

			using (new UsingProcessor(() => listControls.Items.BeginUpdate(), () => listControls.Items.EndUpdate()))
			{
				foreach (object obj in objects)
					listControls.Items.Add(obj, image != null ? 0 : -1);
			}

			if (listControls.Items.Count > 0 && selectedIndex < listControls.Items.Count)
				listControls.SelectedIndex = selectedIndex;
		}

		private void ListControls_SelectedIndexChanged(object sender, EventArgs e)
		{
			PropertyGrid.SelectedObject = null;

			if (ShowProperties && listControls.SelectedIndex >= 0)
				PropertyGrid.SelectedObject = listControls.Items[listControls.SelectedIndex].Value;
		}

		public bool ShowProperties
		{
			get {return layoutProperties.Visibility == LayoutVisibility.Always;}
			set {layoutProperties.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;}
		}

		public int SelectedIndex
		{
			get {return listControls.SelectedIndex;}
			set {listControls.SelectedIndex = (value >= 0 && value < listControls.Items.Count) ? value : -1;}
		}

		public object SelectedObject
		{
			get
			{
				int selectedIndex = SelectedIndex;
				if (selectedIndex >= 0 && selectedIndex < listControls.Items.Count)
					return listControls.Items[selectedIndex].Value;
				return null;
			}
		}
	}
}