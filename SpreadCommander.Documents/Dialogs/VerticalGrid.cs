//Based on http://cs.rthand.com/blogs/blog_with_righthand/archive/2007/08/24/Make-XtraVerticalGrid-fast-as-a-bullet.aspx

#pragma warning disable CRR0047

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevExpress.XtraVerticalGrid;

namespace SpreadCommander.Documents.Dialogs
{
	public class VerticalGrid: VGridControl
	{
		private int lockUpdateCount;

		public override void BeginUpdate()
		{
			lockUpdateCount++;
			base.BeginUpdate();
		}

		public override void EndUpdate()
		{
			lockUpdateCount--;
			base.EndUpdate();
		}

		public override void CancelUpdate()
		{
			lockUpdateCount--;
			base.CancelUpdate();
		}

		public override void InvalidateRecord(int recordIndex)
		{
			if (lockUpdateCount <= 0)
				base.InvalidateRecord(recordIndex);
		}
	}

	public class PropertyGridControl: DevExpress.XtraVerticalGrid.PropertyGridControl
	{
		private int lockUpdateCount;

		public override void BeginUpdate()
		{
			lockUpdateCount++;
			base.BeginUpdate();
		}

		public override void EndUpdate()
		{
			lockUpdateCount--;
			base.EndUpdate();
		}

		public override void CancelUpdate()
		{
			lockUpdateCount--;
			base.CancelUpdate();
		}

		public override void InvalidateRecord(int recordIndex)
		{
			if (lockUpdateCount <= 0)
				base.InvalidateRecord(recordIndex);
		}
	}
}