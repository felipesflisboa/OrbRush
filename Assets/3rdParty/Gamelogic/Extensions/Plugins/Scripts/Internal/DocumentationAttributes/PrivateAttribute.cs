// Copyright Gamelogic (c) http://www.gamelogic.co.za\

using System;

namespace Gamelogic.Extensions.Internal
{
	/// <summary>
	/// Use to mark targets that are private, but cannot be implemented as such
	/// because Unity it needs to be public to work with Unity.
	/// </summary>
	[Version(2)]
	[AttributeUsage(AttributeTargets.All, Inherited = false)]
	public sealed class PrivateAttribute : Attribute
	{ }
}