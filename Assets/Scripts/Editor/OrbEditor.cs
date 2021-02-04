using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Orb))]
public class OrbEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GUILayout.Label(AI.GetDebugString());
    }
}
