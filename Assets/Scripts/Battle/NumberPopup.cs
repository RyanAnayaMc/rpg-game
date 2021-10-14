using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum NumberType {
    NONE,
    Damage,
    AbsorbDamage,
    Heal,
    SpHeal
}

[RequireComponent(typeof(TMP_Text))]
public class NumberPopup : MonoBehaviour {
    private int number;
    private NumberType numType;
    private TMP_Text numberText;

    void Start() {
        
    }

    void Update() {
        
    }
}

