using UnityEditor;
using UnityEngine;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// Property drawer for fields marked with the WarnIfNull.
	/// </summary>
	/// <seealso cref="UnityEditor.PropertyDrawer" />
	[CustomPropertyDrawer(typeof(WarningIfNullAttribute))]
	public class WarningIfNullPropertyDrawer : PropertyDrawer
	{
		WarningIfNullAttribute CommentAttribute
		{
			get { return (WarningIfNullAttribute)attribute; }
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.objectReferenceValue != null)
			{
				return base.GetPropertyHeight(property, label);
			}

			var guiContent = new GUIContent(CommentAttribute.WarningMessage);
			var oldWordWrap = EditorStyles.miniLabel.wordWrap;

			EditorStyles.miniLabel.wordWrap = true;

			var height =
				EditorStyles.miniLabel.CalcHeight(guiContent, Screen.width - 19) +
				EditorGUI.GetPropertyHeight(property, label, true);
			EditorStyles.miniLabel.wordWrap = oldWordWrap;

			return height;

		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.objectReferenceValue != null)
			{
				EditorGUI.PropertyField(position, property);
				return;
			}

			var guiContent = new GUIContent(CommentAttribute.WarningMessage);
			var oldWordWrap = EditorStyles.miniLabel.wordWrap;
			EditorStyles.miniLabel.wordWrap = true;		

			Color color = GUI.color;
			Color contentColor = GUI.contentColor;
			Color backgroundColor = GUI.backgroundColor;

			if (EditorGUIUtility.isProSkin)
			{			
				GUI.color = Color.yellow;
			}
			else
			{
				EditorGUI.DrawRect(position, Color.yellow);
				GUI.contentColor = Color.black;
				GUI.backgroundColor = Color.yellow;
			}

			float graphHeight = EditorGUI.GetPropertyHeight(property, label, true); ;
			float labelHeight = EditorStyles.miniLabel.CalcHeight(guiContent, Screen.width - 19);
			position.height = labelHeight;
			EditorGUI.LabelField(position, CommentAttribute.WarningMessage, EditorStyles.miniLabel	);
					
			position.y += labelHeight;
			position.height = graphHeight;

			EditorGUI.PropertyField(position, property);
			EditorStyles.miniLabel.wordWrap = oldWordWrap;

			if (EditorGUIUtility.isProSkin)
			{
				GUI.color = color;
			}
			else
			{
				GUI.contentColor = contentColor;
				GUI.backgroundColor = backgroundColor;
			}
		}
	}
}