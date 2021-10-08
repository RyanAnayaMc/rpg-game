using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptedSkillEffect : MonoBehaviour
{
    public abstract string DoSkill(BattleUnit user, BattleUnit enemy);
}
