// Copyright Gamelogic (c) http://www.gamelogic.co.za
#if !UNITY_5_3_OR_NEWER || !UNITY_5
using System;

namespace Gamelogic.Extensions.Internal
{
	/// <summary>
	/// A dummy attribute meant to mimic Unity 5's HelpURLAttribute so that scripts for Unity 5 can also compile
	/// under Unity 4.
	/// </summary>
	[Version(2, 4)]
	[AttributeUsage(AttributeTargets.Class)]
	public class HelpURLAttribute : Attribute
	{
		public HelpURLAttribute(string help)
		{
			//Do nothing
		}
	}
}
#endif