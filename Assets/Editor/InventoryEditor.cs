using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#pragma warning disable CS0618

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor {
	public override void OnInspectorGUI() {
		serializedObject.Update();

		SerializedProperty consumables = serializedObject.FindProperty("consumables");
		SerializedProperty quantities = serializedObject.FindProperty("quantities");
		SerializedProperty weapons = serializedObject.FindProperty("weapons");
		SerializedProperty apparel = serializedObject.FindProperty("apparel");
		SerializedProperty accessories = serializedObject.FindProperty("accessories");
		SerializedProperty gold = serializedObject.FindProperty("gold");

		EditorGUILayout.PropertyField(gold);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(consumables);
		EditorGUILayout.PropertyField(quantities);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.PropertyField(weapons);
		EditorGUILayout.PropertyField(apparel);
		EditorGUILayout.PropertyField(accessories);

		serializedObject.ApplyModifiedProperties();
	}

}
