using System;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// <see cref="T:Gamelogic.Extensions.Editor.Internal.GLEditor`1.DrawInspectorButtons"/> draws a button for
	/// each method marked with this attribute. This is also used by 
	/// <see cref="T:Gamelogic.Extensions.Editor.GLMonoBehaviourEditor"/>.
	/// </summary>
	/// <seealso cref="System.Attribute" />
	[AttributeUsage(System.AttributeTargets.Method)]
	public class InspectorButtonAttribute : Attribute
	{
	}
}