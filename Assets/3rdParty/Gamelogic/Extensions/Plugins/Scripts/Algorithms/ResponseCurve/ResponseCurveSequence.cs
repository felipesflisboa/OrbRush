// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;
using UnityEngine;

namespace Gamelogic.Extensions.Algorithms
{
	/// <summary>
	/// A response curve where the outputs are sequences of floats.
	/// </summary>
	[Version(1, 4)]
	public class ResponseCurveFloatSequence : ResponseCurveBase<IList<float>>
	{
		#region Constructors

		/// <summary>
		/// Constructs a new ResponseCurveFloatSequence with the given 
		/// input and output samples.
		/// </summary>
		/// <param name="inputSamples"></param>
		/// <param name="outputSamples">Each item in the output samples should have the same number
		/// of elements.</param>
		public ResponseCurveFloatSequence(
			IEnumerable<float> inputSamples, 
			IEnumerable<IList<float>> outputSamples) 
			: base(inputSamples, outputSamples)
		{
		}

		#endregion

		#region Protected Methods
		/// <summary>
		/// Interpolates two sequences of floats by interpolating corresponding pairs.
		/// </summary>
		/// <example>If the min sequence is (0, 1, 2) and the max sequence is (9, 7, 5), the interpolation
		/// at t = 0.1f is (.9f, 1.6f, 2.3f).
		/// </example>
		protected override IList<float> Lerp(IList<float> outputSampleMin, IList<float> outputSampleMax, float t)
		{
			if (outputSampleMin.Count != outputSampleMax.Count) throw new ArgumentException("The numbers of elements in outputSampleMin and outputSampleMax must match");

			var output = new List<float>();

			for (int i = 0; i < outputSampleMin.Count; i++)
			{
				output.Add(Mathf.Lerp(outputSampleMin[i], outputSampleMax[i], t));
			}

			return output;
		}

		#endregion
	}
}