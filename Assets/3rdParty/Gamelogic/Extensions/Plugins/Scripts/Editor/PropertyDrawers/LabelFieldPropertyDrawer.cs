using System;
using Gamelogic.Extensions.Internal;
using UnityEngine;
using UnityEditor;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// Property drawer for the label field attribute.
	/// </summary>
	/// <seealso cref="UnityEditor.PropertyDrawer" />
	[Version(2, 5)]
	[CustomPropertyDrawer(typeof (LabelFieldAttribute))]
	public class LabelFieldPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var nameFieldAttribute = (LabelFieldAttribute) attribute;
			var nameProperty = property.FindPropertyRelative(nameFieldAttribute.LabelField);

			if (nameProperty != null)
			{
				var name = label.text;

				switch (nameProperty.propertyType)
				{
					case SerializedPropertyType.Generic:
						break;
					case SerializedPropertyType.Integer:
						name = nameProperty.intValue.ToString();
						break;
					case SerializedPropertyType.Boolean:
						name = nameProperty.boolValue.ToString();
						break;
					case SerializedPropertyType.Float:
						name = nameProperty.floatValue.ToString();
						break;
					case SerializedPropertyType.String:
						name = nameProperty.stringValue;
						break;
					case SerializedPropertyType.Color:
						name = property.colorValue.ToString();
						break;
					case SerializedPropertyType.ObjectReference:
						name = nameProperty.objectReferenceValue.name;
						break;
					case SerializedPropertyType.LayerMask:
						break;
					case SerializedPropertyType.Enum:
						name = nameProperty.enumDisplayNames[nameProperty.enumValueIndex];
						break;
					case SerializedPropertyType.Vector2:
						name = nameProperty.vector2Value.ToString();
						break;
					case SerializedPropertyType.Vector3:
						name = nameProperty.vector3Value.ToString();
						break;
					case SerializedPropertyType.Vector4:
						name = nameProperty.vector4Value.ToString();
						break;
					case SerializedPropertyType.Rect:
						name = nameProperty.rectValue.ToString();
						break;
					case SerializedPropertyType.ArraySize:
						break;
					case SerializedPropertyType.Character:
						break;
					case SerializedPropertyType.AnimationCurve:
						break;
					case SerializedPropertyType.Bounds:
						name = nameProperty.boundsValue.ToString();
						break;
					case SerializedPropertyType.Gradient:
						break;
					case SerializedPropertyType.Quaternion:
						name = nameProperty.quaternionValue.ToString();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				label.text = name;
			}

			EditorGUI.PropertyField(position, property, label, true);
		}
	}
}
