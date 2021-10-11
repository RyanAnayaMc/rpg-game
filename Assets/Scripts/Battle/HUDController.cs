using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour {
    // A class that controls a battle HUD with basic information of the enemy
    public TMP_Text unitName; // The name of the Unit being displayed
    public TMP_Text hpText; // Text that displays cHP / maxHP
    public Slider hpBar; // HP bar
    public TMP_Text spText; // Text that displays cSP / maxSP
    public Slider spBar; // SP bar

    /// <summary>
    /// Sets up the HUD based on a Unit's data
    /// </summary>
    /// <param name="unit">The Unit to set up this HUD with.</param>
    public void SetupHUD(Unit unit) {
        unitName.text = unit.unitName;
        hpBar.maxValue = unit.maxHP;
        hpBar.value = unit.cHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
        spBar.maxValue = unit.maxSP;
        spBar.value = unit.cSP;
        spText.text = unit.cSP + "/" + unit.maxSP;
    }

    /// <summary>
    /// Changes the HUD's cHP and maxHP values based on a Unit's condition
    /// </summary>
    /// <param name="unit">The Unit to update this HUD with.</param>
    public void SetHP(Unit unit) {
        hpBar.value = unit.cHP;
        hpBar.maxValue = unit.maxHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
    }

    /// <summary>
    /// Changes the HUD's cSP and maxSP values vased on a Unit's condition
    /// </summary>
    /// <param name="unit">The Unit to update this HUD with.</param>
    public void SetSP(Unit unit) {
        spBar.value = unit.cSP;
        spBar.maxValue = unit.maxSP;
        spText.text = unit.cSP + "/" + unit.maxSP;
    }
}
