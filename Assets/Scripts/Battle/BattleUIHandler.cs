using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class BattleUIHandler : MonoBehaviour {
    [SerializeField]
    private BattleAnimationHandler animationHandler; // The BattleAnimationHandler for the battle

    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
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
    private EnemyTargetingMenuController enemyTargetingMenuController;

    [SerializeField]
    private HUDController playerHUD; // The location of the HUD that shows the player's info

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
    public async Task setupHUD(BattleUnit playerUnit, List<BattleUnit> enemyUnits) {
        skillMenuController.battleController = battleController;
        itemMenuController.battleController = battleController;
        enemyTargetingMenuController.battleController = battleController;

        if (isSetup)
            return;

        isSetup = true;
        playerOptionWindow.SetActive(false);


        // Setup player's HUD
        playerHUD.SetupHUD(playerUnit.unit);
        playerUnit.unitHUD = playerHUD;
        
        // Setup HUD for enemies
        GameObject enemyHUDPrefab = Resources.Load<GameObject>("UI/Battle/EnemyHUD");
        foreach (BattleUnit enemyUnit in enemyUnits) {
            enemyUnit.unitHUD = Instantiate(enemyHUDPrefab, enemyUnit.transform).GetComponentInChildren<HUDController>();
            enemyUnit.unitHUD.SetupHUD(enemyUnit.unit);
            enemyUnit.unitHUD.HideBars();
        }

        // Show battle start message
        if (enemyUnits.Count == 1)
            dialogueBox.ShowDialouge("Engaging " + enemyUnits[0].unit.unitName + "!", 3);
        else
            dialogueBox.ShowDialouge("Engaging " + enemyUnits.Count + " enemies!", 3);
                 
        // Update text on attack button based on equipped weapon
        AttackType atkType = playerUnit.unit.weapon.atkType;
        switch (atkType) {
            case AttackType.Melee:
                attackButtonText.text = "Melee";
                break;
            case AttackType.Magic:
                attackButtonText.text = "Cast";
                break;
            case AttackType.Ranged:
                attackButtonText.text = "Fire";
                break;
        }

        // Hide the skill menu but make sure it is enabled
        skillMenuController.gameObject.SetActive(true);
        await animationHandler.StretchOut(skillMenuController.gameObject, 1f);
    }

    /// <summary>
    /// Shows the Player Phase image
    /// </summary>
    public void ShowPlayerPhaseImage() {
        StartCoroutine(showPlayerPhaseCoroutine());
    }

    private IEnumerator showPlayerPhaseCoroutine() {
        animationHandler.FadeIn(playerPhaseUI, 0.03f);
        yield return new WaitForSeconds(1);
        animationHandler.FadeOut(playerPhaseUI, 0.03f);
    }

    /// <summary>
    /// Shows the Enemy Phase image
    /// </summary>
    public void ShowEnemyPhaseImage() {
        StartCoroutine(showEnemyPhaseCoroutine());
    }

    private IEnumerator showEnemyPhaseCoroutine() {
        animationHandler.FadeIn(enemyPhaseUI, 0.03f);
        yield return new WaitForSeconds(1);
        animationHandler.FadeOut(enemyPhaseUI, 0.03f);
    }

    /// <summary>
    /// Shows the player's option window
    /// </summary>
    public async Task ShowPlayerOptionWindow() {
        await animationHandler.StretchIn(playerOptionWindow, 0.1f);
    }

    /// <summary>
    /// Hides the player's option window
    /// </summary>
    public async Task HidePlayerOptionWindow() {
        await animationHandler.StretchOut(playerOptionWindow, 0.2f);
    }

    /// <summary>
    /// Shows the skill menu
    /// </summary>
    public async Task ShowSkillWindow() {
        GameObject skillWindow = skillMenuController.gameObject;
        skillMenuController.SetSkillMenu();
        await animationHandler.StretchIn(skillWindow, 0.1f);
    }

    /// <summary>
    /// Hides the skill menu
    /// </summary>
    public async Task HideSkillWindow() {
        GameObject skillWindow = skillMenuController.gameObject;
        await animationHandler.StretchOut(skillWindow, 0.1f);
    }

    /// <summary>
    /// Shows the enemy targeting menu
    /// </summary>
    public async Task ShowEnemyTargetingWindow() {
        GameObject enemyWindow = enemyTargetingMenuController.gameObject;
        enemyTargetingMenuController.SetEnemyMenu(battleController.enemyUnits);
        await animationHandler.StretchIn(enemyWindow, 0.1f);
    }

    /// <summary>
    /// Hides the enemy targeting menu
    /// </summary>
    public async Task HideEnemyTargetingWindow() {
        GameObject enemyWindow = enemyTargetingMenuController.gameObject;
        enemyTargetingMenuController.ResetEnemyMenu();
        await animationHandler.StretchOut(enemyWindow, 0.1f);
    }

    /// <summary>
    /// Shows the Item menu
    /// </summary>
    /// <param name="items">The items the player has</param>
    public async Task ShowItemsWindow(InventoryItem<Consumable>[] items) {
        GameObject itemWindow = itemMenuController.gameObject;
        itemMenuController.SetItemMenu(items);
        await animationHandler.StretchIn(itemWindow, 0.1f);
	}

    /// <summary>
    /// Hides the item menu
    /// </summary>
    public async Task HideItemsWindow() {
        GameObject itemWindow = itemMenuController.gameObject;
        itemMenuController.ResetItemMenu();
        await animationHandler.StretchOut(itemWindow, 0.1f);
	}

    /// <summary>
    /// Shows a new text message on the dialogue box
    /// </summary>
    /// <param name="newDiagText">The text to display in the dialogue box</param>
    public void DisplayDialogueText(string newDiagText) {
        dialogueBox.EditDialogue(newDiagText, 2);
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
    public void SetEnemyHUD(BattleUnit enemyUnit) {
        enemyUnit.unitHUD.SetHP(enemyUnit.unit);
        enemyUnit.unitHUD.SetSP(enemyUnit.unit);
    }
}
