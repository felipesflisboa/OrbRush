using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Gamelogic.Extensions.Internal;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Gamelogic.Extensions.Editor.Internal
{
	/// <summary>
	/// Class that can be used as a base class for custom editors with extra convenience methods
	/// and properties.
	/// </summary>
	/// <typeparam name="T">The type this is an editor for.</typeparam>
	/// <seealso cref="UnityEditor.Editor" />
	[Version(1, 3)]
	public class GLEditor<T> : UnityEditor.Editor
		where T : MonoBehaviour
	{
		public T Target
		{
			get { return (T) (object) target; }
		}

		public T[] Targets
		{
			get { return targets.Cast<T>().ToArray(); }
		}

		/// <summary>
		/// Draws a line as a separator in the inspector.
		/// </summary>
		public void Splitter()
		{
			GLEditorGUI.Splitter();
		}


		public static int AddCombo(string[] options, int selectedIndex)
		{
			return EditorGUILayout.Popup(selectedIndex, options);
		}

		public bool HasProperty(string propertyName)
		{
			var property = serializedObject.FindProperty(propertyName);

			return property != null;
		}

		public GLSerializedProperty FindProperty(string propertyName)
		{
			var property = new GLSerializedProperty
			{
				SerializedProperty = serializedObject.FindProperty(propertyName),
				CustomTooltip = ""
			};

			if (property.SerializedProperty == null)
			{
				Debug.LogError("Could not find property " + propertyName + " in class " + typeof (T).Name);

				return property;
			}

			Type type = typeof (T);

			FieldInfo field = type.GetField(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			if (field == null)
			{
				Debug.LogError("Could not find field " + propertyName + " in class " + typeof (T).Name);

				return property;
			}

			return property;
		}

		protected void AddField(SerializedProperty prop)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(prop, true);
			EditorGUILayout.EndHorizontal();
		}

		protected void AddField(GLSerializedProperty prop)
		{
			if (prop == null) return;
			if (prop.SerializedProperty == null) return;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(prop.SerializedProperty,
				new GUIContent(prop.SerializedProperty.name.SplitCamelCase(), prop.CustomTooltip), true);
			EditorGUILayout.EndHorizontal();
		}

		protected void AddLabel(string title, string text)
		{
			EditorGUILayout.LabelField(title, text);
		}

        protected void AddTextAndButton(string text, string buttonLabel, Action buttonAction)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);

            if (GUILayout.Button(buttonLabel))
            {
                if (buttonAction != null)
                    buttonAction();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected void ArrayGUI(SerializedObject obj, SerializedProperty property)
        {
            int size = property.arraySize;

            int newSize = EditorGUILayout.IntField(property.name + " Size", size);

            if (newSize != size)
            {
                property.arraySize = newSize;
            }

            EditorGUI.indentLevel = 3;

            for (int i = 0; i < newSize; i++)
            {
                var prop = property.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(prop);
            }
        }

		/// <summary>
		/// Draws the buttons in the inspector for all method in the target class
		/// that are marked with the InspectorButtonAttribute.
		/// </summary>
		/// <param name="columnCount">The number of columns to draw the buttons in.</param>
		protected void DrawInspectorButtons(int columnCount)
		{
			var methods =  GetParentTypes(Target.GetType())
							.SelectMany(type => type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
							.Where(m => m.GetCustomAttributes(typeof(InspectorButtonAttribute), false).Length > 0)
							.ToArray();

			EditorGUILayout.BeginHorizontal();

			for (int i = 0; i < methods.Length; i++)
			{
				var method = methods[i];

				if (GUILayout.Button(method.Name.SplitCamelCase()))
				{
					if (method.ReturnType == typeof(IEnumerator))
					{
						Target.StartCoroutine((IEnumerator)method.Invoke(Target, new object[] { }));
					}
					else
					{
						method.Invoke(Target, new object[] { });
					}
				}

				if (i % columnCount == columnCount - 1)
				{
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		private static IEnumerable<Type> GetParentTypes(Type type)
		{
			var currentBaWithYpe = type;

			while (currentBaWithYpe != null)
			{
				yield return currentBaWithYpe;
				currentBaWithYpe = currentBaWithYpe.BaseType;
			}
		}
	}
}