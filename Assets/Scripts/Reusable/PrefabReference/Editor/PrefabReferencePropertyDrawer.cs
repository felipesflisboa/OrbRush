using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PrefabReference))]
public class PrefabReferencePropertyDrawer : PropertyDrawer{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        if (!IsNull(property)) {
            if (!IsPrefab(property)) {
                Debug.LogError("You tried to add a non-prefab into a variable with PrefabReference property! Removing reference.");
                property.objectReferenceValue = null;
            }
            if (!(property.objectReferenceValue is GameObject) && !(property.objectReferenceValue is Component))
                Debug.LogWarningFormat("PrefabReference only works with Components and GameObject. {0} isn't neither!", property.displayName);
        }
        EditorGUI.PropertyField(position, property, label, true);
    }

    bool IsNull(SerializedProperty property) {
        return property == null || property.objectReferenceValue == null;
    }

    bool IsPrefab(SerializedProperty property) {
        return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(property.objectReferenceValue));
    }

    GameObject GetGameObject(SerializedProperty property) {
        if (property.objectReferenceValue is GameObject)
            return property.objectReferenceValue as GameObject;
        if (property.objectReferenceValue is Component)
            return (property.objectReferenceValue as Component)?.gameObject;
        return null;
    }

    /* //remove
    System.Type GetType(SerializedProperty property) {
        System.Type parentType = property.serializedObject.targetObject.GetType();
        System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);
        return fi.FieldType;
    }
    */
}