// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System.Linq;

namespace Gamelogic.Extensions.Algorithms
{
	/// <summary>
	/// Dithers responses by adding noise before passing it to a step response.
	/// The noise is provided by an arbitrary generator, and errors are diffused
	/// over several calls.
	/// </summary>
	public class DitherResponse 
	{
		private IResponseCurve<float> quantizer;
		private IGenerator<float> noiseGenerator;
		private int frontIndex;
		private float[] errorBuffer;
		private float[] errorFactors;

		/// <summary>
		/// Gives the quantized value of the given input.
		/// </summary>
		/// <param name="input"></param>
		public float this[float input]
		{
			get
			{
				float error = errorBuffer[frontIndex];
				float newT = input + noiseGenerator.Next() + error;

				float newValue = quantizer[newT];
				float newError = input + error - newValue;
				errorBuffer[frontIndex] = 0;
				frontIndex++;

				if (frontIndex >= errorBuffer.Length)
				{
					frontIndex = 0;
				}

				for (int i = 0; i < errorBuffer.Length; i++)
				{
					errorBuffer[(frontIndex + i) % errorBuffer.Length] += errorFactors[i] * newError;
				}

				return newValue;
			}
		}

		/// <summary>
		/// Constructs a new DitherResponse.
		/// </summary>
		/// <param name="quantizer">The response used to quantize values, such as an instance of StepResponse.</param>
		/// <param name="noiseGenerator">A generator that provides noise. For satisfactory results, the 
		/// mean should be 0.</param>
		/// <param name="errorFactors">An array of factors used to diffuse the error over several calls. For example,
		/// if the error factors are [0.6, 0.3, 0.1], then 60% of the error is given added to the next sample, 
		/// 30% to the sample after that, and 10% to the sample after that.</param>
		public DitherResponse(
			IResponseCurve<float> quantizer, 
			IGenerator<float> noiseGenerator,
			float[] errorFactors)
		{
			this.quantizer = quantizer;
			this.noiseGenerator = noiseGenerator;
			this.errorFactors = errorFactors;

			errorBuffer = new float[errorFactors.Length];

			for(int i = 0; i < errorBuffer.Length; i++)
			{
				errorBuffer[i] = 0;
			}

			frontIndex = 0;
		}

		/// <summary>
		/// Dithers responses by adding noise before passing it to a step response.
		/// If the quality of the noise is 1, the noise is uniform noise.
		/// If the quality of the noise is 2, the noise is triangular noise.
		/// The higher the quality, the closer the noise follow a random distribution.
		/// </summary>
		/// <param name="quantizer">A response curve that converts inputs to discrete outputs.</param>
		/// <param name="quality">The quality.</param>
		/// <param name="noiseScale">The noise scale.</param>
		public DitherResponse(IResponseCurve<float> quantizer, int quality, float noiseScale, float[] errorFactors)
			:this(
				 quantizer,
				 Generator
					.UniformRandomFloat()
					.Group(quality)
					.Select(group => (group.Average() - 0.5f) * noiseScale),
				 errorFactors
				 )
		{
		}
	}
}
