// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A buffer generator that will only generate items that will ensure the buffer can pass the predicate.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class BufferFilterGenerator<T>:IGenerator<T>
	{
		#region Private Fields

		private readonly LinkedList<T> buffer;
		private readonly IGenerator<T> baseGenerator;
		private readonly Func<IEnumerable<T>, bool> predicate;

		#endregion

		#region Constructors

		public BufferFilterGenerator(IGenerator<T> baseGenerator, int bufferCount, Func<IEnumerable<T>, bool> predicate)
		{
			this.baseGenerator = baseGenerator;
			this.predicate = predicate;

			buffer = new LinkedList<T>();

			for (int i = 0; i < bufferCount; i++)
			{
				buffer.AddLast(baseGenerator.Next());

				FixBuffer();
			}
		}

		#endregion

		#region Public Methods

		public IEnumerable<T> PeekAll()
		{
			return buffer;
		}

		public T Next()
		{
			T itemToPop = buffer.First.Value;
			buffer.RemoveFirst();

			buffer.AddLast(baseGenerator.Next());

			FixBuffer();
		
			return itemToPop;
		}

		#endregion

		#region Private Methods

		private void FixBuffer()
		{
			while (!predicate(buffer))
			{
				buffer.RemoveLast();
				buffer.AddLast(baseGenerator.Next());
			}
		}

		object IGenerator.Next()
		{
			return Next();
		}

		#endregion
	}
}