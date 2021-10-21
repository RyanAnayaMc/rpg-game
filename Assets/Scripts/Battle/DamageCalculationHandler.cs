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
    /// <param name="effDefender">The Unit being attacked.</param>
    /// <returns>
    /// Item1 - the dialogue box text to display
    /// Item2 - the damage to deal to the enemy
    /// </returns>
    public (string, int) NormalAttack(Unit attacker, Unit effDefender) {
        AttackType type = attacker.weapon.atkType;

        // Calculate damage
        int damage = 0;
        float accuracy = (1 - ((float) effDefender.effAgi - (float) attacker.effDex) / (float) attacker.effDex);
        float critChance = ((float) attacker.effDex - (float) effDefender.effAgi) / (float) attacker.effDex;
        float rng1 = Random.value;
        float rng2 = Random.value;
        bool miss = rng1 > accuracy;
        bool crit = rng2 < critChance;
        string text = "";

        switch (type) {
            case AttackType.Melee:
                damage = miss ? 0 : attacker.effStr + attacker.weapon.might - effDefender.effDef;
                if (effDefender.isDefending)
                    damage /= 2;
                text = miss ? attacker.unitName + " missed and dealt no damage!" : effDefender.unitName + " took " + damage + " melee damage!";
                break;
            case AttackType.Magic:
                damage = miss ? 0 : attacker.effMag + attacker.weapon.might - effDefender.effRes;
                if (effDefender.isDefending)
                    damage /= 2;
                text = miss ? attacker.unitName + " missed and dealt no damage!" : effDefender.unitName + " took " + damage + " meffAgic damage!";
                break;
            case AttackType.Ranged:
                damage = crit ? attacker.weapon.might : attacker.weapon.might - effDefender.effArm;
                if (effDefender.isDefending)
                    damage /= 2;
                text = crit ? attacker.unitName + " did " + damage + " critical ranged damage to " + effDefender.unitName + "!"
                    : effDefender.unitName + " took " + damage + " ranged damage!";
                break;
        }

        if (damage < 0) {
            damage = 0;
            text = attacker.unitName + "'s attack did 0 damage to " + effDefender.unitName + "!";
        }

        return (text, damage);
    }
}
