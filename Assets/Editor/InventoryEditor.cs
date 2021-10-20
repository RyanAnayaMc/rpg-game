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
		SerializedProperty weaponQuantities = serializedObject.FindProperty("weaponQuantities");
		SerializedProperty apparel = serializedObject.FindProperty("apparel");
		SerializedProperty apparelQuantities = serializedObject.FindProperty("apparelQuantities");
		SerializedProperty accessories = serializedObject.FindProperty("accessories");
		SerializedProperty accessoryQuantities = serializedObject.FindProperty("accessoryQuantities");
		SerializedProperty gold = serializedObject.FindProperty("gold");

		EditorGUILayout.PropertyField(gold);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(consumables);
		EditorGUILayout.PropertyField(quantities);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(weapons);
		EditorGUILayout.PropertyField(weaponQuantities);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(apparel);
		EditorGUILayout.PropertyField(apparelQuantities);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(accessories);
		EditorGUILayout.PropertyField(accessoryQuantities);
		EditorGUILayout.EndHorizontal();



		serializedObject.ApplyModifiedProperties();
	}

}
