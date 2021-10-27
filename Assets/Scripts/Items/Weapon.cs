using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A weapon is an equippable item that allows you to attack and use special moves
public enum AttackType {
    Melee, // Damage scales on STR, MT and enemy DEF
    Magic, // Damage scales on MAG, MT and enemy RES
    Ranged // Damage scales on MT and enemy ARM
}

public enum MeleeWeaponType {
    Sword,
    Dagger
}

public enum MagicWeaponType {
    Staff,
    Tome,
    Spell
}

public enum RangedWeaponType {
    Bow,
    Crossbow,
    Handgun,
    Rifle
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "RPG Element/Item/Weapon")]
public class Weapon : Item {
    /// <summary>
    /// The type of damage the weapon does.
    /// </summary>
    public AttackType atkType;

    public MeleeWeaponType meleeType;
    public MagicWeaponType magicType;
    public RangedWeaponType rangedType;

    /// <summary>
    /// The animation that plays before attacking. Can be null.
    /// </summary>
    public WeaponAnimation castAnimation;

    /// <summary>
    /// The animation that plays while attacking.
    /// </summary>
    public WeaponAnimation weaponAnimation;

    /// <summary>
    /// MT stat. Determines how much damage the weapon does.
    /// </summary>
    public int might;

    /// <summary>
    /// The sound effect that plays when attacking.
    /// </summary>
    public AudioClip attackSFX;

    /// <summary>
    /// The sound effect that plays when casting. Can be null.
    /// </summary>
    public AudioClip castSFX;

    /// <summary>
    /// The multiplier to apply to this weapon's attacks
    /// </summary>
    public float attackDamageMultiplier = 1;

    /// <summary>
    /// The number of hits this weapon does when attacking
    /// </summary>
    public int hits = 1;

    public override string GetDescriptionText() {
        return base.itemDescription + " <color=green>" + might + "<color=white>MT";
	}

	public override int GetNumber() {
        return 1;
	}
}
