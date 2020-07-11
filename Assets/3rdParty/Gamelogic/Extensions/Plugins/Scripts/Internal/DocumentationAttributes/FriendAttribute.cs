// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;

namespace Gamelogic.Extensions.Internal
{
	/// <summary>
	/// Use to mark targets that are only exposed because communication 
	/// between classes is necessary to implement certain Unity features.
	/// Typically, when editor classes need private access to the classes
	/// they edit.
	/// </summary>
	[Version(2)]
	[AttributeUsage(AttributeTargets.All, Inherited = false)]
	public sealed class FriendAttribute : Attribute
	{ }

	/// <summary>
	/// Use to mark targets that are only supposed to be used by internal editor code.
	/// </summary>
	[Version(2, 4)]
	[AttributeUsage(AttributeTargets.All, Inherited = false)]
	public sealed class EditorInternal : Attribute
	{
		
	}
}