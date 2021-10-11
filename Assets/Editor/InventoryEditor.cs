using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#pragma warning disable CS0618

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor {
	public override void OnInspectorGUI() {
		serializedObject.Update();

		SerializedProperty items = serializedObject.FindProperty("items");
		SerializedProperty quantities = serializedObject.FindProperty("quantities");

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(items);
		EditorGUILayout.PropertyField(quantities);
		EditorGUILayout.EndHorizontal();
	}

}
