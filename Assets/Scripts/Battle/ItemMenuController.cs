using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable IDE0044

public class ItemMenuController : MonoBehaviour {
    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
    public BattleController battleController;

    [SerializeField]
    private Transform content; // The transform for the content field for the skills menu
    [SerializeField]
    private GameObject itemButtonPrefab; // The prefab for the item buttons
    [SerializeField]
    private DialogueBoxController dialogueBox; // The text on the dialogue box

    private List<SkillButtonController> itemButtons; // This controller is reused from skills

    public void ResetItemMenu() {
        // Destroy all existing item buttons
        foreach (SkillButtonController buttonController in itemButtons)
            Destroy(buttonController.gameObject);

        itemButtons = new List<SkillButtonController>();
    }

    public void SetItemMenu(InventoryItem<Consumable>[] consumables) {
        // Create itemButtons array if it doesn't exist
        if (itemButtons == null)
            itemButtons = new List<SkillButtonController>();

        // Populate the item list
        for (int i = 0; i < consumables.Length; i++) {
            // Get the item and instantiate the button
            InventoryItem<Consumable> item = consumables[i];
            GameObject itemButton = Instantiate(itemButtonPrefab, content);

            // Setup button text
            SkillButtonController buttonController = itemButton.GetComponent<SkillButtonController>();
            itemButtons.Add(buttonController);
            buttonController.dialogueBox = dialogueBox;
            buttonController.SetData(item);

            // Register the listener for the button
            Button button = itemButton.GetComponent<Button>();
            int j = i;
            button.onClick.AddListener(() => battleController.OnItemButton(j));
		}
    }
}
