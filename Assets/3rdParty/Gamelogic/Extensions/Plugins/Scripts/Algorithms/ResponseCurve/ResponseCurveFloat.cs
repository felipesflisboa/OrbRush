// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System.Collections.Generic;
using Gamelogic.Extensions.Internal;
using UnityEngine;

namespace Gamelogic.Extensions.Algorithms
{
	/// <summary>
	/// A response curve with outputs of float.
	/// </summary>
	[Version(1, 2)]
	public class ResponseCurveFloat : ResponseCurveBase<float>
	{
		#region Static Methods

		public static ResponseCurveFloat GetLerp(float x0, float x1, float y0, float y1)
		{
			var input = new List<float>{x0, x1};
			var output = new List<float>{y0, y1};
			
			var responseCurve = new ResponseCurveFloat(input, output);

			return responseCurve;
		}

		#endregion

		#region Constructors

		public ResponseCurveFloat(IEnumerable<float> inputSamples, IEnumerable<float> outputSamples)
			: base(inputSamples, outputSamples)
		{}

		#endregion

		#region Protected Methods

		protected override float Lerp(float outputSampleMin, float outputSampleMax, float t)
		{
			return outputSampleMin + (outputSampleMax - outputSampleMin) * Mathf.Clamp01(t);
		}

		#endregion
	}

	/// <summary>
	/// A response curve with outputs of float.
	/// </summary>
	[Version(1, 2)]
	public class ResponseCurveInt : ResponseCurveBase<int>
	{
		#region Static Methods

		public static ResponseCurveInt GetLerp(float x0, float x1, int y0, int y1)
		{
			var input = new List<float> { x0, x1 };
			var output = new List<int> { y0, y1 };

			var responseCurve = new ResponseCurveInt(input, output);

			return responseCurve;
		}

		#endregion

		#region Constructors

		public ResponseCurveInt(IEnumerable<float> inputSamples, IEnumerable<int> outputSamples)
			: base(inputSamples, outputSamples)
		{ }

		#endregion

		#region Protected Methods

		protected override int Lerp(int outputSampleMin, int outputSampleMax, float t)
		{
			return Mathf.RoundToInt(outputSampleMin + (outputSampleMax - outputSampleMin) * Mathf.Clamp01(t));
		}

		#endregion
	}

}
