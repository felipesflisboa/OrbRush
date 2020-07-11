// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A generator that generates integers. IIntGenerators are often used
	///	to generate random elements from lists or arrays, where the ints 
	/// generated are used to index into the list or array.
	/// </summary>
	[Version(1, 4)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public interface IIntGenerator : IGenerator<int>
	{}
}