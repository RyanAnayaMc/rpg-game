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

		unit.useBasicSpriteMode = EditorGUILayout.Toggle("Use Basic Sprite Mode", unit.useBasicSpriteMode);

		if (unit.useBasicSpriteMode)
			unit.unitSprite = EditorGUILayout.ObjectField("Sprite", unit.unitSprite, typeof(Sprite), false) as Sprite;
		else {
			unit.meleeSprite = EditorGUILayout.ObjectField("Melee Sprite", unit.meleeSprite, typeof(Sprite), false) as Sprite;
			unit.magicSprite = EditorGUILayout.ObjectField("Magic Sprite", unit.magicSprite, typeof(Sprite), false) as Sprite;
			unit.rangedSprite = EditorGUILayout.ObjectField("Ranged Sprite", unit.rangedSprite, typeof(Sprite), false) as Sprite;
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
