using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    // A class that controls a battle HUD with basic information of the enemy
    public TMP_Text unitName; // The name of the Unit being displayed
    public TMP_Text hpText; // Text that displays cHP / maxHP
    public Slider hpBar; // HP bar
    public TMP_Text spText; // Text that displays cSP / maxSP
    public Slider spBar; // SP bar

    // Sets up the HUD based on a Unit's data
    public void SetupHUD(Unit unit)
    {
        unitName.text = unit.unitName;
        hpBar.maxValue = unit.maxHP;
        hpBar.value = unit.cHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
        spBar.maxValue = unit.maxSP;
        spBar.value = unit.cSP;
        spText.text = unit.cSP + "/" + unit.maxSP;
    }

    // Changes the HUD's cHP related values based on a Unit's condition
    public void SetHP(Unit unit)
    {
        hpBar.value = unit.cHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
    }

    // Changes the HUD's cSP related values vased on a Unit's condition
    public void SetSP(Unit unit)
    {
        spBar.value = unit.cSP;
        spText.text = unit.cSP + "/" + unit.maxSP;
    }
}
