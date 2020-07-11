// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// The base class of the generic Optional class.
	/// </summary>
	/// <remarks>This is an empty class; the reason it exists is so that a single property drawer can be used 
	/// for all classes that derive from the generic Optional class.</remarks>
	public class Optional{ }

	/// <summary>
	/// Useful for displaying optional values in the inspector. 
	/// </summary>
	/// <typeparam name="T">The type of the optional value.</typeparam>
	/// <remarks>For this class to be displayable in the inspector you cannot use it directly. You have to use one of the provided
	/// subclasses (or derive your own).</remarks>
	[Serializable]
	public class Optional<T> : Optional
	{
		[Tooltip("Check this to set a value for this instance")]
		[SerializeField]
		private bool useValue;

		[Tooltip("The value of this instance")]
		[SerializeField]
		private T value;

		/// <summary>
		/// Gets or sets whether to use the value of this instance.
		/// </summary>
		/// <value><c>true</c> if this value should be used; otherwise, <c>false</c>.</value>
		public bool UseValue
		{
			get { return useValue; }
			set { useValue = value; }
		}

		/// <summary>
		/// The value of this instance. It should only be used if UseValue is true. Otherwise, some
		/// other value should be used, or code that does not need it must be executed instead.
		/// </summary>
		/// <value>The value of this Optional instance.</value>
		/// <example>This shows a typical example of how to use this class.
		/// <code>
		/// if (optionalMaterial.UseValue)
		/// {
		///		renderer.material = material;
		/// } //else do not modify the material.
		/// </code></example>
		public T Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public override string ToString()
		{
			if(UseValue)
			{
				return Value.ToString();
			}
			else
			{
				return "[No Value]";
			}
		}
	}

	/// <summary>
	/// Represents an optional int value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Int32}" />
	[Serializable] public class OptionalInt : Optional<int>{ }

	/// <summary>
	/// Represents an optional float value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Single}" />
	[Serializable] public class OptionalFloat : Optional<float>{ }

	/// <summary>
	/// Represents an optional string value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{String}" />
	[Serializable] public class OptionalString : Optional<string>{ }

	/// <summary>
	/// Represents an optional GameObject.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{GameObject}" />
	[Serializable] public class OptionalGameObject : Optional<GameObject>{ }

	/// <summary>
	/// Represents an optional Vector2 value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Vector2}" />
	[Serializable] public class OptionalVector2 : Optional<Vector2> { }

	/// <summary>
	/// Represents an optional Vector3 value.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{Vector3}" />
	[Serializable] public class OptionalVector3 : Optional<Vector3> { }

	/// <summary>
	/// Represents an optional MonoBehaviour.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Optional{MonoBehaviour}" />
	[Serializable] public class OptionalMonoBehaviour : Optional<MonoBehaviour> { }
}