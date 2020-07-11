// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Internal;
using UnityEngine;

namespace Gamelogic.Extensions.Algorithms
{
	/// <summary>
	/// This interface represents a piecewise linear curve, with input-output 
	/// pairs at the bends. Outputs can be any type for which 
	/// continuous interpolation makes sense. 
	/// </summary>
	/// <typeparam name="T">The number type of the input and output, usually 
	/// float or double, but anything that can be interpolated (such as vectors and colors) is possible.</typeparam>
	/// <remarks>
	/// <para>This class is the base of the that described in AI Programming 
	/// Wisdom 1, "The Beauty of Response Curves", by Bob Alexander.</para>
	/// <para>The inputs need not be spread uniformly.</para>
	/// </remarks>
	public interface IResponseCurve<T> // Where T is something that can be interpolated
	{
		#region  Properties

		/// <summary>
		/// Evaluates the curve at the given input and returns the result.
		/// </summary>
		/// <param name="input">The input for which output is sought.</param>
		/// <remarks>
		/// <para>
		/// If the input is below the inputMin given in the constructor, 
		/// the output is clamped to the first output sample.
		/// </para>
		/// <para>
		/// If the input is above the inputMax given in the constructor,
		/// the output is clamped to the last output sample.
		/// </para>
		/// <para>
		/// Otherwise an index is calculated, and the output is interpolated
		/// between outputSample[index] and outputSample[index + 1].
		/// </para>
		/// </remarks>
		T this[float input]
		{
			get;
		}

		#endregion

		#region Public methods
		/// <inheritdoc cref="this[float]"/>
		T Evaluate(float input);
		#endregion
	}

	/// <summary>
	/// A class that can be used as the base of the implementation of a response curve.
	/// </summary>
	[Version(1, 2)]
	public abstract class ResponseCurveBase<T>: IResponseCurve<T> // Where T is something that can be interpolated
	{
		#region Private Fields

		private readonly List<T> outputSamples;
		private readonly List<float> inputSamples;

		#endregion

		#region  Properties
		/// <inheritdoc/>
		public T this[float input]
		{
			get
			{
				return Evaluate(input);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new ResponseCurveBase.
		/// </summary>
		/// <param name="inputSamples">Samples of input. Assumes input is monotonically increasing.</param>
		/// <param name="outputSamples">Samples of outputs.</param>
		public ResponseCurveBase(IEnumerable<float> inputSamples, IEnumerable<T> outputSamples)
		{
			var minCount = Mathf.Min(inputSamples.Count(), outputSamples.Count());

			if (minCount < 2)
			{
				throw new ArgumentException("There must be at least two samples");
			}

			if(!IsStrictlyMonotonic(inputSamples))
			{
				throw new ArgumentException("Input samples must be strictly monotonic");
			}

			this.outputSamples = new List<T>();
			this.outputSamples.AddRange(outputSamples);

			this.inputSamples = new List<float>();
			this.inputSamples.AddRange(inputSamples);
		}

		#endregion

		#region Protected Methods
		/// <summary>
		/// Linearly interpolates between the two given samples.
		/// </summary>
		/// <param name="outputSampleMin">The value when t is less than or equal to 0.</param>
		/// <param name="outputSampleMax">The value when t is greater than or equal to 1.</param>
		/// <param name="t">The fraction of the minimum sample to use.</param>
		protected abstract T Lerp(T outputSampleMin, T outputSampleMax, float t);

		#endregion

		#region Public methods		
		/// <summary>
		/// Evaluates the curve at the specified value.
		/// </summary>
		/// <param name="t">The value at which to evaluate the curve.</param>
		/// <remarks>Equivalent to <c>curve[t]</c>.</remarks>
		public T Evaluate(float t)
		{
			int index = SearchInput(t);

			float inputSampleMin = inputSamples[index];
			float inputSampleMax = inputSamples[index + 1];

			T outputSampleMin = outputSamples[index];
			T outputSampleMax = outputSamples[index + 1];

			return Lerp(t, inputSampleMin, inputSampleMax, outputSampleMin, outputSampleMax);
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Returns the biggest index i such that <c>mInput[i] &amp;= fInputValue</c>.
		/// </summary>
		private int SearchInput(float input)
		{
			if (input< inputSamples[0])
			{
				return 0;
			}

			if (input > inputSamples[inputSamples.Count - 2])
			{
				return inputSamples.Count - 2; //return the but-last node
			}

			return SearchInput(input, 0, inputSamples.Count - 2);
		}

		private int SearchInput(float input, int start, int end)
		{
			while (true)
			{
				if (end - start <= 1)
				{
					return start;
				}

				int mid = (end - start)/2 + start;
				float midValue = inputSamples[mid];

				if (input.CompareTo(midValue) > 0)
				{
					start = mid;
				}
				else
				{
					end = mid;
				}
			}
		}

		private T Lerp(float input, float inputSampleMin, float inputSampleMax, T outputSampleMin,
			T outputSampleMax)
		{
			if (input <= inputSampleMin)
			{
				return outputSampleMin;
			}

			if (input >= inputSampleMax)
			{
				return outputSampleMax;
			}

			float t = (input - inputSampleMin) / (inputSampleMax - inputSampleMin);

			var output = Lerp(outputSampleMin, outputSampleMax, t);

			return output;
		}

		private bool IsStrictlyMonotonic(IEnumerable<float> list)
		{
			float previous = list.First();

			foreach(float item in list.Skip(1))
			{
				if (previous >= item)
				{
					return false;
				}

				previous = item;
			}

			return true;
		}
		#endregion
	}
}
