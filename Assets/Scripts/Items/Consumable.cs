using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType {
    HealthRecover,
    SpecialRecover,
    StatBuff,
    DamageDeal,
    DebuffInflict,
    Scripted
}

public enum Stat {
    CHP,
    MHP,
    CSP,
    MSP,
    STR,
    MAG,
    DEX,
    DEF,
    RES,
    ARM,
    AGI,
    MT
}

[CreateAssetMenu(fileName = "NewConsumable", menuName = "RPG Element/Item/Consumable")]
public class Consumable : Item {
    /// <summary>
    /// The ConsumableType of the item.
    /// </summary>
    public ConsumableType type;

    /// <summary>
    /// If type is HealRecover or SpecialRecover,
    /// the amount to recover.
    /// </summary>
    public int recovery;

    /// <summary>
    /// If type is StatBuff or DebuffInflict, the stat to change.
    /// </summary>
    public Stat stat;

    /// <summary>
    /// If type is StatBuff or DebuffInflict, the flat change to the stat.
    /// Positive numbers for buff, negative numbers for debuff.
    /// </summary>
    public int statChange;

    /// <summary>
    /// If type is StatBuff or DebuffInflict, the multiplier to the stat.
    /// Multiplier takes effect before the flat stat change.
    /// statMultiplier > 1 for buff, 0 < statMultiplier < 1 for debuff.
    /// </summary>
    public float statMultiplier;

    /// <summary>
    /// If type is DamageDeal, the damage to deal.
    /// </summary>
    public int damage;

    /// <summary>
    /// If nonscripted, the animation to perform.
    /// </summary>
    public WeaponAnimation animation;

    /// <summary>
    /// If nonscripted, the sound effect to play when using the item.
    /// </summary>
    public AudioClip soundEffect;

    /// <summary>
    /// If scripted, the skill effect to perform.
    /// </summary>
    public ScriptedSkillEffect scriptedSkillEffect;

    /// <summary>
    /// Performs the effect of the consumable item. Includes performing animation and SFX.
    /// </summary>
    /// <param name="attacker">The unit using the item.</param>
    /// <param name="defender">The enemy of the user.</param>
    /// <param name="sfxHandler">The battle's BattleSFXHandler.</param>
    /// <returns>
    /// Item1 - The dialogue box message.
    /// Item2 - Whether or not defender died.
    /// </returns>
    public (string, bool) Use(BattleUnit attacker, BattleUnit defender, BattleSFXHandler sfxHandler) {
        Debug.Log("2 " + attacker.unit.cHP + " " + defender.unit.cHP);
        switch (type) {
            case ConsumableType.HealthRecover:
                int heal = attacker.Heal(recovery);
                playAnimationAndSFX(attacker, sfxHandler);
                return (attacker.unit.unitName + " restored " + heal + " HP.", false);
            case ConsumableType.SpecialRecover:
                int recover = attacker.RecoverSP(recovery);
                playAnimationAndSFX(attacker, sfxHandler);
                return (attacker.unit.unitName + " recovered " + recover + " SP.", false);
            case ConsumableType.StatBuff:
                return ("not implemented yet", false);
            case ConsumableType.DebuffInflict:
                return ("not implemented yet", false);
            case ConsumableType.DamageDeal:
                bool dead = defender.TakeDamage(damage);
                playAnimationAndSFX(defender, sfxHandler);
                return (defender.unit.unitName + " took " + damage + " item damage!", dead);
            case ConsumableType.Scripted:
                return (scriptedSkillEffect.DoSkill(attacker, defender, sfxHandler), defender.unit.cHP <= 0);
		}

        return ("error", false);
	}

    private void playAnimationAndSFX(BattleUnit unit, BattleSFXHandler sfxHandler) {
        unit.DoAnimation(animation);
        sfxHandler.PlaySFX(soundEffect);
    }
}
