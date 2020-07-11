// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that transforms generated elements with a given transformation function.
	/// </summary>
	/// <typeparam name="T">The type of elements to generate</typeparam>
	/// <typeparam name="U">The type of the elements that underlying generator generates</typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class TransformGenerator<T, U> : IGenerator<T>
	{
		#region Private Fields

		private readonly IGenerator<U> generator;
		private readonly Func<U, T> transform;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new TransformGenerator that generates elements from the given generator, transformed
		/// by the given transform.
		/// </summary>
		/// <param name="generator">The generator to use for generating elements</param>
		/// <param name="transform">The transform to apply to generated elements before they are returned.</param>
		public TransformGenerator(IGenerator<U> generator, Func<U, T> transform)
		{
			this.generator = generator;
			this.transform = transform;
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			return transform(generator.Next());
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