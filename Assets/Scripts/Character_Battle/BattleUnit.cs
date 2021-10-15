using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleUnit : MonoBehaviour {
    /// <summary>
    /// The Unit associated with this BattleUnit. Contains all attributes.
    /// </summary>
    public Unit unit;

    /// <summary>
    /// The BattleUnit's effect renderer. Has an Animator component that handles
    /// animation effects on the unit.
    /// </summary>
    [SerializeField]
    private GameObject effectRenderer;

    /// <summary>
    /// Makes the unit take damage.
    /// </summary>
    /// <param name="damage">The amount of damage to do.</param>
    /// <returns>Whether or not the unit died as a result of the attack.</returns>
    public bool TakeDamage(int damage){
        unit.cHP -= damage;
        bool isDead = unit.cHP <= 0;

        if (isDead)
            unit.cHP = 0;

        return isDead;
    }

    /// <summary>
    /// Performs an animation on this unit. Includes lighting effects.
    /// </summary>
    /// <param name="animation"></param>
    public void DoAnimation(WeaponAnimation animation) {
        effectRenderer.GetComponent<EffectRenderer>().DoAnimation(animation);
	}

    /// <summary>
    /// Heals the current unit's HP. cHP cannot exceed maxHP.
    /// </summary>
    /// <param name="heal">The amount to try to heal.</param>
    /// <returns>The amount actually healed.</returns>
    public int Heal(int heal) {
        if (unit.cHP + heal > unit.maxHP)
            heal = unit.maxHP - unit.cHP;
        unit.cHP += heal;

        return heal;
    }

    /// <summary>
    /// Heals the current unit's SP. cHP cannot exceed maxSP.
    /// </summary>
    /// <param name="recover">The amount to try to recover.</param>
    /// <returns>The amount actually recovered.</returns>
    public int RecoverSP(int recover) {
        if (unit.cSP + recover > unit.maxSP)
            recover = unit.maxSP - unit.cSP;
        unit.cSP += recover;

        return recover;
    }
}
