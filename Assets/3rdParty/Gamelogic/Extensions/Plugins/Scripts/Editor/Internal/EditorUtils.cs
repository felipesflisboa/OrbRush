using UnityEngine;
using UnityEditor;

namespace Gamelogic.Extensions.Editor.Internal
{
	/// <summary>
	/// Provides utility methods used for editor code.
	/// </summary>
	public static class EditorUtils 
	{
		/// <summary>
		/// Draws swatches for a property that is an array of colors.
		/// </summary>
		/// <param name="colorsProp">The colors property.</param>
		/// <param name="position">The position.</param>
		public static void DrawColors(SerializedProperty colorsProp, Rect position)
		{
			int colorCount = colorsProp.arraySize;

			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();

			int columns = (int)(position.width / 16);
			
			float x = position.x;
			float width = 16;
			float height = 16;
			float y = position.y;

			if (columns > 0)
			{
				var indentLevel = EditorGUI.indentLevel;

				EditorGUI.indentLevel = 0;

				for (int i = 0; i < Mathf.Min(100, colorCount); i++)
				{

					if (i != 0 && i % columns == 0)
					{
						x = position.x;
						y += height;
					}

					var colorProp = colorsProp.GetArrayElementAtIndex(i);
					var color = colorProp.colorValue;

					EditorGUIUtility.DrawColorSwatch(new Rect(x, y, width - 2, height - 2), color);
					x += width;
				}

				EditorGUI.indentLevel = indentLevel;
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		/// <summary>
		/// Draws swatches for the list of colors.
		/// </summary>
		/// <param name="colorList">The color list.</param>
		/// <param name="position">The position where to draw the swatches.</param>
		/// <param name="columns">The number of columns.</param>
		/// <param name="maxColors">The maximum number of colors to draw.</param>
		/// <param name="widthOffset">The width offset.</param>
		/// <param name="heightOffset">The height offset.</param>
		public static void DrawColors(
			Color[] colorList, 
			Rect position, 
			int columns = 2, 
			int maxColors = 100, 
			float widthOffset = -2.0f, 
			float heightOffset = -2.0f)
        {
            var colorCount = colorList.Length;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            var x = position.x;
            var width = position.width / columns;
            var height = position.height;
            var y = position.y;

            if (columns > 0)
            {
                var indentLevel = EditorGUI.indentLevel;
                
                EditorGUI.indentLevel = 0;
                
                for (int i = 0; i < Mathf.Min(maxColors, colorCount); i++)
                {
                    //Makes a new row
                    if (i != 0 && i % columns == 0)
                    {
                        x = position.x;
                        y += height;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }

                    var color = colorList[i];

                    EditorGUIUtility.DrawColorSwatch(new Rect(x, y, width + widthOffset, height + heightOffset), color);
                    x += width;
                }

                EditorGUI.indentLevel = indentLevel;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

		/// <summary>
		/// Draws a curve from the start to the end rectangles.
		/// </summary>
		/// <param name="start">The start rectangle.</param>
		/// <param name="end">The end rectangle.</param>
		public static void DrawNodeCurve(Rect start, Rect end)
		{
			var endPos = new Vector3(end.x, end.y + end.height / 2, 0);

			DrawNodeCurve(start, endPos);
		}

		/// <summary>
		/// Draws a curve from the start to the end position.
		/// </summary>
		/// <param name="start">The start rectangle.</param>
		/// <param name="endPosition">The end position.</param>
		public static void DrawNodeCurve(Rect start, Vector3 endPosition)
		{
			var startPosition = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
			var startTangent = startPosition + Vector3.right * 50;
			var endTangent = endPosition + Vector3.left * 50;
			var shadowColor = new Color(0, 0, 0, 0.06f);

			for (int i = 0; i < 3; i++)
			{// Draw a shadow
				Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, shadowColor, null, (i + 1) * 5);
			}

			Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, Color.black, null, 1);

			var oldColor = Handles.color;

			Handles.color = new Color(.5f, 0.1f, 0.1f);

			Handles.DrawSolidDisc(
				(startPosition + endPosition) / 2,
				Vector3.forward,
				5);

			Handles.color = Color.black;

			Handles.DrawWireDisc(
				(startPosition + endPosition) / 2,
				Vector3.forward,
				5);

			Handles.color = oldColor;
		}
	}
}