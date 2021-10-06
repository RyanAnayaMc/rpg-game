using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    // A class that controls a battle HUD with basic information of the enemy
    public Text unitName; // The name of the Unit being displayed
    public Text hpText; // Text that displays cHP / maxHP
    public Slider hpBar; // HP bar

    // Sets up the HUD based on a Unit's data
    public void SetupHUD(Unit unit)
    {
        unitName.text = unit.unitName;
        hpBar.maxValue = unit.maxHP;
        hpBar.value = unit.cHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
    }

    // Changes the HUD's cHP related values based on a Unit's condition
    public void SetHP(Unit unit)
    {
        hpBar.value = unit.cHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
    }
}
