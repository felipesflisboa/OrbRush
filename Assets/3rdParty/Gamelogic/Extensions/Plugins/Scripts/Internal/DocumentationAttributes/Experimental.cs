// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;

namespace Gamelogic.Extensions.Internal
{
	/// <summary>
	/// This attribute is used to mark components as experimental. 
	/// Typically, these are not thoroughly tested, or the design has not been
	/// thought out completely. They are likely to contain bugs and change.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	[Version(1)]
	public sealed class ExperimentalAttribute : Attribute
	{
	}
}