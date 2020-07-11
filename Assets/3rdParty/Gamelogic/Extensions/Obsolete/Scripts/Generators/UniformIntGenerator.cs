// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that generates int values with a uniform distribution.
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class UniformIntGenerator : IIntGenerator
	{
		#region Private Fields

		private readonly int min;
		private readonly int max;
		private readonly IRandom random;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new generator that generates integers in a specified range randomly.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public UniformIntGenerator(int min, int max):
			this(min, max, GLRandom.GlobalRandom)
		{}

		/// <summary>
		/// Creates a new generator that generates integers in a specified range randomly.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="random"></param>
		public UniformIntGenerator(int min, int max, IRandom random)
		{
			this.min = min;
			this.max = max;
			this.random = random;
		}

		/// <summary>
		/// Creates a new generator that generates integers between 0 and the specified maximum randomly.
		/// </summary>
		/// <param name="max"></param>
		public UniformIntGenerator(int max):
			this(max, GLRandom.GlobalRandom)
		{}

		/// <summary>
		/// Creates a new generator that generates integers between 0 and the specified maximum randomly.
		/// </summary>
		/// <param name="max"></param>
		/// <param name="random">The random generator to use.</param>
		public UniformIntGenerator(int max, IRandom random) :
			this(0, max, random)
		{}

		#endregion

		#region Public Methods

		public int Next()
		{
			return random.Next(min, max);
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
