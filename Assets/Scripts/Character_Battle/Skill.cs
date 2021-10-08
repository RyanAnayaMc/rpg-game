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


[CreateAssetMenu(fileName = "NewSkill", menuName = "RPG Element/Skill")]
// A special ability that can be used by a Unit during combat
public class Skill : ScriptableObject
{
    public Sprite icon; // The icon for this skill. Shown in menus.
    public string skillName; // The name of the skill.
    public string skillDescription; // A short description of this skill

    public AudioClip skillSFX; // THe sound effect to play when the skill is used
    public WeaponAnimation skillAnimation; // The animation to play for the skill

    public bool requiresWeaponType; // Whether or not this skill is locked to a weapon type
    public AttackType requiredAtkType; // The required weapon type for this skill if above bool is true
    public bool requiresSubtype; // Whether or not this skill requires a specific weapon subtype
    public WeaponSubtype requiredSubtype; // The required subtype for this skill if above bool is true

    public List<SkillStatScale> statScales; // List of all the stats this skill scales on with multipliers

    public TargetType targetType;

    public int recoilDamage; // Damage the user takes for using this skill
    public int costSP; // SP cost for the skill

    public bool useScriptedSkillEffects;
    public ScriptedSkillEffect scriptedSkillEffect;

    [SerializeField]
    private float unitChpMultiplier;
    [SerializeField]
    private float unitMhpMultiplier;
    [SerializeField]
    private float unitCspMultiplier;
    [SerializeField]
    private float unitMspMultiplier;
    [SerializeField]
    private float unitStrMultiplier;
    [SerializeField]
    private float unitMagMultiplier;
    [SerializeField]
    private float unitDexMultiplier;
    [SerializeField]
    private float unitDefMultiplier;
    [SerializeField]
    private float unitResMultiplier;
    [SerializeField]
    private float unitArmMultiplier;
    [SerializeField]
    private float unitAgiMultiplier;
    [SerializeField]
    private float unitMtMultiplier;

    [SerializeField]
    private float enemyChpMultiplier;
    [SerializeField]
    private float enemyMhpMultiplier;
    [SerializeField]
    private float enemyCspMultiplier;
    [SerializeField]
    private float enemyMspMultiplier;
    [SerializeField]
    private float enemyStrMultiplier;
    [SerializeField]
    private float enemyMagMultiplier;
    [SerializeField]
    private float enemyDexMultiplier;
    [SerializeField]
    private float enemyDefMultiplier;
    [SerializeField]
    private float enemyResMultiplier;
    [SerializeField]
    private float enemyArmMultiplier;
    [SerializeField]
    private float enemyAgiMultiplier;
    [SerializeField]
    private float enemyMtMultiplier;


    // Gets the amount of damage/healing to do for the skill
    public int getValue(Unit user, Unit enemy)
    {
        // Get skill damage
        float value =
            user.cHP * unitChpMultiplier +
            user.maxHP * unitMhpMultiplier +
            user.cSP * unitCspMultiplier +
            user.maxSP * unitMspMultiplier +
            user.str * unitStrMultiplier +
            user.mag * unitMagMultiplier +
            user.dex * unitDexMultiplier +
            user.def * unitDefMultiplier +
            user.res * unitResMultiplier +
            user.arm * unitArmMultiplier +
            user.agi * unitAgiMultiplier +
            user.weapon.might * unitMtMultiplier +
            enemy.cHP * enemyChpMultiplier +
            enemy.maxHP * enemyMhpMultiplier +
            enemy.cSP * enemyCspMultiplier +
            enemy.maxSP * enemyMspMultiplier +
            enemy.str * enemyStrMultiplier +
            enemy.mag * enemyMagMultiplier +
            enemy.dex * enemyDexMultiplier +
            enemy.def * enemyDefMultiplier +
            enemy.res * enemyResMultiplier +
            enemy.arm * enemyArmMultiplier +
            enemy.agi * enemyAgiMultiplier +
            enemy.weapon.might * enemyMtMultiplier;

        // Round damage to an integer and return it
        return (int) value;
    }

    // Do scripted skill effect
    public string doScriptedSkillEffect(BattleUnit user, BattleUnit enemy)
    {
        if (!useScriptedSkillEffects)
            return null;

        return scriptedSkillEffect.DoSkill(user, enemy);
    }
}
