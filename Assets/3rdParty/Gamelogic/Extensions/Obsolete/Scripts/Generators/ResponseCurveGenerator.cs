// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Algorithms;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that uses a response curve to generate elements.
	/// </summary>
	/// <typeparam name="T">The type of element to generate. The response 
	/// curve must be of the same type.</typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class ResponseCurveGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly ResponseCurveBase<T> responseCurve;
		private readonly IGenerator<float> floatGenerator;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new ResponseCurveGenerator with the given 
		/// response curve.
		/// </summary>
		/// <param name="responseCurve"></param>
		public ResponseCurveGenerator(ResponseCurveBase<T> responseCurve):
			this(responseCurve, GLRandom.GlobalRandom)
		{}

		/// <summary>
		/// Creates a new ResponseCurveGenerator with the given 
		/// response curve.
		/// </summary>
		/// <param name="responseCurve"></param>
		/// <param name="random">The random generator to use.</param>
		public ResponseCurveGenerator(ResponseCurveBase<T> responseCurve, IRandom random) :
			this(responseCurve, new UniformFloatGenerator(random))
		{ }

		/// <summary>
		/// Creates a new ResponseCurveGenerator with the given 
		/// response curve.
		/// </summary>
		/// <param name="responseCurve"></param>
		/// <param name="floatGenerator"></param>
		public ResponseCurveGenerator(ResponseCurveBase<T> responseCurve, IGenerator<float> floatGenerator)
		{
			this.responseCurve = responseCurve;
			this.floatGenerator = floatGenerator;
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			return responseCurve[floatGenerator.Next()];
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