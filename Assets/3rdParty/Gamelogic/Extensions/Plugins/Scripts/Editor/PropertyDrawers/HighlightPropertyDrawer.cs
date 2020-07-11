using Gamelogic.Extensions.Internal;
using UnityEditor;
using UnityEngine;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// A property drawer for fields marked with the Highlight Attribute.
	/// </summary>
	[Version(1, 2)]
	[CustomPropertyDrawer(typeof(HighlightAttribute))]
	public class HighlightPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position,
			SerializedProperty property,
			GUIContent label)
		{
			var oldColor = GUI.color;
			GUI.color = Utils.Blue;
			EditorGUI.PropertyField(position, property, label);
			GUI.color = oldColor;

		}
	}
}