using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleUnit : MonoBehaviour, ButtonTextable {
    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
    public BattleController battleController;

    /// <summary>
    /// The unit's battle HUD controller. Player HUDs do not float
    /// but enemy HUDs do float.
    /// </summary>
    public HUDController unitHUD;

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

        UpdateHP();

        return isDead;
    }

    /// <summary>
    /// Performs an animation on this unit. Includes lighting effects.
    /// </summary>
    /// <param name="animation"></param>
    public void DoAnimation(WeaponAnimation animation) {
        // effectRenderer.GetComponent<EffectRenderer>().DoAnimation(animation);
        BattleAnimations.INSTANCE.DoAnimationOn(animation, effectRenderer.transform);
	}

    /// <summary>
    /// Heals the current unit's HP. cHP cannot exceed maxHP.
    /// </summary>
    /// <param name="heal">The amount to try to heal.</param>
    /// <returns>The amount actually healed.</returns>
    public int Heal(int heal) {
        if (unit.cHP + heal > unit.effMaxHP)
            heal = unit.effMaxHP - unit.cHP;
        unit.cHP += heal;

        UpdateHP();

        return heal;
    }

    /// <summary>
    /// Heals the current unit's SP. cHP cannot exceed maxSP.
    /// </summary>
    /// <param name="recover">The amount to try to recover.</param>
    /// <returns>The amount actually recovered.</returns>
    public int RecoverSP(int recover) {
        if (unit.cSP + recover > unit.effMaxSP)
            recover = unit.effMaxSP - unit.cSP;
        unit.cSP += recover;

        UpdateSP();

        return recover;
    }

    /// <summary>
    /// Consumes unit's SP by a given amount if possible.
    /// cSP will never be below zero.
    /// </summary>
    /// <param name="consumption">The amount of SP to try to consume.</param>
    /// <returns>Whether or not the SP was consumed.</returns>
    public bool ConsumeSP(int consumption) {
        if (unit.cSP < consumption)
            return false;
        else {
            unit.cSP -= consumption;
            UpdateSP();
            return true;
		}
	}

    /// <summary>
    /// Updates the unit's HP and SP on their HUD.
    /// </summary>
    public void UpdateHUD() {
        unitHUD.SetHP(unit);
        unitHUD.SetSP(unit);
	}

    /// <summary>
    /// Updates the unit's HP on their HUD
    /// </summary>
    public void UpdateHP() {
        unitHUD.SetHP(unit);
	}

    /// <summary>
    /// Updates the unit's SP on their HUD
    /// </summary>
    public void UpdateSP() {
        unitHUD.SetSP(unit);
	}

	public string GetDescriptionText() {
        return "Lv. " + unit.level + " " + unit.unitName;
	}

	public string GetName() {
        return unit.unitName;
	}

	public int GetNumber() {
        return unit.level;
	}

	public Sprite GetIcon() {
        return unit.unitIcon;
	}
}
