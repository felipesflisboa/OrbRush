// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using Gamelogic.Extensions.Algorithms;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that generates floats given an arbitrary distribution.
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class FrequencyFloatGenerator : IGenerator<float>
	{
		#region Private Fields

		private readonly ResponseCurveFloat responseCurve;
		private readonly UniformFloatGenerator floatGenerator;
		private readonly float accumulativeSum;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new FrequencyFloatGenerator object. The given elements and frequencies
		/// together describe a piecewise linear distribution. 
		/// </summary>
		/// <param name="elements">Samples of elements to generate.</param>
		/// <param name="frequencies">The (relative) frequency to generate the sample at.</param>
		/// <param name="random">The random generator to use.</param>
		public FrequencyFloatGenerator(IEnumerable<float> elements, IEnumerable<float> frequencies, IRandom random)
		{
			var accumulativeProbability = new List<float>();

			accumulativeSum = 0f;

			foreach (var frequency in frequencies)
			{
				accumulativeSum += frequency;
				accumulativeProbability.Add(accumulativeSum);
			}

			responseCurve = new ResponseCurveFloat(accumulativeProbability, elements);
			floatGenerator = new UniformFloatGenerator(random);
		}

		/// <summary>
		/// Constructs a new FrequencyFloatGenerator object. The given elements and frequencies
		/// together describe a piecewise linear distribution. 
		/// </summary>
		/// <param name="elements">Samples of elements to generate.</param>
		/// <param name="frequencies">The (relative) frequency to generate the sample at.</param>
		public FrequencyFloatGenerator(IEnumerable<float> elements, IEnumerable<float> frequencies):
			this(elements, frequencies, GLRandom.GlobalRandom)
		{
		}

		#endregion

		#region Public Methods

		public float Next()
		{
			return responseCurve[floatGenerator.Next()*accumulativeSum];
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