using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basis for scripted skill effects.
/// </summary>
public abstract class ScriptedSkillEffect : MonoBehaviour {

    /// <summary>
    /// Performs the scripted skill effects.
    /// </summary>
    /// <param name="user">The user of the skill.</param>
    /// <param name="enemy">The enemy of the skill user.</param>
    /// <param name="sfxHandler">The battle's BattleSFXHandler.</param>
    /// <returns>The dialogue text to display.</returns>
    public abstract string DoSkill(BattleUnit user, BattleUnit enemy, BattleSFXHandler sfxHandler);
}
