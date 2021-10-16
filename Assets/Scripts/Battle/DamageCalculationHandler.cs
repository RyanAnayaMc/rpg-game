using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculationHandler : MonoBehaviour {
    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
    public BattleController battleController;

    /// <summary>
    /// Gets damage done for a normal attack and text to display.
    /// </summary>
    /// <param name="attacker">The attacking Unit.</param>
    /// <param name="defender">The Unit being attacked.</param>
    /// <returns>
    /// Item1 - the dialogue box text to display
    /// Item2 - the damage to deal to the enemy
    /// </returns>
    public (string, int) NormalAttack(Unit attacker, Unit defender) {
        AttackType type = attacker.weapon.atkType;

        // Calculate damage
        int damage = 0;
        float accuracy = (1 - ((float) defender.agi - (float) attacker.dex) / (float) attacker.dex);
        float critChance = ((float) attacker.dex - (float) defender.agi) / (float) attacker.dex;
        float rng1 = Random.value;
        float rng2 = Random.value;
        bool miss = rng1 > accuracy;
        bool crit = rng2 < critChance;
        string text = "";

        switch (type) {
            case AttackType.MELEE:
                damage = miss ? 0 : attacker.str + attacker.weapon.might - defender.def;
                if (defender.isDefending)
                    damage /= 2;
                text = miss ? attacker.unitName + " missed and dealt no damage!" : defender.unitName + " took " + damage + " melee damage!";
                break;
            case AttackType.MAGIC:
                damage = miss ? 0 : attacker.mag + attacker.weapon.might - defender.res;
                if (defender.isDefending)
                    damage /= 2;
                text = miss ? attacker.unitName + " missed and dealt no damage!" : defender.unitName + " took " + damage + " magic damage!";
                break;
            case AttackType.RANGED:
                damage = crit ? attacker.weapon.might : attacker.weapon.might - defender.arm;
                if (defender.isDefending)
                    damage /= 2;
                text = crit ? attacker.unitName + " did " + damage + " critical ranged damage to " + defender.unitName + "!"
                    : defender.unitName + " took " + damage + " ranged damage!";
                break;
        }

        if (damage < 0) {
            damage = 0;
            text = attacker.unitName + "'s attack did 0 damage to " + defender.unitName + "!";
        }

        return (text, damage);
    }

    /// <summary>
    /// Performs a Skill.
    /// </summary>
    /// <param name="skill">The skill being used.</param>
    /// <param name="attacker">The unit attacking.</param>
    /// <param name="defender">The unit being attacked.</param>
    /// <param name="sfxHandler">The battle's BattleSFXHandler.</param>
    /// <returns>
    /// Item1 - The dialogue text message to display.
    /// Item2 - The damage done or HP to heal to user. Meaningless if skill is scripted.
    /// Item3 - True if it is a damage skill, false if it is a heal skill. Meaningless if skill is scripted.
    /// Item4 - Whether or not the skill effect was scripted.
    /// </returns>
    public (string, int, bool, bool) SpecialAttack(Skill skill, BattleUnit attacker, BattleUnit defender, BattleSFXHandler sfxHandler) {
        string diagMsg = attacker.unit.unitName + " used " + skill.skillName + "!\n";

        if (!skill.useScriptedSkillEffects) {
            if (skill.targetType == TargetType.ENEMY) {
                int damage = skill.getValue(attacker.unit, defender.unit);

                if (defender.unit.isDefending)
                    damage /= 2;

                if (damage < 0)
                    damage = 0;

                diagMsg += attacker.unit.unitName + " did " + damage + " special damage to " + defender.unit.unitName + ".";
                return (diagMsg, damage, true, false);
            } else if (skill.targetType == TargetType.SELF) {
                int heal = skill.getValue(attacker.unit, defender.unit);
               diagMsg += attacker.unit.unitName + " healed " + heal + " HP.";
                return (diagMsg, heal, false, false);
            }
        } else {
            diagMsg += skill.doScriptedSkillEffect(attacker, defender, sfxHandler);
            return (diagMsg, -1, true, true);
        }

        return (null, -1, false, false);
    }
}
