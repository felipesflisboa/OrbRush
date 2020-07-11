// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that generates floating values between 0 and 1 with a uniform distribution.
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class UniformFloatGenerator : IGenerator<float>
	{
		#region Private Fields

		private readonly IRandom random;

		#endregion

		#region Constructors

		public UniformFloatGenerator():
			this(GLRandom.GlobalRandom)
		{}

		public UniformFloatGenerator(IRandom random)
		{
			this.random = random;
		}

		#endregion

		#region Public Methods

		public float Next()
		{
			return (float) random.NextDouble();
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