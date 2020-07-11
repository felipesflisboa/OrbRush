// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Algorithms;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// Returns elements from a batch generator, but shuffles each batch before doing so.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 2)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class ShuffledBatchGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly Queue<T> currentBatch;
		private readonly BatchGenerator<T> batchGenerator;
		private readonly IRandom random;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new ShuffledBatchGenerator that uses the given 
		/// BatchGenerator.
		/// </summary>
		/// <param name="batchGenerator"></param>
		public ShuffledBatchGenerator(BatchGenerator<T> batchGenerator)
		{
		}

		/// <summary>
		/// Constructs a new ShuffledBatchGenerator that uses the given 
		/// BatchGenerator.
		/// </summary>
		/// <param name="batchGenerator"></param>
		/// <param name="random">The random generator to use.</param>
		public ShuffledBatchGenerator(BatchGenerator<T> batchGenerator, IRandom random)
		{
			this.batchGenerator = batchGenerator;
			this.random = random;
			currentBatch = new Queue<T>();

			FillCurrentBatch();
		}

		/// <summary>
		/// Constructs a new ShuffledBatchGenerator that uses the given 
		/// batch template to make a new batch generator to use.
		/// </summary>
		public ShuffledBatchGenerator(IEnumerable<T> batchTemplate):
			this(new BatchGenerator<T>(batchTemplate))
		{}

		/// <summary>
		/// Constructs a new ShuffledBatchGenerator that uses the given 
		/// batch template to make a new batch generator to use.
		/// </summary>
		public ShuffledBatchGenerator(IEnumerable<T> batchTemplate, IRandom random) :
			this(new BatchGenerator<T>(batchTemplate), random)
		{ }

		#endregion

		#region Public Methods

		public T Next()
		{
			if (!currentBatch.Any())
			{
				FillCurrentBatch();
			}

			return currentBatch.Dequeue();
		}

		#endregion

		#region Private Methods

		private void FillCurrentBatch()
		{
			var batch = batchGenerator.Next().ToList();
			
			batch.Shuffle(random);

			foreach (var obj in batch)
			{
				currentBatch.Enqueue(obj);
			}
		}

		object IGenerator.Next()
		{
			return Next();
		}

		#endregion
	}
}