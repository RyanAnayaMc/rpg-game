using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KeyItem))]
public class KeyItemEditor : Editor
{
	public override void OnInspectorGUI() {
        // Draw Item info
        EditorGUILayout.LabelField("Item Properties");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemIcon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("buyPrice"));
    }
}
