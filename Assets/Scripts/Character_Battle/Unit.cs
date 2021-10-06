using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Basis for a playable or enemy Unit that can see combat

    public string unitName; // The unit's name
    public Weapon weapon; // The unit's equipped weapon
    // TODO add more equipment
    // TODO add skills
    public int level; // The unit's level
    public int cHP; // current hp
    public int maxHP; // maximum hp
    public int cSP; // current SP
    public int maxSP; // maximum SP
    public int str; // strength
    public int mag; // magic
    public int dex; // dexterity
    public int def; // defense
    public int res; // resistance
    public int arm; // armor
    public int agi; // agility
    public bool isDefending = false; // Whether or not the unit is defending
    public GameObject effectRenderer; // The object that manages rendering effects on the unit

    // Makes the unit take damage. Returns true if the unit died, false otherwise
    public bool TakeDamage(int damage)
    {
        cHP -= damage;
        bool isDead = cHP <= 0;

        if (isDead)
            cHP = 0;

        return isDead;
    }

    // Heals the unit's HP. cHP cannot exceed maxHP
    public void Heal(int heal)
    {
        cHP += heal;
        if (cHP > maxHP)
            cHP = maxHP;
    }
}
