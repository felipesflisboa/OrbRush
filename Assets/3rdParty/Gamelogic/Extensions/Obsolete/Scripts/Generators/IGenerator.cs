// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;

namespace Gamelogic.Extensions.Obsolete
{
	/// <summary>
	/// A type less Generator that is the base of all generators.
	/// </summary>
	[Version(1, 2)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public interface IGenerator
	{
		#region Public Methods

		/// <summary>
		/// Generates an element of typ object.
		/// </summary>
		/// <returns></returns>
		object Next();

		#endregion
	}

	/// <summary>
	/// Classes that implement this interface can produce a new element of the 
	/// given type. Generators differ from enumerables in that they generally
	/// are infinite, and don't have a "start" position.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Version(1, 2)]
	[Obsolete("Use the static functions in Gamelogic.Generators.Generator instead.")]
	public interface IGenerator<out T> :IGenerator
	{
		#region Public Methods

		/// <summary>
		/// Generates the next element.
		/// </summary>
		/// <returns></returns>
		new T Next();

		#endregion
	}
}