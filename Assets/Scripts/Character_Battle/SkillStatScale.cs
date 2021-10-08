using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for all stats
public enum Stat
{
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
    MT,
    FLAT
}

public class SkillStatScale{
    public Stat stat; // The stat to scale from
    public float multiplier; // A multiplier to this stat
    public bool forSelf; // Whether or not this stat is for the user or the target
}
