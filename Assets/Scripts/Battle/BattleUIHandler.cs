using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUIHandler : MonoBehaviour
{
    [SerializeField]
    private BattleAnimationHandler animationHandler; // The BattleAnimationHandler for the battle

    [SerializeField]
    private GameObject playerOptionWindow; // The window with the player's options
    [SerializeField]
    private TMP_Text attackButtonText; // The text on the "Attack" button
    [SerializeField]
    private SkillMenuController skillMenuController;

    [SerializeField]
    private HUDController playerHUD; // The location of the HUD that shows the player's info
    [SerializeField]
    private HUDController enemyHUD; // The location of the HUD that shows the enemy's info

    [SerializeField]
    private GameObject playerPhaseUI; // The GameObject to display at the beginning of Player Phase
    [SerializeField]
    private GameObject enemyPhaseUI; // The GameObject to display at the beginning of Enemy Phase

    [SerializeField]
    private TMP_Text dialogueText; // The text on the dialogue box

    private bool isSetup = false;

    // Sets up the UI
    public void setupHUD(Unit playerUnit, Unit enemyUnit)
    {
        if (isSetup)
            return;

        isSetup = true;
        playerOptionWindow.SetActive(false);


        // Display player and enemy HUDs
        playerHUD.SetupHUD(playerUnit);
        enemyHUD.SetupHUD(enemyUnit);

        dialogueText.text = "Engaging " + enemyUnit.unitName + "!";

        // Update text on attack button based on equipped weapon
        AttackType atkType = playerUnit.weapon.atkType;
        switch (atkType)
        {
            case AttackType.MELEE:
                attackButtonText.text = "Melee";
                break;
            case AttackType.MAGIC:
                attackButtonText.text = "Cast";
                break;
            case AttackType.RANGED:
                attackButtonText.text = "Fire";
                break;
        }

        // Hide the skill menu but make sure it is enabled
        skillMenuController.gameObject.SetActive(true);
        animationHandler.stretchOut(skillMenuController.gameObject, 1f);
    }

    // Shows the Player Phase image
    public void ShowPlayerPhaseImage()
    {
        StartCoroutine(showPlayerPhaseCoroutine());
    }

    private IEnumerator showPlayerPhaseCoroutine()
    {
        animationHandler.fadeIn(playerPhaseUI, 0.03f);
        yield return new WaitForSeconds(1);
        animationHandler.fadeOut(playerPhaseUI, 0.03f);
    }

    // Shows the Enemy Phase image
    public void ShowEnemyPhaseImage()
    {
        StartCoroutine(showEnemyPhaseCoroutine());
    }

    private IEnumerator showEnemyPhaseCoroutine()
    {
        animationHandler.fadeIn(enemyPhaseUI, 0.03f);
        yield return new WaitForSeconds(1);
        animationHandler.fadeOut(enemyPhaseUI, 0.03f);
    }

    // Shows the player's option window
    public void ShowPlayerOptionWindow()
    {
        animationHandler.stretchIn(playerOptionWindow, 0.1f);
    }

    // Hides the player's option window
    public void HidePlayerOptionWindow()
    {
        animationHandler.stretchOut(playerOptionWindow, 0.2f);
    }

    // Shows the skill menu
    public void ShowSkillWindow()
    {
        GameObject skillWindow = skillMenuController.gameObject;
        skillMenuController.SetSkillMenu();
        animationHandler.stretchIn(skillWindow, 0.1f);
    }

    // Shows the skill menu
    public void HideSkillWindow()
    {
        GameObject skillWindow = skillMenuController.gameObject;
        animationHandler.stretchOut(skillWindow, 0.1f);
    }

    // Shows a new text message on the dialogue box
    public void DisplayDialogueText(string newDiagText)
    {
        dialogueText.text = newDiagText;
    }

    public void SetPlayerHUD(Unit playerUnit)
    {
        playerHUD.SetHP(playerUnit);
        playerHUD.SetSP(playerUnit);
    }

    public void SetEnemyHUD(Unit enemyUnit)
    {
        enemyHUD.SetHP(enemyUnit);
        enemyHUD.SetSP(enemyUnit);
    }
}
