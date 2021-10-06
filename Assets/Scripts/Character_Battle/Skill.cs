using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    NONE,
    SELF,
    ENEMY
}

public enum AbilityType
{
    BUFF,
    DAMAGE,
    HEAL,
    ABSORB
}

// A special ability that can be used by a Unit during combat
public class Skill : MonoBehaviour
{
    public Sprite icon; // The icon for this skill. Shown in menus.
    public string skillName; // The name of the skill.
    public string skillDescription; // A short description of this skill

    public bool requiresWeaponType; // Whether or not this skill is locked to a weapon type
    public AttackType requiredAtkType; // The required weapon type for this skill if above bool is true
    public bool requiresSubtype; // Whether or not this skill requires a specific weapon subtype
    public WeaponSubtype requiredSubtype; // The required subtype for this skill if above bool is true

    public List<SkillStatScale> statScales; // List of all the stats this skill scales on with multipliers

    public TargetType targetType;

    public int recoilDamage; // Damage the user takes for using this skill
    public int costSP; // SP cost for the skill

    // Gets the amount of damage/healing to do for the skill
    public int getValue(Unit user, Unit enemy)
    {
        // Default value is zero
        float value = 0;

        // Get scale for each stat
        foreach (SkillStatScale scale in statScales)
        {
            Unit unit = scale.forSelf ? user : enemy;
            float tempVal = 0;

            switch (scale.stat)
            {
                case Stat.AGI:
                    tempVal = unit.agi;
                    break;
                case Stat.ARM:
                    tempVal = unit.arm;
                    break;
                case Stat.CHP:
                    tempVal = unit.cHP;
                    break;
                case Stat.CSP:
                    tempVal = unit.cSP;
                    break;
                case Stat.DEF:
                    tempVal = unit.def;
                    break;
                case Stat.DEX:
                    tempVal = unit.dex;
                    break;
                case Stat.FLAT:
                    tempVal = 1;
                    break;
                case Stat.MAG:
                    tempVal = unit.mag;
                    break;
                case Stat.MHP:
                    tempVal = unit.maxHP;
                    break;
                case Stat.MSP:
                    tempVal = unit.maxSP;
                    break;
                case Stat.RES:
                    tempVal = unit.res;
                    break;
                case Stat.STR:
                    tempVal = unit.str;
                    break;
            }

            // Add the scaled stat to the total
            value += tempVal * scale.multiplier;
        }

        // Round damage to an integer and return it
        return (int) value;
    }
}
