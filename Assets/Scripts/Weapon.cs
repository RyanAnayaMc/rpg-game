using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A weapon is an equippable item that allows you to attack and use special moves
public enum AttackType
{
    MELEE, // Damage scales on STR, MT and enemy DEF
    MAGIC, // Damage scales on MAG, MT and enemy RES
    RANGED // Damage scales on MT and enemy ARM
}

// Determines if a unit can equip a certain weapon type
public enum WeaponSubtype
{
    // Melee weapons
    SWORD,
    DAGGER,
    LANCE,
    AXE,
    CLUB,

    // Magic weapons
    RING,
    TOME,
    STAFF,
    SCROLL,
    WAND,

    // Ranged weapons
    BOW,
    HANDGUN,
    RIFLE,
    SHOTGUN
}

// Determines which animation plays when casting/attacking
public enum WeaponAnimation
{
    NONE,
    SlashAttack,
    FireAttack,
    CastAnimation
}

public class Weapon : MonoBehaviour
{
    public AttackType atkType; // The type of damage the weapon does
    public WeaponSubtype subtype; // The subtype of the weapon
    public WeaponAnimation castAnimation; // The animation that plays before attacking
    public WeaponAnimation weaponAnimation; // The animation that plays while attacking
    public string weaponName; // The name of the weapon
    public string weaponDescription; // A short description of the weapon
    public int might; // MT, determines how much damage the weapon does
    public AudioClip attackSFX; // The sound effect that plays when attacking. Can be null
    public AudioClip castSFX; // The sound effect that plays when casting. Can be null
    public Sprite weaponIcon; // The icon for the weapon in the inventory
}
