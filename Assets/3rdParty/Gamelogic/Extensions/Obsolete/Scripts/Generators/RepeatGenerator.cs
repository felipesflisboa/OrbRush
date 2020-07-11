// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that generates the same element each time.
	/// This is useful in situations where a generator is expected,
	/// but a constant is desired (for example, when constructing 
	/// compound generators).
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class RepeatGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		/// <summary>
		/// The element that this generator will repeat.
		/// </summary>
		private readonly T item;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new generator that repeats the given element.
		/// </summary>
		/// <param name="item">The element to repeat.</param>
		public RepeatGenerator(T item)
		{
			this.item = item;
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			return item;
		}

		#endregion

		#region Private Methods

		object IGenerator.Next()
		{
			return Next();
		}

		#endregion
	}
}