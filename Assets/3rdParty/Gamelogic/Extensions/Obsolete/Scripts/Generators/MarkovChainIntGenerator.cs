// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// Generates elements with frequencies that are different for each element, 
	/// and also depends on the previously generated elements. 
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class MarkovChain2IntGenerator : IIntGenerator
	{
		#region Private Fields

		private int lastSymbol;
		private readonly FrequencyIntGenerator[] generators;

		#endregion

		#region Constructors

		/// <summary>Constructs a new MarkovChain2IntGenerator
		/// </summary>
		/// <param name="frequencies">The conditional frequencies for the elements to generate,
		/// where frequencies[m][n] is the relative prob of generating n given m was generated 
		/// the last time </param>
		public MarkovChain2IntGenerator(float[][] frequencies) :
			this(frequencies, GLRandom.GlobalRandom)
		{
		}

		/// <summary>Constructs a new MarkovChain2IntGenerator
		/// </summary>
		/// <param name="frequencies">The conditional frequencies for the elements to generate,
		/// where frequencies[m][n] is the relative prob of generating n given m was generated 
		/// the last time </param>
		/// <param name="random">The random generator to use.</param>
		public MarkovChain2IntGenerator(float[][] frequencies, IRandom random)
		{
			int symbolCount = frequencies.Length;

			var initialFrequencies = new float[symbolCount];
			generators = new FrequencyIntGenerator[symbolCount];

			for (int i = 0; i < symbolCount; i++)
			{
				for (int j = 0; j < symbolCount; j++)
				{
					initialFrequencies[j] += frequencies[i][j];
				}

				generators[i] = new FrequencyIntGenerator(frequencies[i], random);
			}

			var initialGenerator = new FrequencyIntGenerator(initialFrequencies, random);
			lastSymbol = initialGenerator.Next();
		}

		#endregion

		#region Public Methods

		public int Next()
		{
			var nextSymbol = generators[lastSymbol].Next();
			lastSymbol = nextSymbol;
			return nextSymbol;
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