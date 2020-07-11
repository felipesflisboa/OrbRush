// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that repeats a given sequence.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class RepeatSequenceGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly IEnumerator<T> enumerator;

		#endregion

		#region Constructors

		/// <summary>
		/// Construts a new RepeateSequenceGenerator.
		/// </summary>
		/// <param name="sequence">The sequence that this sequence will repeat.</param>
		public RepeatSequenceGenerator(IEnumerable<T> sequence)
		{
			enumerator = sequence.GetEnumerator();
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			bool hasNext = enumerator.MoveNext();
			var current = enumerator.Current;

			if (!hasNext)
			{
				enumerator.Reset();
			}

			return current;
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