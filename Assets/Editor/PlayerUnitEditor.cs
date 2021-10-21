using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerUnit))]
public class PlayerUnitEditor : Editor
{
	public override void OnInspectorGUI() {
		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("unitName"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("characterFace"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("level"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("xp"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("useBasicPrefabMode"));

		if (serializedObject.FindProperty("useBasicPrefabMode").boolValue)
			EditorGUILayout.PropertyField(serializedObject.FindProperty("unitPrefab"));
		else {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("meleePrefab"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("magicPrefab"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("rangedPrefab"));
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponId"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("showStats"));

		if (serializedObject.FindProperty("showStats").boolValue) {
			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField(serializedObject.FindProperty("cHP"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHP"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("cSP"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSP"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("str"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("mag"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dex"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("def"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("res"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("arm"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("agi"));
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("skills"), true);
		serializedObject.ApplyModifiedProperties();
	}
}
