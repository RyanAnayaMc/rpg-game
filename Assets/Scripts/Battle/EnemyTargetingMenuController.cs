using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable IDE0044

public class EnemyTargetingMenuController : MonoBehaviour {
    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
    public BattleController battleController;

    [SerializeField]
    private Transform content; // The transform for the content field for the skills menu
    [SerializeField]
    private GameObject enemyButtonPrefab; // The prefab for the item buttons
    [SerializeField]
    private DialogueBoxController dialogueBox; // The text on the dialogue box

    private List<SkillButtonController> enemyButtons; // This controller is reused from skills

    public void ResetEnemyMenu() {
        // Destroy all existing item buttons
        foreach (SkillButtonController buttonController in enemyButtons)
            Destroy(buttonController.gameObject);

        enemyButtons = new List<SkillButtonController>();
    }

    public void SetEnemyMenu(List<BattleUnit> enemies) {
        // Create itemButtons array if it doesn't exist
        if (enemyButtons == null)
            enemyButtons = new List<SkillButtonController>();

        // Populate the item list
        for (int i = 0; i < enemies.Count; i++) {
            // Get the item and instantiate the button
            BattleUnit enemy = enemies[i];
            GameObject enemyButton = Instantiate(enemyButtonPrefab, content);

            // Setup button text
            SkillButtonController buttonController = enemyButton.GetComponent<SkillButtonController>();
            enemyButtons.Add(buttonController);
            buttonController.dialogueBox = dialogueBox;
            buttonController.SetData(enemy);
            buttonController.elementName.text = enemy.unit.unitName;

            // Register the listener for the button
            Button button = enemyButton.GetComponent<Button>();
            int j = i;
            button.onClick.AddListener(() => battleController.OnEnemyButton(j));
        }
    }
}
