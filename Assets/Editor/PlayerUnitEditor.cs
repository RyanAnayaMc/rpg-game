using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerUnit))]
public class PlayerUnitEditor : Editor
{
	public override void OnInspectorGUI() {
		PlayerUnit unit = target as PlayerUnit;

		unit.unitName = EditorGUILayout.TextField("Name", unit.unitName);
		unit.level = EditorGUILayout.IntField("Level", unit.level);
		unit.xp = EditorGUILayout.IntField("Experience Points", unit.xp);

		unit.useBasicPrefabMode = EditorGUILayout.Toggle("Use Basic Prefab Mode", unit.useBasicPrefabMode);

		if (unit.useBasicPrefabMode)
			unit.unitPrefab = EditorGUILayout.ObjectField("Sprite", unit.unitPrefab, typeof(GameObject), false) as GameObject;
		else {
			unit.meleePrefab = EditorGUILayout.ObjectField("Melee Prefab", unit.meleePrefab, typeof(GameObject), false) as GameObject;
			unit.magicPrefab = EditorGUILayout.ObjectField("Magic Prefab", unit.magicPrefab, typeof(GameObject), false) as GameObject;
			unit.rangedPrefab = EditorGUILayout.ObjectField("Ranged Prefab", unit.rangedPrefab, typeof(GameObject), false) as GameObject;
		}

		unit.weapon = EditorGUILayout.ObjectField("Weapon", unit.weapon, typeof(Weapon), false) as Weapon;

		unit.showStats = EditorGUILayout.Foldout(unit.showStats, "Stats", true);

		if (unit.showStats) {
			EditorGUI.indentLevel++;
			unit.cHP = EditorGUILayout.IntField("Current HP", unit.cHP);
			unit.maxHP = EditorGUILayout.IntField("Max HP", unit.maxHP);
			unit.cSP = EditorGUILayout.IntField("Current SP", unit.cSP);
			unit.maxSP = EditorGUILayout.IntField("Max SP", unit.maxSP);
			unit.str = EditorGUILayout.IntField("Strength", unit.str);
			unit.mag = EditorGUILayout.IntField("Magic", unit.mag);
			unit.dex = EditorGUILayout.IntField("Dexterity", unit.dex);
			unit.def = EditorGUILayout.IntField("Defense", unit.def);
			unit.res = EditorGUILayout.IntField("Resistance", unit.res);
			unit.arm = EditorGUILayout.IntField("Armor", unit.arm);
			unit.agi = EditorGUILayout.IntField("Agility", unit.agi);
			EditorGUI.indentLevel--;
		}

		serializedObject.Update();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("skills"), true);
		serializedObject.ApplyModifiedProperties();
	}
}
