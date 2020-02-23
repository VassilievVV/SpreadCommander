using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Management.Automation;

namespace SpreadCommander.Common.Code
{
	//TODO: Expressions, evaluator - see SqlSpirit.Plugins.Grid.DataViewExpressionEvaluator
	//https://docs.devexpress.com/WindowsForms/6212/common-features/expressions/expression-editor
	//https://www.devexpress.com/Support/Center/Question/Details/T479782/expressioneditor-as-a-standalone-control

	public class TypedList : List<object>, ITypedList
	{
		public Type ItemType { get; set; }

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			var itemType = GetItemType();

			PropertyDescriptorCollection pdc;

			if (listAccessors != null && listAccessors.Length > 0)
			{
				// Return child list shape.
				pdc = ListBindingHelper.GetListItemProperties(listAccessors[0].PropertyType);
			}
			else
			{
				// Return properties in sort order.
				pdc = ListBindingHelper.GetListItemProperties(itemType);
			}

			return pdc;
		}

		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			return "List";
		}

		protected Type GetItemType()
		{
			var itemType = ItemType;
			if (itemType != null)
				return itemType;

			for (int i = 0; i < Count; i++)
			{
				var obj = this[i];
				if (obj != null)
					itemType = obj.GetType();
			}

			//If ItemType is not specified - use common base type of all objects in list.
			if (Count > 0 && itemType != null)
			{
				while (itemType != null)
				{
					if (IsTypeCommon(itemType))
						break;
					itemType = itemType.BaseType;
					if (itemType == null || itemType == typeof(object))
						break;
				}
			}

			if (itemType == null)
				itemType = typeof(object);

			return itemType;
			

			bool IsTypeCommon(Type type)
			{
				for (int i = 0; i < Count; i++)
				{
					var obj = this[i];

					if (obj != null && !type.IsInstanceOfType(obj))
						return false;
				}
				return true;
			}
		}
	}
}
