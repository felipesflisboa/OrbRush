// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that generates integers given an arbitrary distribution.
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class FrequencyIntGenerator : IIntGenerator
	{
		#region Private Fields

		private readonly float[] buckets;
		private readonly int[] indices0;
		private readonly int[] indices1;
		private readonly IRandom random;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new FrequencyIntGenerator object. The given elements and frequencies
		/// together describe a piecewise linear distribution. 
		/// </summary>
		/// <param name="relativeFrequencies">The (relative) frequency to generate integers at. The size of this
		///	sequence determines which frequencies are being generated. If the size is n, then integers from 0 
		/// to n - 1 are generated.</param>
		public FrequencyIntGenerator(IEnumerable<float> relativeFrequencies):
			this(relativeFrequencies, GLRandom.GlobalRandom)
		{}

		/// <summary>
		/// Constructs a new FrequencyIntGenerator object. The given elements and frequencies
		/// together describe a piecewise linear distribution. 
		/// </summary>
		/// <param name="relativeFrequencies">The (relative) frequency to generate integers at. The size of this
		///	sequence determines which frequencies are being generated. If the size is n, then integers from 0 
		/// to n - 1 are generated.</param>
		/// <param name="random">The random generator to use.</param>
		public FrequencyIntGenerator(IEnumerable<float> relativeFrequencies, IRandom random)
		{
			float[] frequencies = relativeFrequencies as float[] ?? relativeFrequencies.ToArray();

			if (frequencies.Length == 0)
			{
				throw new ArgumentException("Array cannot be empty");
			}

			if (frequencies.Length == 1)
			{
				if (frequencies[0] <= 0)
				{
					throw new ArgumentException("Sum of frequencies cannot be 0");
				}

				buckets = new[]{1f}; 
				indices0 = new[]{0};
				indices1 = new[]{0};
			}

			float sum = frequencies.Sum();

			if (sum <= 0)
			{
				throw new ArgumentException("Sum of frequencies cannot be 0");
			}

			if (frequencies.Any(x => x < 0))
			{
				throw new Exception("Frequencies must be non-negative");
			}

			float[] absoluteProbabilities = frequencies.Select(x => x/sum*frequencies.Length).ToArray();

			buckets = new float[absoluteProbabilities.Length];
			indices0 = Enumerable.Range(0, absoluteProbabilities.Length).ToArray();
			indices0 = indices0.OrderBy(i => absoluteProbabilities[i]).ToArray();

			int leftIndex = 0;
			int rightIndex = absoluteProbabilities.Length - 1;

			indices1 = new int[indices0.Length];

			while (leftIndex <= rightIndex)
			{	
				buckets[leftIndex] = absoluteProbabilities[indices0[leftIndex]];

				absoluteProbabilities[indices0[leftIndex]] = 0;
				absoluteProbabilities[indices0[rightIndex]] -= (1 - buckets[leftIndex]); 

				indices1[leftIndex] = indices0[rightIndex];

				leftIndex++;
				indices0 = indices0
					.Take(leftIndex)
					.Concat(
						indices0.Skip(leftIndex).OrderBy(i => absoluteProbabilities[i]))
					.ToArray();
			
			}

			this.random = random;
		}

		#endregion

		#region Public Methods

		public int Next()
		{
			if (buckets == null)
			{
				return 0;
			}

			float r = (float)random.NextDouble()*buckets.Length;

			int i = (int)Math.Floor(r);
			float x = r - i;

			return x < buckets[i] ? indices0[i] : indices1[i];
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