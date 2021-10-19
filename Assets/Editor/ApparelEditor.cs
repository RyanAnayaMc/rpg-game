using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ApparelEditor : Editor {
	public override void OnInspectorGUI() {
        serializedObject.Update();

        // Draw Item info
        EditorGUILayout.LabelField("Item Properties");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemIcon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isSellable"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("buyPrice"));

        // Draw apparel info
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("resChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("armChange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("agiChange"));

        serializedObject.ApplyModifiedProperties();
    }
}
