// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A buffer is a generator that buffers a fixed number of elements at a time from another
	/// generator before returning them.
	/// </summary>
	/// <typeparam name="T">The type of items this generator will generate.</typeparam>
	[Version(1, 2)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class Buffer<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly Queue<T> buffer;
		private readonly IGenerator<T> baseGenerator;

		#endregion

		#region Constructors

		public Buffer(IGenerator<T> baseGenerator, int bufferCount)
		{
			buffer = new Queue<T>();

			for (int i = 0; i < bufferCount; i++)
			{
				buffer.Enqueue(baseGenerator.Next());
			}

			this.baseGenerator = baseGenerator;
		}

		#endregion

		#region Public Methods

		public IEnumerable<T> PeekAll()
		{
			return buffer;
		}

		public T Next()
		{
			T itemToPop = buffer.Dequeue();
			buffer.Enqueue(baseGenerator.Next());

			return itemToPop;
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