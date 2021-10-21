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
    public bool isFloating; // Is this a floating HP bar?

    /// <summary>
    /// Sets up the HUD based on a Unit's data
    /// </summary>
    /// <param name="unit">The Unit to set up this HUD with.</param>
    public void SetupHUD(Unit unit, bool isFloating = false) {
        if (unitName != null)
            unitName.text = unit.unitName;
        
        if (hpBar != null) {
            hpBar.maxValue = unit.effMaxHP;
            hpBar.value = unit.cHP;
        }

        if (hpText != null)
            hpText.text = unit.cHP + "/" + unit.effMaxHP;

        if (spBar != null) {
            spBar.maxValue = unit.effMaxSP;
            spBar.value = unit.cSP;
        }
        
        if (spText != null)
            spText.text = unit.cSP + "/" + unit.effMaxSP;
    }

    /// <summary>
    /// Changes the HUD's cHP and maxHP values based on a Unit's condition
    /// </summary>
    /// <param name="unit">The Unit to update this HUD with.</param>
    public void SetHP(Unit unit) {
        if (hpBar != null) {
            hpBar.maxValue = unit.effMaxHP;
            hpBar.value = unit.cHP;
        }

        if (hpText != null)
            hpText.text = unit.cHP + "/" + unit.effMaxHP;
    }

    /// <summary>
    /// Changes the HUD's cSP and maxSP values vased on a Unit's condition
    /// </summary>
    /// <param name="unit">The Unit to update this HUD with.</param>
    public void SetSP(Unit unit) {
        if (spBar != null) {
            spBar.maxValue = unit.effMaxSP;
            spBar.value = unit.cSP;
        }

        if (spText != null)
            spText.text = unit.cSP + "/" + unit.effMaxSP;
    }

    /// <summary>
    /// Shows the unit's HP bar if it's floating.
    /// Does nothing to non-floating HUDs.
    /// </summary>
    public void ShowHPBar() {
        if (isFloating)
            hpBar.gameObject.SetActive(true);
	}

    /// <summary>
    /// Hides the unit's HP bar if it's floating.
    /// Does nothing to non-floating HUDs.
    /// </summary>
    public void HideHPBar() {
        if (isFloating)
            hpBar.gameObject.SetActive(false);
	}

    /// <summary>
    /// Shows the unit's SP bar if it's floating.
    /// Does nothing to non-floating HUDs.
    /// </summary>
    public void ShowSPBar() {
        if (isFloating)
            spBar.gameObject.SetActive(true);
	}

    /// <summary>
    /// Hides the unit's SP bar if it's floating.
    /// Does nothing to non-floating HUDs.
    /// </summary>
    public void HideSPBar() {
        if (isFloating)
            spBar.gameObject.SetActive(false);
	}

    /// <summary>
    /// Shows the unit's HP and SP bars if it's floating.
    /// Does nothing to non-floating HUDs.
    /// </summary>
    public void ShowBars() {
        if (isFloating) {
            hpBar.gameObject.SetActive(true);
            spBar.gameObject.SetActive(true);
        }
	}

    /// <summary>
    /// Hides the unit's HP and SP bars if it's floating.
    /// Does nothing to non-floating HUDs.
    /// </summary>
    public void HideBars() {
        if (isFloating) {
            hpBar.gameObject.SetActive(false);
            spBar.gameObject.SetActive(false);
        }
	}
}
