// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// Generates elements chosen randomly (with uniform distribution).
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class RandomElementGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly ListSelectorGenerator<T> generator;

		#endregion

		#region Constructors

		public RandomElementGenerator(IEnumerable<T> list):
			this(list, GLRandom.GlobalRandom)
		{}

		public RandomElementGenerator(IEnumerable<T> list, IRandom random) 
		{
			list.ThrowIfNull("list");

			generator = new ListSelectorGenerator<T>(list, new UniformIntGenerator(list.Count(), random));
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			return generator.Next();
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
