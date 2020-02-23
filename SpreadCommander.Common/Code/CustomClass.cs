#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CRR0046
#pragma warning disable CRR0047
#pragma warning disable CRR0050

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace SpreadCommander.Common.Code
{
	public class CustomPropertyDescriptor: PropertyDescriptor
	{
		private Type    _ComponentType  = null;
#pragma warning disable IDE0052 // Remove unread private members
		private string	_Name			= string.Empty;
#pragma warning restore IDE0052 // Remove unread private members
		private string	_Category		= string.Empty;
		private string	_Description	= string.Empty;
		private string	_DisplayName	= string.Empty;
		private Type	_ValueType		= null;
		private object	_Value			= null;
		private bool	_ReadOnly		= false;
		private bool	_Visible		= true;

		public CustomPropertyDescriptor(Type componentType, string name, bool readOnly, bool visible, 
			string category, string description, string displayName,
			Type valueType, object value, Attribute[] attrs):
			base(name, attrs)
		{
			_ComponentType  = componentType;
			_Name			= name;
			_Category		= category;
			_Description	= description;
			_DisplayName	= displayName;
			_ValueType		= valueType;
			_Value			= value;
			_ReadOnly		= readOnly;
			_Visible		= visible;
		}

		public CustomPropertyDescriptor(Type componentType, PropertyDescriptor baseDescriptor, object value):
			base(baseDescriptor)
		{
			_ComponentType  = componentType;
			_Name			= baseDescriptor.Name;
			_Category		= baseDescriptor.Category;
			_Description	= baseDescriptor.Description;
			_DisplayName	= baseDescriptor.DisplayName;
			_ValueType		= baseDescriptor.PropertyType;
			_Value			= value;
			_ReadOnly		= baseDescriptor.IsReadOnly;
			_Visible		= baseDescriptor.IsBrowsable;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override Type ComponentType
		{
			get {return _ComponentType;}
		}

		public override object GetValue(object component)
		{
			return _Value;
		}

		public override string Category
		{
			get {return _Category;}
		}

		public override string Description
		{
			get {return _Description;}
		}

		public override string DisplayName
		{
			get {return _DisplayName;}
		}

		public override bool IsBrowsable
		{
			get {return _Visible;}
		}

		public override bool IsReadOnly
		{
			get {return _ReadOnly;}
		}

		public override void ResetValue(object component)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		public override void SetValue(object component, object value)
		{
			_Value = value;
		}

		public override Type PropertyType
		{
			get {return _ValueType;}
		}

		public object Value
		{
			get {return _Value;}
			set {_Value = value;}
		}
	}

	public class CustomClass: List<CustomPropertyDescriptor>, ICustomTypeDescriptor
	{
		private string _ClassName;
		
		public CustomClass(string className)
		{
			_ClassName = className;
		}
	
		public virtual void Remove(string Name)
		{
			foreach (CustomPropertyDescriptor prop in this)
			{
				if (prop.Name == Name)
				{
					base.Remove(prop);
					return;
				}
			}
		}

		public virtual AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		public virtual string GetClassName()
		{
			if (_ClassName != null)
				return _ClassName;
			else
				return TypeDescriptor.GetClassName(this, true);
		}

		public virtual string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		public virtual TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		public virtual EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		public virtual PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		public virtual object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public virtual EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public virtual EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return new PropertyDescriptorCollection(ToArray());
		}

		public virtual PropertyDescriptorCollection GetProperties()
		{
			return TypeDescriptor.GetProperties(this, true);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}
	}
}