using UnityEditor;

using Gamelogic.Extensions.Editor.Internal;
using UnityEngine;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// This universal editor makes it possible to
	/// add buttons that will execute static methods
	/// to the inspector by adding the InspectorButton
	/// attribute to the method.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.Editor.Internal.GLEditor{GLMonoBehaviour}" />
	/// <remarks>You can also add this behaviour to your own editor by extending from GLEditor, and 
	/// calling DrawInspectorButtons. </remarks>
	[CustomEditor(typeof(GLMonoBehaviour), true)]
	[CanEditMultipleObjects]
	public class GLMonoBehaviourEditor : GLEditor<GLMonoBehaviour>
	{
		const int ColumnCount = 3;

		/*
		protected override void OnHeaderGUI()
		{
			//base.OnHeaderGUI();
		}
		*/
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			DrawInspectorButtons(ColumnCount);
		}		
	}
}