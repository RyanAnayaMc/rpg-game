using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A weapon is an equippable item that allows you to attack and use special moves
public enum AttackType {
    MELEE, // Damage scales on STR, MT and enemy DEF
    MAGIC, // Damage scales on MAG, MT and enemy RES
    RANGED // Damage scales on MT and enemy ARM
}

public enum MeleeWeaponType {
    SWORD,
    DAGGER,
    LANCE,
    AXE,
    CLUB
}

public enum MagicWeaponType {
    STAFF,
    TOME,
    RING,
    SPELL,
    SCROLL
}

public enum RangedWeaponType {
    BOW,
    CROSSBOW,
    HANDGUN,
    RIFLE
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
}
