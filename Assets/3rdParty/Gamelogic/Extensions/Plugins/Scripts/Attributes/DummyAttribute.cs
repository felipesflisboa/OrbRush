using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// Used to mark the last field in a MonoBehaviour 
	/// as a dummy so that it is not drawn. This is 
	/// useful to add a decorator that should be displayed 
	/// below all fields.
	/// </summary>
	/// <seealso cref="UnityEngine.PropertyAttribute" />
	public class DummyAttribute : PropertyAttribute
	{

	}
}