// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;
using Random = UnityEngine.Random;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// Extension methods defined on generators.
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public static class GeneratorExtensions
	{
		#region Static Methods

		public static IGenerator<T> Select<T, U>(this IGenerator<U> generator, Func<U, T> transform)
		{
			return new TransformGenerator<T, U>(generator, transform);
		}

		public static IGenerator<T> Where<T>(this IGenerator<T> generator, Func<T, bool> predicate)
		{
			return new FilterGenerator<T>(generator, predicate);
		}

		/// <summary>
		/// Gets the next n elements from the generator.
		/// </summary>
		public static IEnumerable<T> Next<T>(this IGenerator<T> generator, int n)
		{
			for (int i = 0; i < n; i++)
			{
				yield return generator.Next();
			}
		}

		#endregion

		#region Experimental - Design not finalised.

		public static IGenerator<T> Cast<T>(this IGenerator generator)
		{
			return CastImpl<T>(generator).ToGenerator();
		}

		public static IGenerator<T> OfType<T>(this IGenerator generator) where T : class
		{
			return OfTypeImpl<T>(generator).ToGenerator();
		}

		public static IGenerator<IEnumerable<T>> Window<T>(this IGenerator<T> generator, int windowSize)
		{
			return WindowImpl(generator, windowSize).ToGenerator();
		}

		public static IGenerator<U> Aggregate<T, U>(this IGenerator<IEnumerable<T>> generator, Func<IEnumerable<T>, U> aggregate)
		{
			return generator.Select(aggregate);
		}

		public static IGenerator<U> AggregateWindow<T, U>(this IGenerator<T> generator, int windowSize, Func<IEnumerable<T>, U> aggregate)
		{
			return generator.Window(windowSize).Aggregate(aggregate);
		}

		private static IEnumerator<IEnumerable<T>> WindowImpl<T>(IGenerator<T> generator, int windowSize)
		{
			var window = new Queue<T>();

			for (int i = 0; i < windowSize; i++)
			{
				window.Enqueue(generator.Next());
			}

			yield return window;

			while (true)
			{
				window.Dequeue();
				window.Enqueue(generator.Next());

				yield return window;
			}
		}

		private static IEnumerator<T> OfTypeImpl<T>(IGenerator generator) where T : class
		{
			while (true)
			{
				var nextElement = generator.Next() as T;

				if (nextElement != null)
				{
					yield return nextElement;
				}
			}
		}

		private static IEnumerator<T> CastImpl<T>(IGenerator generator)
		{
			while (true)
			{
				yield return (T) generator.Next();
			}
		}

		public static IGenerator<T> WhereBuffer<T>(this IGenerator<T> generator, int n,
			Func<IEnumerable<T>, bool> predicate)
		{
			return new BufferFilterGenerator<T>(generator, n, predicate);
		}

		public static IGenerator<T> Buffer<T>(this IGenerator<T> generator, int bufferCount)
		{
			return new Buffer<T>(generator, bufferCount);
		}

		public static IGenerator<T> ToPeriodicGenerator<T>(this IEnumerable<T> enumerable)
		{
			return new RepeatSequenceGenerator<T>(enumerable);
		}

		public static IGenerator<T> ToRandomElementGenerator<T>(this IEnumerable<T> enumerable)
		{
			return new  RandomElementGenerator<T>(enumerable);
		}

		private static IGenerator<T> ToGenerator<T>(this IEnumerator<T> enumerator)
		{
			return new EnumeratorGenerator<T>(enumerator);
		}

		private static IGenerator<T> ToGenerator<T>(this IEnumerable<T> enumerator)
		{
			return new EnumeratorGenerator<T>(enumerator.GetEnumerator());
		}

		public static IGenerator<T> RepeatWithProbability<T>(this IGenerator<T> generator, float probability)
		{
			//asset probability is between 0 and 1
			return RepeatWithProbabilityImpl(generator, probability).ToGenerator();
		}

		private static IEnumerator<T> RepeatWithProbabilityImpl<T>(IGenerator<T> generator, float probability)
		{
			T next = generator.Next();

			yield return next;

			while (true)
			{
				while (Random.value < probability)
				{
					yield return next;
				}

				next = generator.Next();
				yield return next;
			}
		}

		#endregion
	}

	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	class EnumeratorGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly IEnumerator<T> enumerator;

		#endregion

		#region Constructors

		public EnumeratorGenerator(IEnumerator<T> enumerator)
		{
			this.enumerator = enumerator;
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			bool hasNext = enumerator.MoveNext();
			var current = enumerator.Current;

			if (!hasNext)
			{
				enumerator.Reset();
			}

			return current;
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

