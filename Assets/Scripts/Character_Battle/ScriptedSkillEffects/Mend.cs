using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScritpedSkillEffect for the Mend skill.
/// Heals HP and SP for 10% of max plus unit's level.
/// </summary>
public class Mend : ScriptedSkillEffect {
    [SerializeField]
    private AudioClip healSFX;

    public override string DoSkill(BattleUnit user, BattleUnit enemy, BattleSFXHandler sfxHandler) {
        // Get HP and SP to heal
        int hpHeal = (int) ((float) user.unit.maxHP * 0.1 + user.unit.level);
        int spHeal = (int) ((float) user.unit.maxSP * 0.1 + user.unit.level);

        // Play heal animation
        Animator animator = user.effectRenderer.GetComponent<Animator>();
        animator.SetTrigger(WeaponAnimation.HealAnimation.ToString());
        sfxHandler.PlaySFX(healSFX);

        // Recover HP and SP
        hpHeal = user.Heal(hpHeal);
        spHeal = user.RecoverSP(spHeal);

        // Get message
        string message = user.unit.unitName + " recovered " + hpHeal + " HP and " + spHeal + " SP.";
        return message;
    }
}
