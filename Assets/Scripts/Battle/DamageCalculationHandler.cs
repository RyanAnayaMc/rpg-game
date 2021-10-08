using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculationHandler : MonoBehaviour
{
    // Gets damage done for a normal attack and text to display
    public (string, int) NormalAttack(Unit attacker, Unit defender)
    {
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

        Debug.Log(rng1 + " " + rng2 + " " + accuracy + " " + critChance);

        switch (type)
        {
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

        if (damage < 0)
        {
            damage = 0;
            text = attacker.unitName + "'s attack did 0 damage to " + defender.unitName + "!";
        }

        return (text, damage);
    }

    public (string, int, bool) SpecialAttack(Skill skill, BattleUnit attacker, BattleUnit defender)
    {
        // TODO scripted skill effect handling

        if (skill.targetType == TargetType.ENEMY)
        {
            int damage = skill.getValue(attacker.unit, defender.unit);
            string diagMsg = attacker.unit.unitName + " did " + damage + " special damage to " + defender.unit.unitName + ".";
            return (diagMsg, damage, true);
        }
        else if (skill.targetType == TargetType.SELF)
        {
            int heal = skill.getValue(attacker.unit, defender.unit);
            string diagMsg = attacker.unit.unitName + " healed " + heal + " HP.";
            return (diagMsg, heal, false);
        }

        return (null, 0, false);
    }
}
