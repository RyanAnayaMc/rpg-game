using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    MELEE,
    MAGIC,
    RANGED
}

public enum WeaponSubtype
{
    SWORD,
    DAGGER,
    LANCE,
    AXE,
    CLUB,
    RING,
    TOME,
    STAFF,
    SCROLL,
    WAND,
    BOW,
    HANDGUN,
    RIFLE
}

public class Weapon : MonoBehaviour
{
    public AttackType atkType;
    public WeaponSubtype subtype;
    public string weaponName;
    public string weaponDescription;
    public int might;
    public AudioClip attackSFX;
    public Sprite weaponIcon;
}
