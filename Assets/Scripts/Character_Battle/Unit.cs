using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "RPG Element/Unit")]
public class Unit : ScriptableObject
{
    // Basis for a playable or enemy Unit that can see combat
    public Sprite unitSprite;
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
    public List<Skill> skills; // Skills the Unit can use in combat
}
