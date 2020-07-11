// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;

namespace Gamelogic.Extensions.Algorithms
{
	/// <summary>
	/// Contains extension methods for Response curves.
	/// </summary>
	public static class ResponseCurve
	{
		//class used to implement the Select method.
		private class SelectResponse<TSource, TResult> : IResponseCurve<TResult>
		{
			private IResponseCurve<TSource> source;
			private Func<TSource, TResult> selector;

			public TResult this[float input]
			{
				get
				{
					return selector(source[input]);
				}
			}

			public TResult Evaluate(float t)
			{
				return selector(source.Evaluate(t));
			}

			public SelectResponse(IResponseCurve<TSource> source, Func<TSource, TResult> selector)
			{
				this.source = source;
				this.selector = selector;
			}
		}

		/// <summary>
		/// Creates a response curve that transforms the output of the given curve 
		/// using the given function.
		/// </summary>
		/// <typeparam name="TSource">The type of the source response curve.</typeparam>
		/// <typeparam name="TResult">The type of the result response curve.</typeparam>
		/// <param name="source">The source curve.</param>
		/// <param name="selector">The selector used to transform results from the source curve.</param>
		/// <returns>IResponseCurve&lt;TResult&gt;.</returns>
		/// <example>The following makes a response curve that returns string representation of the 
		/// results of a float response curve:
		/// <code>
		/// var curve = new ResponseCurveFloat(new[] {0f, 0.5f, 1f}, new []{0f, 1f, 10f}).Select(x => x.TosString());
		/// 
		/// uiComponent.text = (curve[0.75f]);
		/// </code></example>

		public static IResponseCurve<TResult> Select<TSource, TResult>(
			this IResponseCurve<TSource> source, 
			Func<TSource, TResult> selector)
		{
			return new SelectResponse<TSource, TResult>(source, selector);
		}
	}
}
