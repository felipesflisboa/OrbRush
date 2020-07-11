// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// Generates items from a list using an index generator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public class ListSelectorGenerator<T> : IGenerator<T>
	{
		#region Private Fields

		private readonly IIntGenerator indexGenerator;
		private readonly List<T> list;

		#endregion

		#region Constructors

		public ListSelectorGenerator(IEnumerable<T> list, IIntGenerator indexGenerator)
		{
			list.ThrowIfNull("list");

			this.list = list.ToList();

			if (!this.list.Any())
			{
				throw new ArgumentException("cannot be empty", "list");
			}

			this.indexGenerator = indexGenerator;
		}

		#endregion

		#region Public Methods

		public T Next()
		{
			int index = indexGenerator.Next();

			return list[index];
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
