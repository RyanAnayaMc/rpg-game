using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
	public override void OnInspectorGUI() {
		serializedObject.Update();

		// Draw item info
		EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("itemIcon"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("isSellable"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("buyPrice"));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("atkType"));
		switch ((AttackType) serializedObject.FindProperty("atkType").enumValueIndex) {
			case AttackType.MELEE:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeType"));
				break;
			case AttackType.MAGIC:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("magicType"));
				break;
			case AttackType.RANGED:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rangedType"));
				break;
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("might"));

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("castSFX"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("castAnimation"));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSFX"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponAnimation"));
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}
}
