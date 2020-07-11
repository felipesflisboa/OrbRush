using UnityEditor;
using UnityEngine;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// Draws a property marked with the Dummy attribute (that is, does not draw it).
	/// </summary>
	[CustomPropertyDrawer(typeof(DummyAttribute))]
	public class DummyPorpertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position,
			SerializedProperty prop,
			GUIContent label)
		{
			//Do nothing
		}

		public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
		{
			return 0;
		}
	}
}