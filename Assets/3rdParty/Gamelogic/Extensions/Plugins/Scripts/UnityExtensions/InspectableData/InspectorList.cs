using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Internal;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// The base class for the generic InspectorList. This class exists so that 
	/// a single property drawer can be use for all sub classes.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class InspectorList
	{}

	/// <summary>
	/// Exactly the same as generic <c>List</c>, but has a custom property drawer 
	/// that draws a re-orderable list in the inspector.
	/// </summary>
	/// <typeparam name="T">The type of the contents of this list.</typeparam>
	/// <remarks>This class should not be used directly (otherwise, it will not appear in the inspector).
	/// Instead, use either one of the provided sub classes, or a define a new custom non-generic subclass
	/// and use that.</remarks>
	[Version(2, 5)]
	[Serializable]
	public class InspectorList<T> : InspectorList, IList<T>
	{
		[SerializeField]
		private List<T> values;

		public InspectorList()
		{
			values = new List<T>();
		}

		public InspectorList(IEnumerable<T> initialValues)
		{
			values = initialValues.ToList();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)values).GetEnumerator();
		}

		public void Add(T item)
		{
			values.Add(item);
		}

		public void Clear()
		{
			values.Clear();
		}

		public bool Contains(T item)
		{
			return values.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			values.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return values.Remove(item);
		}

		public int Count
		{
			get { return values.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public int IndexOf(T item)
		{
			return values.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			values.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			values.RemoveAt(index);
		}

		public T this[int index]
		{
			get { return values[index]; }
			set { values[index] = value; }
		}
	}

	/// <summary>
	/// An <c>InspectorList</c> of type <c>int</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class IntList : InspectorList<int> { }

	/// <summary>
	/// An <c>InspectorList</c> of type <c>float</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class FloatList : InspectorList<float> { }

	/// <summary>
	/// An <c>InspectorList</c> of type <c>string</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class StringList : InspectorList<string> { }

	/// <summary>
	/// An <c>InspectorList</c> of type <c>Object</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class ObjectList : InspectorList<UnityEngine.Object> { }

	/// <summary>
	/// An <c>InspectorList</c> of type <c>MonoBehaviour</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class MonoBehaviourList : InspectorList<MonoBehaviour> { }

	/// <summary>
	/// An <c>InspectorList</c> of type <c>Color</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class ColorList : InspectorList<Color>
	{
		public ColorList() : base(Utils.DefaultColors)
		{ }

		public ColorList(IEnumerable<Color> defaultColors) : base(defaultColors)
		{ }
	}

	/// <summary>
	/// An <c>InspectorList</c> of type <c>Vector2</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class Vector2List : InspectorList<Vector2> { }

	/// <summary>
	/// An <c>InspectorList</c> of type <c>Vector3</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class Vector3List : InspectorList<Vector3> { }


	/// <summary>
	/// An <c>InspectorList</c> of type <c>Vector4</c>.
	/// </summary>
	[Version(2, 5)]
	[Serializable]
	public class Vector4List : InspectorList<Vector4> { }

}