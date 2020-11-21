using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace SpreadCommander.Common.Code
{
	public class ListTypeConverter: TypeConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
		{
			if (destType == typeof(string) && value != null && value is IList list)
				return $"{list.Count} item(s)";
			return base.ConvertTo(context, culture, value, destType);
		}
	}
}