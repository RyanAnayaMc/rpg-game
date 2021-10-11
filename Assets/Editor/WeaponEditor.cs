using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
	public override void OnInspectorGUI() {
		Weapon weapon = target as Weapon;

		// Draw item info
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(40));
		weapon.itemName = EditorGUILayout.TextField(weapon.itemName);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Icon", GUILayout.MaxWidth(40));
		weapon.itemIcon = EditorGUILayout.ObjectField(weapon.itemIcon, typeof(Sprite), false) as Sprite;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("Description");
		weapon.itemDescription = EditorGUILayout.TextArea(weapon.itemDescription);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Sellable?", GUILayout.MaxWidth(60));
		weapon.isSellable = EditorGUILayout.Toggle(weapon.isSellable);
		EditorGUILayout.LabelField("Price", GUILayout.MaxWidth(40));
		weapon.buyPrice = EditorGUILayout.IntField(weapon.buyPrice);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Weapon Type", GUILayout.MaxWidth(80));
		weapon.atkType = (AttackType) EditorGUILayout.EnumPopup(weapon.atkType);
		switch (weapon.atkType) {
			case AttackType.MELEE:
				weapon.meleeType = (MeleeWeaponType) EditorGUILayout.EnumPopup(weapon.meleeType);
				break;
			case AttackType.MAGIC:
				weapon.magicType = (MagicWeaponType) EditorGUILayout.EnumPopup(weapon.magicType);
				break;
			case AttackType.RANGED:
				weapon.rangedType = (RangedWeaponType) EditorGUILayout.EnumPopup(weapon.rangedType);
				break;
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Might", GUILayout.MaxWidth(40));
		weapon.might = EditorGUILayout.IntField(weapon.might);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Cast SFX/Animation", GUILayout.MaxWidth(130));
		weapon.castSFX = EditorGUILayout.ObjectField(weapon.castSFX, typeof(AudioClip), false) as AudioClip;
		weapon.castAnimation = (WeaponAnimation) EditorGUILayout.EnumPopup(weapon.castAnimation);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Attack SFX/Animation", GUILayout.MaxWidth(130));
		weapon.attackSFX = EditorGUILayout.ObjectField(weapon.attackSFX, typeof(AudioClip), false) as AudioClip;
		weapon.weaponAnimation = (WeaponAnimation) EditorGUILayout.EnumPopup(weapon.weaponAnimation);
		EditorGUILayout.EndHorizontal();
	}
}
