using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HGS.Tools
{
	[Serializable]
	public abstract class HGSReorderableArray<T> : ICloneable, IList<T>, ICollection<T>, IEnumerable<T>
	{

		[SerializeField]
		private List<T> array = new List<T>();

		public HGSReorderableArray()
			: this(0)
		{
		}

		public HGSReorderableArray(int length)
		{

			array = new List<T>(length);
		}

		public T this[int index]
		{

			get { return array[index]; }
			set { array[index] = value; }
		}

		public int Length
		{

			get { return array.Count; }
		}

		public bool IsReadOnly
		{

			get { return false; }
		}

		public int Count
		{

			get { return array.Count; }
		}

		public object Clone()
		{

			return new List<T>(array);
		}

		public void CopyFrom(IEnumerable<T> value)
		{

			array.Clear();
			array.AddRange(value);
		}

		public bool Contains(T value)
		{

			return array.Contains(value);
		}

		public int IndexOf(T value)
		{

			return array.IndexOf(value);
		}

		public void Insert(int index, T item)
		{

			array.Insert(index, item);
		}

		public void RemoveAt(int index)
		{

			array.RemoveAt(index);
		}

		public void Add(T item)
		{

			array.Add(item);
		}

		public void Clear()
		{

			array.Clear();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{

			this.array.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{

			return array.Remove(item);
		}

		public T[] ToArray()
		{

			return array.ToArray();
		}

		public IEnumerator<T> GetEnumerator()
		{

			return array.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{

			return array.GetEnumerator();
		}
	}
}