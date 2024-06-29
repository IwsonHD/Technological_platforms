using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class SortableSearchableBindingList<T> : BindingList<T>
{
	private bool isSorted;
	private PropertyDescriptor sortProperty;
	private ListSortDirection sortDirection;

	public SortableSearchableBindingList() : base() { }

	public SortableSearchableBindingList(IList<T> list) : base(list) { }

	protected override bool SupportsSortingCore => true;

	protected override ListSortDirection SortDirectionCore => sortDirection;

	protected override PropertyDescriptor SortPropertyCore => sortProperty;

	protected override bool IsSortedCore => isSorted;

	protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
	{
		if (prop.PropertyType.GetInterface(nameof(IComparable)) != null)
		{
			var items = Items as List<T>;
			if (items != null)
			{
				PropertyComparer<T> comparer = new PropertyComparer<T>(prop, direction);
				items.Sort(comparer);
				isSorted = true;
				sortProperty = prop;
				sortDirection = direction;
			}
			else
			{
				isSorted = false;
			}
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
		else
		{
			throw new NotSupportedException($"Cannot sort by {prop.Name}. This {prop.PropertyType.Name} does not implement IComparable.");
		}
	}

	protected override void RemoveSortCore()
	{
		isSorted = false;
		sortProperty = null;
	}

	protected override bool SupportsSearchingCore => true;

	protected override int FindCore(PropertyDescriptor prop, object key)
	{
		if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(int))
		{
			var items = Items as List<T>;
			if (items != null)
			{
				for (int i = 0; i < items.Count; i++)
				{
					var item = items[i];
					var value = prop.GetValue(item);
					if (value != null && value.Equals(key))
					{
						return i;
					}
				}
			}
		}
		return -1;
	}

	public int Find(PropertyDescriptor prop, object key)
	{
		return FindCore(prop, key);
	}

	private class PropertyComparer<TItem> : IComparer<TItem>
	{
		private readonly PropertyDescriptor property;
		private readonly ListSortDirection direction;

		public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
		{
			this.property = property;
			this.direction = direction;
		}

		public int Compare(TItem x, TItem y)
		{
			object xValue = property.GetValue(x);
			object yValue = property.GetValue(y);

			int result;

			if (xValue == null)
			{
				result = (yValue == null) ? 0 : -1;
			}
			else if (yValue == null)
			{
				result = 1;
			}
			else if (xValue is IComparable xComparable && yValue is IComparable yComparable)
			{
				result = xComparable.CompareTo(yComparable);
			}
			else
			{
				throw new ArgumentException("Cannot compare values that do not implement IComparable.");
			}

			return direction == ListSortDirection.Ascending ? result : -result;
		}
	}
}
