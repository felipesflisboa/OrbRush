// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// Generates items at the same frequencies as they
	/// occur in a set from which this generator is constructed.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class FrequencyElementGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly ListSelectorGenerator<T> elementGenerator;

		#endregion

		#region Constructors

		public FrequencyElementGenerator(IEnumerable<T> elements):
			this(elements, GLRandom.GlobalRandom)
		{}

		public FrequencyElementGenerator(IEnumerable<T> elements, IRandom random)
		{
			elements.ThrowIfNull("elements");

			var counts = new Dictionary<T, float>();

			foreach (var element in elements)
			{
				if (counts.ContainsKey(element))
				{
					counts[element]++;
				}
				else
				{
					counts[element] = 1;
				}
			}

			var indexGenerator = new FrequencyIntGenerator(counts.Values, random);
			elementGenerator = new ListSelectorGenerator<T>(counts.Keys, indexGenerator);
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			return elementGenerator.Next();
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
