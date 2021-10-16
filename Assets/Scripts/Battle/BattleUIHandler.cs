using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUIHandler : MonoBehaviour {
    [SerializeField]
    private BattleAnimationHandler animationHandler; // The BattleAnimationHandler for the battle

    public BattleController battleController;

    [SerializeField]
    private GameObject playerOptionWindow; // The window with the player's options
    [SerializeField]
    private TMP_Text attackButtonText; // The text on the "Attack" button
    [SerializeField]
    private SkillMenuController skillMenuController;
    [SerializeField]
    private ItemMenuController itemMenuController;

    [SerializeField]
    private HUDController playerHUD; // The location of the HUD that shows the player's info
    public HUDController enemyHUD; // The location of the HUD that shows the enemy's info

    [SerializeField]
    private GameObject playerPhaseUI; // The GameObject to display at the beginning of Player Phase
    [SerializeField]
    private GameObject enemyPhaseUI; // The GameObject to display at the beginning of Enemy Phase

    [SerializeField]
    private DialogueBoxController dialogueBox; // The battle's dialogue box controller

    private bool isSetup = false;

    /// <summary>
    /// Sets up the player and enemy HUD displays.
    /// </summary>
    /// <param name="playerUnit">The Unit representing the player.</param>
    /// <param name="enemyUnit">The Unit representing the enemy.</param>
    public void setupHUD(Unit playerUnit, Unit enemyUnit) {
        if (isSetup)
            return;

        isSetup = true;
        playerOptionWindow.SetActive(false);


        // Display player and enemy HUDs
        playerHUD.SetupHUD(playerUnit);

        GameObject enemyHUDPrefab = Resources.Load<GameObject>("UI/Battle/EnemyHUD");
        enemyHUD = Instantiate(enemyHUDPrefab, battleController.enemyLocation).GetComponentInChildren<HUDController>();
        enemyHUD.SetupHUD(enemyUnit);

        dialogueBox.ShowDialouge("Engaging " + enemyUnit.unitName + "!", 3);

        // Update text on attack button based on equipped weapon
        AttackType atkType = playerUnit.weapon.atkType;
        switch (atkType) {
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

    /// <summary>
    /// Shows the Player Phase image
    /// </summary>
    public void ShowPlayerPhaseImage() {
        StartCoroutine(showPlayerPhaseCoroutine());
    }

    private IEnumerator showPlayerPhaseCoroutine() {
        animationHandler.fadeIn(playerPhaseUI, 0.03f);
        yield return new WaitForSeconds(1);
        animationHandler.fadeOut(playerPhaseUI, 0.03f);
    }

    /// <summary>
    /// Shows the Enemy Phase image
    /// </summary>
    public void ShowEnemyPhaseImage() {
        StartCoroutine(showEnemyPhaseCoroutine());
    }

    private IEnumerator showEnemyPhaseCoroutine() {
        animationHandler.fadeIn(enemyPhaseUI, 0.03f);
        yield return new WaitForSeconds(1);
        animationHandler.fadeOut(enemyPhaseUI, 0.03f);
    }

    /// <summary>
    /// Shows the player's option window
    /// </summary>
    public void ShowPlayerOptionWindow() {
        animationHandler.stretchIn(playerOptionWindow, 0.1f);
    }

    /// <summary>
    /// Hides the player's option window
    /// </summary>
    public void HidePlayerOptionWindow() {
        animationHandler.stretchOut(playerOptionWindow, 0.2f);
    }

    /// <summary>
    /// Shows the skill menu
    /// </summary>
    public void ShowSkillWindow() {
        GameObject skillWindow = skillMenuController.gameObject;
        skillMenuController.SetSkillMenu();
        animationHandler.stretchIn(skillWindow, 0.1f);
    }

    /// <summary>
    /// Hides the skill menu
    /// </summary>
    public void HideSkillWindow() {
        GameObject skillWindow = skillMenuController.gameObject;
        animationHandler.stretchOut(skillWindow, 0.1f);
    }

    /// <summary>
    /// Shows the Item menu
    /// </summary>
    /// <param name="items">The items the player has</param>
    public void ShowItemsWindow(List<InventoryItem> items) {
        GameObject itemWindow = itemMenuController.gameObject;
        itemMenuController.SetItemMenu(items);
        animationHandler.stretchIn(itemWindow, 0.1f);
	}

    /// <summary>
    /// Hides the item menu
    /// </summary>
    public void HideItemsWindow() {
        GameObject itemWindow = itemMenuController.gameObject;
        itemMenuController.ResetItemMenu();
        animationHandler.stretchOut(itemWindow, 0.1f);
	}

    /// <summary>
    /// Shows a new text message on the dialogue box
    /// </summary>
    /// <param name="newDiagText">The text to display in the dialogue box</param>
    public void DisplayDialogueText(string newDiagText) {
        dialogueBox.EditDialogue(newDiagText, 3);
    }

    /// <summary>
    /// Updates the cHP, mHP, cSP, and mSP values on the player's HUD.
    /// </summary>
    /// <param name="playerUnit">The unit representing the Player.</param>
    public void SetPlayerHUD(Unit playerUnit) {
        playerHUD.SetHP(playerUnit);
        playerHUD.SetSP(playerUnit);
    }

    /// <summary>
    /// Updates the cHP, mHP, cSP, and mSP values on the player's HUD.
    /// </summary>
    /// <param name="enemyUnit">The uint representing the Enemy.</param>
    public void SetEnemyHUD(Unit enemyUnit) {
        enemyHUD.SetHP(enemyUnit);
        enemyHUD.SetSP(enemyUnit);
    }
}
