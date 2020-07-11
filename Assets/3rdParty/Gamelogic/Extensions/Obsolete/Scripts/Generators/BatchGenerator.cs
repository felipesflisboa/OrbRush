// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// Generates batches of items. The same batch is returned each time.
	/// Bath generators are more useful when used in conjunction with another 
	/// generator that processes the batches, such as ShuffledBatchGenerator.
	/// </summary>
	/// <typeparam name="T">The type of items this generator will generate.</typeparam>
	[Version(1, 2)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class BatchGenerator<T>:IGenerator<IEnumerable<T>>
	{
		#region Public Fields

		public List<T> batchTemplate;

		#endregion

		#region Constructors

		public BatchGenerator()
		{
			batchTemplate = new List<T>();
		}

		public BatchGenerator(IEnumerable<T> batchTemplate)
		{
			this.batchTemplate = batchTemplate.ToList();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a new item to the batch template.
		/// </summary>
		public void Add(T batchElement)
		{
			batchTemplate.Add(batchElement);
		}

		/// <summary>
		/// Removes an item from the batch template.
		/// </summary>
		public void Remove(T batchElement)
		{
			batchTemplate.Remove(batchElement);
		}

		public IEnumerable<T> Next()
		{
			return batchTemplate;
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