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
		weapon.itemName = EditorGUILayout.TextField(weapon.itemName);
		weapon.itemIcon = EditorGUILayout.ObjectField(weapon.itemIcon, typeof(Sprite), false) as Sprite;
		weapon.itemDescription = EditorGUILayout.TextField(weapon.itemDescription);
		weapon.isSellable = EditorGUILayout.Toggle(weapon.isSellable);
		weapon.buyPrice = EditorGUILayout.IntField(weapon.buyPrice);

		EditorGUILayout.Space();

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

		weapon.might = EditorGUILayout.IntField(weapon.might);
		weapon.castSFX = EditorGUILayout.ObjectField(weapon.castSFX, typeof(AudioClip), false) as AudioClip;
		weapon.castAnimation = (WeaponAnimation) EditorGUILayout.EnumPopup(weapon.castAnimation);
		weapon.attackSFX = EditorGUILayout.ObjectField(weapon.attackSFX, typeof(AudioClip), false) as AudioClip;
		weapon.weaponAnimation = (WeaponAnimation) EditorGUILayout.EnumPopup(weapon.weaponAnimation);
	}
}
