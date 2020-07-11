using Gamelogic.Extensions.Internal;
using UnityEditor;
using UnityEngine;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// A property drawer for fields marked with the InspectorFlags Attribute.
	/// </summary>
	[Version(1, 4, 3)]
	[CustomPropertyDrawer(typeof(InspectorFlagsAttribute))]
	public class InspectorFlagsPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position,
			SerializedProperty prop,
			GUIContent label)
		{
			EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
			EditorGUI.BeginChangeCheck();

			var newValue = EditorGUI.MaskField(position, label, prop.intValue, prop.enumNames);

			if (EditorGUI.EndChangeCheck())
			{
				prop.intValue = newValue;
			}
		}
	}
}