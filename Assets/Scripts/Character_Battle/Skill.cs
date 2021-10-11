using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType {
    NONE,
    SELF,
    ENEMY
}

public enum AbilityType {
    BUFF,
    DAMAGE,
    HEAL,
    SCRIPTED
}

/// <summary>
/// A special ability that can be used by a Unit during combat.
/// </summary>
[CreateAssetMenu(fileName = "NewSkill", menuName = "RPG Element/Skill")]
public class Skill : ScriptableObject, ButtonTextable {
    /// <summary>
    /// The icon for this skill. Shown in menus.
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// The name of the skill.
    /// </summary>
    public string skillName;

    /// <summary>
    /// A short description of this skill.
    /// </summary>
    public string skillDescription;

    /// <summary>
    /// Whether or not this skill is locked to a weapon type.
    /// </summary>
    public bool requiresWeaponType;

    /// <summary>
    /// The required weapon type for this skill if requiresWeaponType.
    /// </summary>
    public AttackType requiredAtkType;

    /// <summary>
    /// SP cost for the skill.
    /// </summary>
    public int costSP;

    /// <summary>
    /// Whether or not this skill uses scripted skill effects.
    /// </summary>
    public bool useScriptedSkillEffects;

    /// <summary>
    /// If useScriptedSkillEffects, the ScriptedSkillEffect.
    /// </summary>
    public ScriptedSkillEffect scriptedSkillEffect;

    /// <summary>
    /// The sound effect to play when the skill is used if not scripted.
    /// </summary>
    public AudioClip skillSFX;

    /// <summary>
    /// The animation to play for the skill if not scripted.
    /// </summary>
    public WeaponAnimation skillAnimation;

    /// <summary>
    /// The target type for the skill.
    /// </summary>
    public TargetType targetType;

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

    [SerializeField]
    private int flatValue;

    /// <summary>
    /// Gets the amount of damage/healing to do for the skill if not scripted.
    /// </summary>
    /// <param name="user">The user of the skill.</param>
    /// <param name="enemy">The Unit targeted by the skill.</param>
    /// <returns></returns>
    public int getValue(Unit user, Unit enemy) {
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
            enemy.weapon.might * enemyMtMultiplier
            + flatValue;

        // Round damage to an integer and return it
        return (int) value;
    }

    /// <summary>
    /// If scripted, does the scripted skill effect.
    /// </summary>
    /// <param name="user">The user of the skill.</param>
    /// <param name="enemy">The target of the skill.</param>
    /// <param name="sfxHandler">The battle's BattleSFXHandler.</param>
    /// <returns>The dialogue text to display for the skill.</returns>
    public string doScriptedSkillEffect(BattleUnit user, BattleUnit enemy, BattleSFXHandler sfxHandler) {
        if (!useScriptedSkillEffects)
            return null;

        return scriptedSkillEffect.DoSkill(user, enemy, sfxHandler);
    }

	public string GetDescriptionText() {
        return skillDescription;
	}

	public string GetName() {
        return skillName;
	}

	public int GetNumber() {
        return costSP;
	}

	public Sprite GetIcon() {
        return icon;
	}
}