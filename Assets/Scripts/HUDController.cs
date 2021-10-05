using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Text name;
    public Text hpText;
    public Slider hpBar;

    public void SetupHUD(Unit unit)
    {
        name.text = unit.name;
        hpBar.maxValue = unit.maxHP;
        hpBar.value = unit.cHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
    }

    public void SetHP(Unit unit)
    {
        hpBar.value = unit.cHP;
        hpText.text = unit.cHP + "/" + unit.maxHP;
    }
}
