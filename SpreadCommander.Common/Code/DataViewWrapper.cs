using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace SpreadCommander.Common.Code
{
	public class DataViewWrapper: Component, IBindingListView, IBindingList, IList,
		ICollection, IEnumerable, ITypedList, ISupportInitializeNotification, ISupportInitialize
	{
		private readonly DataView _view;

		private DataViewWrapper()
		{
		}

		public DataViewWrapper(DataView view)
		{
			_view = view ?? throw new ArgumentNullException(nameof(view), "View can not be null.");
			_view.Initialized += new EventHandler(View_Initialized);
		}

		private void View_Initialized(object sender, EventArgs e)
		{
			Initialized?.Invoke(this, e);
		}

		[Browsable(false)]
		public DataView DataView
		{
			get {return _view;}
		}

		[Browsable(false)]
		public DataTable Table
		{
			get {return _view?.Table;}
		}

		public override ISite Site
		{
			get {return base.Site;}
			set 
			{
				base.Site = value;
				if (base.Site != null)
				{
					try
					{
						base.Site.Name = "DataSource";
					}
					catch (Exception)
					{
					}
				}
			}
		}


		#region IBindingListView Members
		void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
		{
			((IBindingListView)_view).ApplySort(sorts);
		}

		string IBindingListView.Filter
		{
			get {return ((IBindingListView)_view).Filter;}
			set {((IBindingListView)_view).Filter = value;}
		}

		void IBindingListView.RemoveFilter()
		{
			((IBindingListView)_view).RemoveFilter();
		}

		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get {return ((IBindingListView)_view).SortDescriptions;}
		}

		bool IBindingListView.SupportsAdvancedSorting
		{
			get {return ((IBindingListView)_view).SupportsAdvancedSorting;}
		}

		bool IBindingListView.SupportsFiltering
		{
			get {return ((IBindingListView)_view).SupportsFiltering;}
		}
		#endregion


		#region IBindingList Members
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			((IBindingList)_view).AddIndex(property);
		}

		object IBindingList.AddNew()
		{
			return ((IBindingList)_view).AddNew();
		}

		bool IBindingList.AllowEdit
		{
			get {return ((IBindingList)_view).AllowEdit;}
		}

		bool IBindingList.AllowNew
		{
			get {return ((IBindingList)_view).AllowNew;}
		}

		bool IBindingList.AllowRemove
		{
			get {return ((IBindingList)_view).AllowRemove;}
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			((IBindingList)_view).ApplySort(property, direction);
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			return ((IBindingList)_view).Find(property, key);
		}

		bool IBindingList.IsSorted
		{
			get {return ((IBindingList)_view).IsSorted;}
		}

		event ListChangedEventHandler IBindingList.ListChanged
		{
			add {((IBindingList)_view).ListChanged += value;}
			remove {((IBindingList)_view).ListChanged -= value;}
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			((IBindingList)_view).RemoveIndex(property);
		}

		void IBindingList.RemoveSort()
		{
			((IBindingList)_view).RemoveSort();
		}

		ListSortDirection IBindingList.SortDirection
		{
			get {return ((IBindingList)_view).SortDirection;}
		}

		PropertyDescriptor IBindingList.SortProperty
		{
			get {return ((IBindingList)_view).SortProperty;}
		}

		bool IBindingList.SupportsChangeNotification
		{
			get {return ((IBindingList)_view).SupportsChangeNotification;}
		}

		bool IBindingList.SupportsSearching
		{
			get {return ((IBindingList)_view).SupportsSorting;}
		}

		bool IBindingList.SupportsSorting
		{
			get {return ((IBindingList)_view).SupportsSorting;}
		}
		#endregion


		#region IList Members
		int IList.Add(object value)
		{
			return ((IList)_view).Add(value);
		}

		void IList.Clear()
		{
			((IList)_view).Clear();
		}

		bool IList.Contains(object value)
		{
			return ((IList)_view).Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return ((IList)_view).IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			((IList)_view).Insert(index, value);
		}

		bool IList.IsFixedSize
		{
			get {return ((IList)_view).IsFixedSize;}
		}

		bool IList.IsReadOnly
		{
			get {return ((IList)_view).IsReadOnly;}
		}

		void IList.Remove(object value)
		{
			((IList)_view).Remove(value);
		}

		void IList.RemoveAt(int index)
		{
			((IList)_view).RemoveAt(index);
		}

		object IList.this[int index]
		{
			get {return ((IList)_view)[index];}
			set {((IList)_view)[index] = value;}
		}
		#endregion


		#region ICollection Members
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)_view).CopyTo(array, index);
		}

		int ICollection.Count
		{
			get {return ((ICollection)_view).Count;}
		}

		bool ICollection.IsSynchronized
		{
			get {return ((ICollection)_view).IsSynchronized;}
		}

		object ICollection.SyncRoot
		{
			get {return ((ICollection)_view).SyncRoot;}
		}
		#endregion


		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_view).GetEnumerator();
		}
		#endregion


		#region ITypedList Members
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return ((ITypedList)_view).GetItemProperties(listAccessors);
		}

		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return ((ITypedList)_view).GetListName(listAccessors);
		}
		#endregion


		#region ISupportInitializeNotification Members
		public event EventHandler Initialized;

		[Browsable(false)]
		public bool IsInitialized
		{
			get {return ((ISupportInitializeNotification)_view).IsInitialized;}
		}
		#endregion


		#region ISupportInitialize Members
		public void BeginInit()
		{
			((ISupportInitialize)_view).BeginInit();
		}

		public void EndInit()
		{
			((ISupportInitialize)_view).EndInit();
		}
		#endregion
	}
}