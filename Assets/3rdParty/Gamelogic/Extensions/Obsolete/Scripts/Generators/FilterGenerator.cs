// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that only generates elements that passes the predicate.
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class FilterGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly IGenerator<T> generator;
		private readonly Func<T, bool> predicate;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new generator that only returns elements that passes the predicate.
		/// </summary>
		public FilterGenerator(IGenerator<T> generator, Func<T, bool> predicate)
		{
			this.generator = generator;
			this.predicate = predicate;
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			T nextPossibleElement = generator.Next();

			while (!predicate(nextPossibleElement))
			{
				nextPossibleElement = generator.Next();
			}

			return nextPossibleElement;
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