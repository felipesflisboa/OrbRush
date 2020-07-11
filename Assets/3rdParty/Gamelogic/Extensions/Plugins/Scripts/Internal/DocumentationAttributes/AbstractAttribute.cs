// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;

namespace Gamelogic.Extensions.Internal
{
	/// <summary>
	/// Use to mark classes and methods that are abstract, but cannot be implemented as such
	/// because Unity does not serialize such classes properly, especially abstract ScriptableObjects.
	/// </summary>
	[Version(1, 4)]
	[AttributeUsage(AttributeTargets.Class |
					AttributeTargets.Struct | 
					AttributeTargets.Method |
					AttributeTargets.Property, Inherited = false)]
	public sealed class AbstractAttribute : Attribute
	{}
}
